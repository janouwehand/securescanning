using System;
using System.Collections.Generic;
using System.Linq;

namespace SecureScan.Base.Extensions
{
  public static class ListExtensions
  {
    public static IEnumerable<List<T>> SplitList<T>(this IEnumerable<T> _source, int sublistsize)
    {
      var source = _source.ToList();

      for (var i = 0; i < source.Count; i += sublistsize)
      {
        yield return source.GetRange(i, Math.Min(sublistsize, source.Count - i));
      }
    }
  }
}
