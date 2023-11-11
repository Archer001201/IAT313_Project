using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    [Serializable]
    public class SummaryInfo
    {
        public Vector2 playerAttributes;
        public List<BranchInfo> branches;
    }
}
