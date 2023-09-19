using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000.0f;    // Control how much thrust to use
    [SerializeField] float rotationThrust = 100.0f;   // Control rotation speed
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    Rigidbody rb;   // Rigidbody variable
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    /// <summary>
    /// Process thrust
    /// </summary>
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    /// <summary>
    /// Add thrust to object with sound
    /// </summary>
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); // Play sound

        }
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    /// <summary>
    /// Stop thrust sound
    /// </summary>
    private void StopThrusting()
    {
        audioSource.Stop(); // Stop playing sound
        mainBooster.Stop();
    }

    /// <summary>
    /// Process rotation
    /// </summary>
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    /// <summary>
    /// Apply thrust and rotation to go left on object with sound
    /// </summary>
    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);

        // Check if booster particle is playing
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    /// <summary>
    /// Apply thrust and rotation to go right on object with sound
    /// </summary>
    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);

        // Check if booster particle is playing
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    /// <summary>
    /// Stop left and right thrusting sound
    /// </summary>
    private void StopRotating()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }

    /// <summary>
    /// Apply rotation
    /// </summary>
    /// <param name="rotationThisFrame"></param>
    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;   // Freeze rotation to allow manual rotation below
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;  // Unfreeze rotation so the physics system can take over
    }
}
