using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideMekanik : MonoBehaviour
{
    private bool isHidden = false; // om spilleren er skjult 
    [SerializeField] private GameObject player; // spilleren
    [SerializeField] private LayerMask chunkLayer; // layer for chunks
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
