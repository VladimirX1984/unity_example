using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using EG;
using EG.Players;

public class GameManager : EG.UnitySingleton<GameManager> {

  #region Texts

  public Canvas _canvas;
  public Text _menuText;
  public Text _highScoreText;
  public Text _gameTitle;

  public Text _stageText;

  public Text _livesPlayerText;

  public Text _levelWinText;

  #endregion

  #region GameObjects

  public GameObject prefabPlayer;
  public GameObject prefabCursor;
  public Button prefabGoLevelButton;

  #endregion

  #region Animation

  public RuntimeAnimatorController[] gemBonusAnims = new RuntimeAnimatorController[7];

  #endregion

  public Camera MainCamera;

  public int HighScore {
    get { return _highScore; }
    set {
      if (value <= _highScore) {
        return;
      }
      _highScore = value;
      PlayerPrefs.SetInt("HighScore", _highScore);
    }
  }
  private int _highScore;

  public IGameLevel GetGameLevel() {
    return _gameLevel;
  }

  public GameLevel GetMonoGameLevel() {
    return _gameLevel as GameLevel;
  }

  public IPlayer GetPlayer() {
    return _player;
  }

  public void StartLevel(int levelNumber) {
    _levelNumber = levelNumber;
    _LevelUp();
  }

  #region Методы создания бордюр

  public Border CreateSpriteVBorder(string id, Vector2 pos, Vector2 size,
                                    Transform parentTransform, bool bTop) {
    var coef = bTop ? 0.5f : -0.5f;
    var border = EGHelpers.CreateSpriteByScript<Border>(
                   new Vector2(pos.x, pos.y + coef * size.y), _gameLevel.border,
                   "vBorder" + id, parentTransform, "Border");

    border.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);
    border.transform.localScale = new Vector3(size.x * 100f / _gameLevel.border.rect.width,
        size.y * 100f / _gameLevel.border.rect.height, 1.0f);
    return border;
  }

  public Border CreateVBorder(string id, Vector2 pos, Vector2 size,
                              Transform parentTransform, bool bTop) {
    var coef = bTop ? 0.5f : -0.5f;
    var border = EGHelpers.CreateObjectByScript<Border>(
                   new Vector2(pos.x, pos.y + coef * size.y), "vBorder" + id,
                   parentTransform, "Border");
    var boxCollider = border.GetComponent<BoxCollider2D>();
    boxCollider.size = new Vector2(size.x, size.y);
    return border;
  }

  public Border CreateSpriteHBorder(string id, Vector2 pos, Vector2 size,
                                    Transform parentTransform, bool bRight) {
    var coef = bRight ? 0.5f : -0.5f;
    var border = EGHelpers.CreateSpriteByScript<Border>(
                   new Vector2(pos.x + coef * size.x, pos.y), _gameLevel.border,
                   "hBorder" + id, parentTransform, "Border");

    border.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 0.1f);
    border.transform.localScale = new Vector3(size.x * 100f / _gameLevel.border.rect.width,
        size.y * 100f / _gameLevel.border.rect.height, 1.0f);
    return border;
  }

  public Border CreateHBorder(string id, Vector2 pos, Vector2 size,
                              Transform parentTransform, bool bRight) {
    var coef = bRight ? 0.5f : -0.5f;
    var border = EGHelpers.CreateObjectByScript<Border>(
                   new Vector2(pos.x + coef * size.x, pos.y), "hBorder" + id,
                   parentTransform, "Border");
    var boxCollider = border.GetComponent<BoxCollider2D>();
    boxCollider.size = new Vector2(size.x, size.y);
    return border;
  }

  #endregion

  protected GameManager() {
    IsAutoCreate = false;
  }

  GameObject _menuPanel = null;

  #region MonoBehaviour Events

  // Use this for initialization
  void Start() {
    DebugLogger.WriteInfo("GameManager.Start()");
    Screen.autorotateToPortrait = false;
    Screen.autorotateToPortraitUpsideDown = false;
    Screen.orientation = ScreenOrientation.Landscape;
    _highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;

    MainCamera = Camera.main;

    _inputManager = GetComponent<InputManager>();

    var obj = GameObject.Find("Reveal1");
    _reveal1 = obj.transform;
    _reveal1.gameObject.SetActive(false);
    obj = GameObject.Find("Reveal2");
    _reveal2 = obj.transform;
    _reveal2.gameObject.SetActive(false);

    _stageText.enabled = false;

    _highScoreText.text = String.Format("High Score: {0}", _highScore);

    _textLevels = Resources.LoadAll<TextAsset>("levels");

    _menuPanel = GameObject.Find("MenuPanel");
    for (int i = 1; i <= _textLevels.Length; ++i) {
      var goLevelButton = (Button)GameObject.Instantiate(prefabGoLevelButton, _menuPanel.transform);
      int x = i;
      int y = 0;
      if (i > 10) {
        x = i - 10;
        y = -90;
      }
      goLevelButton.transform.localPosition = new Vector3(-490 + x * 90, y);
      goLevelButton.transform.localScale = new Vector3(1f, 1f, 1f);

      DebugLogger.WriteInfo("GameManager.Start button i = {0}", i);
      var text = goLevelButton.GetComponentInChildren<Text>();
      text.text = i < 10 ? String.Format("0{0}", i) : i.ToString();
      var goLevelTouch = goLevelButton.GetComponent<GoLevelTouch>();
      goLevelTouch.Init(i);
      goLevelButton.onClick = goLevelTouch.Init(i);
    }

    _levelWinText.enabled = false;

    StartCoroutine(_WaitAndStartEnd());
  }

  // Update is called once per frame
  void Update() {
    if (!_bStart) {
      return;
    }
    /*if (!_bLevelSetup && Input.GetButtonDown("Submit")) {
      _levelNumber = 0;
      _LevelUp();
      }*/
  }

  #endregion

  #region Objects

  private StartCursor _cursorScript;

  private Skelsp _player;

  private GameLevel _gameLevel;

  private AudioSource _audio;

  private Transform _reveal1;
  private Transform _reveal2;

  #endregion

  private int _levelNumber = 0;
  private bool _bStart = false;
  private bool _bLevelSetup = false;
  private TextAsset[] _textLevels;   // Уровни игры в текстовом виде

  private Coroutine _demo;

  private InputManager _inputManager;

  #region Закрытие методы - логика загрузки уровня

  private IEnumerator _WaitAndStartEnd() {
    yield return new WaitForEndOfFrame();
    _StartEnd();
  }

  private void _StartEnd() {
    _MenuOnOff(true);
    _cursorScript = EGHelpers.CreateObjectByPrefab<StartCursor>(new Vector3(0, 0, 0), prefabCursor);
    //_demo = StartCoroutine(_TimerForDemo());
    _bStart = true;
  }

  private IEnumerator _TimerForDemo() {
    yield return new WaitForSeconds(2);
    _MenuOnOff(true);
    Application.LoadLevel("Demo");
  }

  private void _MenuOnOff(bool onOff) {
    _menuPanel.SetActive(onOff);
  }

  private void _LevelUp() {
    DebugLogger.WriteInfo("_LevelUp _levelNumber = {0}", _levelNumber);
    _reveal1.gameObject.SetActive(true);
    _reveal2.gameObject.SetActive(true);
    if (_cursorScript != null) {
      _cursorScript.gameObject.SetActive(false);
    }
    StartCoroutine(EGHelpers.Move(_reveal1, -5.0f, _LevelUpEnd));
    StartCoroutine(EGHelpers.Move(_reveal2, 5.0f));
  }

  private void _LevelUpEnd() {
    StartCoroutine(_LevelUpEndCoroutine());
  }

  private IEnumerator _LevelUpEndCoroutine() {
    DebugLogger.WriteInfo("_LevelUpEndCoroutine");
    //_LevelOff();
    //_reveal1.transform.Translate(new Vector3(-2.1f, 0, 0));
    //_reveal2.transform.Translate(new Vector3(-2.1f, 0, 0));
    _MenuOnOff(false);
    _LevelOn();
    _stageText.text = String.Format("STAGE {0}", _levelNumber);
    _stageText.enabled = true;
    _canvas.sortingOrder = 3;

    //_audio.Play();
    //float lengthMusic = _audio.clip.length;
    yield return new WaitForSeconds(1);
    //Application.LoadLevel(_level);

    _stageText.enabled = false;
    _canvas.sortingOrder = 0;

    StartCoroutine(EGHelpers.Move(_reveal1, 5.0f, _LevelStartEnd));
    StartCoroutine(EGHelpers.Move(_reveal2, -5.0f));
  }

  private void _LevelStartEnd() {
    _reveal1.gameObject.SetActive(false);
    _reveal2.gameObject.SetActive(false);
    _gameLevel.Begin();
  }

  private void _LevelOn() {
    if (!_bLevelSetup) {
      _bLevelSetup = true;
      _gameLevel = GetComponentInChildren<GameLevel>();
      _player = EGHelpers.CreateObjectByPrefab<Skelsp>(Vector3.zero, prefabPlayer);
      _gameLevel.Init(_player);
      _inputManager.SetPlayer(_player);
      _gameLevel.OnLevelWin += OnLevelWin;
    }

    _player.gameObject.SetActive(true);
    DebugLogger.WriteInfo("_LevelOn _levelNumber = {0}", _levelNumber);
    _gameLevel.Number = _levelNumber;
    _gameLevel.LoadLevel(_textLevels[_levelNumber - 1].text);
  }

  private void _LevelOff() {
    if (!_bLevelSetup) {
      return;
    }
    StartCoroutine(__LevelOffEnd(this));
  }

  private IEnumerator __LevelOffEnd(GameManager gameManager) {
    DebugLogger.WriteInfo("GameManager.__LevelOffEnd");
    var player = _player;
    player.IsControlable = false;
    yield return new WaitForEndOfFrame();
    _levelWinText.transform.localPosition = player.gameObject.transform.localPosition;
    _levelWinText.enabled = true;
    player.gameObject.SetActive(false);
    yield return new WaitForSeconds(3f);
    _levelWinText.enabled = false;
    _gameLevel.Exit();
    player.SetPosition(Vector2.zero);
    if (_highScore < player.Score) {
      HighScore = player.Score;
      _highScoreText.text = String.Format("High Score: {0}", _highScore);
    }
    _MenuOnOff(true);
  }

  private void OnLevelWin() {
    _LevelOff();
  }

  #endregion
}
