using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;

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
    [SerializeField] private List<Transform> VCP_LeftSide;
    [SerializeField] private List<Transform> VCP_RightSide;
    public Transform VideoListPosterButtonTransform;
    public Transform GameListPosterButtonTransform;
    bool PosterButtonSpawnOK = false;

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

            if(PosterButtonSpawnOK == false)
            {
                PosterButtonSpawnOK = true;

                for (int i = 0; i < 20; i++)
                {
                    var VideoCategoryPosterButton = Instantiate(PosterButton, Vector3.zero, Quaternion.identity) as GameObject;
                    VideoCategoryPosterButton.transform.SetParent(VideoCategoriesPosterButtonTransform);
                    VideoCategoryPosterButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(197f, -324, 0);
                    VideoCategoryPosterButton.transform.localScale = Vector3.one;

                    foreach (Transform VideoPosterButtonChilditem in VideoCategoriesPosterButtonTransform)
                    {
                        if (VideoPosterButtonChilditem.GetSiblingIndex() % 2 == 0)
                        {
                            VideoPosterButtonChilditem.gameObject.name = "VideoCategoryPosterButton0";
                        }
                        else
                        {
                            VideoPosterButtonChilditem.gameObject.name = "VideoCategoryPosterButton1";
                        }
                    }
                    VCP_LeftSide.Add(VideoCategoryPosterButton.transform);

                }
            }
            if(PosterButtonSpawnOK == true)
            {
                for (int l = 0; l < VCP_LeftSide.Count; l++)
                {
                    int index = VCP_LeftSide.FindIndex(a => a.gameObject.name == "VideoCategoryPosterButton0");
                    int index1 = VCP_LeftSide.FindIndex(a => a.gameObject.name == "VideoCategoryPosterButton1");

                    VCP_LeftSide[index].GetComponent<RectTransform>().anchoredPosition = new Vector3(197f,
                               (VCP_LeftSide[index].GetComponent<RectTransform>().anchoredPosition.y) + (-672f), 0);
                    VCP_LeftSide[index1].GetComponent<RectTransform>().anchoredPosition = new Vector3(628f,
                                (VCP_LeftSide[index1].GetComponent<RectTransform>().anchoredPosition.y) + (-672f), 0);
                }
            }

        }
        if (LoadingOKForLodingPanel == true)
        {
            LoadingOKForLodingPanel = false;

            DataLoading.Instance.HideLoading();
        }
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
        MenuPanel.gameObject.SetActive(false);
    }
    public void VideoListOpen()
    {
        VideoList.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
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


//tekliler x 197 y baþlangý. -324 + -672 = -996
//çiftliler x 628 y baþlangý. -324 + -672 = -996