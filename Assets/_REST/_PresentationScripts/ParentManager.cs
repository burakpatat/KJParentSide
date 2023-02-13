using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    bool LoadingOKForLodingPanel = false;

    public Transform MenuPanel;
    public Transform GameList;
    public Transform VideoList;
    public Transform GameDetails;
    public Transform VideoDetails;
    public Transform QRPanel;
    public Transform UserGuide;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadingOKForLodingPanel = true;
        }

        if (LoadingOKForLodingPanel == true)
        {
            LoadingOKForLodingPanel = false;

            DataLoading.Instance.HideLoading();
        }
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
}
