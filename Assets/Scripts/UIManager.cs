using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private bool isMenuOpen = false;
    [SerializeField] private TextMeshProUGUI dungeonFloorText;

    [Header("Health UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpSliderText;
    [Header("Message UI")]
    [SerializeField] private int sameMessageCount = 0; //Read-only
    [SerializeField] private string lastMessage; //Read-only
    [SerializeField] private bool isMessageHistoryOpen = false; //Read-only
    [SerializeField] private GameObject messageHistory;
    [SerializeField] private GameObject messageHistoryContent;
    [SerializeField] private GameObject lastFiveMessagesContent;
    [Header("Inventory UI")]
    [SerializeField] private bool isInventoryOpen = false; //Read-only
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventoryContent;
    [Header("Drop Menu UI")]
    [SerializeField] private bool isDropMenuOpen = false; //Read-only
    [SerializeField] private GameObject dropMenu;
    [SerializeField] private GameObject dropMenuContent;
    [Header("Escape Menu UI")]
    [SerializeField] private bool isEscapeMenuOpen = false;
    [SerializeField] private GameObject escapeMenu;
    [Header("Character Information Menu UI")]
    [SerializeField] private bool isCharacterInformationMenuOpen = false; //Read-only
    [SerializeField] private GameObject characterInformationMenu;
    [SerializeField] private GameObject limbInformationMenu;
    [SerializeField] private Slider leftArmSlider;
    [SerializeField] private Slider rightArmSlider;
    [SerializeField] private Slider leftLegSlider;
    [SerializeField] private Slider rightLegSlider;
    [SerializeField] private Slider torsoSlider;
    [SerializeField] private Slider headSlider;
    [SerializeField] private TextMeshProUGUI leftArmText;
    [SerializeField] private TextMeshProUGUI rightArmText;
    [SerializeField] private TextMeshProUGUI leftLegText;
    [SerializeField] private TextMeshProUGUI rightLegText;
    [SerializeField] private TextMeshProUGUI torsoText;
    [SerializeField] private TextMeshProUGUI headText;
    [Header("Recovery Screen Menu UI")]
    [SerializeField] private bool isRecoveryScreenOpen;
    [SerializeField] private int recoveryAmount = 0;
    [SerializeField] private Actor actor;
    [SerializeField] private GameObject recoveryScreenMenu;
    [SerializeField] private GameObject recoveryScreenContent;
    [Header("Level Up Menu UI")]
    [SerializeField] private bool isLevelUpMenuOpen = false; //Read-only
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private GameObject levelUpMenuContent;

    public bool IsMenuOpen { get => isMenuOpen; }
    public bool IsMessageHistoryOpen { get => isMessageHistoryOpen; }
    public bool IsInventoryOpen {  get => isInventoryOpen; }
    public bool IsDropMenuOpen { get => isDropMenuOpen; }
    public bool IsEscapeMenuOpen { get => isEscapeMenuOpen; }
    public bool IsCharacterInformationMenuOpen {  get => isCharacterInformationMenuOpen; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetDungeonFloorText(SaveManager.instance.CurrentFloor);

        if (SaveManager.instance.Save.SavedFloor is 0)
        {
            AddMessage("Welcome player.", "#0da2ff"); //Light blue
        }
        else
        {
            AddMessage("Welcome back, player.", "#0da2ff"); //Light blue
        }
    }

    public void SetHealthMax(int maxHp)
    {
        hpSlider.maxValue = maxHp;
    }

    public void SetHealth(int hp, int maxHp)
    {
        hpSlider.value = hp;
        hpSliderText.text = $"HP: {hp}/{maxHp}";
    }

    public void SetLimbMaxHealth(int limbMaxHp, BodyPart limb)
    {
        switch (limb.limb)
        {
            case BodyPart.LimbType.LeftArm:
                leftArmSlider.maxValue = limbMaxHp;
                break;
            case BodyPart.LimbType.RightArm:
                rightArmSlider.maxValue= limbMaxHp;
                break;
            case BodyPart.LimbType.LeftLeg:
                leftLegSlider.maxValue = limbMaxHp;
                break;
            case BodyPart.LimbType.RightLeg:
                rightLegSlider.maxValue = limbMaxHp;
                break;
            case BodyPart.LimbType.Torso:
                torsoSlider.maxValue = limbMaxHp;
                break;
            case BodyPart.LimbType.Head:
                headSlider.maxValue = limbMaxHp;
                break;
        }
    }

    public void SetLimbHealth(int limbHP, int limbMaxHP, BodyPart limb)
    {
        switch (limb.limb)
        {
            case BodyPart.LimbType.LeftArm:
                leftArmSlider.value = limbHP;
                leftArmText.text = $"LArm: {limbHP}/{limbMaxHP}";
                break;
            case BodyPart.LimbType.RightArm:
                rightArmSlider.value = limbHP;
                rightArmText.text = $"RArm: {limbHP}/{limbMaxHP}";
                break;
            case BodyPart.LimbType.LeftLeg:
                leftLegSlider.value = limbHP;
                leftLegText.text = $"LLeg: {limbHP}/{limbMaxHP}";
                break;
            case BodyPart.LimbType.RightLeg:
                rightLegSlider.value = limbHP;
                rightLegText.text = $"RLeg: {limbHP}/{limbMaxHP}";
                break;
            case BodyPart.LimbType.Torso:
                torsoSlider.value = limbHP;
                torsoText.text = $"Torso: {limbHP}/{limbMaxHP}";
                break;
            case BodyPart.LimbType.Head:
                headSlider.value = limbHP;
                headText.text = $"Head: {limbHP}/{limbMaxHP}";
                break;
        }
    }

    public void SetDungeonFloorText(int floor)
    {
        dungeonFloorText.text = $"Dungeon Floor: {floor}";
    }

    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = !isMenuOpen;

            switch (true)
            {
                case bool _ when isMessageHistoryOpen:
                    ToggleMessageHistory();
                    break;
                case bool _ when isInventoryOpen:
                    ToggleInventory();
                    break;
                case bool _ when isDropMenuOpen:
                    ToggleDropMenu();
                    break;
                case bool _ when isEscapeMenuOpen:
                    ToggleEscapeMenu();
                    break;
                case bool _ when isCharacterInformationMenuOpen:
                    ToggleCharacterInformationMenu();
                    break;
                default:
                    break;
            }
        }
    }


    public void ToggleMessageHistory()
    {
        isMessageHistoryOpen = !isMessageHistoryOpen;
        SetBooleans(messageHistory, isMessageHistoryOpen);
    }

    public void ToggleInventory(Actor actor = null)
    {
        isInventoryOpen = !isInventoryOpen;
        SetBooleans(inventory, isInventoryOpen);

        if (isMenuOpen)
        {
            UpdateMenu(actor, inventoryContent);
        }
    }

    public void ToggleDropMenu(Actor actor = null)
    {
        isDropMenuOpen = !isDropMenuOpen;
        SetBooleans(dropMenu, isDropMenuOpen);

        if (isMenuOpen)
        {
            UpdateMenu(actor, dropMenuContent);
        }
    }

    public void ToggleEscapeMenu()
    {
        isEscapeMenuOpen = !isEscapeMenuOpen;
        SetBooleans(escapeMenu, isEscapeMenuOpen);

        eventSystem.SetSelectedGameObject(escapeMenu.transform.GetChild(0).gameObject);
    }

    public void ToggleLevelUpMenu(Actor actor)
    {
        isLevelUpMenuOpen = !isLevelUpMenuOpen;
        SetBooleans(levelUpMenu, isLevelUpMenuOpen);

        GameObject constitutionButton = levelUpMenuContent.transform.GetChild(0).gameObject;
        GameObject strengthButton = levelUpMenuContent.transform.GetChild(1).gameObject;
        GameObject agilityButton = levelUpMenuContent.transform.GetChild(2).gameObject;

        constitutionButton.GetComponent<TextMeshProUGUI>().text = $"a) Constitution (+20 HP, from {actor.GetComponent<Fighter>().MaxHp})";
        strengthButton.GetComponent<TextMeshProUGUI>().text = $"b) Strength (+1 attack, from {actor.GetComponent<Fighter>().Power()})";
        agilityButton.GetComponent<TextMeshProUGUI>().text = $"c) Agility (+1 defense, from {actor.GetComponent<Fighter>().Defense()})";
        
        foreach (Transform child in levelUpMenuContent.transform)
        {
            child.GetComponent<Button>().onClick.RemoveAllListeners();

            child.GetComponent<Button>().onClick.AddListener(() => {
                if (constitutionButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreaseMaxHp();
                }
                else if (strengthButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreasePower();
                }
                else if (agilityButton == child.gameObject)
                {
                    actor.GetComponent<Level>().IncreaseDefense();
                }
                else
                {
                    Debug.LogError("No button found!");
                }
                ToggleLevelUpMenu(actor);
                SetHealthMax(actor.GetComponent<Fighter>().MaxHp);
            });
        }

        eventSystem.SetSelectedGameObject(levelUpMenuContent.transform.GetChild(0).gameObject);
        
    }

    public void ToggleCharacterInformationMenu(Actor actor = null)
    {
        isCharacterInformationMenuOpen = !isCharacterInformationMenuOpen;
        SetBooleans(characterInformationMenu, isCharacterInformationMenuOpen);

        if (actor is not null)
        {
            characterInformationMenu.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level: {actor.GetComponent<Level>().CurrentLevel}";
            characterInformationMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"XP: {actor.GetComponent<Level>().CurrentXp}";
            characterInformationMenu.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"XP for next level: {actor.GetComponent<Level>().XpToNextLevel}";
            characterInformationMenu.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Attack: {actor.GetComponent<Fighter>().Power()}";
            characterInformationMenu.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = $"Defense: {actor.GetComponent<Fighter>().Defense()}";
        }
        
    }

    public void ToggleRecoveryScreenMenu(List<BodyPart> bodyParts, Actor consumer, int healAmount)
    {
        isRecoveryScreenOpen = !isRecoveryScreenOpen;
        SetBooleans(recoveryScreenMenu, isRecoveryScreenOpen);

        int limbCount = 0;
        foreach (BodyPart part in bodyParts)
        {
            recoveryScreenContent.transform.GetChild(limbCount).GetComponentInChildren<TextMeshProUGUI>().text = $"{part.limb.ToString()}: {part.LimbHP}/{part.LimbMaxHP}";
            recoveryScreenContent.transform.GetChild(limbCount).GetComponent<BodyPartHolder>().heldBodyPart = part;
            limbCount++;
        }
        actor = consumer;
        recoveryAmount = healAmount;
    }

    public void HealSelectedLimb()
    {
        BodyPart partHealed = EventSystem.current.currentSelectedGameObject.GetComponent<BodyPartHolder>().heldBodyPart;


        if(partHealed.LimbHP == partHealed.LimbMaxHP)
        {
            AddMessage($"That limb is in good health.", "#000000");
        }
        int newHPValue = partHealed.LimbHP + recoveryAmount;
        if(newHPValue > partHealed.LimbMaxHP)
        {
            newHPValue = partHealed.LimbMaxHP;
        }
        int amountRecovered = newHPValue - partHealed.LimbHP;
        partHealed.Heal(newHPValue);
        AddMessage($"Your {EventSystem.current.currentSelectedGameObject.GetComponent<BodyPartHolder>().name} recovered {amountRecovered}", "#808080");

        ToggleRecoveryScreenMenu(actor.GetComponent<PFighter>().BodyParts, actor, recoveryAmount);
    }

    private void SetBooleans(GameObject menu, bool menuBool)
    {
        isMenuOpen = menuBool;
        menu.SetActive(menuBool);
    }
    public void Save()
    {
        SaveManager.instance.SaveGame(false);
        AddMessage("Time slows as you catch your breath. The world will remember your place in it.", "#0da2ff");
    }

    public void Load()
    {
        SaveManager.instance.LoadGame();
        AddMessage("You find yourself where you once were. A dream perhaps?", "#0da2ff");
        ToggleMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void AddMessage(string newMessage, string colorHex)
    {
        if (lastMessage == newMessage)
        {
            TextMeshProUGUI messageHistoryLastChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lastFiveHistoryLastChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
            messageHistoryLastChild.text = $"{newMessage} (x{++sameMessageCount})";
            lastFiveHistoryLastChild.text = $"{newMessage} (x{++sameMessageCount})";
            return;
        }
        else if (sameMessageCount > 0)
        {
            sameMessageCount = 0;
        }

        lastMessage = newMessage;

        TextMeshProUGUI messagePrefab = Instantiate(Resources.Load<TextMeshProUGUI>("Message")) as TextMeshProUGUI;
        messagePrefab.text = newMessage;
        messagePrefab.color = GetColorFromHex(colorHex);
        messagePrefab.transform.SetParent(messageHistoryContent.transform, false );
        
        for(int i = 0; i < lastFiveMessagesContent.transform.childCount; i++)
        {
            if(messageHistoryContent.transform.childCount - 1 < i)
            {
                return;
            }

            TextMeshProUGUI lastFiveHistoryChild = lastFiveMessagesContent.transform.GetChild(lastFiveMessagesContent.transform.childCount - 1 - i).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI messageHistoryChild = messageHistoryContent.transform.GetChild(messageHistoryContent.transform.childCount -1 -i).GetComponent<TextMeshProUGUI>();
            lastFiveHistoryChild.text = messageHistoryChild.text;
            lastFiveHistoryChild.color = messageHistoryChild.color;
        }
    }

    private Color GetColorFromHex(string v)
    {
        Color color;
        if(ColorUtility.TryParseHtmlString(v, out color))
        {
            return color;
        }
        else
        {
            Debug.Log("GetColorFromHex: Could not parse color from string");
            return Color.white;
        }
    }

    private void UpdateMenu(Actor actor, GameObject menuContent)
    {
        for (int resetNum = 0; resetNum < menuContent.transform.childCount; resetNum++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(resetNum).gameObject;
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            menuContentChild.GetComponent<Button>().onClick.RemoveAllListeners();
            menuContentChild.SetActive(false);
        }

        char c = 'a';

        for (int itemNum = 0; itemNum < actor.Inventory.Items.Count; itemNum++)
        {
            GameObject menuContentChild = menuContent.transform.GetChild(itemNum).gameObject;
            Item item = actor.Inventory.Items[itemNum];
            menuContentChild.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"({c++}) {item.name}";
            menuContentChild.GetComponent<Button>().onClick.AddListener(() => {
                if (menuContent == inventoryContent)
                {
                    if (item.Consumable is not null)
                    {
                        Action.UseAction(actor, item);
                    }
                    else if (item.Equippable is not null)
                    {
                        Action.EquipAction(actor, item);
                    }
                }
                else if (menuContent == dropMenuContent)
                {
                    Action.DropAction(actor, item);
                }
            });
            menuContentChild.SetActive(true);
        }
        eventSystem.SetSelectedGameObject(menuContent.transform.GetChild(0).gameObject);
    }
}
