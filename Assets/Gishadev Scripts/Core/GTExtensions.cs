using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gishadev.tools.Core
{
    public static class GTExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            // Attempt to get component from GameObject
            T retrievedComp = obj.GetComponent<T>();

            if (retrievedComp != null)
                return retrievedComp;

            // This component wasn't found on the object, so add it.
            return obj.AddComponent<T>();
        }
        
        public static T GetNextValue<T>(this IEnumerable<T> source, int currentIndex)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (currentIndex < 0)
                throw new ArgumentOutOfRangeException("currentIndex", "Index is out of range.");

            List<T> list = source.ToList();
            if (list.Count == 0)
                throw new ArgumentException("The source collection is empty.");

            int nextIndex = (currentIndex + 1) % list.Count;
            return list[nextIndex];
        }
    }
}