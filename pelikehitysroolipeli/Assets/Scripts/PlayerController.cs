using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;
    DoorController currentDoor;
    
    public GameObject doorUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        Button openbutton = GameObject.Find("OpenButton").GetComponent<Button>();
        openbutton.onClick.AddListener(OnOpenButton);

        Button closebutton = GameObject.Find("CloseButton").GetComponent<Button>();
        closebutton.onClick.AddListener(OnCloseButton);

        Button unlockbutton = GameObject.Find("UnlockButton").GetComponent<Button>();
        unlockbutton.onClick.AddListener(OnUnlockButton);

        Button lockbutton = GameObject.Find("LockButton").GetComponent<Button>();
        lockbutton.onClick.AddListener(OnLockButton);

        doorUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnOpenButton()
    {
        if (currentDoor != null)
        {
            currentDoor.ReceiveAction(DoorController.Toiminto.avaa);
            Debug.Log("Open button was pressed");
        }
    }

    void OnCloseButton()
    {
        if (currentDoor != null)
        {
            currentDoor.ReceiveAction(DoorController.Toiminto.sulje);
            Debug.Log("Close button was pressed");
        }
    }

    void OnUnlockButton()
    {
        if (currentDoor != null)
        {
            currentDoor.ReceiveAction(DoorController.Toiminto.avaalukko);
            Debug.Log("Unlock button was pressed");
        }
    }

    void OnLockButton()
    {
        if (currentDoor != null)
        {
            currentDoor.ReceiveAction(DoorController.Toiminto.lukitse);
            Debug.Log("Lock button was pressed");
        }
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Huomaa mitä pelaaja löytää
        if (collision.CompareTag("Door"))
        {
            currentDoor = 
            collision.gameObject.GetComponent<DoorController>();
            Debug.Log("Found Door");
            doorUI.SetActive(true);
        }
        else if (collision.CompareTag("Merchant"))
        {
            Debug.Log("Found Merchant");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor = null;
            doorUI.SetActive(false);
        }
    }

    void OnMoveAction(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        lastMovement = v;
    }    
}
