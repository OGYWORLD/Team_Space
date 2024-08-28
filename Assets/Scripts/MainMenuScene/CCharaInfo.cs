using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CCharaInfo : MonoBehaviour
{
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI level;

    public TMP_InputField nameChangeInputField;

    public Button changeNameButton;

    public Image expBar;

    private void Start()
    {
        DatabaseManager.Instance.changeUserInfo += PrintUserInfo;
        PrintUserInfo();
    }

    private void PrintUserInfo()
    {
        if(DatabaseManager.Instance.data is not null)
        {
            nickname.text = DatabaseManager.Instance.data.name;
            level.text = (DatabaseManager.Instance.data.EXP / 100 + 1).ToString();

            // TODO: fill amount �°� ����ߴ��� ���� ����ġ ��� ���� �ְ� Ȯ��
            expBar.fillAmount = (DatabaseManager.Instance.data.EXP % 100) * 0.001f;
        }
    }
}
