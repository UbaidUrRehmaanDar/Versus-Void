using UnityEngine;

public class DynamicCharacterLoader : MonoBehaviour 
{
    public Sprite[] allCharacterSprites; 
    public RuntimeAnimatorController[] allControllers;

    void Awake() 
    {
        int charID = 0;
        
        Debug.Log("==========================================");
        Debug.Log("🎭 DynamicCharacterLoader on:  " + gameObject.name);
        
        if (CampaignManager.instance != null)
        {
            Debug. Log(" CampaignManager found!");
            
            PlayerController pc = GetComponent<PlayerController>();
            AIController ai = GetComponent<AIController>();
            
            if (pc != null)
            {
                charID = CampaignManager.instance.playerSelectedCharacter;
                Debug. Log(" I'm PLAYER!  Loading character: " + charID);
            }
            else if (ai != null)
            {
                charID = CampaignManager.instance.GetCurrentOpponentCharacterID();
                Debug.Log(" I'm AI! Loading character: " + charID);
            }
            else
            {
                Debug.LogError(" No PlayerController or AIController!");
            }
        }
        else if (CharacterManager.instance != null)
        {
            Debug. Log(" CharacterManager found (1v1 mode)");
            
            PlayerController pc = GetComponent<PlayerController>();
            if (pc != null)
            {
                charID = (pc.playerID == 1) ? CharacterManager.instance.p1SelectedCharacter : CharacterManager.instance.p2SelectedCharacter;
                Debug.Log(" Player " + pc.playerID + " loading:  " + charID);
            }
        }
        else
        {
            Debug.LogWarning(" No manager!  Using character 0");
            charID = 0;
        }

        if (charID < 0 || charID >= allCharacterSprites.Length)
        {
            Debug.LogWarning(" Invalid charID " + charID + ", using 0");
            charID = 0;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && allCharacterSprites != null && charID < allCharacterSprites. Length && allCharacterSprites[charID] != null)
        {
            sr.sprite = allCharacterSprites[charID];
            Debug.Log(" Sprite loaded: " + allCharacterSprites[charID]. name);
        }
        else
        {
            Debug.LogError(" FAILED to load sprite!");
        }
        
        Animator anim = GetComponent<Animator>();
        if (anim != null && allControllers != null && charID < allControllers.Length && allControllers[charID] != null)
        {
            anim.runtimeAnimatorController = allControllers[charID];
            Debug.Log(" Animator loaded: " + allControllers[charID].name);
        }
        else
        {
            Debug.LogError(" FAILED to load animator!");
        }
        
    }
}