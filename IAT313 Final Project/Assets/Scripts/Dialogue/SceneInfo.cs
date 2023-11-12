using System;
using System.Collections.Generic;

namespace Dialogue
{
    [Serializable]
    public class SceneInfo
    {
        public string sceneName;
        public List<EventInfo> events;
        public List<NpcInfo> npcs;
    }
}
