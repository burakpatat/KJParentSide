using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class User_Child
    {
        public int id;
        public string childname;
        public List<User_ContentLimitation> content_limitation;
    }
    [Serializable]
    public class User_ContentLimitation
    {
        public List<User_Video> Videos;
        public List<User_Game> Games;
    }
    [Serializable]
    public class User_Data
    {
        public string id;
        public string first_name;
        public string email;
        public string password;
        public string avatar;
        public object language;
        public string status;
        public string role;
        public string token;
        public List<User_Profile> profile;
    }
    [Serializable]
    public class User_Game
    {
        public User_GamesId Games_id;
    }
    [Serializable]
    public class User_GamesId
    {
        public int id;
        public string name;
    }
    [Serializable]
    public class User_InteractiveVideoId
    {
        public int id;
        public string videoname;
    }
    [Serializable]
    public class User_Profile
    {
        public int id;
        public string KJId;
        public string name;
        public List<User_Child> Child;
    }
    [Serializable]
    public class UserClass
    {
        public List<User_Data> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
    [Serializable]
    public class User_Video
    {
        public User_InteractiveVideoId InteractiveVideo_id;
    }
    [Serializable]
    public class User_TokenData
    {
        public string access_token;
        public int expires;
        public string refresh_token;
    }
    [Serializable]
    public class UserTokenClass
    {
        public User_TokenData data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
