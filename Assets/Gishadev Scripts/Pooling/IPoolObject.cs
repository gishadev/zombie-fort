using UnityEngine;

namespace gishadev.tools.Pooling
{
    public interface IPoolObject
    {
        string Name { get; }
        int[] InstanceIds { get; }
        GameObject GetPrefab();
    }
}