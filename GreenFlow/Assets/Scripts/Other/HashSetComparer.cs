using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
class HashSetComparer<T> : IEqualityComparer<HashSet<T>>
{
    public bool Equals(HashSet<T>? x, HashSet<T>? y)
    {
        if (x == null || y == null)
            return x == y;
        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<T> obj)
    {
        int hash = 0;
        foreach (var item in obj.OrderBy(e => e)) // Ensure consistent ordering
        {
            hash ^= item!.GetHashCode(); // XOR for commutative hashing
        }
        return hash;
    }
}