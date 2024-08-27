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

    public GameObject loginFailTMP;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLogin);
    }

    private void OnLogin()
    {
        DatabaseManager.Instance.Login(emailInput.text, pwInput.text, SuccessLogin, FailLogin);
    }

    private void SuccessLogin()
    {
        // TODO: 씬이동
    }

    private void FailLogin()
    {
        loginFailTMP.SetActive(true);
    }
}
