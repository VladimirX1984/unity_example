using UnityEngine;
using System;
using System.Linq;

using EG;
using EG.Misc;
using EG.Objects;
using EG.Calc.Geometric;
using System.Collections.Generic;

public class Island : BaseIDObject<int> {

  public enum Side {
    Left,
    Top,
    Right,
    Bottom
  }

  public Vector2 Pos { get; set; }
  public Vector2 Size { get; set; }

  public Rectangle IslandRect {
    get { return new Rectangle(Pos.x - 0.5f * Size.x, Pos.y - 0.5f * Size.y, Size.x, Size.y); }
  }

  public class IslandBridges : Dictionary<Side, List<Bridge>> {

    public void AddBridge(Side side, Bridge bridge) {
      if (!ContainsKey(side)) {
        Add(side, new List<Bridge>());
      }
      this[side].Add(bridge);
    }
  }

  public IslandBridges Bridges = new IslandBridges();

  public bool Load(FullSerializer.fsData jsIsland) {
    var jsIslandData = jsIsland.AsDictionary;
    Id = (int)jsIslandData["id"].AsInt64;

    var islandPos = GameLevelJsonLoader.GetPos(jsIslandData["pos"]);
    var size = GameLevelJsonLoader.GetSize(jsIslandData["size"]);
    var resIndex = jsIslandData["res"].AsInt64;
    Pos = islandPos;
    Size = size;

    // back
    var sprite = _gameLevel.spriteLevelBack[(int)resIndex];
    var backObj = EGHelpers.CreateSprite(Pos, sprite, "back_" + Id, _parentTransform, false);
    DebugLogger.WriteInfo("Island.Load sprite.rect = " + sprite.rect.ToString());
    backObj.transform.localScale = new Vector3(Size.x * 100f / sprite.rect.width,
                                               Size.y * 100f / sprite.rect.height, 1f);
    var sprRender = backObj.GetComponent<SpriteRenderer>();
    sprRender.sortingOrder = -2;

    if (jsIslandData.ContainsKey("backMesgColor")) {
      var sBackMesgColor = jsIslandData["backMesgColor"].AsString;
      float[] colors = sBackMesgColor.Split<float>(',');
      sprRender.color = new Color(colors[0], colors[1], colors[2]);
    }
    if (jsIslandData.ContainsKey("trees")) {
      var jsTreeList = jsIslandData["trees"].AsList;
      foreach (var jsTree in jsTreeList) {
        var jsTreeData = jsTree.AsDictionary;
        var treePos = GameLevelJsonLoader.GetPos(jsTreeData["pos"]);
        var treeObj = EGHelpers.CreateSpriteByScript<Tree>(islandPos + treePos, _gameLevel.spriteTree, "tree",
                                                           _parentTransform, GameLevel.TerrainTagName);
      }
    }

    return true;
  }

  public void CreateBorders() {
    var thickness = 0.05f;

    var bridgeList = Bridges.ContainsKey(Side.Bottom) ? Bridges[Side.Bottom] : null;
    CreateVBorders(bridgeList, thickness, false);
    bridgeList = Bridges.ContainsKey(Side.Top) ? Bridges[Side.Top] : null;
    CreateVBorders(bridgeList, thickness, true);

    bridgeList = Bridges.ContainsKey(Side.Left) ? Bridges[Side.Left] : null;
    CreateHBorders(bridgeList, thickness, false);
    bridgeList = Bridges.ContainsKey(Side.Right) ? Bridges[Side.Right] : null;
    CreateHBorders(bridgeList, thickness, true);
  }

  public Island(GameLevel gameLevel, Transform parentTransform)
  : base(String.Empty, (int)0) {
    _gameLevel = gameLevel;
    _parentTransform = parentTransform;
  }

  private GameLevel _gameLevel;
  private Transform _parentTransform;

  private bool bUnity56 = true;

  private void CreateVBorders(List<Bridge> bridgeList, float thickness, bool bTop) {
    var fieldWidth = Size.x;
    var fieldHeight = Size.y;
    var coef = bTop ? 0.5f : -0.5f;

    if (bridgeList == null || !bridgeList.Any()) {
      GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_{1}", Id, bTop.ToInt()),
                                               new Vector2(Pos.x, Pos.y + coef * Size.y),
                                               new Vector2(Size.x, thickness), _parentTransform, bTop);
      // в Unity 5.6 создание бордюра методом CreateSpriteVBorder не обеспечивает препятствие
      if (bUnity56) {
        GameManager.Instance.CreateVBorder(String.Format("{0}_{1}", Id, bTop.ToInt()),
                                           new Vector2(Pos.x, Pos.y + coef * Size.y),
                                           new Vector2(Size.x, thickness), _parentTransform, bTop);
      }
    }
    else if (bridgeList.Count < 2) {
      var bridge = bridgeList[0];
      var x = ((Pos.x - 0.5f * fieldWidth) + (bridge.Pos.x - 0.5f * bridge.Size.x)) * 0.5f;
      var sizeX = (bridge.Pos.x - 0.5f * bridge.Size.x) - (Pos.x - 0.5f * fieldWidth) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_0_{1}", Id, bTop.ToInt()),
                                               new Vector2(x, Pos.y + coef * fieldHeight),
                                               new Vector2(sizeX, thickness), _parentTransform, bTop);
      if (bUnity56) {
        GameManager.Instance.CreateVBorder(String.Format("{0}_0_{1}", Id, bTop.ToInt()),
                                           new Vector2(x, Pos.y + coef * fieldHeight),
                                           new Vector2(sizeX, thickness), _parentTransform, bTop);
      }

      x = ((Pos.x + 0.5f * fieldWidth) + (bridge.Pos.x + 0.5f * bridge.Size.x)) * 0.5f;
      sizeX = (Pos.x + 0.5f * fieldWidth) - (bridge.Pos.x + 0.5f * bridge.Size.x) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_1_{1}", Id, bTop.ToInt()),
                                               new Vector2(x, Pos.y + coef * fieldHeight),
                                               new Vector2(sizeX, thickness), _parentTransform, bTop);
      if (bUnity56) {
        GameManager.Instance.CreateVBorder(String.Format("{0}_1_{1}", Id, bTop.ToInt()),
                                           new Vector2(x, Pos.y + coef * fieldHeight),
                                           new Vector2(sizeX, thickness), _parentTransform, bTop);
      }
    }
    else {
      var bridgeList_ = bridgeList.OrderBy(it => it.Pos.x).ToArray();
      var bridge = bridgeList_[0];
      var x = ((Pos.x - 0.5f * fieldWidth) + (bridge.Pos.x - 0.5f * bridge.Size.x)) * 0.5f;
      var sizeX = (bridge.Pos.x - 0.5f * bridge.Size.x) - (Pos.x - 0.5f * fieldWidth) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_0_{1}", Id, bTop.ToInt()),
                                               new Vector2(x, Pos.y + coef * fieldHeight),
                                               new Vector2(sizeX, thickness), _parentTransform, bTop);
      if (bUnity56) {
        GameManager.Instance.CreateVBorder(String.Format("{0}_0_{1}", Id, bTop.ToInt()),
                                           new Vector2(x, Pos.y + coef * fieldHeight),
                                           new Vector2(sizeX, thickness), _parentTransform, bTop);
      }

      for (int i = 0; i < bridgeList_.Length - 1; ++i) {
        bridge = bridgeList_[i];
        var nextBridge = bridgeList_[i + 1];
        x = ((nextBridge.Pos.x - 0.5f * nextBridge.Size.x) + (bridge.Pos.x + 0.5f * bridge.Size.x)) * 0.5f;
        sizeX = (nextBridge.Pos.x - 0.5f * bridge.Size.x) - (bridge.Pos.x + 0.5f * bridge.Size.x) - Bridge.Thickness;
        GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_{1}_{2}", Id, i, bTop.ToInt()),
                                                 new Vector2(x, Pos.y + coef * fieldHeight),
                                                 new Vector2(sizeX, thickness), _parentTransform, bTop);
        if (bUnity56) {
          GameManager.Instance.CreateVBorder(String.Format("{0}_{1}_{2}", Id, i, bTop.ToInt()),
                                             new Vector2(x, Pos.y + coef * fieldHeight),
                                             new Vector2(sizeX, thickness), _parentTransform, bTop);
        }
      }
      bridge = bridgeList_[bridgeList_.Length - 1];
      x = ((Pos.x + 0.5f * fieldWidth) + (bridge.Pos.x + 0.5f * bridge.Size.x)) * 0.5f;
      sizeX = (Pos.x + 0.5f * fieldWidth) - (bridge.Pos.x + 0.5f * bridge.Size.x) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_{1}_{2}", Id, bridgeList.Count - 1, bTop.ToInt()),
                                               new Vector2(x, Pos.y + coef * fieldHeight),
                                               new Vector2(sizeX, thickness), _parentTransform, bTop);
      if (bUnity56) {
        GameManager.Instance.CreateVBorder(String.Format("{0}_{1}_{2}", Id, bridgeList.Count - 1, bTop.ToInt()),
                                           new Vector2(x, Pos.y + coef * fieldHeight),
                                           new Vector2(sizeX, thickness), _parentTransform, bTop);
      }
    }
  }

  private void CreateHBorders(List<Bridge> bridgeList, float thickness, bool bRight) {
    var fieldWidth = Size.x;
    var fieldHeight = Size.y;
    var coef = bRight ? 0.5f : -0.5f;

    if (bridgeList == null || !bridgeList.Any()) {
      GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_{1}", Id, bRight.ToInt()),
                                               new Vector2(Pos.x + coef * Size.x, Pos.y),
                                               new Vector2(thickness, Size.y), _parentTransform, bRight);
      if (bUnity56) {
        GameManager.Instance.CreateHBorder(String.Format("{0}_{1}", Id, bRight.ToInt()),
                                           new Vector2(Pos.x + coef * Size.x, Pos.y),
                                           new Vector2(thickness, Size.y), _parentTransform, bRight);
      }
    }
    else if (bridgeList.Count < 2) {
      var bridge = bridgeList[0];
      var y = ((Pos.y - 0.5f * fieldHeight) + (bridge.Pos.y - 0.5f * bridge.Size.y)) * 0.5f;
      var sizeY = (bridge.Pos.y - 0.5f * bridge.Size.y) - (Pos.y - 0.5f * fieldWidth) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_1_{1}", Id, bRight.ToInt()),
                                               new Vector2(Pos.x + coef * fieldWidth, y),
                                               new Vector2(thickness, sizeY), _parentTransform, bRight);
      if (bUnity56) {
        GameManager.Instance.CreateHBorder(String.Format("{0}_1_{1}", Id, bRight.ToInt()),
                                           new Vector2(Pos.x + coef * fieldWidth, y),
                                           new Vector2(thickness, sizeY), _parentTransform, bRight);
      }
      y = ((Pos.y + 0.5f * fieldHeight) + (bridge.Pos.y + 0.5f * bridge.Size.y)) * 0.5f;
      sizeY = (Pos.y + 0.5f * fieldHeight) - (bridge.Pos.y + 0.5f * bridge.Size.y) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_2_{1}", Id, bRight.ToInt()),
                                               new Vector2(Pos.x + coef * fieldWidth, y),
                                               new Vector2(thickness, sizeY), _parentTransform, bRight);
      if (bUnity56) {
        GameManager.Instance.CreateHBorder(String.Format("{0}_2_{1}", Id, bRight.ToInt()),
                                           new Vector2(Pos.x + coef * fieldWidth, y),
                                           new Vector2(thickness, sizeY), _parentTransform, bRight);
      }
    }
    else {
      var bridgeList_ = bridgeList.OrderBy(it => it.Pos.y).ToArray();
      var bridge = bridgeList_[0];
      var y = ((Pos.y - 0.5f * fieldHeight) + (bridge.Pos.y - 0.5f * bridge.Size.y)) * 0.5f;
      var sizeY = (bridge.Pos.y - 0.5f * bridge.Size.y) - (Pos.y - 0.5f * fieldHeight) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_0_{1}", Id, bRight.ToInt()),
                                               new Vector2(Pos.x + coef * fieldWidth, y),
                                               new Vector2(thickness, sizeY), _parentTransform, bRight);
      if (bUnity56) {
        GameManager.Instance.CreateHBorder(String.Format("{0}_0_{1}", Id, bRight.ToInt()),
                                           new Vector2(Pos.x + coef * fieldWidth, y),
                                           new Vector2(thickness, sizeY), _parentTransform, bRight);
      }

      for (int i = 0; i < bridgeList_.Length - 1; ++i) {
        bridge = bridgeList_[i];
        var nextBridge = bridgeList_[i + 1];
        y = ((nextBridge.Pos.y - 0.5f * nextBridge.Size.y) + (bridge.Pos.y + 0.5f * bridge.Size.y)) * 0.5f;
        sizeY = (nextBridge.Pos.y - 0.5f * bridge.Size.y) - (bridge.Pos.y + 0.5f * bridge.Size.y) - Bridge.Thickness;
        GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_{1}_{2}", Id, i, bRight.ToInt()),
                                                 new Vector2(Pos.x + coef * fieldWidth, y),
                                                 new Vector2(thickness, sizeY), _parentTransform, bRight);
        if (bUnity56) {
          GameManager.Instance.CreateHBorder(String.Format("{0}_{1}_{2}", Id, i, bRight.ToInt()),
                                             new Vector2(Pos.x + coef * fieldWidth, y),
                                             new Vector2(thickness, sizeY), _parentTransform, bRight);
        }
      }
      bridge = bridgeList_[bridgeList_.Length - 1];
      y = ((Pos.y + 0.5f * fieldHeight) + (bridge.Pos.y + 0.5f * bridge.Size.y)) * 0.5f;
      sizeY = (Pos.y + 0.5f * fieldHeight) - (bridge.Pos.y + 0.5f * bridge.Size.y) - Bridge.Thickness;
      GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_{1}_{2}", Id, bridgeList.Count - 1, bRight.ToInt()),
                                               new Vector2(Pos.x + coef * fieldWidth, y),
                                               new Vector2(thickness, sizeY), _parentTransform, bRight);
      if (bUnity56) {
        GameManager.Instance.CreateHBorder(String.Format("{0}_{1}_{2}", Id, bridgeList.Count - 1, bRight.ToInt()),
                                           new Vector2(Pos.x + coef * fieldWidth, y),
                                           new Vector2(thickness, sizeY), _parentTransform, bRight);
      }
    }
  }
}
