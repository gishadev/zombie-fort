using System;

namespace gishadev.fort.Level
{
    public interface IIslandLevelController
    {
        int CurrentLevel { get; }
        event Action<int> LevelChanged;
    }
}