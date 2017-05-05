using System;
using System.Collections.Generic;

namespace EG.Containers {
[Serializable]
public class KeyValuePairRef<TKey, TValue> : IKeyValuePairRef<TKey, TValue> {

  #region Реализация интерфейса IKeyValuePairRef<TKey, TValue>

  public TKey Key {
    get { return _keyValuePair.Key; }
  }

  public TValue Value {
    get { return _keyValuePair.Value; }
    private set { }
  }

  public void Set(TKey key, TValue value) {
    Set(key, value, true);
  }

  public void Set(TKey key, TValue value, bool isCopyIfPossible) {
    if (isCopyIfPossible) {
      if (key is ICloneable && value is ICloneable) {
        _keyValuePair = new KeyValuePair<TKey, TValue>((TKey)(key as ICloneable).Clone(),
            (TValue)(value as ICloneable).Clone());
        return;
      }
      if (key is ICloneable) {
        _keyValuePair = new KeyValuePair<TKey, TValue>((TKey)(key as ICloneable).Clone(), value);
        return;
      }
      if (value is ICloneable) {
        _keyValuePair = new KeyValuePair<TKey, TValue>(key, (TValue)(value as ICloneable).Clone());
        return;
      }
    }
    _keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
  }

  public void SetKey(TKey key) {
    TValue value = _keyValuePair.Value;
    if (key is ICloneable) {
      _keyValuePair = new KeyValuePair<TKey, TValue>((TKey)(key as ICloneable).Clone(), value);
      return;
    }
    _keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
  }

  public void SetValue(TValue value) {
    TKey key = _keyValuePair.Key;
    if (value is ICloneable) {
      _keyValuePair = new KeyValuePair<TKey, TValue>(key, (TValue)(value as ICloneable).Clone());
      return;
    }
    _keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
  }

  public void SetRef(KeyValuePair<TKey, TValue> pair) {
    _keyValuePair = pair;
  }

  #endregion

  #region Реализация интерфейса ICloneable

  public object Clone() {
    return new KeyValuePairRef<TKey, TValue>(Key, Value);
  }

  #endregion

  public KeyValuePairRef() {

  }

  public KeyValuePairRef(TKey key, TValue value) {
    Set(key, value);
  }

  public KeyValuePairRef(TKey key, TValue value, bool isCopyIfPossible) {
    Set(key, value, isCopyIfPossible);
  }

  public KeyValuePairRef(KeyValuePair<TKey, TValue> pair, bool isLinked,
                         bool isCopyIfPossible = true) {
    if (isLinked) {
      SetRef(pair);
    }
    else {
      Set(pair.Key, pair.Value, isCopyIfPossible);
    }
  }

  public static implicit operator KeyValuePair<TKey, TValue>(KeyValuePairRef<TKey, TValue>
      keyValuePairRef) {
    return keyValuePairRef._keyValuePair;
  }

  public static implicit operator KeyValuePairRef<TKey, TValue>(KeyValuePair<TKey, TValue>
      _keyValuePair) {
    return new KeyValuePairRef<TKey, TValue>(_keyValuePair.Key, _keyValuePair.Value);
  }

  public override string ToString() {
    return String.Format("{0},{1}", _keyValuePair.Key, _keyValuePair.Value);
  }

  private KeyValuePair<TKey, TValue> _keyValuePair;
}
}
