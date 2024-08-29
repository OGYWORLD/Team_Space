using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFireProjectile : MonoBehaviour
{
    private float speed = 7f;

    private bool isDetectWall;

    private void OnEnable()
    {
        isDetectWall = false;
        StartCoroutine(Move());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo("Wall") == 0)
        {
            isDetectWall = true;
        }
    }

    private IEnumerator Move()
    {
        float sumTime = 0;
        float totalTime = 10f;

        while(!isDetectWall && sumTime < totalTime)
        {
            transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
            sumTime += Time.deltaTime;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
