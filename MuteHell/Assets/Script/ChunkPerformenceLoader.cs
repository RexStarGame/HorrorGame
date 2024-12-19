using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPerformenceLoader : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] chunkParents;
    [SerializeField] private float activationDistance = 1.0f;
    [SerializeField] private float checkInterval = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine((IEnumerator)CheckChunkDistance());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerable CheckChunkDistance()
    {
        while (true)
        {
            foreach (GameObject chunckparent in chunkParents)
            {
                float distanceToPlayer = Vector2.Distance(player.transform.position, chunckparent.transform.position);

                if(distanceToPlayer <= activationDistance)
                {
                    chunckparent.SetActive(true);
                }
                else
                {
                    chunckparent.SetActive(false);
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
