using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD : MonoBehaviour
{
    [SerializeField] private GameObject highDetailModel;   
    [SerializeField] private GameObject mediumDetailModel; 
    [SerializeField] private GameObject lowDetailModel;    

    [SerializeField] private float mediumDetailDistance = 20f; 
    [SerializeField] private float lowDetailDistance = 40f;    

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        UpdateLOD();
    }

    void Update()
    {
        UpdateLOD();
    }

    private void UpdateLOD()
    {
        if (!mainCamera) return;

        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);

        if (distance <= mediumDetailDistance)
        {
            // Close enough for high detail
            SetLODState(true, false, false);
        }
        else if (distance > mediumDetailDistance && distance <= lowDetailDistance)
        {
            // Medium range, use medium detail
            SetLODState(false, true, false);
        }
        else
        {
            // Far away, use low detail
            SetLODState(false, false, true);
        }
    }

    private void SetLODState(bool high, bool medium, bool low)
    {
        if (highDetailModel) highDetailModel.SetActive(high);
        if (mediumDetailModel) mediumDetailModel.SetActive(medium);
        if (lowDetailModel) lowDetailModel.SetActive(low);
    }
}
