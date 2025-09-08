using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }   
    }

    public GameObject Get(int i)
    {
        GameObject select = null;

        foreach (GameObject item in pools[i])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;

            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[i], transform);
            pools[i].Add(select);
        }

        return select;
    }
}

