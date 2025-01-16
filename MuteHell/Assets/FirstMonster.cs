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
    public int target;
    public bool targetsSound = false;
    public float targetingdistance = 15;
    public GameObject targetObject = null;
    NavMeshAgent monsterAI;
    [SerializeField] int currentWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        monsterAI = GetComponent<NavMeshAgent>();
        if (target == 0)
        monsterAI.SetDestination(waypoints[currentWaypoint].position);
    }
    // Update is called once per frame
    void Update()
    {
        //prowling
        if (target == 0 && Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.8f)
        {
            currentWaypoint++;
            if (currentWaypoint == waypoints.Length)
                currentWaypoint = 0;
            monsterAI.SetDestination(waypoints[currentWaypoint].position);
        }
        //finding and targeting the player
        GameObject player = GameObject.FindWithTag("Player");
        if (Vector3.Distance(transform.position, player.transform.position) < targetingdistance)
        {
            if (targetsSound == false || targetsSound && player.GetComponent<PlayerController>().isRunning || targetsSound && player.GetComponent<PlayerController>().isJumping)
                target = 1;
        }
        if (target == 1)
            monsterAI.SetDestination(player.transform.position);
        //targeting the defined target object (for distractions)
        if (target == 2)
        {
            monsterAI.SetDestination(targetObject.transform.position);
        }
    }
}