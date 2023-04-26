using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettingsData
{
    public long lastUpdated;

    // General
    bool useMinimap;

    // Audio

    // Video


    public GameSettingsData()
    {
        useMinimap = true;
    }
}
