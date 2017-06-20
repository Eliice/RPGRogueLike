using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryMenu : Menu
{
    [SerializeField] private Transform itemGrid;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private ToolBar toolbar;
    [SerializeField] private Text title;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private GameObject droppedPrefab;


    private delegate void Action1();
    private delegate void Action2();
    private Action1 firstInput;
    private Action2 secondInput;

    private MyButton selectedButton = null;
    private Inventory crtInventory = null;
    private Inventory backInventory = null;
    private Equipement playerEquipement = null;

    private List<MyButton> myButtonList = new List<MyButton>();

    protected override void Awake()
    {
        base.Awake();
        InputManager.Instance.inventoryAction1 += FirstInput;
        InputManager.Instance.inventoryAction2 += SecondInput;
    }

    public override void OnShow(params object[] parameters)
    {
        InputManager.Instance.Mode = InputMode.Inventory;

        InventoryType type = (InventoryType)parameters[0];
        Inventory inv = parameters[1] as Inventory;

        toolbar.UpdateInput(type);
        tooltip.UpdateTooltip(selectedButton);
        firstInput = null;
        secondInput = null;
    
    	OpenInventory(inv, type);

        if (type == InventoryType.PLAYER)
        {
            playerEquipement = parameters[2] as Equipement;
            playerEquipement.myButtonList = myButtonList;
            FindAllButtonEquip();
        }
        else if (type == InventoryType.PLAYER_SHOP)
            FindAllButtonEquip();
        else if (type == InventoryType.PLAYER_STORE)
            FindAllButtonEquip();

    }
    public override void OnHide(params object[] parameters)
    {
        selectedButton = null;
		tooltip.ResetTooltip();
    }
    public override void UpdateDisplay()
    {
        toolbar.UpdateGoldAndWeight();
    }

    private void OpenInventory(Inventory inventory, InventoryType invType)
    {
        List<SO_Item> itemList = inventory.InventoryList;
        title.text = invType.ToString();

        if (itemList != null)
        {
            if (myButtonList.Count != 0)
            {
                foreach (Transform child in itemGrid)
                    Destroy(child.gameObject);

                myButtonList.Clear();
            }

            if (invType != InventoryType.PLAYER && invType != InventoryType.ENEMY)
                AddSwitchButton(invType, crtInventory);

            crtInventory = inventory;

            foreach (SO_Item item in itemList)
                AddButton(item);

            switch (invType)
            {
                case InventoryType.CHEST:
                    backInventory = inventory;
                    firstInput += () => { TakeItem(selectedButton, inventory); };
                    secondInput += () => { TakeAllItems(inventory); };
                    break;

                case InventoryType.ENEMY:
                    backInventory = inventory;
                    firstInput += () => { TakeItem(selectedButton, inventory); };
                    secondInput += () => { TakeAllItems(inventory); };
                    break;

                case InventoryType.PLAYER:
                    backInventory = inventory;
                    firstInput += () => { playerEquipement.AskToEquip(selectedButton); };
                    secondInput += () => { DropItem(selectedButton); };
                    secondInput += () => { GameManager.Instance.Player.Inventory.Remove(selectedButton); };
                    secondInput += () => { AskToDestroyButton(); };
                    break;

                case InventoryType.SHOP:
                    backInventory = inventory;
                    firstInput += () => { BuyItem(selectedButton, inventory); };
                    break;

                case InventoryType.PLAYER_SHOP:
                    firstInput += () => { SellItem(selectedButton); };
                    break;
                case InventoryType.PLAYER_STORE:
                    firstInput += () => { backInventory.Receive(selectedButton); };
                    firstInput += () => { StockItem(selectedButton); };
                    break;

                default:
                    break;
            }
        }
    }
    private void AddSwitchButton(InventoryType invType, Inventory inventory)
    {
        if (inventory == null)
            inventory = GameManager.Instance.Player.Inventory;

        GameObject newInstOfButton = Instantiate(itemButtonPrefab);
        newInstOfButton.transform.SetParent(itemGrid);
        Button b = newInstOfButton.GetComponent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = Color.cyan;
        b.colors = cb;
        myButtonList.Add(newInstOfButton.GetComponent<MyButton>());

        switch (invType)
        {
            case InventoryType.CHEST:
                newInstOfButton.GetComponent<MyButton>().nameText.text = "Player Inv";
                newInstOfButton.GetComponent<Button>().onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.PLAYER_STORE, GameManager.Instance.Player.Inventory); });
                break;

            case InventoryType.PLAYER_STORE:
                newInstOfButton.GetComponent<MyButton>().nameText.text = "Chest Inv";
                newInstOfButton.GetComponent<Button>().onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.CHEST, inventory); });
                break;

            case InventoryType.PLAYER_SHOP:
                newInstOfButton.GetComponent<MyButton>().nameText.text = "Shop Inv";
                newInstOfButton.GetComponent<Button>().onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.SHOP, inventory); });
                break;

            case InventoryType.ENEMY:
                newInstOfButton.GetComponent<MyButton>().nameText.text = "Player Inv";
                newInstOfButton.GetComponent<Button>().onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.PLAYER_STORE, inventory); });
                break;

            case InventoryType.SHOP:
                newInstOfButton.GetComponent<MyButton>().nameText.text = "Player Inv";
                newInstOfButton.GetComponent<Button>().onClick.AddListener(() => { UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Show, InventoryType.PLAYER_SHOP, GameManager.Instance.Player.Inventory); });
                break;

            default:
                break;
        }

    }
    private void AddButton(SO_Item item)
    {
        bool buttonIsFound = false;

        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (itButton.item.name == item.name)
                {
                    buttonIsFound = true;
                    itButton.StackItem();
                    myButtonList.Add(itButton);
                    break;
                }
            }
        }

        if (!buttonIsFound)
        {
            GameObject newInstOfButton = Instantiate(itemButtonPrefab);
            MyButton myButton = newInstOfButton.GetComponent<MyButton>();
            myButtonList.Add(myButton);
            myButton.item = item;
            newInstOfButton.transform.SetParent(itemGrid);
            newInstOfButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { selectedButton = myButton; tooltip.UpdateTooltip(selectedButton); });
        }
    }
    private void AskToDestroyButton()
    {
        if (selectedButton != null)
        {
            if (selectedButton.RemoveButton(selectedButton))
            {
                selectedButton.item = null;
                tooltip.UpdateTooltip(selectedButton);
            }
        }
    }
    private void FindAllButtonEquip()
    {
        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (playerEquipement != null)
                {
                    for (int i = 0; i < playerEquipement.EquipementList.Count; ++i)
                    {
                        if (playerEquipement.EquipementList[i] != null)
                        {
                            if (playerEquipement.EquipementList[i].name == itButton.item.name)
                                itButton.EquipItem();
                        }
                    }
                }
            }
        }
    }

    private void FirstInput()
    {
        if (firstInput != null)
            firstInput();
    }
    private void SecondInput()
    {
        if (secondInput != null)
            secondInput();
    }

    private void TakeAllItems(Inventory inventory)
    {
        for (int i = 0; i < myButtonList.Count; ++i)
        {
            TakeItem(myButtonList[i], inventory);
        }
        foreach (Transform child in itemGrid)
            Destroy(child.gameObject);
        myButtonList.Clear();
        UIManager.Instance.Notify(UIRequest.Inventory, UIRequestMode.Hide);
        UIManager.Instance.Notify(UIRequest.HUD, UIRequestMode.Show);
    }
    private bool TakeItem(MyButton button, Inventory inventory)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                if (GameManager.Instance.Player.Inventory.CrtWeight < GameManager.Instance.Player.Inventory.MaxWeight)
                {
                    GameManager.Instance.Player.Inventory.Receive(button);
                    inventory.Remove(button);
                    AskToDestroyButton();
                    return true;
                }
                else
                {
                    print("You can t carry more!");
                    return false;
                }
            }
        }
        return false;
    }
    private void BuyItem(MyButton button, Inventory inventory)
    {
        if (button != null)
        {
            if (GameManager.Instance.Player.Inventory.Gold >= button.item.value)
            {
                if (TakeItem(button, inventory))
                    GameManager.Instance.Player.Inventory.Gold -= button.item.value;
            }
            else
                print("you don't have enough gold!");
        }
    }
    private void StockItem(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                if (playerEquipement != null)
                {
                    if (playerEquipement.EquipementList[0] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[1] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[2] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[3] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[4] == button.item)
                        playerEquipement.UnEquipWeapon(button.item as SO_Weapon);
                    else if (playerEquipement.EquipementList[5] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                }

                GameManager.Instance.Player.Inventory.Remove(selectedButton);
                AskToDestroyButton();
            }
        }
    }
    private void SellItem(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                if (playerEquipement != null)
                {
                    if (playerEquipement.EquipementList[0] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[1] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[2] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[3] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[4] == button.item)
                        playerEquipement.UnEquipWeapon(button.item as SO_Weapon);
                    else if (playerEquipement.EquipementList[5] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                }

                GameManager.Instance.Player.Inventory.Gold += button.item.value;
                GameManager.Instance.Player.Inventory.Remove(selectedButton);
                backInventory.Receive(selectedButton);
                AskToDestroyButton();
            }
        }
    }
    private void DropItem(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                if (playerEquipement != null)
                {
                    if (playerEquipement.EquipementList[0] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[1] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[2] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[3] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                    else if (playerEquipement.EquipementList[4] == button.item)
                        playerEquipement.UnEquipWeapon(button.item as SO_Weapon);
                    else if (playerEquipement.EquipementList[5] == button.item)
                        playerEquipement.UnEquipArmor(button.item as SO_Armor);
                }

                GameObject itemPrefab = Instantiate(droppedPrefab);
                itemPrefab.GetComponent<DroppedItem>().CreateDroppedItem(button.item);
                Vector3 spawnPos = GameManager.Instance.Player.transform.localPosition;
                spawnPos += GameManager.Instance.Player.transform.forward;
                spawnPos.y = 0.5f;
                itemPrefab.transform.localPosition = spawnPos;
                itemPrefab.transform.localRotation = GameManager.Instance.Player.transform.localRotation;
            }
        }
    }
}
