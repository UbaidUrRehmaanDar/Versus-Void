using UnityEngine;
using UnityEngine.UI;

public class MenuSelectionSystem : MonoBehaviour
{
    public int p1HoverIndex = 0;
    public int p2HoverIndex = 1;
    public Image p1Border; 
    public Image p2Border;
    public Image p1FullSpriteDisplay; 
    public Image p2FullSpriteDisplay; 
    public RectTransform[] buttonPositions; 
    public Sprite[] allCharacterSprites;

    private bool p1Locked = false;
    private bool p2Locked = false;
    private bool isTransitioning = false;

    void Awake() {
        // Force the flip BEFORE the first frame is even rendered
        if (p2FullSpriteDisplay != null) {
            p2FullSpriteDisplay.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Start() {
        UpdateSelectionUI();
    }

    void Update() {
        if (isTransitioning || CharacterManager.instance == null) return;

        if (!p1Locked) {
            if (Input.GetKeyDown(KeyCode.D)) p1HoverIndex = (p1HoverIndex + 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.A)) p1HoverIndex = (p1HoverIndex + buttonPositions.Length - 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.E)) {
                p1Locked = true;
                CharacterManager.instance.p1SelectedCharacter = p1HoverIndex;
            }
        }

        if (!p2Locked) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) p2HoverIndex = (p2HoverIndex + 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) p2HoverIndex = (p2HoverIndex + buttonPositions.Length - 1) % buttonPositions.Length;
            if (Input.GetKeyDown(KeyCode.Return)) {
                p2Locked = true;
                CharacterManager.instance.p2SelectedCharacter = p2HoverIndex;
            }
        }

        if (p1Locked && p2Locked && !isTransitioning) {
            isTransitioning = true;
            CharacterManager.instance.LaunchFight();
        }

        if (!isTransitioning) UpdateSelectionUI();
    }

    void UpdateSelectionUI() {
        if (p1Border == null || p2Border == null || p1FullSpriteDisplay == null || p2FullSpriteDisplay == null) return;

        p1Border.transform.position = buttonPositions[p1HoverIndex].position;
        p2Border.transform.position = buttonPositions[p2HoverIndex].position;
        
        p1FullSpriteDisplay.sprite = allCharacterSprites[p1HoverIndex];
        p2FullSpriteDisplay.sprite = allCharacterSprites[p2HoverIndex];

        // Constant reinforcement of the P2 flip
        p2FullSpriteDisplay.rectTransform.localScale = new Vector3(-1, 1, 1);
    }
}