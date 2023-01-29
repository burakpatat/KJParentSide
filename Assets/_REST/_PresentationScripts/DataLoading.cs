using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DataLoading : MonoBehaviour
{
    #region Singleton
    public static DataLoading Instance;
    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    public LoadingScreenManager loadingScreenManager;
    public Transform fakeLoadImage;
    public Canvas canvas;
    void Awake()
    {
        Singleton();

        DontDestroyOnLoad(canvas);

        SceneManager.LoadSceneAsync(0);
    }
    void Start()
    {
        fakeLoadImage.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180), .6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
    public void HideLoading()
    {
        loadingScreenManager.HideLoadingScreen();
        fakeLoadImage.GetComponent<RectTransform>().DOScale(Vector3.zero, .6f).SetEase(Ease.Linear);

        Invoke("reset", .5f);
    }
    void reset()
    {
        canvas.gameObject.SetActive(false);
        loadingScreenManager.RevealLoadingScreen();
    }
    public void ReOp()
    {
        canvas.gameObject.SetActive(true);
    }
}
