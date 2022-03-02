using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public Vector3 direction = Vector3.left;
    public int windSpeed = 3;
    private List<GameObject> objects = new List<GameObject>();

    // FixedUpdate is called once per fixed frame
    void FixedUpdate()
    {
        for(int i = 0; i < objects.Count; i++)
        {
            if (objects[i].CompareTag("Player"))
            {
                PlayerFlyController flyPlayer = objects[i].GetComponent<PlayerFlyController>();
                if (flyPlayer != null && flyPlayer.isStunned())
                {
                    windSpeed = 10;
                }
                else
                {
                    windSpeed = 3;
                }
            }
            direction.Normalize();
            objects[i].transform.position += direction * windSpeed * Time.deltaTime;
        }
    }
  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            objects.Add(collision.gameObject);
        }
    }
  
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            objects.Remove(collision.gameObject);
        }
    }
}
