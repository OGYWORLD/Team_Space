using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/* ����ȭ�鿡�� �α����� �����ϴ� ��ũ��Ʈ�Դϴ�.
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
        // TODO: ���̵�
    }

    private void FailLogin()
    {
        loginFailTMP.SetActive(true);
    }
}
