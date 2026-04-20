using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    Vector2 lastMovement;
    Rigidbody2D rb;
    [SerializeField]
    float moveSpeed;

    [Header("Door Interaction")]
    DoorController currentDoor;
    public GameObject doorUI;

    [Header("Merchant Interaction")]
    private Merchant currentMerchant;
    private MerchantInteraction merchantInteraction;

    public Reppu playerReppu = new Reppu();
    //public TMP_Text inventoryText;

    public Nuoli chosenArrow;
    public Tavara chosenWeapon;
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;
    public GameObject InventoryPanel;
    Button inventoryButton;

    float lastShotTime = 0;
    public float shootCooldown = 0.5f;
    public GameObject[] arrowPrefabs;
    public Transform firePoint;


    void Start()
    {
        lastMovement = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();

        merchantInteraction = FindFirstObjectByType<MerchantInteraction>();
        if (merchantInteraction == null)
        {
            Debug.LogError("No MerchantInteraction found in scene!");
        }

        Button openbutton = GameObject.Find("OpenButton").GetComponent<Button>();
        openbutton.onClick.AddListener(OnOpenButton);

        Button closebutton = GameObject.Find("CloseButton").GetComponent<Button>();
        closebutton.onClick.AddListener(OnCloseButton);

        Button unlockbutton = GameObject.Find("UnlockButton").GetComponent<Button>();
        unlockbutton.onClick.AddListener(OnUnlockButton);

        Button lockbutton = GameObject.Find("LockButton").GetComponent<Button>();
        lockbutton.onClick.AddListener(OnLockButton);

        inventoryButton = GameObject.Find("InventoryButton").GetComponent<Button>();
        inventoryButton.onClick.AddListener(ToggleInventory);

        doorUI.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;

            Vector2 direction = (mousePos - transform.position).normalized;

            float distance = 0.5f;

            firePoint.position = transform.position + (Vector3)(direction * distance);

            ShootArrow(mousePos);
        }
    }

    private void FixedUpdate()  
    {
        rb.MovePosition(rb.position + lastMovement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnOpenButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.avaa);
    }

    void OnCloseButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.sulje);
    }

    void OnUnlockButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.avaalukko);
    }

    void OnLockButton()
    {
        if (currentDoor != null)
            currentDoor.ReceiveAction(DoorController.Toiminto.lukitse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor = other.GetComponent<DoorController>();
            doorUI.SetActive(true);
        }
        else if (other.CompareTag("Merchant"))
        {
            Merchant merchant = other.GetComponentInParent<Merchant>();
            if (merchant != null)
            {
                currentMerchant = merchant;

                if (merchantInteraction != null)
                {
                    merchantInteraction.SetCurrentMerchant(currentMerchant);
                    merchantInteraction.OpenMerchantUI();
                }
            }
        }
        else if (other.CompareTag("Item"))
        {
            Tavara item = other.GetComponent<Tavara>();

            if (item != null)
            {
                if (playerReppu.AddItem(item))
                {
                    Debug.Log("Picked up: " + item.itemName);

                    UpdateInventoryUI();

                    other.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Inventory full!");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            currentDoor = null;
            doorUI.SetActive(false);
        }

        if (other.CompareTag("Merchant"))
        {
            Merchant merchant = other.GetComponentInParent<Merchant>();
            if (merchant == currentMerchant)
            {
                if (merchantInteraction != null)
                    merchantInteraction.CloseMerchantUI();

                currentMerchant = null;
            }
        }
    }

    void OnMoveAction(InputValue value)
    {
        lastMovement = value.Get<Vector2>();
    }

    public void Buy()
    {
        if (currentMerchant == null) return;

        if (currentMerchant.merchantType == MerchantType.Arrow)
        {
            merchantInteraction.BuyArrowFromDropdown();
        }
        else
        {
            merchantInteraction.BuyFoodFromDropdown();
        }
    }

    public void UseFirstItem()
    {
        if (playerReppu.GetItems().Count > 0)
        {
            Tavara item = playerReppu.GetItems()[0];
            UseItem(item);
        }
    }

    public void UseItem(Tavara item)
    {
        bool success = item.Use(this);

        if (success)
    {
        if (item is Ateria || item is Vesi)
        {
            playerReppu.RemoveItem(item);
        }

        UpdateInventoryUI();
    }
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
        foreach(Tavara item in playerReppu.GetItems())
        {
            GameObject obj = Instantiate(inventoryItemPrefab, inventoryContent);
            InventoryItemUI ui = obj.GetComponent<InventoryItemUI>();
            ui.Setup(item, this);
        }
    }
    public void ToggleInventory()
    {
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);
    }

    public void ShootArrow(Vector3 target)
    {
        if (chosenArrow == null)
        {
            Debug.Log("No arrow selected!");
            return;
        }

        int index = (int)chosenArrow.ArrowType;

        GameObject arrowObj = Instantiate(
            arrowPrefabs[index],
            firePoint.position,
            Quaternion.identity
        );

        ArrowController arrowController = arrowObj.GetComponent<ArrowController>();

        Vector2 direction = (target - transform.position).normalized;
        firePoint.right = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        arrowController.Launch(direction);
    }

    public void TakeDamage(int amount)
    {
        PlayerDataManager.Instance.RemoveHealth(amount);
    }
}