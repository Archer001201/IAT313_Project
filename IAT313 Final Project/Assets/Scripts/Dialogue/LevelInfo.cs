using System;
using System.Collections.Generic;

namespace Dialogue
{
    [Serializable]
    public class LevelInfo
    {
        public string levelID;
        public List<SceneInfo> scenes;
        public string summaryFile;
        public string audioFile;
    }
}
