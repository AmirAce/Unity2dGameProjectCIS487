using UnityEngine;
using UnityEngine.SceneManagement;
//using TMPro;

public class EnterDoor : MonoBehaviour
{
    private bool playerHasKey = false;
    //public TextMeshProUGUI keyMessage;
 //   private float messageDisplayTime = 3f;
  //  private float messageTimer = 0f;
    private Collider2D doorCollider;

    private void Start()
    {
        // Get the door's Collider2D component
        doorCollider = GetComponent<Collider2D>();

        // Check if keyMessage is assigned to prevent null reference errors
        //if (keyMessage == null)
        //{
        //    Debug.LogError("Key Message TextMeshProUGUI is not assigned in the Inspector.");
        //}
    }

    public void SetPlayerHasKey(bool hasKey)
    {
        playerHasKey = hasKey;
    }

    private void Update()
    {
        // Check if the message is being displayed and hide it after 3 seconds
        //if (keyMessage != null && keyMessage.gameObject.activeSelf)
        //{
        //    messageTimer += Time.deltaTime;
        //    if (messageTimer >= messageDisplayTime)
        //    {
        //        keyMessage.gameObject.SetActive(false);
        //        messageTimer = 0f;
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the door
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHasKey)
            {
                Debug.Log("Opening the door to the next level!");
                if (doorCollider != null)
                {
                    doorCollider.enabled = false;  // Disable the collider to allow the player to pass through
                }

                // Load the next level
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.Log("No more levels available. Game completed!");
                }
            }
            else
            {
                Debug.Log("You need the key to open this door!");
                //if (keyMessage != null)
                //{
                //    keyMessage.gameObject.SetActive(true);
                //}
            }
        }
    }
}
