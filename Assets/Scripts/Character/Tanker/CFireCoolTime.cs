using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CFireCoolTime : MonoBehaviour
{
    public Image skillImage;
    public TextMeshProUGUI countTMP;

    public bool isCoolTime { get; set; } = true;

    public void StartCoolTime()
    {
        isCoolTime = false;

        skillImage.color = Color.black;

        StartCoroutine(CoolTime());
    }

    private IEnumerator CoolTime()
    {
        countTMP.text = "3";
        yield return new WaitForSeconds(1f);
        countTMP.text = "2";
        yield return new WaitForSeconds(1f);
        countTMP.text = "1";
        yield return new WaitForSeconds(1f);
        countTMP.text = "";

        isCoolTime = true;
        skillImage.color = Color.white;
    }
}
