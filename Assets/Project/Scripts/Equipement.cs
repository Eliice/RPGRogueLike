using UnityEngine;
using System.Collections.Generic;

public class Equipement : MonoBehaviour
{
    private SO_Armor headSlot = null;
    private SO_Armor chestSlot = null;
    private SO_Armor greavesSlot = null;
    private SO_Armor bootSlot = null;
    private SO_Weapon rightHandedSlot = null;
    public SO_Weapon RightHandedSlot
    {
        get { return rightHandedSlot; }
    }
    private SO_Armor leftHandedSlot = null;

    private List<SO_Item> equipementList = new List<SO_Item>();
    public List<SO_Item> EquipementList
    {
        get
        {
            equipementList[0] = headSlot;
            equipementList[1] = chestSlot;
            equipementList[2] = greavesSlot;
            equipementList[3] = bootSlot;
            equipementList[4] = rightHandedSlot;
            equipementList[5] = leftHandedSlot;
            return equipementList;
        }
    }

    public List<MyButton> myButtonList = null;
    public GameObject crtWeapon = null;

    public int WeaponBaseRange = 2;
    private int weaponBaseDamage = 150;
    public int WeaponBaseDamage
    {
        get { return weaponBaseDamage; }
    }
    private int armor;
    public int Armor
    {
        get { return armor; }
    }

    private void Start()
    {
        equipementList.Add(headSlot);
        equipementList.Add(chestSlot);
        equipementList.Add(greavesSlot);
        equipementList.Add(bootSlot);
        equipementList.Add(rightHandedSlot);
        equipementList.Add(leftHandedSlot);
    }

    public void EquipWeapon(SO_Weapon itemToEquip)
    {
        switch (itemToEquip.slot)
        {
            case Slot.RIGHT_HANDED:
                if (rightHandedSlot != null)
                    UnEquipWeapon(rightHandedSlot);

                rightHandedSlot = itemToEquip;
                weaponBaseDamage += itemToEquip.damage;
                break;

            default:
                break;
        }
        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (itButton.item.name == itemToEquip.name)
                {
                    itButton.EquipItem();
                }
            }
        }
        GameObject weaponPrefab = Instantiate(itemToEquip.prefab);
        weaponPrefab.transform.SetParent(GameManager.Instance.Player.PlayerWeaponAnchor);
        weaponPrefab.transform.localPosition = Vector3.zero;
        weaponPrefab.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        crtWeapon = weaponPrefab;
    }

    private void EquipArmor(SO_Armor itemToEquip)
    {
        switch (itemToEquip.slot)
        {
            case Slot.BOOT:
                if (bootSlot != null)
                    UnEquipArmor(bootSlot);
                
                bootSlot = itemToEquip;
                armor += itemToEquip.armor;
                break;

            case Slot.CHEST:
                if (chestSlot != null)
                    UnEquipArmor(chestSlot);

                chestSlot = itemToEquip;
                armor += itemToEquip.armor;
                break;

            case Slot.HEAD:
                if (headSlot != null)
                    UnEquipArmor(headSlot);

                headSlot = itemToEquip;
                armor += itemToEquip.armor;
                break;

            case Slot.LEGS:
                if (greavesSlot != null)
                    UnEquipArmor(greavesSlot);

                greavesSlot = itemToEquip;
                armor += itemToEquip.armor;
                break;

            case Slot.LEFT_HANDED:
                if (leftHandedSlot != null)
                    UnEquipArmor(leftHandedSlot);

                leftHandedSlot = itemToEquip;
                armor += itemToEquip.armor;

                GameObject weaponPrefab = Instantiate(itemToEquip.prefab);
                weaponPrefab.transform.SetParent(GameManager.Instance.Player.PlayerShieldAnchor);
                weaponPrefab.transform.localPosition = Vector3.zero;
                weaponPrefab.transform.localRotation = Quaternion.Euler(95f, 0f, 0f);
                break;

            default:
                break;
        }
        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (itButton.item.name == itemToEquip.name)
                {
                    itButton.EquipItem();
                }
            }
        }
    }

    public void AskToEquip(MyButton button)
    {
        if (button != null)
        {
            if (button.item != null)
            {
                SO_Equipable itemToEquip = button.item as SO_Equipable;
                switch (itemToEquip.slot)
                {
                    case Slot.BOOT:
                        EquipArmor(itemToEquip as SO_Armor);
                        break;

                    case Slot.CHEST:
                        EquipArmor(itemToEquip as SO_Armor);
                        break;

                    case Slot.HEAD:
                        EquipArmor(itemToEquip as SO_Armor);
                        break;

                    case Slot.LEGS:
                        EquipArmor(itemToEquip as SO_Armor);
                        break;

                    case Slot.RIGHT_HANDED:
                        EquipWeapon(itemToEquip as SO_Weapon);
                        break;

                    case Slot.LEFT_HANDED:
                        EquipArmor(itemToEquip as SO_Armor);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void UnEquipWeapon(SO_Weapon itemToUnEquip)
    {
        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (itButton.item.name == itemToUnEquip.name)
                {
                    itButton.UnEquipItem();
                    break;
                }
            }
        }

        switch (itemToUnEquip.slot)
        {
            case Slot.RIGHT_HANDED:
                weaponBaseDamage -= itemToUnEquip.damage;
                Destroy(GameManager.Instance.Player.PlayerWeaponAnchor.GetChild(0).gameObject);
                crtWeapon = null;
                rightHandedSlot = null;
                break;

            default:
                break;
        }
    }

    public void UnEquipArmor(SO_Armor itemToUnEquip)
    {
        foreach (MyButton itButton in myButtonList)
        {
            if (itButton.item != null)
            {
                if (itButton.item.name == itemToUnEquip.name)
                {
                    itButton.UnEquipItem();
                    break;
                }
            }
        }

            switch (itemToUnEquip.slot)
            {
                case Slot.BOOT:
                    armor -= bootSlot.armor;
                    bootSlot = null;
                    break;

                case Slot.CHEST:
                    armor -= chestSlot.armor;
                    chestSlot = null;
                    break;

                case Slot.HEAD:
                    armor -= headSlot.armor;
                    headSlot = null;
                    break;

                case Slot.LEGS:
                    armor -= greavesSlot.armor;
                    greavesSlot = null;
                    break;

                case Slot.LEFT_HANDED:
                    armor -= leftHandedSlot.armor;
                    Destroy(GameManager.Instance.Player.PlayerShieldAnchor.GetChild(0).gameObject);
                    leftHandedSlot = null;
                    break;

                default:
                    break;
            }
        }
    }

