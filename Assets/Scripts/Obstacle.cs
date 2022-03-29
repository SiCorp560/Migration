using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // All the pieces of this obstacle
    public List<SpriteRenderer> pieces;

    // The materials to switch between
    public Material defaultMat, windyMat;

    // Overall web resistance compared to gust power
    public int gustRes = 0;

    // The amount of time it takes to break one piece
    public float timeToBreak = 0.5f;

    // Tracks whether this web is currently being broken
    private bool breaking = false;

    // Coroutine in control of the countdown timer
    private Coroutine breakTimer = null;

    // Associated trapped butterfly
    public GameObject trappedButterfly;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!breaking && collision.CompareTag("Gust") && gustRes <= collision.gameObject.transform.parent.GetComponent<PlayerFlyController>().GustPower())
        {
            // Start breaking the obstacle
            StartBreakVisual();
            breaking = true;
            breakTimer = StartCoroutine(BreakCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (breaking && collision.CompareTag("Gust"))
        {
            // Cancel breaking the obstacle
            breaking = false;
            StopCoroutine(breakTimer);
            StopBreakVisual();
        }
    }

    private void StartBreakVisual()
    {
        foreach (SpriteRenderer sprite in pieces)
        {
            sprite.material = windyMat;
        }
    }

    private void StopBreakVisual()
    {
        foreach (SpriteRenderer sprite in pieces)
        {
            sprite.material = defaultMat;
        }
    }

    private IEnumerator BreakCountdown()
    {
        // Wait a certain amount of time to break
        yield return new WaitForSeconds(timeToBreak);

        // Break the next piece of the obstacle
        BreakPiece();

        // Reset the breaking boolean
        breaking = false;
    }

    private void BreakPiece()
    {
        // Destroy the next piece in the list
        if (pieces.Count > 0)
        {
            SpriteRenderer piece = pieces[0];
            pieces.RemoveAt(0);
            Destroy(piece);
        }

        // Destroy this when the last piece is destroyed
        if (pieces.Count == 0)
        {
            if (trappedButterfly != null)
            {
                trappedButterfly.GetComponent<FollowerBehavior>().InitialFree();
            }

            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}
