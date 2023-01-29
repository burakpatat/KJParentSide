using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class GData
    {
        public int id;
        public string status;
        public string name;
        public string icon;
        public string description;
        public List<MainBundle> MainBundle;
    }
    [Serializable]
    public class File
    {
        public int id;
        public int AssetBundle_id;
        public string directus_files_id;
    }
    [Serializable]
    public class MainBundle
    {
        public int id;
        public List<string> platformCheck;
        public string scenename;
        public List<File> files;
    }
    [Serializable]
    public class GameClass
    {
        public List<GData> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
