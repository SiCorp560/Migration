using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehavior : MonoBehaviour
{
    private bool hunting = true;
    public GameObject butterfly;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null && butterfly != null)
        {
            butterfly.GetComponent<FollowerBehavior>().Free();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            List<GameObject> following = collision.gameObject.GetComponent<PlayerFlyController>().followers;
            if (following.Count > 0 && hunting)
            {
                butterfly = following[0];
                following.RemoveAt(0);
                butterfly.GetComponent<FollowerBehavior>().Trap(gameObject);
            }
        }
    }
}
