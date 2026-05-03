using UnityEngine;

public class WallCollider : MonoBehaviour
{
    [Tooltip("Optional: If checked, the drone will completely stop executing its command queue when it hits this wall.")]
    public bool stopQueueOnCrash = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandleCrash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HandleCrash();
        }
    }

    private void HandleCrash()
    {
        Debug.Log($"[Wall] Drone crashed into {gameObject.name}!");

        // Trigger the puzzle failed screen instead of hard reloading the scene
        FeedbackManager feedbackManager = Object.FindFirstObjectByType<FeedbackManager>();
        if (feedbackManager != null)
        {
            feedbackManager.ShowFailure();
        }

        // Make sure to stop the drone's command queue if requested
        if (stopQueueOnCrash)
        {
            CommandQueue queue = Object.FindFirstObjectByType<CommandQueue>();
            if (queue != null) queue.Stop();
        }
    }
}