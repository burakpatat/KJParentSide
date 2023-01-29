using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetGame : AbstractGetData
    {
        public static int _CloumnCount = 0;
        public static GameClass GameClass;
        public static IEnumerator GetGameDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/Games?fields=*,MainBundle.*,MainBundle.files.*");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                GameClass _datas = new GameClass();
                _datas = JsonUtility.FromJson<GameClass>(_GETResponseResult);
                _CloumnCount = _datas.data.Count;
                GameClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
        public static string GetMedia()
        {
            return ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.FILE, new string[1], "");
        }
    }
}
