using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    // The amount of time in between droplets (set in editor)
    public float spawnTime;
    // The prefab for the water drop
    public GameObject waterDropPrefab;

    // Used to countdown time between droplets
    private float timer;

    private void Start()
    {
        timer = spawnTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            // Spawn a drop whenever the time runs out
            Instantiate(waterDropPrefab, transform);
            // Reset the timer
            timer = spawnTime;
        }
    }
}
