using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System;

namespace _Environments._Mutual.Connection
{
    public class Login : MonoBehaviour
    {
        public static string LoginToken ="";

        public static bool TOKENCreated = false;

        public static bool FirstOpen = false;

        public static string LoginMail;
        private void OnEnable()
        {
            
        }
        public static IEnumerator GuestUserLogin()
        {
            AllStateCRUDModel.User_Login _datas = new AllStateCRUDModel.User_Login();

            _datas.email = "guest@mtekbilisim.com";
            _datas.password = "guest@mtek";

            LoginMail = "guest@mtekbilisim.com";

            yield return GetUser.Login(_datas);

            LoginToken = GetUser.UserTokenClass.data.access_token;

            ConnectionManager.Instance.ActiveToken = LoginToken;
            ConnectionManager.Instance.GuestLoginTokenCreated = true;

            TOKENCreated = true;

        }
        public static IEnumerator UserLogin(string _mailIF, string _passwordIF)
        {
            AllStateCRUDModel.User_Login _datas = new AllStateCRUDModel.User_Login();

            _datas.email = _mailIF;
            _datas.password = _passwordIF;

            LoginMail = _mailIF;

            yield return GetUser.Login(_datas);

            LoginToken = GetUser.UserTokenClass.data.access_token;

            ConnectionManager.Instance.ActiveToken = LoginToken;
            ConnectionManager.Instance.UserLoginTokenCreated = true;

            TOKENCreated = true;

        }

        private void OnApplicationQuit()
        {
            LoginToken = "";
            LoginMail = "";
            TOKENCreated = false;
        }
    }
}
