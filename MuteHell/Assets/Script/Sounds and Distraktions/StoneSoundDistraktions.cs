using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StoneSoundDistraktions : MonoBehaviour
{
    private bool hasBeenThrown = false;
    private bool hasCollidedAfterThrow = false;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure variables are reset
        hasBeenThrown = false;
        hasCollidedAfterThrow = false;


    }

    // Call this method to indicate that the stone has been thrown
    public void SetThrown()
    {
        hasBeenThrown = true;
        hasCollidedAfterThrow = false; // Reset collision flag
  
    }

    // Call this method when the stone is picked up
    public void OnPickedUp()
    {
        hasBeenThrown = false;
        hasCollidedAfterThrow = false;
    
    }

    // Call this method when the stone is dropped
    public void OnDropped()
    {
        hasBeenThrown = false;
        hasCollidedAfterThrow = false;

    }


    void OnCollisionEnter(Collision collision)
    {


        if (hasBeenThrown && !hasCollidedAfterThrow)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                float speed = rb.velocity.magnitude;


                if (speed > 0.1f) // Adjust threshold as needed
                {
                    // Play the sound using PlayOneShot
                    if (audioSource != null && audioSource.clip != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip);

                    }
                    GameObject monster = GameObject.FindWithTag("Monster");
                    if (monster != null)
                    {
                        monster.GetComponent<FirstMonster>().target = 3;
                        monster.GetComponent<FirstMonster>().targetObject = this.gameObject;
                    }
                    // Set the flag so the sound doesn't play again until the stone is thrown again
                    hasCollidedAfterThrow = true;
                }
            
            }
        }
    }
}
