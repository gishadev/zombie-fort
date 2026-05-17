using System;

namespace gishadev.fort.Level
{
    public interface IIslandLevelController
    {
        int CurrentLevel { get; }
        float CurrentProgress { get; }
        event Action<int> LevelChanged;
        event Action<float> ProgressChanged;
    }
}