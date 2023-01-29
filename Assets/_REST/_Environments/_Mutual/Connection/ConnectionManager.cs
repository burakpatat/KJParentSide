using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using System;
using System.Linq;
using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Local;

namespace _Environments._Mutual.Connection
{
    public class ConnectionManager : MonoBehaviour
    {
        #region Singleton

        public static ConnectionManager Instance;
        void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        #endregion

        [SerializeField]
        private string _BaseUrl = "";

        [SerializeField]
        private string _GetToken;

        [SerializeField]
        private string _ActiveToken;

        [Header("Auth Info")]
        [SerializeField]
        private string _AuthID;
        [SerializeField]
        private string _AuthName;
        [SerializeField]
        private string _ParentName;
        [SerializeField]
        private List<string> _ChildsName;
        [SerializeField]
        private string _Avatar;

        [HideInInspector] public List<User_Data> _userdatas = new List<User_Data>();
        [HideInInspector] public List<Profile_Data> _profiledatas = new List<Profile_Data>();

        //other
        [HideInInspector] public List<ChildSettingsData> _childSettingsDatas = new List<ChildSettingsData>();
        [HideInInspector] public List<ContentLimitationData> _contentLimitationDatas = new List<ContentLimitationData>();
        [HideInInspector] public List<GData> _Gamedatas = new List<GData>();
        [HideInInspector] public List<Video_Data> _Videodatas = new List<Video_Data>();

        public string BaseUrl { get => _BaseUrl; set => _BaseUrl = value; }
        public string ActiveToken { get => _ActiveToken; set => _ActiveToken = value; }

        public string AuthID_KJ { get => _AuthID; set => _AuthID = value; }
        public string AuthName { get => _AuthName; set => _AuthName = value; }
        public string ParentName { get => _ParentName; set => _ParentName = value; }
        public List<string> ChildsName { get => _ChildsName; set => _ChildsName = value; }
        public string Avatar { get => _Avatar; set => _Avatar = value; }

        public bool BaseLoadedOK = false;

        [Header("TOKEN CREATE")]
        public bool GuestLoginTokenCreated = false;
        public bool UserLoginTokenCreated = false;
        public string LoginName = "";

        public bool GeneralUserLogin = false;
        public void setBaseUrl()
        {
            String mainUrl = String.Empty;
#if TEST
            mainUrl = "https://kidzjungle.directus.app";
#elif PROD
            mainUrl = "https://api.kidzjungle.com/kidsvid";
#endif
#if UNITY_EDITOR
            BaseUrl = mainUrl;
            Debug.Log("EDITOR VERSION STARTED");
            Debug.Log("Caching size : " + Caching.cacheCount + " - " + Caching.ready);
            AssetBundle.UnloadAllAssetBundles(false);
            bool cleared = Caching.ClearCache();
            Debug.Log("Cache cleared : " + cleared);
#elif UNITY_ANDROID
            Debug.Log("Android PLATFORM VERSION STARTED");
            Debug.Log("Caching size : " + Caching.cacheCount + " - " + Caching.ready);
            AssetBundle.UnloadAllAssetBundles(false);
            bool cleared = Caching.ClearCache();
            Debug.Log("Cache cleared : " + cleared);
            BaseUrl = mainUrl;
#endif
        }
        private void Awake()
        {
            Singleton();

            StartCoroutine(WebServices());
        }
        IEnumerator WebServices()
        {
            setBaseUrl();

            yield return auth();
            //OTHER DMO
            if (GeneralUserLogin)
            {
                yield return DMO();
            }

            BaseLoadedOK = true;
            Debug.Log("Base Loaded");
        }
        IEnumerator auth()
        {
            if (Login.TOKENCreated)
            {
                ActiveToken = Login.LoginToken;

                yield return new WaitForSeconds(.2f);

                yield return GetUser.GetUserDatas();
                _userdatas = GetUser.UserClass.data;
                if (_userdatas != null)
                {
                    GeneralUserLogin = true;

                    foreach (var item in _userdatas)
                    {
                        if(item.email == Login.LoginMail)
                        {
                            LoginName = item.first_name;
                        }
                    }
                }

                if (GeneralUserLogin)
                {
                    authInfo();
                }
            }
        }
        void authInfo()
        {
            foreach (var row in _userdatas)
            {
                if (Profile.Instance.guestProfileRegister == false)
                {
                    for (int i = 0; i < row.profile.Count; i++)
                    {
                        if (row.profile[i].name == LoginName)
                        {
                            AuthID_KJ = row.profile[i].KJId;
                            AuthName = row.profile[i].name;
                            ParentName = row.profile[i].name;
                            Avatar = row.avatar;

                            for (int a = 0; a < row.profile[i].Child.Count; a++)
                            {
                                ChildsName.Add(row.profile[i].Child[a].childname);
                            }
                        }
                    }
                }
            }
        }
        IEnumerator DMO()
        {
            yield return GetGame.GetGameDatas();
            _Gamedatas = GetGame.GameClass.data;
            yield return GetVideo.GetVideoDatas();
            _Videodatas = GetVideo.VideoClass.data;

            yield return GetProfile.GetProfileDatas();
            _profiledatas = GetProfile.ProfileClass.data;
        }
    }
}


