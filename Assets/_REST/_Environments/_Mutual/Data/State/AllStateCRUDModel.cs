using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class AllStateCRUDModel
    {
        [Serializable]
        public class User_Login
        {
            public string email;
            public string password;
        }
        [Serializable]
        public class User_Register
        {
            public string id;
            public string first_name;
            public string email;
            public string password;
            public string avatar;
            public string role;
        }
        [Serializable]
        public class User_RegisterForPostProfile
        {
            public string name;
            public string KJId;
        }
        [Serializable]
        public class User_RegisterForPostProfileUpdate
        {
            public string duid;
        }
        [Serializable]
        public class User_RegisterForPostProfileChildSettings
        {
            public string childname;
        }
        [Serializable]
        public class User_RegisterForPostProfileChildSettingsUpdate
        {
            public int cid;
            public string childname;
        }
        [Serializable]
        public class User_RegisterCreateContentLimitation
        {
            public string findId;
        }
        [Serializable]
        public class User_RegisterUpdateContentLimitation
        {
            public int contentId;
        }
        [Serializable]
        public class Post_Profile_Game
        {
            public int Content_Limitation_id;
            public GData Games_id;
        }
        [Serializable]
        public class Post_Profile_Video
        {
            public int Content_Limitation_id;
            public Video_Data InteractiveVideo_id;
        }
        [Serializable]
        public class Delete_Profile_Game
        {
            public int id;
        }
    }
}
