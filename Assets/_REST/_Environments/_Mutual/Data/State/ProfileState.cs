using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class Profile_Child
    {
        public int id;
        public string childname;
        public List<ContentLimitation> content_limitation;
    }
    [Serializable]
    public class ContentLimitation
    {
        public int id;
        public int contentId;
        public List<Profile_Video> Videos;
        public List<Profile_Game> Games;
    }
    [Serializable]
    public class Profile_Data
    {
        public int id;
        public string KJId;
        public string name;
        public List<Profile_Child> Child;
    }
    [Serializable]
    public class Profile_Game
    {
        public int id;
        public Profile_GamesId Games_id;
    }
    [Serializable]
    public class Profile_GamesId
    {
        public int id;
        public string status;
        public string name;
    }
    [Serializable]
    public class Profile_InteractiveVideoId
    {
        public int id;
        public string videoname;
    }
    [Serializable]
    public class Profile_Video
    {
        public int id;
        public Profile_InteractiveVideoId InteractiveVideo_id;
    }
    [Serializable]
    public class ProfileClass
    {
        public List<Profile_Data> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }

    //Child Settings
    [Serializable]
    public class ChildSettingsData
    {
        public int id;
        public int cid;
        public string childname;
    }
    [Serializable]
    public class ChildSettingsClass
    {
        public List<ChildSettingsData> data;
    }
    //Content Limitation
    [Serializable]
    public class ContentLimitationData
    {
        public int id;
        public int contentId;
        public string findId;
    }
    [Serializable]
    public class ContentLimitationClass
    {
        public List<ContentLimitationData> data;
    }
}
