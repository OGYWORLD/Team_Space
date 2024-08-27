using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CCreateAccount : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField pwInput;
    public TMP_InputField pwCheckInput;

    public Button dpCheckButton;
    public Button createButton;
    public Button homeButton;

    public TextMeshProUGUI dpCheckTMP;

    private bool isCheckEmailDP;

    private void Awake()
    {
        emailInput.onValueChanged.AddListener(OnEmailValueChanged);

        homeButton.onClick.AddListener(OnToHomeButton);
        dpCheckButton.onClick.AddListener(CheckEmailDuplication);
    }

    private void OnCrateAccount()
    {
        
    }

    private void OnToHomeButton()
    {
        PanelManager.Instance.InitPanel((int)Panel.loginPanel);
    }

    private void CheckEmailDuplication()
    {
        if (DatabaseManager.Instance.CheckEmailDuplication(emailInput.text))
        {
            isCheckEmailDP = true;

            dpCheckTMP.color = Color.green;
            dpCheckTMP.text = "Verified Duplication Email";
            
        }
        else
        {
            dpCheckTMP.color = Color.red;
            dpCheckTMP.text = "Email is Duplicated";
        }
    }

    private void OnEmailValueChanged(string s)
    {
        isCheckEmailDP = false;
        dpCheckTMP.text = "";
    }
}
