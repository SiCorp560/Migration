using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehavior : MonoBehaviour
{
    private bool hunting = true;
    private bool dropping = false;
    private bool release = false;
    public Transform dropDest;
    public Transform rest;
    public Transform stop;
    public int moveSpeed;
    public GameObject trappedButterfly;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null && butterfly != null)
        {
            FallOffWeb();
        }

        if (dropping)
        {
            transform.position = Vector2.MoveTowards(transform.position, dropDest.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            List<GameObject> following = collision.gameObject.GetComponent<PlayerFlyController>().followers;
            if (following.Count > 0 && hunting)
            {
                hunting = false;
                release = true;
                trappedButterfly = following[0];
                following.RemoveAt(0);
                butterfly.GetComponent<FollowerBehavior>().Trap(gameObject);
            }
        }
        else if (collision.CompareTag("SpiderRest"))
        {
            dropping = false;
        }
        else if (collision.CompareTag("SpiderStop"))
        {
            dropDest = rest;
        }
    }

    private void FallOffWeb()
    {
        if (trappedButterfly != null && release)
        {
            trappedButterfly.GetComponent<FollowerBehavior>().Free();
        }
        release = false;
        hunting = false;
        rb.gravityScale = 1;
        Destroy(gameObject, 3.0f);
    }

    public void StartDrop()
    {
        if (!dropping)
        {
            dropping = true;
            dropDest = stop;
        }
    }
}
