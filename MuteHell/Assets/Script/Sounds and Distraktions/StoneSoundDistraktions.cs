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

        Debug.Log($"{gameObject.name} Awake called. hasBeenThrown: {hasBeenThrown}, hasCollidedAfterThrow: {hasCollidedAfterThrow}");
    }

    // Call this method to indicate that the stone has been thrown
    public void SetThrown()
    {
        hasBeenThrown = true;
        hasCollidedAfterThrow = false; // Reset collision flag
        Debug.Log($"{gameObject.name} SetThrown called. hasBeenThrown set to true.");
    }

    // Call this method when the stone is picked up
    public void OnPickedUp()
    {
        hasBeenThrown = false;
        hasCollidedAfterThrow = false;
        Debug.Log($"{gameObject.name} OnPickedUp called. hasBeenThrown set to false.");
    }

    // Call this method when the stone is dropped
    public void OnDropped()
    {
        hasBeenThrown = false;
        hasCollidedAfterThrow = false;
        Debug.Log($"{gameObject.name} OnDropped called. hasBeenThrown set to false.");
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{gameObject.name} OnCollisionEnter called. hasBeenThrown: {hasBeenThrown}, hasCollidedAfterThrow: {hasCollidedAfterThrow}");

        if (hasBeenThrown && !hasCollidedAfterThrow)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                float speed = rb.velocity.magnitude;
                Debug.Log($"Stone speed on collision: {speed}");

                if (speed > 0.1f) // Adjust threshold as needed
                {
                    // Play the sound using PlayOneShot
                    if (audioSource != null && audioSource.clip != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip);
                        Debug.Log("Stone collided and sound played using PlayOneShot.");
                    }
                    else
                    {
                        Debug.LogWarning("AudioSource or AudioClip is null on " + gameObject.name);
                    }

                    // Set the flag so the sound doesn't play again until the stone is thrown again
                    hasCollidedAfterThrow = true;
                }
                else
                {
                    Debug.Log("Collision velocity too low; sound not played.");
                }
            }
            else
            {
                Debug.LogWarning("Rigidbody component missing on " + gameObject.name);
            }
        }
    }
}
