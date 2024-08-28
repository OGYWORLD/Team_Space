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
    private int cameraIndex;


    private void Start()
    {
        originCameraTrans = mainCamera.transform;
        PanelManager.Instance.toMainMenu += TurnCameraOnMainMenu;

        StartCoroutine(CameraMove());
    }

    private void TurnCameraOnMainMenu()
    {
        StartCoroutine(CameraTurn());
    }

    private IEnumerator CameraMove()
    {
        int cameraIndex = 0;

        float moveTime = 1f;
        float sumTime = 0f;

        // position01���� ������
        while(sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[cameraIndex].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }

        cameraIndex++;
        sumTime = 0f;

        // position02���� ������
        while (sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.position = Vector3.Lerp(originCameraTrans.position, pos[cameraIndex].position, time);

            sumTime += Time.deltaTime;

            yield return null;
        }

    }

    private IEnumerator CameraTurn()
    {
        cameraIndex++;
        
        float moveTime = 1f;
        float sumTime = 0f;

        // position01���� ������
        while (sumTime <= moveTime)
        {
            float time = sumTime / moveTime;
            mainCamera.transform.rotation = Quaternion.Lerp(originCameraTrans.rotation, pos[cameraIndex].rotation, time);

            sumTime += Time.deltaTime;

            yield return null;
        }
    }

}
