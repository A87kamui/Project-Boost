using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;   // Not Serialized since this value will not change
    [SerializeField] Vector3 movementVector;
    float movementFactor;    // Sets a range for the variable
    [SerializeField] float period = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        // Store current position
        startingPosition = transform.position;
        Debug.Log(startingPosition);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if period a valid number
        if (period == 0)
        {
            return;
        }

        // Time to travel a full circle
        // Time.time is how much time has elapsed in seconds
        // Will increase the longer the time 
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2; // constant value of 6.283
        // Gets a sin wave value
        // How much time has elasped * tau
        // Going from -1 to 1
        float rawSinWave = Mathf.Sin(cycles * tau);

        // Adds 1 to rawSinWave (-1) so it can't be below 0
        // Divides by 2 so it can be between 0-1
        // This will allow the object to move
        movementFactor = (rawSinWave + 1.0f) / 2.0f;

        Vector3 offSet = movementVector * movementFactor;
        transform.position = startingPosition + offSet;
    }
}
