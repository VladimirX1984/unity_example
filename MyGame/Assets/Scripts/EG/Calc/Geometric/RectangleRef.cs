using UnityEngine;

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

using EG.Kernel;
using EG.Misc;

namespace EG.Calc.Geometric {
[Serializable]
public class Rectangle {
  public float Left {
    get { return _location.x; }
    set { _location.x = value; }
  }

  public float Right {
    get { return _location.x + _size.x; }
    set { _size.x = value - _location.x; }
  }

  public float Top {
    get { return _location.y; }
    set { _location.y = value; }
  }

  public float Bottom {
    get { return _location.y - _size.y; }
    set { _size.y = _location.y - value; }
  }

  public float Width {
    get { return _size.x; }
    set { _size.x = value; }
  }

  public float Height {
    get { return _size.y; }
    set { _size.y = value; }
  }

  public Vector2 Location {
    get { return _location; }
    set {
      if (_location != value) {
        _location = new Vector2(value.x, value.y);
      }
    }
  }
  private Vector2 _location;

  public Vector2 Size {
    get { return _size; }
    set {
      if (_size != value) {
        _size = new Vector2(value.x, value.y);
      }
    }
  }
  private Vector2 _size;

  public float X {
    get { return _location.x; }
    set { _location.x = value; }
  }

  public float Y {
    get { return _location.y; }
    set { _location.y = value; }
  }

  public Vector2 Center {
    get {
      return new Vector2(Location.x + 0.5f * Size.x,
                         Location.y + 0.5f * Size.y);
    }
  }

  public float Square {
    get { return Width * Height; }
  }

  public void Set(float x, float y, float width, float height) {
    _location.x = x;
    _location.y = y;
    _size.x = width;
    _size.y = height;
  }

  public void Set(Rect rect) {
    _location.x = rect.x;
    _location.x = rect.y;
    _size.x = rect.width;
    _size.y = rect.height;
  }

  public void Set(Rectangle rect) {
    _location.x = rect.X;
    _location.x = rect.Y;
    _size.x = rect.Width;
    _size.y = rect.Height;
  }


  public Rectangle() {

  }

  public Rectangle(float x, float y, float width, float height) {
    Set(x, y, width, height);
  }

  public Rectangle(Vector2 point, Vector2 size) {
    _location = new Vector2(point.x, point.y);
    _size = new Vector2(size.x, size.y);
  }

  public Rectangle(Rect rect) {
    Set(rect);
  }

  public bool Contains(Vector2 pt) {
    return ((pt.x >= Left && pt.x <= Right) && (pt.y >= Top && pt.y <= Bottom));
  }

  public bool Contains(Rectangle rect) {
    return (rect.Left >= Left && rect.Right <= Right) && (rect.Top >= Top && rect.Bottom <= Bottom);
  }

  public bool IsIntersected(Rectangle rect) {
    return ((rect.Left <= Left && Left <= rect.Right) || (Left <= rect.Left &&
            rect.Left <= Right)) &&
           ((rect.Top <= Top && Top <= rect.Bottom) || (Top <= rect.Top && rect.Top <= Bottom)) &&
           !((rect.Left.IsGreater(Left) && rect.Right.IsLess(Right)) && (rect.Top.IsGreater(Top) &&
               rect.Bottom.IsLess(Bottom))) &&
           !((Left.IsGreater(rect.Left) && Right.IsLess(rect.Right)) && (Top.IsGreater(rect.Top) &&
               Bottom.IsLess(rect.Bottom)));
  }

  public bool IntersectsWith(Rectangle rect) {
    return (rect.X.IsLess(X + Width)) && (X.IsLess(rect.X + rect.Width)) &&
           (rect.Y.IsLess(Y + Height)) && (Y.IsLess(rect.Y + rect.Height));
  }

  public void Intersect(Rectangle rect1, Rectangle rect2) {
    if (rect1.Left.IsLess(rect2.Left)) {
      Left = rect2.Left;
    }
    else {
      Left = rect1.Left;
    }

    if (rect1.Right.IsLess(rect2.Right)) {
      Width = rect1.Right - Left;
    }
    else {
      Width = rect2.Right - Left;
    }

    if (rect1.Top.IsLess(rect2.Top)) {
      Top = rect2.Top;
    }
    else {
      Top = rect1.Top;
    }

    if (rect1.Bottom.IsLess(rect2.Bottom)) {
      Height = rect1.Bottom - Top;
    }
    else {
      Height = rect2.Bottom - Top;
    }

    if (Width.IsLess(0.0f)) {
      Width = 0.0f;
    }
    if (Height.IsLess(0.0f)) {
      Height = 0.0f;
    }
  }

  public bool IsEqual(Rectangle rect) {
    return MathEx.IsEqual(Top, rect.Top) && MathEx.IsEqual(Left, rect.Left) &&
           MathEx.IsEqual(Right, rect.Right) && MathEx.IsEqual(Bottom, rect.Bottom);
  }

  public bool IsEmpty() {
    return Size.x.IsEqual(0.0f) && Size.y.IsEqual(0.0f);
  }

  public bool IsNotZero() {
    return Size.x.IsGreater(0.0f) && Size.y.IsGreater(0.0f);
  }

  public bool IsNotZero(float threshold) {
    return Size.x.IsGreater(0.0f, threshold) && Size.y.IsGreater(0.0f, threshold);
  }

  public override string ToString() {
    return String.Format("(({0};{1});({2};{3}))", X.ToStr(4), Y.ToStr(4), Width.ToStr(4),
                         Height.ToStr(4));
  }

  public override bool Equals(object o) {
    return base.Equals(o);
  }

  public override int GetHashCode() {
    return base.GetHashCode();
  }

  public static implicit operator Rect(Rectangle rect) {
    var rectRef = new Rect(rect.Left, rect.Top, rect.Width, rect.Height);
    return rectRef;
  }

  public static implicit operator Rectangle(Rect rect) {
    var rectRef = new Rectangle(rect.x, rect.y, rect.width, rect.height);
    return rectRef;
  }

  public static Rectangle Empty {
    get { return new Rectangle(0, 0, 0, 0); }
  }
}
}
