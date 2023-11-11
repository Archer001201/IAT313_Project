using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class BranchInfo
    {
        public int[] conditionStudy;
        public int[] conditionLove;
        [TextArea] public string branch;
        public string nextLevel;
    }
}
