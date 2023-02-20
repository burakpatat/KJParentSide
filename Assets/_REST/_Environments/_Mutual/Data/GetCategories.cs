using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Proyecto26;
using _Environments._Mutual.Connection;
using _Environments._Mutual.Data.State;

namespace _Environments._Mutual.Data
{
    public class GetCategories : AbstractGetData
    {
        public static int _CloumnCount = 0;
        public static int _PositionCloumnCount = 0;
        public static CategoriesClass CategoriesClass;
        public static IEnumerator GetCategoriesDatas()
        {
            string mainUrl = ConnectionManager.Instance.BaseUrl + Get_SubUrl(GetTarget.BASE, new string[1], "/items/Categories");
            yield return GetResultResponse(GetTarget.BASE, new string[1], mainUrl);
            yield return new WaitUntil(() => _GETResponseResult != "");

            try
            {
                CategoriesClass _datas = new CategoriesClass();
                _datas = JsonUtility.FromJson<CategoriesClass>(_GETResponseResult);
                _CloumnCount = _datas.data.Count;
                CategoriesClass = _datas;
            }
            catch (System.Exception ex)
            {
                Debug.Log("exception:" + ex.Message + ex.InnerException?.Message);
            }
        }
    }
}
