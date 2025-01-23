using System;
using System.Collections;
using UnityEngine;

public class ChunkPerformanceLoader : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] chunkParents;
    [SerializeField] private float activationDistance = 1.0f;
    [SerializeField] private float checkInterval = 1f;
    

    [SerializeField] private LayerMask chunkLayer; // layer for chunks


    void Start()
    {
        StartCoroutine(CheckChunkDistance()); // starter coroutine
    }
    private IEnumerator CheckChunkDistance()
    {
        while (true)
        {
            foreach (GameObject chunkParent in chunkParents)
            {
                float distanceToPlayer = Vector3.Distance(player.transform.position, chunkParent.transform.position); // distance til spilleren

                RaycastHit hit;
                bool rammernoget = Physics.Raycast(player.transform.position, Vector3.down, out hit, 100 , chunkLayer); // raycast ned til chunkLayer
                if (distanceToPlayer <= activationDistance || rammernoget) // hvis vi rammer chunkLayer
                {
                    chunkParent.SetActive(true);
                }
                else if (distanceToPlayer <= activationDistance || !rammernoget) // hvis vi ikke rammer chunkLayer
                {
                    chunkParent.SetActive(false); // sætter chunkParent til false
                }
            }

            yield return new WaitForSeconds(checkInterval); // venter checkInterval sekunder
        }
    }
}
