using UnityEngine;
using System;
using System.Collections.Generic;

using EG;
using EG.Calc.Geometric;
using EG.Objects;

public class Bridge : BaseIDObject<int> {

  public Vector2 Pos;
  public Vector2 Size { get; set; }

  public static float Thickness = 0.05f;

  public bool Load(FullSerializer.fsData jsBridge) {
    var jsBridgeData = jsBridge.AsDictionary;
    int beginId = (int)jsBridgeData["begin"].AsInt64;
    int endId = (int)jsBridgeData["end"].AsInt64;
    var resIndex = jsBridgeData["res"].AsInt64;
    if (!jsBridgeData.ContainsKey("orientation")) {
      return false;
    }
    int orientation = (int)jsBridgeData["orientation"].AsInt64;
    if (!jsBridgeData.ContainsKey("pos")) {
      return false;
    }
    //Pos = GameLevelJsonLoader.GetPos(jsBridgeData["pos"]);
    float width = (float)jsBridgeData["w"].AsDouble;
    var beginIsland = _islandList.Find(it => it.Id == beginId);
    var endIsland = _islandList.Find(it => it.Id == endId);
    if (beginIsland == null || endIsland == null) {
      DebugLogger.WriteError("Bridge.LoadLevel island is not found");
      return false;
    }
    Pos = Vector2.zero;
    if (orientation == 0) {
      // мост будет горизонтально
      Rectangle rect = beginIsland.IslandRect;
      Rectangle rect2 = endIsland.IslandRect;
      if (rect.IntersectsWith(rect2)) {
        DebugLogger.WriteError("Bridge.LoadLevel islands is intersects");
        return false;
      }
      Pos.y = (float)jsBridgeData["pos"].AsDouble;
      Rectangle leftIslandRect;
      Rectangle rightIslandRect;
      Island leftIsland;
      Island rightIsland;
      if (rect.X < rect2.X) {
        leftIslandRect = rect;
        leftIsland = beginIsland;
        rightIslandRect = rect2;
        rightIsland = endIsland;
      }
      else {
        leftIslandRect = rect2;
        leftIsland = endIsland;
        rightIslandRect = rect;
        rightIsland = beginIsland;
      }
      Pos.x = (leftIsland.Pos.x + rightIsland.Pos.x) / 2;
      /*if (Pos.x > rightIsland.Pos.x || Pos.x < leftIsland.Pos.x) {
        DebugLogger.WriteError("Bridge.LoadLevel horizontal bridge of islands is impossible create");
        return false;
        }*/
      leftIsland.Bridges.AddBridge(Island.Side.Right, this);
      rightIsland.Bridges.AddBridge(Island.Side.Left, this);
      Size = new Vector2(rightIslandRect.Left - leftIslandRect.Right, width);
      CreateVBorders(Pos, new Vector2(Size.x, Thickness));
    }
    else {
      // мост будет вертикально
      Rectangle rect = beginIsland.IslandRect;
      Rectangle rect2 = endIsland.IslandRect;
      if (rect.IntersectsWith(rect2)) {
        DebugLogger.WriteError("Bridge.LoadLevel islands is intersects");
        return false;
      }
      Pos.x = (float)jsBridgeData["pos"].AsDouble;
      Rectangle topIslandRect;
      Rectangle bottomIslandRect;
      Island topIsland;
      Island bottomIsland;
      if (rect.Y > rect2.Y) {
        topIslandRect = rect;
        topIsland = beginIsland;
        bottomIslandRect = rect2;
        bottomIsland = endIsland;
      }
      else {
        topIslandRect = rect2;
        topIsland = endIsland;
        bottomIslandRect = rect;
        bottomIsland = beginIsland;
      }
      Pos.y = (topIsland.Pos.y + bottomIsland.Pos.y) / 2;
      /*if (Pos.y < bottomIsland.Pos.y || Pos.y > topIsland.Pos.y) {
        DebugLogger.WriteError("Bridge.LoadLevel vertical bridge of islands is impossible create");
        return false;
        }*/
      topIsland.Bridges.AddBridge(Island.Side.Bottom, this);
      bottomIsland.Bridges.AddBridge(Island.Side.Top, this);
      Size = new Vector2(width, topIslandRect.Bottom - bottomIslandRect.Top);
      CreateHBorders(Pos, new Vector2(Thickness, Size.y));
    }
    // back
    var sprite = _gameLevel.spriteLevelBack[(int)resIndex];
    DebugLogger.WriteInfo("Bridge.LoadLevel Pos = {0}; Size = {1}", Pos.ToString(), Size.ToString());
    var backObj = EGHelpers.CreateSprite(Pos, sprite, "bridgeback_" + Id, _parentTransform, false);
    backObj.transform.localScale = new Vector3(Size.x * 100f / sprite.rect.width,
                                               Size.y * 100f / sprite.rect.height, 1f);
    var sprRender = backObj.GetComponent<SpriteRenderer>();
    sprRender.sortingOrder = -1;

    return true;
  }

  public Bridge(GameLevel gameLevel, Transform parentTransform, List<Island> islandList)
  : base() {
    _gameLevel = gameLevel;
    _parentTransform = parentTransform;
    _islandList = islandList;
  }

  private GameLevel _gameLevel;
  private Transform _parentTransform;
  private List<Island> _islandList;

  private bool bUnity56 = true;

  private void CreateVBorders(Vector2 pos, Vector2 size) {
    GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_0_1", Id), pos + new Vector2(0f, 0.5f * Size.y),
                                             size, _parentTransform, true);
    GameManager.Instance.CreateSpriteVBorder(String.Format("{0}_0_2", Id), pos - new Vector2(0f, 0.5f * Size.y),
                                             size, _parentTransform, false);
    if (bUnity56) {
      GameManager.Instance.CreateVBorder(String.Format("{0}_0_1", Id), pos + new Vector2(0f, 0.5f * Size.y),
                                         size, _parentTransform, true);
      GameManager.Instance.CreateVBorder(String.Format("{0}_0_2", Id), pos - new Vector2(0f, 0.5f * Size.y),
                                         size, _parentTransform, false);
    }
  }

  private void CreateHBorders(Vector2 pos, Vector2 size) {
    GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_1_1", Id), pos + new Vector2(0.5f * Size.x, 0f),
                                             size, _parentTransform, true);
    GameManager.Instance.CreateSpriteHBorder(String.Format("{0}_1_2", Id), pos - new Vector2(0.5f * Size.x, 0f),
                                             size, _parentTransform, false);
    if (bUnity56) {
      GameManager.Instance.CreateHBorder(String.Format("{0}_1_1", Id), pos + new Vector2(0.5f * Size.x, 0f),
                                         size, _parentTransform, true);
      GameManager.Instance.CreateHBorder(String.Format("{0}_1_2", Id), pos - new Vector2(0.5f * Size.x, 0f),
                                         size, _parentTransform, false);
    }
  }
}
