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
        if (target == 0)
        monsterAI.SetDestination(transform.position);
        if (target == 1 && Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.8f)
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
            RaycastHit hit;
            Physics.Raycast(transform.position, (player.transform.position + Vector3.up) - transform.position, out hit, targetingdistance );
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Player"))
            {
                target = 2;
                Debug.Log("Kan se spiller");
            }
            Debug.DrawRay(transform.position, (  (player.transform.position+ Vector3.up)- transform.position) * 5, Color.red);
           // if (targetsSound == false || targetsSound && player.GetComponent<PlayerController>().isRunning || targetsSound && player.GetComponent<PlayerController>().isJumping)
            //    target = 2;
        }
        if (target == 2)
            monsterAI.SetDestination(player.transform.position);
        //targeting the defined target object (for distractions)
        if (target == 3)
            monsterAI.SetDestination(targetObject.transform.position);
    }
}