using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 시작화면에서 초반 카메라를 움직이는 스크립트 입니다.
 */

public class CCameraMove : MonoBehaviour
{
    public Transform[] pos;
    public GameObject mainCamera;

    private Transform originCameraTrans;

    private void Start()
    {
        originCameraTrans = mainCamera.transform;

        StartCoroutine(CameraMove());
    }

    private IEnumerator CameraMove()
    {
        int index = 0;

        float moveTime = 1f;
        float sumTime = 0f;

        // position01까지 움직임
        while(sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[index].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }

        index++;
        sumTime = 0f;

        // position02까지 움직임
        while (sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[index].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }
    }

}
