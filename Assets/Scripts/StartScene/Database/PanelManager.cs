using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*����ȭ���� �α��� �гΰ� ���� ���� �г��� �����ϴ� �г� �Ŵ����Դϴ�.
 */

public enum Panel
{
    loginPanel,
    createAccountPanel,
    mainMenuPanel,
    matchingPanel
}

public delegate void ToMainMenu();

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    public GameObject[] panels;

    public ToMainMenu toMainMenu;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitPanel((int)Panel.loginPanel);
    }

    public void InitPanel(int whichPanel)
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        panels[whichPanel].SetActive(true);
    }
}
