using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetUser : AbstractGetData
    {
        public static UserClass UserClass;
        public static UserTokenClass UserTokenClass;
        public static bool LoginOK = false;
        public static bool UserCreate = false;

        public static IEnumerator GetUserDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/users?fields=*,profile.*,*,profile.Child.*," +
                "*profile.Child.content_limitation.Videos.InteractiveVideo_id.*,*,profile.Child.content_limitation.Games.Games_id.*");

            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                UserClass _datas = new UserClass();
                _datas = JsonUtility.FromJson<UserClass>(_GETResponseResult);
                UserClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static IEnumerator Login(AllStateCRUDModel.User_Login _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/auth/login");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");

            if (_POSTResponseResult != null)
            {
                LoginOK = true;
                Debug.Log("LoginOK!");
                UserTokenClass _Tdatas = new UserTokenClass();
                _Tdatas = JsonUtility.FromJson<UserTokenClass>(_POSTResponseResult);
                UserTokenClass = _Tdatas;
            }
            
        }
        public static IEnumerator Register(AllStateCRUDModel.User_Register _Userdatas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/users");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _Userdatas);
            yield return new WaitUntil(() => _POSTResponseResult != "");

            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK!");
                UserCreate = true;
            }

        }
        public static string GetMedia()
        {
            return ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.FILE, new string[1], "");
        }
    }
}
