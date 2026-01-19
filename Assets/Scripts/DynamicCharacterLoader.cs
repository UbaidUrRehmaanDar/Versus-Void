using UnityEngine;

public class DynamicCharacterLoader : MonoBehaviour 
{
    public Sprite[] allCharacterSprites; 
    public RuntimeAnimatorController[] allControllers;

    void Awake() 
    {
        int pID = GetComponent<PlayerController>().playerID;
        int charID = (pID == 1) ? CharacterManager.instance.p1SelectedCharacter : CharacterManager.instance.p2SelectedCharacter;

        // Default to 0 if none selected
        if (charID == -1) charID = 0;

        GetComponent<SpriteRenderer>().sprite = allCharacterSprites[charID];
        GetComponent<Animator>().runtimeAnimatorController = allControllers[charID];
    }
}