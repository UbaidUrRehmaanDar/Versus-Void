using UnityEngine;

public class DynamicCharacterLoader : MonoBehaviour 
{
    public Sprite[] allCharacterSprites; 
    public RuntimeAnimatorController[] allControllers;

    void Awake() 
    {
        int charID = 0;
        bool isCampaignMode = false;
        
        // CHECK 1: Is this Campaign Mode?
        if (CampaignManager.instance != null)
        {
            isCampaignMode = true;
            Debug.Log("[" + gameObject.name + "] Campaign Mode detected!");
            
            // Check if this is Player or AI
            PlayerController pc = GetComponent<PlayerController>();
            AIController ai = GetComponent<AIController>();
            
            if (pc != null)
            {
                // This is the human player
                charID = CampaignManager.instance.playerSelectedCharacter;
                Debug.Log("[" + gameObject.name + "] Loading PLAYER character:   " + charID);
            }
            else if (ai != null)
            {
                // This is the AI opponent
                charID = CampaignManager. instance.GetCurrentOpponentCharacterID();
                Debug.Log("[" + gameObject.name + "] Loading AI OPPONENT character:  " + charID);
            }
        }
        // CHECK 2: Is this 1v1 Mode?
        else if (CharacterManager.instance != null)
        {
            Debug.Log("[" + gameObject. name + "] 1v1 Mode detected!");
            
            PlayerController pc = GetComponent<PlayerController>();
            if (pc == null)
            {
                Debug.LogError("[" + gameObject.name + "] No PlayerController found in 1v1 mode!");
                return;
            }
            
            charID = (pc.playerID == 1) ? CharacterManager. instance.p1SelectedCharacter : CharacterManager.instance.p2SelectedCharacter;
            Debug.Log("[" + gameObject.name + "] Loading character for Player " + pc.playerID + ": " + charID);
        }
        // CHECK 3: No manager found - use default
        else
        {
            Debug.LogWarning("[" + gameObject.name + "] No CharacterManager or CampaignManager found!  Using default character (index 0).");
            charID = 0;
        }

        // Default to 0 if invalid
        if (charID == -1 || charID < 0) 
        {
            Debug.LogWarning("[" + gameObject.name + "] Invalid character ID (" + charID + "), defaulting to 0");
            charID = 0;
        }

        // Apply sprite
        if (allCharacterSprites != null && charID < allCharacterSprites.Length && allCharacterSprites[charID] != null)
        {
            GetComponent<SpriteRenderer>().sprite = allCharacterSprites[charID];
            Debug.Log("[" + gameObject.name + "] ✅ Loaded sprite for character " + charID);
        }
        else
        {
            Debug.LogError("[" + gameObject.name + "] ❌ Failed to load sprite for character " + charID);
        }
        
        // Apply animator
        if (allControllers != null && charID < allControllers.Length && allControllers[charID] != null)
        {
            GetComponent<Animator>().runtimeAnimatorController = allControllers[charID];
            Debug. Log("[" + gameObject.name + "] ✅ Loaded animator for character " + charID);
        }
        else
        {
            Debug.LogError("[" + gameObject.name + "] ❌ Failed to load animator for character " + charID);
        }
    }
}