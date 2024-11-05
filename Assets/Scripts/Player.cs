using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public AudioClip DeathSound;
    public AudioClip LaserSound;
    public AudioSource audioSource;
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile laser;
    private Vector2 movementDirection;
    private Animator anim;
    private Rigidbody2D rb; // Rigidbody2D for physics interactions
    public AudioClip KeySound;


    private bool hasKey = false;  // Tracks if the player has collected the special key

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Initialize the Rigidbody2D
        audioSource = GetComponent<AudioSource>();
        KeySound = Resources.Load<AudioClip>("Sound/Key");
        DeathSound = Resources.Load<AudioClip>("Sound/DeathSound");
        LaserSound = Resources.Load<AudioClip>("Sound/LaserSound");

    }

    private void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Move the player
        transform.position += (Vector3)movementDirection * speed * Time.deltaTime;

        // Flip player sprite when moving left or right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 2, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 2, 1);

        // Set animator parameters
        anim.SetBool("run", movementDirection != Vector2.zero);

        // Clamp the position of the player within camera bounds
        Vector3 position = transform.position;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        // Clamp the position of the player within camera bounds and the y-axis limit
        position.x = Mathf.Clamp(position.x, bottomLeft.x, topRight.x);
        position.y = Mathf.Clamp(position.y, -11, topRight.y); // Limit y-axis to a minimum of -7
        transform.position = position;

        transform.position = position;

        // Fire a laser if there isn't one already active
        if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = movementDirection * 10f;

            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(LaserSound);
            }


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            audioSource.PlayOneShot(KeySound);
            Destroy(other.gameObject);
            Debug.Log("Key Collected!");

            GameObject door = GameObject.FindWithTag("Door");
            if (door != null)
            {
                EnterDoor doorScript = door.GetComponent<EnterDoor>();
                if (doorScript != null)
                {
                    doorScript.SetPlayerHasKey(true);
                }
            }
        }

        if (other.CompareTag("Door"))
        {
            if (hasKey)
            {
                Debug.Log("Opening the door to Level 2!");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
            }
            else
            {
                Debug.Log("You need the key to open this door!");
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            Die();
        }
    }

    private void Die()
    {
        audioSource.PlayOneShot(DeathSound);
        anim.SetTrigger("IsDead");
        Invoke(nameof(NotifyGameManagerOfDeath), 3f);
    }

    private void NotifyGameManagerOfDeath()
    {
        GameManager.Instance.OnPlayerKilled(this);
    }
}
