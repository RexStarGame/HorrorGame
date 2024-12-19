using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class FirstMonster : MonoBehaviour
{
    //default state for the monster is prowling
    public Transform[] waypoints;
    public string target;
    NavMeshAgent monsterAI;
    [SerializeField] int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<NavMeshAgent>();
        if (target == null)
            monsterAI.SetDestination(waypoints[currentIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && Vector3.Distance(transform.position, waypoints[currentIndex].position) < 0.3f)
        {
            Debug.Log("waypoint " + currentIndex + " er nået");
            currentIndex++;
            if (currentIndex == waypoints.Length)
                currentIndex = 0;
            monsterAI.SetDestination(waypoints[currentIndex].position);
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) < 5)
            target = "Player";
        if (target == "Player")
        {
            monsterAI.SetDestination(player.transform.position);
        }
    }
}
