using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetProfile: AbstractGetData
    {
        public static ProfileClass ProfileClass;
        public static ChildSettingsClass ChildSettingsClass;
        public static ContentLimitationClass ContentLimitationClass;

        public static bool ContentLimitationIDUpdate = false;
        public static IEnumerator GetProfileDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/Profile?fields=*,Child.*,Child.content_limitation.*,Child.content_limitation.Videos.*,Child.content_limitation.Videos.InteractiveVideo_id.*,Child.content_limitation.Games.*,Child.content_limitation.Games.Games_id.*");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                ProfileClass _datas = new ProfileClass();
                _datas = JsonUtility.FromJson<ProfileClass>(_GETResponseResult);
                ProfileClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static IEnumerator PostProfileDatas(AllStateCRUDModel.Post_Profile_Game _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Content_Limitation_Games?fields=*,Games_id.*");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");

        }
        public static IEnumerator DeleteProfileDatas(AllStateCRUDModel.Delete_Profile_Game _datas, string DeleteID)
        {
            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(_datas));
            string mainUrl = ConnectionManager.Instance.BaseUrl + Delete_SubUrl(DeleteTarget.DELETEBASE, "/items/Content_Limitation_Games/" + DeleteID);
            yield return DeleteData(mainUrl, DeleteTarget.DELETEBASE, _datas);
        }
        public static IEnumerator UserProfileRegister(AllStateCRUDModel.User_RegisterForPostProfile _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Profile");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");
            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK! -> For Profile Collection");
            }
        }
        public static IEnumerator UserProfileRegisterChildSettings(AllStateCRUDModel.User_RegisterForPostProfileChildSettings _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/ChildSettings");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");
            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK! -> For Profile Collection and Child Settings");
            }
        }
        public static IEnumerator GetChildSettingsDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/ChildSettings");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                ChildSettingsClass _datas = new ChildSettingsClass();
                _datas = JsonUtility.FromJson<ChildSettingsClass>(_GETResponseResult);
                ChildSettingsClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static IEnumerator UserProfileRegisterUpdate(AllStateCRUDModel.User_RegisterForPostProfileUpdate _datas, string UpdateID)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Update_SubUrl(UpdateTarget.UPDATEBASE, "/items/Profile/" + UpdateID);
            yield return UpdateData(mainUrl, UpdateTarget.UPDATEBASE, _datas);

            Debug.Log("RegisterOK! -> For Profile Collection Id Updated");
        }
        public static IEnumerator UserProfileRegisterChildSettingsUpdate(AllStateCRUDModel.User_RegisterForPostProfileChildSettingsUpdate _datas, string UpdateID)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Update_SubUrl(UpdateTarget.UPDATEBASE, "/items/ChildSettings/" + UpdateID);
            yield return UpdateData(mainUrl, UpdateTarget.UPDATEBASE, _datas);

            Debug.Log("RegisterOK! -> For Profile Collection and Child Settings for Id Updated");
        }
        public static IEnumerator GetContentLimitationDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/Content_Limitation/");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                ContentLimitationClass _datas = new ContentLimitationClass();
                _datas = JsonUtility.FromJson<ContentLimitationClass>(_GETResponseResult);
                ContentLimitationClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static IEnumerator UserProfileRegisterCreateContentLimitation(AllStateCRUDModel.User_RegisterCreateContentLimitation _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Content_Limitation/");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");
            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK! -> Create Content Limitation");
            }
        }
        public static IEnumerator UserProfileRegisterCreateContentLimitationGames(AllStateCRUDModel.Post_Profile_Game _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Content_Limitation_Games?fields=*,Games_id.*");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");
            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK! -> Create Content Limitation Game");
            }
        }
        public static IEnumerator UserProfileRegisterCreateContentLimitationVideo(AllStateCRUDModel.Post_Profile_Video _datas)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Post_SubUrl(PostTarget.NEWBASE, new string[1], "/items/Content_Limitation_InteractiveVideo?fields=*,InteractiveVideo_id.*");
            yield return PostData(mainUrl, PostTarget.NEWBASE, _datas);
            yield return new WaitUntil(() => _POSTResponseResult != "");
            if (_POSTResponseResult != null)
            {
                Debug.Log("RegisterOK! -> Create Content Limitation Video");
            }
        }
        public static IEnumerator UserProfileRegisterUpdateContentLimitation(AllStateCRUDModel.User_RegisterUpdateContentLimitation _datas, string UpdateID)
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Update_SubUrl(UpdateTarget.UPDATEBASE, "/items/Content_Limitation/" + UpdateID);
            yield return UpdateData(mainUrl, UpdateTarget.UPDATEBASE, _datas);

            ContentLimitationIDUpdate = true;
            Debug.Log("RegisterOK! -> Content Limitation for Id Updated");
        }
    }
}
