using System;
using System.Collections;
using UnityEngine;

public class ChunkPerformanceLoader : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] chunkParents;
    [SerializeField] private float activationDistance = 1.0f;
    [SerializeField] private float checkInterval = 1f;

    void Start()
    {
        StartCoroutine(CheckChunkDistance());
    }

    private IEnumerator CheckChunkDistance()
    {
        while (true)
        {
            foreach (GameObject chunkParent in chunkParents)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, chunkParent.transform.position);

                if (distanceToPlayer <= activationDistance)
                {
                    chunkParent.SetActive(true);
                }
                else
                {
                    chunkParent.SetActive(false);
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
