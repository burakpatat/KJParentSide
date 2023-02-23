using Newtonsoft.Json;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;

namespace _Environments._Mutual.Data.State
{
    [Serializable]
    public class Video_Data
    {
        public int id;
        public string videofile;
        public string videoname;
        public List<string> isInteractive;
        public string videoThumbnail;
        public string VideoThumbnail_Horizontal;
        public List<Video_Category> category;
        public List<IVideo_Question> questions;
    }
    [Serializable]
    public class Video_CategoriesId
    {
        public List<string> base_categories;
        public List<string> sub_categories;
        public string category_title;
    }
    [Serializable]
    public class Video_Category
    {
        public Video_CategoriesId Categories_id;
    }
    [Serializable]
    public class IVideo_Pos
    {
        public int id;
        public string caseName;
        public string x;
        public string y;
        public string size;
        public bool isAnswer;
        public int vid;
    }
    [Serializable]
    public class IVideo_Question
    {
        public int id;
        public string name;
        public string intime;
        public string outtime;
        public string satime;
        public string firstwrongtime;
        public string secondwrongtime;
        public string truetime;
        public int qid;
        public List<IVideo_Pos> pos;
    }
    [Serializable]
    public class VideoClass
    {
        public List<Video_Data> data;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}
