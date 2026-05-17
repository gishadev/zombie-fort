using UnityEngine;

namespace gishadev.tools.Effects
{
    public interface IPoolEmitter
    {
        GameObject EmitAt(int index, Vector3 position, Quaternion rotation);
    }
}