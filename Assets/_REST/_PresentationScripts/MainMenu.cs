using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using System.Threading.Tasks;

using DG.Tweening;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Singleton

    public static MainMenu Instance;
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

    public TMP_Text UUIDText;

    public TMP_Text usernameText;
    int starcount, diacount;

    public TMP_Text ScoreText;

    bool LoadingOKForLodingPanel = false;
    public Transform Avatar;

    [Header("Register")]
    public Transform RegisterTransform;

    public TMP_InputField UsernameIF;
    public TMP_InputField MailIF;
    public TMP_InputField PasswordIF;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        starcount = PlayerPrefs.GetInt("starVal");
        diacount = PlayerPrefs.GetInt("diaVal");
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();
    }
    void guestQuery()
    {
        if (ConnectionManager.Instance.BaseLoadedOK && ConnectionManager.Instance.AuthName == ConnectionManager.Instance.LoginName)
        {
            ConnectionManager.Instance.BaseLoadedOK = false;

            UUIDText.text = Profile.Instance.guestUUID;
            usernameText.text = "childName : " + ConnectionManager.Instance.ChildsName[0] + "\n" + "parentName : " + ConnectionManager.Instance.AuthName;
            //image
            StartCoroutine(SetPoster(GetUser.GetMedia() + ConnectionManager.Instance.Avatar, ConnectionManager.Instance.ChildsName[0], Avatar));
        }

        if (LoadingOKForLodingPanel == true)
        {
            LoadingOKForLodingPanel = false;
            DataLoading.Instance.HideLoading();
        }
    }
    void Update()
    {
        guestQuery();
    }
    public void StarButton()
    {
        starcount += 10;
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        PlayerPrefs.SetInt("starVal", starcount);
    }
    public void DiaButton()
    {
        diacount += 1;
        ScoreText.text = "star : " + starcount.ToString() + "\n" + "diamond : " + diacount.ToString();

        PlayerPrefs.SetInt("diaVal", diacount);
    }
    public void WatchButton()
    {
        DataLoading.Instance.ReOp();
        StartCoroutine(Load(3));
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(4);
        DataLoading.Instance.loadingScreenManager.RevealLoadingScreen();
    }
    public void RegisterPanelButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        RegisterTransform.gameObject.SetActive(true);
    }
    public void RegisterButton()
    {
        StartCoroutine(Register.UserRegister(UsernameIF.text, MailIF.text, PasswordIF.text));
    }
    public void RegisterBackButton()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        RegisterTransform.gameObject.SetActive(false);
    }
    IEnumerator Load(int index)
    {
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            int p = (int)(asyncOperation.progress * 100f);
            Debug.Log(p.ToString() + "%");
            
            if(p >= 89)
            {
                p = 100;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    public IEnumerator SetPoster(string url, string name, Transform _intanceObj)
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
