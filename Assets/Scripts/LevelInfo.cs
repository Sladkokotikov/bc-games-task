using System;
using Dreamteck.Splines;
using UnityEngine;

namespace DrawAndRun
{
    [Serializable]
    public class LevelInfo
    {
        public Texture2D texture;
        public int startBuddyCount;
        public SplineComputer computer;
    }
}