using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSelectionSystem : MonoBehaviour
{
    [Header("Character Selection")]
    public int p1HoverIndex = 0;
    public int p2HoverIndex = 1;
    public Image p1Border; 
    public Image p2Border;
    public Image p1FullSpriteDisplay; 
    public Image p2FullSpriteDisplay; 
    public RectTransform[] buttonPositions; 
    public Sprite[] allCharacterSprites;
    
    [Header("Stage Selection")]
    public GameObject stageSelectionPanel; // Panel with stage buttons
    public TextMeshProUGUI selectionStatusText; // Shows "Select Stage" or "Ready to Start"
    
    [Header("Start Button")]
    public Button startButton; // The START button
    
    private bool p1Locked = false;
    private bool p2Locked = false;
    private bool stageSelected = false;
    private bool isTransitioning = false;

    void Awake() 
    {
        // Force the flip BEFORE the first frame is even rendered
        if (p2FullSpriteDisplay != null) 
        {
            p2FullSpriteDisplay.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Start() 
    {
        UpdateSelectionUI();
        
        // Disable start button initially
        if (startButton != null)
            startButton.interactable = false;
            
        // Hide stage selection until both characters locked
        if (stageSelectionPanel != null)
            stageSelectionPanel.SetActive(false);
            
        UpdateStatusText();
    }

    void Update() 
    {
        if (isTransitioning || CharacterManager.instance == null) return;

        // Player 1 character selection
        if (!p1Locked) 
        {
            if (Input.GetKeyDown(KeyCode.D)) 
                p1HoverIndex = (p1HoverIndex + 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.A)) 
                p1HoverIndex = (p1HoverIndex + buttonPositions.Length - 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                p1Locked = true;
                CharacterManager. instance.p1SelectedCharacter = p1HoverIndex;
                Debug.Log("✅ Player 1 locked character: " + p1HoverIndex);
                CheckBothPlayersLocked();
            }
        }

        // Player 2 character selection
        if (!p2Locked) 
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) 
                p2HoverIndex = (p2HoverIndex + 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) 
                p2HoverIndex = (p2HoverIndex + buttonPositions.Length - 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                p2Locked = true;
                CharacterManager. instance.p2SelectedCharacter = p2HoverIndex;
                Debug.Log("✅ Player 2 locked character: " + p2HoverIndex);
                CheckBothPlayersLocked();
            }
        }

        // REMOVED AUTO-START!  Now only happens when Start button is clicked

        if (! isTransitioning) 
            UpdateSelectionUI();
    }

    void CheckBothPlayersLocked()
    {
        if (p1Locked && p2Locked)
        {
            Debug.Log("🎭 Both players locked!  Show stage selection");
            
            // Show stage selection panel
            if (stageSelectionPanel != null)
                stageSelectionPanel.SetActive(true);
                
            UpdateStatusText();
        }
    }

    public void OnStageSelected(string stageName)
    {
        if (!p1Locked || !p2Locked) return;
        
        stageSelected = true;
        CharacterManager.instance.SelectStage(stageName);
        
        Debug.Log("🏟️ Stage selected: " + stageName);
        
        // Enable start button
        if (startButton != null)
            startButton.interactable = true;
            
        UpdateStatusText();
    }

    public void OnStartButtonClicked()
    {
        if (!p1Locked || ! p2Locked || !stageSelected)
        {
            Debug.LogWarning("❌ Cannot start!  Characters or stage not selected");
            return;
        }
        
        if (isTransitioning) return;
        
        isTransitioning = true;
        Debug.Log("🚀 Starting fight!");
        CharacterManager.instance.LaunchFight();
    }

    void UpdateSelectionUI() 
    {
        if (p1Border == null || p2Border == null || p1FullSpriteDisplay == null || p2FullSpriteDisplay == null) 
            return;

        p1Border.transform.position = buttonPositions[p1HoverIndex]. position;
        p2Border. transform.position = buttonPositions[p2HoverIndex].position;
        
        p1FullSpriteDisplay.sprite = allCharacterSprites[p1HoverIndex];
        p2FullSpriteDisplay.sprite = allCharacterSprites[p2HoverIndex];

        // Constant reinforcement of the P2 flip
        p2FullSpriteDisplay.rectTransform. localScale = new Vector3(-1, 1, 1);
    }
    
    void UpdateStatusText()
    {
        if (selectionStatusText == null) return;
        
        if (! p1Locked || !p2Locked)
        {
            selectionStatusText.text = "Select Characters\nP1: E to Lock | P2: Enter to Lock";
        }
        else if (! stageSelected)
        {
            selectionStatusText.text = "Select a Stage";
        }
        else
        {
            selectionStatusText. text = "Press START to Fight! ";
        }
    }
}