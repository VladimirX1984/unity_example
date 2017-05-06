using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using EG;
using EG.Kernel;
using EG.Misc;
using EG.Objects;

[Flags]
enum GameObjectGroup {
  EnemyGroup = 1 << 7,
  NeutralGroup = 1 << 6,
  BulletGroup = 1 << 5,
  PlayerGroup = 1 << 4,
}

public enum GameObjectType {
  Tree = GameObjectGroup.NeutralGroup + 1,
  Bonus = GameObjectGroup.NeutralGroup + 2,

  Portal = GameObjectGroup.EnemyGroup + 1,
  SmallMonster = GameObjectGroup.EnemyGroup + 2,
  Tendrils = GameObjectGroup.EnemyGroup + 3,
  IntelligentTendrils = GameObjectGroup.EnemyGroup + 4,
  Mouth = GameObjectGroup.EnemyGroup + 5,
  IntelligentMouth = GameObjectGroup.EnemyGroup + 6,
  Beetle = GameObjectGroup.EnemyGroup + 7,

  Bullet = GameObjectGroup.BulletGroup + 1,

  Skelsp = GameObjectGroup.PlayerGroup + 1
}

public class GameLevel : BaseMonoObject, IGameLevel {

  #region Sprites

  public Sprite[] spriteLevelBack = new Sprite[9];
  public Sprite spriteTree;
  public Sprite border;
  public Sprite shield;

  #endregion

  #region GameObjects

  public GameObject prefabBeetle;
  public GameObject prefabMouth;
  public GameObject prefabPortal;
  public GameObject prefabSmallMonster;
  public GameObject prefabTendrils;
  public GameObject prefabTree;

  #endregion

  #region Animation

  //public RuntimeAnimatorController shield;

  #endregion

  #region Texts

  public Text _levelText;
  public Text _playerHeathText;
  public Text _playerScoreText;
  public Text _playerAmmoText;
  public Text _playerGunText;

  #endregion

  #region Field

  public Canvas GameCanvas;

  public GameObject GameUI;

  public Rect Field;

  #endregion

  #region Constansts

  public const string BulletTagName = "Bullet";
  public const string BorderTagName = "Border";
  public const string EnemyTagName = "Enemy";
  public const string NeutralTagName = "Neutral";
  public const string PlayerTagName = "Player";
  public const string TerrainTagName = "Terrain";

  #endregion

  #region Реализация интерфейса IGameLevel

  public int Number { get; set; }

  public void Begin() {
    _levelText.text = String.Format("L: {0}", Number);
    _playerHeathText.text = String.Format("H: {0}", _player.Health);
    _playerScoreText.text = String.Format("S: {0}", _player.Score);
    _playerAmmoText.text = String.Format("A: {0}", _player.Ammo);
    _playerGunText.text = String.Format("G: {0}", _player.CarringWeapo.ToString());
    GameCanvas.enabled = true;
    GameUI.SetActive(true);
    _player.IsControlable = true;
  }

  public void Exit() {
    DebugLogger.WriteInfo("GameLevel.Exit");
    DestroyImmediate(_levelAllSprites);
    GameCanvas.enabled = false;
    GameUI.SetActive(false);
    _enemyManager.Clear();
  }
  
  public event UnityAction OnLevelWin;
  public event UnityAction<int> OnUserExit;

  #endregion

  public struct GameObjectUpgrade {
    public float SleepTime;
    public float SpeedTime;
    public float ShieldTime;
    public float ChildrenShieldTime;
    public float Health;
  }

  public GameObjectUpgrade PlayerUpgrade = new GameObjectUpgrade();

  public GameObjectUpgrade EnemyUpgrade = new GameObjectUpgrade();

  public void Init(Skelsp player) {
    _player = player;
    _player.AddObserver(_DiedObject);
    _player.OnMoved += OnPlayerMoved;
    _player.OnPosSetted += OnPlayerPosSetted;
    _player.IsControlable = false;
    _player.OnAmmoChanged += (ammo) => {
      _playerAmmoText.text = String.Format("A: {0}", ammo);
    };
    _player.OnCarringWeapoChanged += (carringWeapo) => {
      _playerGunText.text = String.Format("G: {0}", carringWeapo.ToString());
    };
    _player.OnParamChanged += (name, prev, val) => {
      if (name == "Health") {
        _playerHeathText.text = String.Format("H: {0}", Convert.ToSingle(val));
        return;
      }
      if (name == "Score") {
        _playerScoreText.text = String.Format("S: {0}", Convert.ToInt32(val));
        return;
      }
    };
    _enemyManager.Init(player);
  }

  public void LoadLevel(string levelParams) {
    _player.Reset();

    FullSerializer.fsData jsData = null;
    FullSerializer.fsResult res = FullSerializer.fsJsonParser.Parse(levelParams, out jsData);
    DebugLogger.WriteInfo("GameLevel.LoadLevel res.Failed = {0}", res.Failed.ToInt());
    if (res.Failed) {
      DebugLogger.WriteError("GameLevel.LoadLevel error = {0}", res.FormattedMessages);
      return;
    }
    DebugLogger.WriteInfo("GameLevel.LoadLevel data.IsDictionary = {0}", jsData.IsDictionary.ToInt());
    if (!jsData.IsDictionary) {
      DebugLogger.WriteError("GameLevel.LoadLevel json data is incorrect format");
      return;
    }

    var isDict = jsData.IsDictionary;
    if (!isDict) {
      DebugLogger.WriteError("GameLevel.LoadLevel json data must be have node 'level'");
      return;
    }
    jsData = jsData.AsDictionary["level"];
    DebugLogger.WriteInfo("GameLevel.LoadLevel data.AsDictionary = {0}", jsData.IsDictionary.ToInt());
    if (!jsData.IsDictionary) {
      DebugLogger.WriteError("GameLevel.LoadLevel level data is not dictionary");
      return;
    }

    var jsLevelParams = jsData.AsDictionary;
    var jsTerrain = jsLevelParams["terrain"];
    if (!jsTerrain.IsDictionary) {
      DebugLogger.WriteError("GameLevel.LoadLevel terrain data is not dictionary");
      return;
    }

    _levelAllSprites = new GameObject("GameLevel");

    var jsIslands = jsTerrain.AsDictionary["islands"];
    var jsIslandList = jsIslands.AsList;

    var islandList = new List<Island>();

    foreach (var jsIsland in jsIslandList) {
      var island = new Island(this, _levelAllSprites.transform);
      island.Load(jsIsland);
      islandList.Add(island);
    }

    if (islandList.Count == 1) {
      var island = islandList[0];
      Field = new Rect(new Vector2(island.Pos.x - 0.5f * island.Size.x,
                                   island.Pos.y - 0.5f * island.Size.y),
                       new Vector2(island.Size.x, island.Size.y));
    }
    else {
      var minPosX = float.MaxValue;
      var maxPosX = float.MinValue;
      var minPosY = float.MaxValue;
      var maxPosY = float.MinValue;
      foreach (var island in islandList) {
        if (minPosX > island.Pos.x - 0.5f * island.Size.x) {
          minPosX = island.Pos.x - 0.5f * island.Size.x;
        }
        if (maxPosX < island.Pos.x + 0.5f * island.Size.x) {
          maxPosX = island.Pos.x + 0.5f * island.Size.x;
        }
        if (minPosY > island.Pos.y - 0.5f * island.Size.y) {
          minPosY = island.Pos.y - 0.5f * island.Size.y;
        }
        if (maxPosY < island.Pos.y + 0.5f * island.Size.y) {
          maxPosY = island.Pos.y + 0.5f * island.Size.y;
        }
      }
      Field = new Rect(new Vector2(minPosX, minPosY),
                       new Vector2(maxPosX - minPosX, maxPosY - minPosX));
    }

    if (jsTerrain.AsDictionary.ContainsKey("bridges")) {
      var jsBrigdeList = jsTerrain.AsDictionary["bridges"].AsList;
      foreach (var jsBrigde in jsBrigdeList) {
        var bridge = new Bridge(this, _levelAllSprites.transform, islandList);
        if (!bridge.Load(jsBrigde)) {
          DebugLogger.WriteError("GameLevel.LoadLevel load bridge is failed");
        }
      }
    }

    foreach (var island in islandList) {
      island.CreateBorders();
    }

    if (jsLevelParams.ContainsKey("enemy")) {
      var jsEnemy = jsLevelParams["enemy"];
      if (jsEnemy.AsDictionary.ContainsKey("portals")) {
        var jsPortalList = jsEnemy.AsDictionary["portals"].AsList;
        foreach (var jsPortal in jsPortalList) {
          var jsPortalData = jsPortal.AsDictionary;
          int portalLevel = jsPortalData.ContainsKey("level") ? (int)jsPortalData["level"].AsInt64 : 0;

          var portalPos = GameLevelJsonLoader.GetPos(jsPortalData["pos"]);
          int islandId = jsPortalData.ContainsKey("islandId") ? (int)jsPortalData["islandId"].AsInt64 : -1;
          var portal = EGHelpers.CreateObjectByPrefab<Portal>((islandId == -1 ? portalPos :
                       (portalPos + islandList.Find(it => it.Id == islandId).Pos)), prefabPortal, _levelAllSprites.transform);
          portal.Level = portalLevel;
          portal.AddObserver(_DiedObject);
          _enemyManager.AddEnemy(portal);
        }
      }
    }

    var jsBonus = jsLevelParams["bonus"];
    var jsBonusData = jsBonus.AsDictionary;
    var jsGoodBonus = jsBonusData["player"].AsDictionary;
    PlayerUpgrade.SleepTime = (float)jsGoodBonus["sleepTime"].AsDouble;
    PlayerUpgrade.SpeedTime = (float)jsGoodBonus["speedTime"].AsDouble;
    PlayerUpgrade.ShieldTime = (float)jsGoodBonus["shieldTime"].AsDouble;
    PlayerUpgrade.ChildrenShieldTime = (float)jsGoodBonus["childrenShieldTime"].AsDouble;
    PlayerUpgrade.Health = (float)jsGoodBonus["health"].AsDouble;
    var jsEnemyBonus = jsBonusData["enemy"].AsDictionary;
    EnemyUpgrade.SleepTime = (float)jsEnemyBonus["sleepTime"].AsDouble;
    EnemyUpgrade.SpeedTime = (float)jsEnemyBonus["speedTime"].AsDouble;
    EnemyUpgrade.ShieldTime = (float)jsEnemyBonus["shieldTime"].AsDouble;
    EnemyUpgrade.Health = (float)jsEnemyBonus["health(%)"].AsDouble;
  }

  public void SleepEnemies(float time) {
    _enemyManager.SleepEnemies(time);
  }

  public void SetGunType(int weapon) {
    DebugLogger.WriteInfo("GameLevel.SetGunType weapon = {0}", weapon);
    _player.SetGunType((Gun.Weapons)weapon);
  }

  public void UserExit(int reason) {
    DebugLogger.WriteInfo("GameLevel.UserExit reason = {0}", reason);
    StartCoroutine(_UserExitCoroutine(reason));
  }

  private IEnumerator _UserExitCoroutine(int reason) {
    yield return new WaitForSeconds(0.1f);
    if (OnUserExit != null) {
      OnUserExit(reason);
    }
  }

  #region MonoBehaviour Events

  protected override void Start() {
    base.Start();
    GameCanvas.enabled = false;
    GameUI.SetActive(false);
    _enemyManager = GetComponent<EnemyManager>();
    _enemyManager.OnEnemyZero += OnEnemyZero;
  }

  #endregion

  private Skelsp _player;
  private GameObject _levelAllSprites;
  private EnemyManager _enemyManager;

  private float shiftX = 0f;

  private void OnPlayerMoved(Vector2 vec) {    
    var vec2 = new Vector3(_player.__Transform.localPosition.x - shiftX,
                           _player.__Transform.localPosition.y,
                           GameManager.Instance.MainCamera.transform.localPosition.z);
    GameManager.Instance.MainCamera.transform.localPosition = vec2;
    GameUI.transform.localPosition = vec2;
    GameCanvas.transform.localPosition = -1f * vec2;
  }

  private void OnPlayerPosSetted(Vector2 pos) {
    DebugLogger.WriteInfo("OnPlayerPosSetted vec = {0}", pos.ToString());
    var vec2 = new Vector3(pos.x - shiftX, pos.y, GameManager.Instance.MainCamera.transform.localPosition.z);
    GameManager.Instance.MainCamera.transform.localPosition = vec2;
    GameUI.transform.localPosition = vec2;
    GameCanvas.transform.localPosition = -1f * vec2;
  }

  private void _DiedObject(IGameObject obj, int reason) {
    DebugLogger.WriteVerbose("GameLevel._DeadObject obj.gameObject.tag = {0}; reason = {1}", obj.gameObject.tag, reason);
    if (reason > 0) {
      return;
    }
  }

  private void OnEnemyZero() {
    DebugLogger.WriteInfo("GameLevel.OnEnemyZero");
    StartCoroutine(_OnEnemyZero());
  }

  private IEnumerator _OnEnemyZero() {
    yield return new WaitForSeconds(3.0f);
    if (OnLevelWin != null) {
      OnLevelWin();
    }
  }
}
