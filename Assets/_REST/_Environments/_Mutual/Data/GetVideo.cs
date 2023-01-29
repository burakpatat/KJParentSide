using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetVideo : AbstractGetData
    {
        public static int _CloumnCount = 0;
        public static int _PositionCloumnCount = 0;
        public static VideoClass VideoClass;
        public static IEnumerator GetVideoDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/InteractiveVideo?fields=*,questions.*,questions.pos.*,category.Categories_id.*");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                VideoClass _datas = new VideoClass();
                _datas = JsonUtility.FromJson<VideoClass>(_GETResponseResult);
                _CloumnCount = _datas.data.Count;
                VideoClass = _datas;
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
