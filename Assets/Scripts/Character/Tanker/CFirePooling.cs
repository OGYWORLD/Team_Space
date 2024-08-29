using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFirePooling : MonoBehaviour
{
    public GameObject firePrefab;

    public List<GameObject> pool { get; set; } = new List<GameObject>();
    private int pooSize = 5;
    public int poolIdx { get; set; } = 0;

    private void Start()
    {
        // Ç® »ý¼º
        for(int i = 0; i < pooSize; i++)
        {
            GameObject obj = Instantiate(firePrefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

}
