using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _Environments._Mutual.Connection;
using _Environments._Mutual.Data;
using _Environments._Mutual.Data.State;
using _Environments._Local;

using UnityEngine.SceneManagement;
using TMPro;
using PoisonArch;

public class Profile : MonoBehaviour
{
    #region Singleton

    public static Profile Instance;
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

    public bool guestProfileRegister;
    public string guestUUID;

    public Transform FirstPanel;
    public Transform LoginPanel;

    public TMP_InputField MailIF;
    public TMP_InputField PasswordIF;

    bool entry = false;
    private void Awake()
    {
        Singleton();

        Screen.orientation = ScreenOrientation.Portrait;

        GuestProfileSave.LoadFile();
    }
    private void Update()
    {
        if (ConnectionManager.Instance.GuestLoginTokenCreated == true || ConnectionManager.Instance.UserLoginTokenCreated == true)
        {
            SceneManager.LoadScene(1);
            LevelManager.Instance.NextLevel();
        }
    }
    //splash
    public void ContinueButton()
    {
        /*GuestProfileSave.SaveFile();

        guestProfileRegister = false;
        SceneManager.LoadScene(1);*/

        if (GuestProfileSave.FILEFOUND == true && entry == false)
        {
            entry = true;
            guestUUID = GuestProfileSave.GUESTUUID;

            //Guest Login
            StartCoroutine(Login.GuestUserLogin());

        }
    }
    //splash
    public void LoginPanelButton()
    {
        FirstPanel.gameObject.SetActive(false);
        LoginPanel.gameObject.SetActive(true);
    }
    public void BackButton()
    {
        LoginPanel.gameObject.SetActive(false);
        FirstPanel.gameObject.SetActive(true);
    }
    public void LoginButton()
    {
        if (entry == false)
        {
            entry = true;
            //User Login
            
               // StartCoroutine(Login.UserLogin(MailIF.text, PasswordIF.text));

            StartCoroutine(Login.UserLogin("test@mtek.com", "1234"));
        }
    }
}
