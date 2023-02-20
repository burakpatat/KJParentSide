using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System.Linq;

public class ParentManager : MonoBehaviour
{
    bool LoadingOKForLodingPanel = false;

    [Header("UI Transactions")]
    public Transform MenuPanel;
    public Transform GameCategories;
    public Transform VideoCategories;
    public Transform GameList;
    public Transform VideoList;
    public Transform GameDetails;
    public Transform VideoDetails;
    public Transform QRPanel;
    public Transform UserGuide;

    public Transform ProfileAvatar;
    public TMP_Text KJIDText;
    public TMP_Text ParentName;
    public TMP_Text ChildName;

    [Header("PosterButton")]
    public GameObject PosterButton;
    public Transform VideoCategoriesPosterButtonTransform;
    public Transform GameCategoriesPosterButtonTransform;
    [SerializeField] private List<Transform> VCP_List;
    public Transform VideoListPosterButtonTransform;
    public Transform GameListPosterButtonTransform;
    bool PosterButtonSpawnOK = false;

    [Header("DATAS")]
    private List<Categories_Data> _CategoryDatas = new List<Categories_Data>();
    private List<Video_Data> _VideoDatas = new List<Video_Data>();
    private List<GData> _GameDatas = new List<GData>();
    public string CategoryClickName;

    private string _cachePath;
    private void Start()
    {
        _cachePath = Application.persistentDataPath + "/Posters";
    }
    private void Update()
    {
        if (ConnectionManager.Instance.BaseLoadedOK && ConnectionManager.Instance.AuthName == ConnectionManager.Instance.LoginName)
        {
            ConnectionManager.Instance.BaseLoadedOK = false;

            KJIDText.text = ConnectionManager.Instance.AuthID_KJ;
            ParentName.text = ConnectionManager.Instance.AuthName;
            ChildName.text = ConnectionManager.Instance.ChildsName[0];

            //image
            StartCoroutine(SetUserAvatar(GetUser.GetMedia() + ConnectionManager.Instance.Avatar, ConnectionManager.Instance.ChildsName[0], ProfileAvatar));

            //Categories
            _VideoDatas = GetVideo.VideoClass.data;

            if (PosterButtonSpawnOK == false)
            {
                PosterButtonSpawnOK = true;
                var ControlSub = 0;
                for (int i = 0; i < _VideoDatas.Count; i++)
                {
                    if(_VideoDatas[i].category[0].Categories_id.base_categories[0] == "video")
                    {
                        ControlSub = _VideoDatas.Where(a => a.category[0].Categories_id.sub_categories[0] == _VideoDatas[i].category[0].Categories_id.sub_categories[0]).Count();
                        if (ControlSub > 1)
                        {
                            ControlSub = 1;
                        }
                    }
                }
                for (int control = 0; control < ControlSub; control++)
                {
                    var VideoCategoryPosterButton = Instantiate(PosterButton, Vector3.zero, Quaternion.identity) as GameObject;
                    VideoCategoryPosterButton.transform.SetParent(VideoCategoriesPosterButtonTransform);
                    VideoCategoryPosterButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(197f, -324, 0);
                    VideoCategoryPosterButton.transform.localScale = Vector3.one;

                    //Data Write
                    VideoCategoryPosterButton.transform.GetChild(1).GetComponent<TMP_Text>().text = _VideoDatas[control].category[0].Categories_id.sub_categories[0];
                    VideoCategoryPosterButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        VideoCategoryClick(_VideoDatas[control].category[0].Categories_id.sub_categories[0]);
                    });

                    int c = 0, t = 1;
                    foreach (Transform VideoPosterButtonChilditem in VideoCategoriesPosterButtonTransform)
                    {
                        if (VideoPosterButtonChilditem.GetSiblingIndex() % 2 == 0)
                        {
                            VideoPosterButtonChilditem.gameObject.name = "VideoCategoryPosterButton" + c.ToString();
                            c += 2;
                        }
                        else
                        {
                            VideoPosterButtonChilditem.gameObject.name = "VideoCategoryPosterButton" + t.ToString();
                            t += 2;
                        }
                    }
                    VCP_List.Add(VideoCategoryPosterButton.transform);
                }
            }
            if(PosterButtonSpawnOK == true)
            {
                int c = 0, t = 1; int Ypos = -324;

                for (int l = 0; l < VCP_List.Count; l++)
                {
                    int PosterLindex = VCP_List.FindIndex(a => a.gameObject.name == "VideoCategoryPosterButton" + c.ToString());
                    int PosterRindex = VCP_List.FindIndex(a => a.gameObject.name == "VideoCategoryPosterButton" + t.ToString());

                    if (PosterRindex >= VCP_List.Count - 1)
                        PosterLindex = VCP_List.Count - 2;

                    if (l == 0 || l == 1)
                    {
                        VCP_List[PosterLindex].GetComponent<RectTransform>().anchoredPosition = new Vector3(197f,
                          -324, 0);
                        VCP_List[PosterRindex].GetComponent<RectTransform>().anchoredPosition = new Vector3(628f,
                                    -324, 0);
                    }
                    else
                    {
                        VCP_List[PosterLindex].GetComponent<RectTransform>().anchoredPosition = new Vector3(197f,
                               (Ypos), 0);
                        VCP_List[PosterRindex].GetComponent<RectTransform>().anchoredPosition = new Vector3(628f,
                                    (Ypos), 0);
                    }

                    if (l > 1 && PosterRindex < VCP_List.Count - 1)
                    {
                        c += 2; t += 2;
                        Ypos += -672;
                    }

                }

                PosterButtonSpawnOK = false;
            }

        }
        if (LoadingOKForLodingPanel == true)
        {
            LoadingOKForLodingPanel = false;

            DataLoading.Instance.HideLoading();
        }
    }
    void VideoCategoryClick(string _categoryName)
    {
        CategoryClickName = _categoryName;

        VideoListOpen();

        DataLoading.Instance.ReOp();
    }
    public void GameCategoryOpen()
    {
        GameCategories.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
    }
    public void VideoCategoryOpen()
    {
        VideoCategories.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
    }
    public void GameListOpen()
    {
        GameList.gameObject.SetActive(true);
        GameCategories.gameObject.SetActive(false);
    }
    public void VideoListOpen()
    {
        VideoList.gameObject.SetActive(true);
        VideoCategories.gameObject.SetActive(false);
    }
    public void GameDetailsOpen()
    {
        GameDetails.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
    }
    public void VideoDetailsOpen()
    {
        VideoDetails.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
    }
    public IEnumerator SetUserAvatar(string url, string name, Transform _intanceObj)
    {
        if (!Directory.Exists(_cachePath))
        {
            Debug.Log("No directory found for temporary files. Creating one.");
            Directory.CreateDirectory(_cachePath);
        }

        string posterPath = _cachePath + "/" + DataTypeExtensions.RemoveDigits(name).ToLower() + "Avatar.poster";
        bool valid = true;
        Texture2D _texture = new Texture2D(1080, 1920);

        if (System.IO.File.Exists(posterPath))
        {
            Debug.Log("Game visual exist :  " + posterPath);
            System.TimeSpan since = System.DateTime.Now.Subtract(System.IO.File.GetLastWriteTime(posterPath));
            if (since.Days >= 1)
            {
                valid = false;
                Debug.Log("But visual is old :  " + since.Days + " days");
            }
        }
        else
        {
            Debug.Log("Game visual does not exist :  " + posterPath);
            valid = false;
        }

        if (!valid)
        {
            Debug.Log("Renewing cached visual for game " + name);
            UnityWebRequest trq = UnityWebRequest.Get(url);
            yield return trq.SendWebRequest();
            if (trq.result == UnityWebRequest.Result.Success)
            {
                byte[] result = trq.downloadHandler.data;
                System.IO.File.WriteAllBytes(posterPath, result);
                valid = true;
            }

        }

        if (valid)
        {
            _texture.LoadImage(System.IO.File.ReadAllBytes(posterPath));

            Rect rec = new Rect(0, 0, _texture.width, _texture.height);
            _intanceObj.transform.GetComponent<Image>().sprite = Sprite.Create(_texture, rec, new Vector2(0, 0), 1);

            yield return new WaitForSeconds(.5f);
            LoadingOKForLodingPanel = true;
        }

        yield return null;
    }
}


//tekliler x 197 y ba�lang�. -324 + -672 = -996
//�iftliler x 628 y ba�lang�. -324 + -672 = -996