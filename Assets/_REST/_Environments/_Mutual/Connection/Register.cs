using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Connection
{
    public class Register : MonoBehaviour
    {
        static readonly string Role = "b1090449-00be-491f-a27f-25ed62dd3eae";
        static readonly string[] AvatarFileName = new string[8] 
        {
            "4f542625-f184-47e5-9daa-7b8d8479a325",
            "973fef97-51d7-4654-9d4f-7a877e45eb58",
            "640343bb-5539-4dfd-aaa0-867d5c0d6da9",
            "990dda5a-b0b6-40b4-99fe-15e4456f8c84",
            "9be4d960-4c65-4b41-9219-06fc39866b6b",
            "f15e0114-4036-40b0-936e-fb6d987ef474",
            "9c4a0260-17e7-4949-9e11-59ed2fb60826",
            "e25e9a9f-c13c-469d-a019-232c641249c1"
        };

        static int _ContentLimitationID;
        public static IEnumerator UserRegister(string username, string mail, string password)
        {
            AllStateCRUDModel.User_Register _Userdatas = new AllStateCRUDModel.User_Register();

            AllStateCRUDModel.User_RegisterForPostProfile _Profiledatas = new AllStateCRUDModel.User_RegisterForPostProfile();
            AllStateCRUDModel.User_RegisterForPostProfileUpdate _ProfiledatasUpdate = new AllStateCRUDModel.User_RegisterForPostProfileUpdate();

            AllStateCRUDModel.User_RegisterForPostProfileChildSettings _ProfiledatasChildSettings = new AllStateCRUDModel.User_RegisterForPostProfileChildSettings();
            AllStateCRUDModel.User_RegisterForPostProfileChildSettingsUpdate _ProfiledatasChildSettingsUpdate = new AllStateCRUDModel.User_RegisterForPostProfileChildSettingsUpdate();

            _Userdatas.first_name = username;
            _Userdatas.email = mail;
            _Userdatas.password = password;
            _Userdatas.role = Role;

            int r = Random.Range(0, AvatarFileName.Length);
            _Userdatas.avatar = AvatarFileName[r];

            _Profiledatas.name = username;
            _Profiledatas.KJId = Profile.Instance.guestUUID;
            
            yield return GetUser.Register(_Userdatas);
            yield return GetProfile.UserProfileRegister(_Profiledatas);

            System.Guid ChildNameUUID = System.Guid.NewGuid();
            _ProfiledatasChildSettings.childname = ChildNameUUID.ToString();
            yield return GetProfile.UserProfileRegisterChildSettings(_ProfiledatasChildSettings);

            yield return RegisterCreateContentLimitation(ChildNameUUID.ToString());

            yield return new WaitForSeconds(1.2f);

            // ------- ALL Relation Settings ------

            string _myKJId = Profile.Instance.guestUUID;
            int _myProfileId = 0;

            yield return GetProfile.GetProfileDatas();
            ConnectionManager.Instance._profiledatas = GetProfile.ProfileClass.data;
            foreach (var prow in ConnectionManager.Instance._profiledatas)
            {
                if (prow.KJId == _myKJId)
                {
                    Debug.Log("Founded KJID...!");
                    _myProfileId = prow.id;
                    print("Register My Profile Id : " + _myProfileId);
                }
            }

            // ------- User -> Profile Relation (duid) -------

            yield return GetUser.GetUserDatas();
            ConnectionManager.Instance._userdatas = GetUser.UserClass.data;

            foreach (var item in ConnectionManager.Instance._userdatas)
            {
                if(item.first_name == username)
                {
                    //**** User -> Profile
                    _ProfiledatasUpdate.duid = item.id;
                }
            }
            yield return GetProfile.UserProfileRegisterUpdate(_ProfiledatasUpdate, _myProfileId.ToString());

            // ------- Child Settings Relation -------

            yield return GetProfile.GetChildSettingsDatas();
            ConnectionManager.Instance._childSettingsDatas = GetProfile.ChildSettingsClass.data;
            yield return GetProfile.GetContentLimitationDatas();
            ConnectionManager.Instance._contentLimitationDatas = GetProfile.ContentLimitationClass.data;

            if (GetUser.UserCreate == true)
            {
                string _ChildID_InChildSettings = "";
                
                foreach (var item in ConnectionManager.Instance._childSettingsDatas)
                {
                    if (item.childname == ChildNameUUID.ToString())
                    {
                        Debug.Log("ChildNameOK...!");
                        _ChildID_InChildSettings = item.id.ToString();
                        _ProfiledatasChildSettingsUpdate.cid = _myProfileId;
                        _ProfiledatasChildSettingsUpdate.childname = username + "sChild";
                    }
                }
                yield return GetProfile.UserProfileRegisterChildSettingsUpdate(_ProfiledatasChildSettingsUpdate, _ChildID_InChildSettings);

                // ------- Content Limitation Relation -------

                string _ContentLimitationID = "";
                foreach (var item in ConnectionManager.Instance._contentLimitationDatas)
                {
                    if (item.findId == ChildNameUUID.ToString())
                    {
                        Debug.Log("FindIdOK...!");
                        _ContentLimitationID = item.id.ToString();
                    }
                }
                yield return RegisterUpdateContentLimitation(_ChildID_InChildSettings.ToInt32(), _ContentLimitationID);
            }
        }
        private static IEnumerator RegisterCreateContentLimitation(string _findUUID)
        {
            AllStateCRUDModel.User_RegisterCreateContentLimitation _createContentLimitation = new AllStateCRUDModel.User_RegisterCreateContentLimitation();

            _createContentLimitation.findId = _findUUID;

            yield return GetProfile.UserProfileRegisterCreateContentLimitation(_createContentLimitation);
           
        }
        private static IEnumerator RegisterUpdateContentLimitation(int _childSettingsId, string _updateID)
        {
            AllStateCRUDModel.User_RegisterUpdateContentLimitation _updateContentLimitation = new AllStateCRUDModel.User_RegisterUpdateContentLimitation();

            _updateContentLimitation.contentId = _childSettingsId;

            yield return GetProfile.UserProfileRegisterUpdateContentLimitation(_updateContentLimitation, _updateID);

            AllStateCRUDModel.Post_Profile_Game _postGamedatas = new AllStateCRUDModel.Post_Profile_Game();
            AllStateCRUDModel.Post_Profile_Video _postVideodatas = new AllStateCRUDModel.Post_Profile_Video();

            _postGamedatas.Content_Limitation_id = _updateID.ToInt32();
            _postGamedatas.Games_id = ConnectionManager.Instance._Gamedatas[0];
            _postVideodatas.Content_Limitation_id = _updateID.ToInt32();
            _postVideodatas.InteractiveVideo_id = ConnectionManager.Instance._Videodatas[0];

            yield return GetProfile.UserProfileRegisterCreateContentLimitationGames(_postGamedatas);
            yield return GetProfile.UserProfileRegisterCreateContentLimitationVideo(_postVideodatas);
        }
    }
}
