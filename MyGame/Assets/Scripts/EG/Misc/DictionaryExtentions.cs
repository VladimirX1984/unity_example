using System.Collections.Generic;
using System.Linq;

namespace EG.Misc {
public static class DictionaryExtentions {
  public static void Insert<TKey, TValue>(this IDictionary<TKey, TValue> dict, int index, TKey key,
                                          TValue value) {
    var objects = dict.ToList();
    objects.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
    dict.Clear();
    foreach (var obj in objects) {
      dict.Add(obj.Key, obj.Value);
    }
  }
}
}
