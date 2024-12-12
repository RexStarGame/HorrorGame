using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstMonster : MonoBehaviour
{
    public Transform[] waypoints;
    //for now this is an int. 0 means the monster has no target, 1 means the player is the target
    public int target;
    NavMeshAgent monsterAI;
    [SerializeField] int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<NavMeshAgent>();
        if (target == 0)
            monsterAI.SetDestination(waypoints[currentIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, waypoints[currentIndex].position) < 0.3f && target == 0)
        {
            Debug.Log("waypoint " + currentIndex + " er nået");
            currentIndex++;
            if (currentIndex == waypoints.Length)
            {
                currentIndex = 0;
            }
            monsterAI.SetDestination(waypoints[currentIndex].position);
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (target == 1)
        {
            monsterAI.SetDestination(player.transform.position);
        }
    }
}
