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
    public TextMeshProUGUI pwCheckTMP;

    private bool isCheckEmailDP;
    private bool isCheckPW;

    private void Awake()
    {
        emailInput.onValueChanged.AddListener(OnEmailValueChanged);
        pwInput.onValueChanged.AddListener(OnCheckPWSame);
        pwCheckInput.onValueChanged.AddListener(OnCheckPWSame);

        homeButton.onClick.AddListener(OnToHomeButton);
        dpCheckButton.onClick.AddListener(CheckEmailDuplication);
        createButton.onClick.AddListener(OnCrateAccount);
    }

    private void OnCrateAccount()
    {
        if(!isCheckEmailDP)
        {
            ColorBlock colorBlock = dpCheckButton.colors;
            colorBlock.normalColor = Color.red;
            dpCheckButton.colors = colorBlock;
        }

        if(!isCheckPW)
        {
            ColorBlock colorBlock = pwInput.colors;
            colorBlock.normalColor = Color.red;
            pwInput.colors = colorBlock;
        }

        StartCoroutine(InitColor());

        if (!isCheckEmailDP && !isCheckPW) return;
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

    private void OnCheckPWSame(string s)
    {
        if(pwInput.text.CompareTo(pwCheckInput.text) == 0)
        {
            pwCheckTMP.color = Color.green;
            pwCheckTMP.text = "Password is Same.";
            isCheckPW = true;
        }
        else
        {
            pwCheckTMP.color = Color.red;
            pwCheckTMP.text = "Password is Different.";
            isCheckPW = false;
        }
    }

    private IEnumerator InitColor()
    {
        yield return new WaitForSeconds(0.5f);

        ColorBlock colorBlock = dpCheckButton.colors;
        colorBlock.normalColor = Color.white;
        dpCheckButton.colors = colorBlock;
        pwInput.colors = colorBlock;
    }
}
