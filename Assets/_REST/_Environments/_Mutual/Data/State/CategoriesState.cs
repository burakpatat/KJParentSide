using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class Categories_Data
    {
        public int id;
        public int? category_id;
        public List<string> base_categories;
        public List<string> sub_categories;
        public string category_title;
    }
    [Serializable]
    public class CategoriesClass
    {
        public List<Categories_Data> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
