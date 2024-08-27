using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ����ȭ�鿡�� �ʹ� ī�޶� �����̴� ��ũ��Ʈ �Դϴ�.
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

        // position01���� ������
        while(sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[index].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }

        index++;
        sumTime = 0f;

        // position02���� ������
        while (sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[index].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }
    }

}
