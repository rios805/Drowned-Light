using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public RawImage itemDisplayImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemQuantityText;
    public Transform modelDisplaySpot;
    public RenderTexture renderTexture;
    public Camera itemDisplayCamera;

    private Inventory playerInventory;
    private int currentIndex = 0;
    private GameObject currentModelInstance;
    [SerializeField] private Image leftArrow;
    [SerializeField] private Image rightArrow;

    void Start()
    {
        inventoryPanel.SetActive(false);
        itemDisplayCamera.targetTexture = renderTexture;
        playerInventory = Player.Instance.GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();

        if (!inventoryPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E))
            NextItem();
        else if (Input.GetKeyDown(KeyCode.Q))
            PreviousItem();
    }

    void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);
        Cursor.visible = !isActive;
        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
        Time.timeScale = isActive ? 1f : 0f;

        if (!isActive && playerInventory.collectedItems.Count > 0)
        {
            Debug.Log("Inventory has items. Displaying first one.");
            currentIndex = 0;
            ShowItem(currentIndex);
        }
        else
        {
            Debug.Log("Inventory is empty or UI is closing.");
            ClearCurrentModel();
        }
    }

    void ShowItem(int index)
    {
        Debug.Log($"Showing item at index {index}");
        ClearCurrentModel();

        var item = playerInventory.collectedItems[index];
        itemNameText.text = item.itemName;

        currentModelInstance = Instantiate(item.prefab.gameObject, modelDisplaySpot.position, Quaternion.identity, modelDisplaySpot);
        currentModelInstance.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f); // Lays item face-up like a coin
        SetLayerRecursively(currentModelInstance, LayerMask.NameToLayer("ItemDisplay"));

        Debug.Log("Spawned item prefab: " + item.itemName);

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;

        int quantity = playerInventory.itemCounts.TryGetValue(item, out int count) ? count : 1;
        itemQuantityText.text = $"x{quantity}";
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    void ClearCurrentModel()
    {
        if (currentModelInstance != null)
            Destroy(currentModelInstance);
    }

    void NextItem()
    {
        currentIndex = (currentIndex + 1) % playerInventory.collectedItems.Count;
        ShowItem(currentIndex);
    }

    void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + playerInventory.collectedItems.Count) % playerInventory.collectedItems.Count;
        ShowItem(currentIndex);
    }

    void LateUpdate()
    {
        if (currentModelInstance != null)
        {
            currentModelInstance.transform.Rotate(Vector3.up, 20f * Time.unscaledDeltaTime, Space.Self);
        }
    }

}
