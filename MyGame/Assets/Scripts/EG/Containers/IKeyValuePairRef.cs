using System;
using System.Collections.Generic;

namespace EG.Containers {
public interface IKeyValuePairRef<TKey, TValue> : IBaseGroup, ICloneable {

  TKey Key { get; }

  TValue Value { get; }

  void Set(TKey key, TValue value);

  void Set(TKey key, TValue value, bool isCopyIfPossible);

  void SetKey(TKey key);

  void SetValue(TValue value);

  void SetRef(KeyValuePair<TKey, TValue> pair);
}
}
