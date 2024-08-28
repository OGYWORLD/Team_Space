using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* 시작화면에서 로그인을 관리하는 스크립트입니다.
 */

public class CLogin : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField pwInput;

    public Button loginButton;
    public Button createButton;

    public GameObject loginFailTMP;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLogin);
        createButton.onClick.AddListener(OnToCreateAccount);
    }

    private void OnEnable()
    {
        loginFailTMP.SetActive(false);

        emailInput.text = "";
        pwInput.text = "";
    }

    private void OnLogin()
    {
        DatabaseManager.Instance.Login(emailInput.text, pwInput.text, SuccessLogin, FailLogin);
    }

    private void SuccessLogin()
    {
        PanelManager.Instance.InitPanel((int)Panel.mainMenuPanel);
        PanelManager.Instance.toMainMenu?.Invoke();
    }

    private void FailLogin()
    {
        loginFailTMP.SetActive(true);
    }

    private void OnToCreateAccount()
    {
        PanelManager.Instance.InitPanel((int)Panel.createAccountPanel);
    }
}
