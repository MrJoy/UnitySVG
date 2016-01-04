using System.Collections.Generic;

namespace UnitySVG {
  public static class Extensions {
    public static string GetValue<TKey>(this Dictionary<TKey, string> dictionary, TKey key) {
      return dictionary.ContainsKey(key) ? dictionary[key] : string.Empty;
    }
  }
}
