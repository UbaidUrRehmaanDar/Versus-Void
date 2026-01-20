using UnityEngine;

public class DynamicCharacterLoader : MonoBehaviour 
{
    public Sprite[] allCharacterSprites; 
    public RuntimeAnimatorController[] allControllers;

    void Awake() 
    {
        int charID = 0;
        
        // TRY CAMPAIGN MODE FIRST
        if (CampaignManager.instance != null)
        {
            Debug.Log("🎮 [" + gameObject.name + "] CampaignManager found!");
            
            PlayerController pc = GetComponent<PlayerController>();
            AIController ai = GetComponent<AIController>();
            
            if (pc != null)
            {
                // Human player
                charID = CampaignManager.instance.playerSelectedCharacter;
                Debug.Log("👤 [" + gameObject. name + "] I'm the PLAYER!  Loading character ID: " + charID);
            }
            else if (ai != null)
            {
                // AI opponent
                charID = CampaignManager.instance.GetCurrentOpponentCharacterID();
                Debug.Log("🤖 [" + gameObject.name + "] I'm the AI!  Loading character ID: " + charID);
            }
            else
            {
                Debug.LogError("❌ [" + gameObject.name + "] Has DynamicCharacterLoader but no PlayerController or AIController!");
            }
        }
        // TRY 1V1 MODE
        else if (CharacterManager. instance != null)
        {
            Debug.Log("⚔️ [" + gameObject.name + "] CharacterManager found (1v1 mode)");
            
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null)
            {
                charID = (pc.playerID == 1) ? CharacterManager.instance.p1SelectedCharacter : CharacterManager.instance.p2SelectedCharacter;
                Debug.Log("👤 [" + gameObject. name + "] Player " + pc.playerID + " loading character ID: " + charID);
            }
        }
        // NO MANAGER
        else
        {
            Debug.LogWarning("⚠️ [" + gameObject.name + "] No manager found! Using character 0");
            charID = 0;
        }

        // Validate
        if (charID < 0 || charID >= allCharacterSprites.Length)
        {
            Debug.LogWarning("⚠️ [" + gameObject.name + "] Invalid charID " + charID + ", using 0");
            charID = 0;
        }

        // APPLY SPRITE
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && allCharacterSprites != null && charID < allCharacterSprites.Length && allCharacterSprites[charID] != null)
        {
            sr.sprite = allCharacterSprites[charID];
            Debug.Log("✅ [" + gameObject.name + "] Sprite loaded for character " + charID);
        }
        else
        {
            Debug.LogError("❌ [" + gameObject.name + "] FAILED to load sprite for character " + charID);
        }
        
        // APPLY ANIMATOR
        Animator anim = GetComponent<Animator>();
        if (anim != null && allControllers != null && charID < allControllers.Length && allControllers[charID] != null)
        {
            anim.runtimeAnimatorController = allControllers[charID];
            Debug.Log("✅ [" + gameObject.name + "] Animator loaded for character " + charID);
        }
        else
        {
            Debug.LogError("❌ [" + gameObject.name + "] FAILED to load animator for character " + charID);
        }
    }
}