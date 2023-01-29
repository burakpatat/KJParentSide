using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Mutual.Connection;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;

public class KovukList : MonoBehaviour
{
    #region Singleton

    public static KovukList Instance;
    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion
    private string _cachePath;
    private void Awake()
    {
        Singleton();
        _cachePath = Application.persistentDataPath + "/Posters";
    }
    public GameObject PosterButton;
    public List<Transform> PosterTransform;

    public TMP_Text UserNameText;
    public Transform Posters;

    List<Video_Data> _datas = new List<Video_Data>();

    public int ClickPosterID;

    bool LoadingOKForLodingPanel = false;
    private void Start()
    {
        
    }
    void Update()
    {
        InitData();

        if(LoadingOKForLodingPanel == true)
        {
            LoadingOKForLodingPanel = false;

            DataLoading.Instance.HideLoading();
        }

    }
    void InitData()
    {
        if (ConnectionManager.Instance.BaseLoadedOK)
        {
            ConnectionManager.Instance.BaseLoadedOK = false;
            _datas = GetVideo.VideoClass.data;
            GetList();
        }
    }
    void GetList()
    {
        int cloumnCount = GetVideo._CloumnCount;
        for (int i = 0; i < cloumnCount; i++)
        {
            var _intanceObj = Instantiate(PosterButton, PosterTransform[i].transform.position, Quaternion.identity);
            _intanceObj.transform.SetParent(Posters);

            _intanceObj.transform.localScale = Vector3.one;

            //image
            StartCoroutine(SetPoster(GetVideo.GetMedia() + _datas[i].videoThumbnail, _datas[i].videoname, _intanceObj.transform.GetChild(0)));
            _intanceObj.transform.GetChild(1).gameObject.SetActive(true);

           
            foreach (var prow in ConnectionManager.Instance._profiledatas)
            {
                for (int p = 0; p < prow.Child.Count; p++)
                {
                    Debug.Log(p);
                    if (prow.Child[p].childname == ConnectionManager.Instance.ChildsName[0])
                    {
                        if (prow.Child[p].content_limitation[0].Videos != null)
                        {
                            for (int a = 0; a < prow.Child[p].content_limitation[0].Videos.Count; a++)
                            {
                                if (prow.Child[p].content_limitation[0].Videos[a].InteractiveVideo_id.videoname.Contains(_datas[i].videoname))
                                {
                                    _intanceObj.transform.GetChild(1).gameObject.SetActive(false);
                                    _intanceObj.GetComponent<Button>().interactable = true;

                                    _intanceObj.transform.GetComponent<Button>().onClick.AddListener(() => GETClick(
                                    int.Parse(_intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text)));
                                }
                            }
                        }
                    }
                }
            }
            _intanceObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _datas[i].id.ToString();
        }
    }
    public void GETClick(int id)
    {
        ClickPosterID = id;
        SceneManager.LoadScene(5);

        DataLoading.Instance.ReOp();
    }
    public IEnumerator SetPoster(string url, string name, Transform _intanceObj)
    {
        if (!Directory.Exists(_cachePath))
        {
            Debug.Log("No directory found for temporary files. Creating one.");
            Directory.CreateDirectory(_cachePath);
        }
        
        string posterPath = _cachePath + "/" + DataTypeExtensions.RemoveDigits(name).ToLower() + ".poster";
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
