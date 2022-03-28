using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehavior : MonoBehaviour
{
    private bool hunting = true;
    private bool dropping = true;
    public int moveSpeed;
    public GameObject trappedButterfly;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("Spider doesn't have rigidbody component.");
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
        {
            if (trappedButterfly != null)
            {
                trappedButterfly.GetComponent<FollowerBehavior>().Free();
            }
            FallOffWeb();
        }
        //DropDown();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            List<GameObject> following = collision.gameObject.GetComponent<PlayerFlyController>().followers;
            if (following.Count > 0 && hunting)
            {
                trappedButterfly = following[0];
                following.RemoveAt(0);
                trappedButterfly.GetComponent<FollowerBehavior>().Trap(gameObject);
            }
        }
    }

    private void FallOffWeb()
    {
        hunting = false;
        rb.gravityScale = 1;
        Destroy(gameObject, 3.0f);
    }

    private void DropDown()
    {
        transform.position -= transform.up * Time.deltaTime * moveSpeed;
    }
}
