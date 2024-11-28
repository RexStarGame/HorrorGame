using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickupController : MonoBehaviour
{
    public float pickupRange = 2f;
    public LayerMask pickupLayer;
    public Animator animator;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.T; // Key to throw items
    public int maxItems = 10;

    public float normalThrowForce = 10f;
    public float maxThrowForce = 20f;
    public float forceUp = 5f; 
    private float currentThrowForce;
    public float chargeSpeed = 10f; // How fast the throw force charges
    private bool isChargingThrow = false;

    private bool isPickingUp = false;
    private GameObject currentItem;
    public List<GameObject> inventory = new List<GameObject>();

    public List<GameObject> floorPickupItems = new List<GameObject>();
    public List<GameObject> tablePickupItems = new List<GameObject>();

    private int selectedItemIndex = 0; 

    private bool canPickUp = true;
    private bool isThrowing = false;

    public Transform raycastOrigin;
    public Transform handTransform; 

    private GameObject equippedItem;
    // Charging Settings
    public float minChargeTime = 1.5f; 
    private float chargeTimer = 0f;    

    public float floorPickupAnimationDuration = 2f; 
    public float tablePickupAnimationDuration = 2f; 


    public bool isThrowingAktiv = false;
    public bool IsTrowingLow = false;

    public bool pickedItemUpNear = false;
    void Update()
    {
        HandleThrowing();
        DetectPickupableItem();

        if (Input.GetKeyDown(pickupKey))
        {
            PickupItem();
        }

        // Add this to handle dropping the item
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }

        HandleItemSwitching();
    }

    void DetectPickupableItem()
    {
        if (raycastOrigin == null)
        {
            Debug.LogWarning("RaycastOrigin is not assigned in the inspector.");
            return;
        }

        Vector3 rayOrigin = raycastOrigin.position;
        Vector3 rayDirection = raycastOrigin.forward;

        RaycastHit hit;
        Debug.DrawRay(rayOrigin, rayDirection * pickupRange, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pickupRange, pickupLayer))
        {
            GameObject detectedItem = hit.collider.gameObject;

            if (floorPickupItems.Contains(detectedItem) || tablePickupItems.Contains(detectedItem))
            {
                currentItem = detectedItem;
            }
            else
            {
                currentItem = null;
            }
        }
        else
        {
            currentItem = null;
        }
    }

    void PickupItem()
    {
        if (!canPickUp || isPickingUp)
        {
            return;
        }

        if (inventory.Count >= maxItems)
        {
            Debug.LogWarning("Inventory is full, can't pick up more items.");
            return;
        }

        if (currentItem == null)
        {
            Debug.LogWarning("No valid item to pick up.");
            return;
        }

        // Check if the item is in the pickup lists
        if (floorPickupItems.Contains(currentItem))
        {
            animator.SetBool("pickUpItemsFloor", true); 
            Debug.Log("Set PickUpItemsFloor to true");
        }
        else if (tablePickupItems.Contains(currentItem))
        {
            animator.SetBool("pickUpItemsTable", true); 
            Debug.Log("Set PickUpItemsTable to true");
        }
        else
        {
            Debug.LogWarning("Item not recognized for pick-up: " + currentItem.name);
            return;
        }

        // Start the pick-up coroutine
        StartCoroutine(CompletePickup(currentItem));
    }
    void HandleThrowing()
    {
        if (equippedItem != null && !isThrowing)
        {
            // Tasten T starter kast
            if (Input.GetKeyDown(throwKey))
            {
                isChargingThrow = true;
                currentThrowForce = normalThrowForce; // Start med normal kastkraft
                chargeTimer = 0f; // Nulstil opladningstimer

            }

            // Mens T holdes nede (oplader kastkraft)
            if (Input.GetKey(throwKey))
            {
                chargeTimer += Time.deltaTime;
                currentThrowForce += chargeSpeed * Time.deltaTime;
                currentThrowForce = Mathf.Clamp(currentThrowForce, normalThrowForce, maxThrowForce);
            }

            // Når T slippes
            if (Input.GetKeyUp(throwKey))
            {
                isChargingThrow = false; // Stop opladning
                PerformThrow();
            }
        
    

            // If the player releases the throw key
            if (Input.GetKeyUp(throwKey))
            {
                // Throw the item
                if (isChargingThrow)
                {
                    isChargingThrow = false;
                    ReleaseThrow();
                }
                if (chargeTimer >= minChargeTime)
                {
                    ReleaseThrow();
                }
                else
                {
                   
                    //CancelThrow();
                    CancelThrow();
                }
            }
        }
    }
    void CancelThrow()
    {
        Debug.Log("Throw canceled due to insufficient charge time.");

        //reset animationen.
        animator.SetBool("throwItem", false);
        animator.SetBool("throwItem2", false);

        
    }

    void ReleaseThrow()
    {
    
        if (currentThrowForce >= maxThrowForce)
        {
            animator.SetBool("throwItem2", false);
            Debug.Log("Set throwItem2 to false");
            isThrowingAktiv = true;
            IsTrowingLow = false;


        }
        else
        {
            animator.SetBool("throwItem", false);
            Debug.Log("Set throwItem to false");
            isThrowingAktiv = false;
            IsTrowingLow = true;
        }

        // kør coroutine.
        StartCoroutine(ThrowItemCoroutine());
    }
    IEnumerator ThrowItemCoroutine()
    {
        isThrowing = true; // Angiv at kast er startet

        float kasteTidspunkt = 1f; // Tidspunkt i sekunder, hvor kastet skal ske, bør kun bruges til vores mediumcharge.
        float kasteTidsPunkt2 = 2.5f; //TisPunkt i sekunder, hvor minKast kaster object.
                                      
        float animationVarighed = GetAnimationClipLength(currentThrowForce >= maxThrowForce ? "throwItem2" : "throwItem");

        if(isThrowingAktiv == true && IsTrowingLow == false) //kaster langt og smider med sin item efter 1 sekund.
        {
            // Kast objektet efter en bestemt tid
            yield return new WaitForSeconds(kasteTidspunkt);
        }
        else
        {
            yield return new WaitForSeconds(kasteTidsPunkt2); // hvis ikke min Throwforce er støre eller = max throw force så kaster vi først med objectet efter 2.5 secunder.
        }
           

        if (equippedItem != null)
        {
            // Få referencer til Rigidbody og Collider én gang
            Rigidbody itemRigidbody = equippedItem.GetComponent<Rigidbody>();
            Collider itemCollider = equippedItem.GetComponent<Collider>();

            if (itemRigidbody != null)
            {
                // Fjern fra hånden og aktiver fysik
                equippedItem.transform.SetParent(null);
                itemRigidbody.isKinematic = false;
                itemRigidbody.useGravity = true;
                itemRigidbody.AddForce(transform.forward * currentThrowForce, ForceMode.Impulse);
            }

            if (itemCollider != null)
            {
                itemCollider.enabled = true; // Genaktivér collider
            }

            // Fjern objektet fra inventaret
            inventory.Remove(equippedItem);
            equippedItem = null; // Nulstil referencen
        }

        // Lad animationen afslutte naturligt
       
      
            // Kast objektet efter en bestemt tid
            yield return new WaitForSeconds(animationVarighed);
       

        isThrowing = false; // Kastet er færdigt
    }

    public void PerformThrow()
    {
        //check om throwable, nu kan kaste alt
        if ( equippedItem != null && isThrowing == false)
        {
            Debug.Log("PerformThrow kaldt af Animation Event.");

            if (currentThrowForce == maxThrowForce)
                animator.SetTrigger("MediumCharges"); // spilleren kaster langt
            else
                animator.SetTrigger("MinCharges"); // spilleren kaster ikke langt. 



            StartCoroutine(ThrowItemCoroutine()); // Start throw
            
        }
        else
        {
            Debug.LogWarning("PerformThrow kaldt, men der kastes ikke eller intet objekt er udstyret.");
        }
    }

    float GetAnimationClipLength(string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        // Default wait time if clip not found
        return 1f;
    }
    IEnumerator CompletePickup(GameObject item)
    {
        isPickingUp = true;
        float waitTime = 0f;
        float itemSpawnsInHandsEarly = 1.52f; // Tid efter hvilken genstanden dukker op i hånden

        // Bestem den samlede varighed af animationen
        if (floorPickupItems.Contains(item))
        {
            waitTime = floorPickupAnimationDuration;
        }
        else if (tablePickupItems.Contains(item))
        {
            waitTime = tablePickupAnimationDuration;
        }

        // Vent indtil genstanden skal dukke op i hånden
        yield return new WaitForSeconds(itemSpawnsInHandsEarly);

        // Tilføj genstanden til inventaret
        inventory.Add(item);

        // Udstyr genstanden med det samme
        selectedItemIndex = inventory.Count; // Indeks for den nye genstand
        ShowSelectedItem();

        // Vent for resten af animationens varighed
        yield return new WaitForSeconds(waitTime - itemSpawnsInHandsEarly);

        // Nulstil animationsbools
        animator.SetBool("pickUpItemsFloor", false);
        animator.SetBool("pickUpItemsTable", false);

        isPickingUp = false;

        StartCoroutine(PickupCooldown());
    }


    void EquipItem(GameObject item)
    {
        // Detach any previously equipped item
        if (equippedItem != null)
        {
            Debug.Log($"Detaching previously equipped item: {equippedItem.name}");

            // Detach from hand
            equippedItem.transform.SetParent(null);

            // Re-enable physics on the previously equipped item
            Rigidbody prevItemRigidbody = equippedItem.GetComponent<Rigidbody>();
            if (prevItemRigidbody != null)
            {
                prevItemRigidbody.isKinematic = false;
                prevItemRigidbody.useGravity = true;
            }
            else
            {
                Debug.LogWarning("Rigidbody component missing on " + equippedItem.name);
            }

            // Re-enable the collider on the previously equipped item
            Collider prevItemCollider = equippedItem.GetComponent<Collider>();
            if (prevItemCollider != null)
            {
                prevItemCollider.enabled = true;
            }
            else
            {
                Debug.LogWarning("Collider component missing on " + equippedItem.name);
            }

            // Inform the StoneSoundDistraktions script that the item has been dropped
            StoneSoundDistraktions prevSoundScript = equippedItem.GetComponent<StoneSoundDistraktions>();
            if (prevSoundScript != null)
            {
                prevSoundScript.OnDropped();
            }
            else
            {
                Debug.LogWarning("StoneSoundDistraktions script missing on " + equippedItem.name);
            }

            // Remove the item from the inventory
            inventory.Remove(equippedItem);

            // Remove reference to the previously equipped item
            equippedItem = null;
        }

        // Move the new item to the hand
        item.transform.SetParent(handTransform);

        // Reset local position and rotation
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Disable physics on the new item
        Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = true;
        }
        else
        {
            Debug.LogWarning("Rigidbody component missing on " + item.name);
        }

        // Disable the item's collider if needed
        Collider itemCollider = item.GetComponent<Collider>();
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Collider component missing on " + item.name);
        }

        // Inform the StoneSoundDistraktions script that the item has been picked up
        StoneSoundDistraktions soundScript = item.GetComponent<StoneSoundDistraktions>();
        if (soundScript != null)
        {
            soundScript.OnPickedUp();
        }
        else
        {
            Debug.LogWarning("StoneSoundDistraktions script missing on " + item.name);
        }

        // Store reference to the new equipped item
        equippedItem = item;

        Debug.Log($"Equipped new item: {equippedItem.name}");
    }
    
    IEnumerable PickUpitem()
    {
        float pickupEarly = 4; // hvornår under animationen samler spilleren object op.



        yield return new WaitForSeconds(pickupEarly);

        //...
        if(true)
        {

        }
    }
    
    void DropItem()
    {
        if (equippedItem != null)
        {
            // Remove the item from the hand
            equippedItem.transform.SetParent(null);

            // Re-enable physics on the item
            Rigidbody itemRigidbody = equippedItem.GetComponent<Rigidbody>();
            if (itemRigidbody != null)
            {
                itemRigidbody.isKinematic = false;
                itemRigidbody.useGravity = true;
            }

            // Re-enable the collider
            Collider itemCollider = equippedItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true;
            }

            // Remove the item from the inventory
            inventory.Remove(equippedItem);

            // Remove reference to the equipped item
            equippedItem = null;

            // Adjust the selectedItemIndex
            if (inventory.Count == 0)
            {
                selectedItemIndex = 0; // Empty hand
            }
            else
            {
                selectedItemIndex = selectedItemIndex % (inventory.Count + 1);
                ShowSelectedItem();
            }
        }
    }

    IEnumerator PickupCooldown()
    {
        canPickUp = false;
        yield return new WaitForSeconds(0.5f);
        canPickUp = true;
    }

    void HandleItemSwitching()
    {
        int totalSlots = inventory.Count + 1; // +1 for empty hand

        if (totalSlots <= 1)
        {
            return; // Only empty hand available, no switching needed
        }

        // Scroll wheel to switch items
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedItemIndex = (selectedItemIndex + 1) % totalSlots;
            ShowSelectedItem();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedItemIndex = (selectedItemIndex - 1 + totalSlots) % totalSlots;
            ShowSelectedItem();
        }

        // Number keys 0 (empty hand), 1, 2, 3, etc.
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedItemIndex = 0; // Empty hand
            ShowSelectedItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventory.Count >= 1)
        {
            selectedItemIndex = 1;
            ShowSelectedItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventory.Count >= 2)
        {
            selectedItemIndex = 2;
            ShowSelectedItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventory.Count >= 3)
        {
            selectedItemIndex = 3;
            ShowSelectedItem();
        }
        // Add more keys if needed
    }
    // New method to handle the throw action via Animation Event
    // New method to handle the throw action via Animation Event


    


    void ShowSelectedItem()
    {
        // Deactivate all items in inventory
        foreach (GameObject item in inventory)
        {
            item.SetActive(false);
        }

        // If selectedItemIndex == 0, no item is equipped (empty hand)
        if (selectedItemIndex == 0)
        {
            // No item equipped
            if (equippedItem != null)
            {
                equippedItem = null;
            }
            return;
        }

        // Equip the selected item
        GameObject selectedItem = inventory[selectedItemIndex - 1];
        selectedItem.SetActive(true);

        // Move the item to the hand if it's not already there
        EquipItem(selectedItem);
    }

}
