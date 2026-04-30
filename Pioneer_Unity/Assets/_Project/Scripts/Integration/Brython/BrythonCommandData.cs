using System.Collections.Generic;

namespace Pioneer.Integration.Brython
{
    [System.Serializable]
    public class BrythonCommandData
    {
        public string action;
        public float value;
    }

    [System.Serializable]
    public class BrythonResponseData
    {
        public List<BrythonCommandData> commands;
        public string error;
    }
}