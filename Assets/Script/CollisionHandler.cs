using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Access to SceneManagement class

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2.0f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;   // Keep track of Rocket state
    bool collisionDisabled = false; // Keep track of state collision 

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RespondToDebugKeys();
    }

    /// <summary>
    /// Perform different actions based on what player collides with
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // return if isTransitioning or collisionDisabled is true
        if (isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    /// <summary>
    /// Load next level
    /// </summary>
    void StartSuccessSequence()
    {
        // todo add particle effect for success
        isTransitioning = true;

        audioSource.Stop(); //Stop all previous AudioClip
        audioSource.PlayOneShot(success);
        successParticles.Play();

        // Disable Movement script
        GetComponent<Movement>().enabled = false;

        // Wait and then call method
        Invoke("LoadNextLevel", levelLoadDelay);   
    }

    /// <summary>
    /// Disable player movement control
    /// Call ReloadLevel
    /// </summary>
    void StartCrashSequence()
    {
        isTransitioning = true;

        audioSource.Stop(); //Stop previous AudioClip
        audioSource.PlayOneShot(crash);
        crashParticles.Play();

        // Disable Movement script
        GetComponent<Movement>().enabled = false;
        

        // Wait for number of seconds stated and then call the method stated
        Invoke("ReloadLevel", levelLoadDelay); 
    }

    /// <summary>
    /// Reload current scene
    /// </summary>
    void ReloadLevel()
    {
        //SceneManager.LoadScene(0);  // Load scene at index 0 from Build Settings

        // Gets the active scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Use when unsure what index number the active scene is
        // Loads the scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// Loan next level
    /// </summary>
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Checks if nextSceneIndex == the total number of scenes in Build Settings
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            // Go back to index 0 Scene
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    /// <summary>
    /// Cheat and Debug 
    /// </summary>
    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // Toggle Collision state 
            collisionDisabled = !collisionDisabled;
        }
    }
}
