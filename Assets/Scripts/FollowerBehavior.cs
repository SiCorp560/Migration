using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowerBehavior : MonoBehaviour
{
    public AIPath aiPath;
    public AIDestinationSetter destSet;
    public GameObject playerButterfly;
    private bool initialTrapped = true;
    private bool trapped = true;

    // Start is called before the first frame update
    void Start()
    {
        aiPath.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialTrapped && !trapped)
        {
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-2f, 2f, 2f);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(2f, 2f, 2f);
            }
        }
    }

    public void Trap(GameObject trapper)
    {
        destSet.target = trapper.transform;
        aiPath.endReachedDistance = 0;
        trapped = true;
    }

    public void Free()
    {
        destSet.target = playerButterfly.transform;
        playerButterfly.GetComponent<PlayerFlyController>().followers.Add(gameObject);
        aiPath.endReachedDistance = 3;
    }

    public void InitialFree()
    {
        initialTrapped = false;
        aiPath.enabled = true;
        destSet.target = playerButterfly.transform;
        playerButterfly.GetComponent<PlayerFlyController>().followers.Add(gameObject);
    }
}
