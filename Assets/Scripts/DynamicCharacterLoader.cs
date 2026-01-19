using UnityEngine;

public class DynamicCharacterLoader : MonoBehaviour 
{
    public Sprite[] allCharacterSprites; 
    public RuntimeAnimatorController[] allControllers;

    void Awake() 
    {
        // Safety check:  If CharacterManager doesn't exist, use defaults
        if (CharacterManager. instance == null)
        {
            Debug.LogWarning("[" + gameObject.name + "] CharacterManager not found! Using default character (index 0).");
            
            if (allCharacterSprites != null && allCharacterSprites.Length > 0)
                GetComponent<SpriteRenderer>().sprite = allCharacterSprites[0];
            
            if (allControllers != null && allControllers.Length > 0)
                GetComponent<Animator>().runtimeAnimatorController = allControllers[0];
            
            return;
        }
        
        PlayerController pc = GetComponent<PlayerController>();
        if (pc == null)
        {
            Debug.LogError("[" + gameObject.name + "] No PlayerController found!");
            return;
        }
        
        int charID = (pc.playerID == 1) ? CharacterManager.instance.p1SelectedCharacter : CharacterManager.instance.p2SelectedCharacter;

        // Default to 0 if none selected
        if (charID == -1) charID = 0;

        if (allCharacterSprites != null && charID < allCharacterSprites. Length)
            GetComponent<SpriteRenderer>().sprite = allCharacterSprites[charID];
        
        if (allControllers != null && charID < allControllers. Length)
            GetComponent<Animator>().runtimeAnimatorController = allControllers[charID];
    }
}