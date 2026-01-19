using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public Sprite[] characterSprites; // Drag your 4 pixel character sprites here
    public RuntimeAnimatorController[] controllers; // Drag their Animator Controllers here

    void Start()
    {
        int choice = CharacterManager.instance.p1SelectedCharacter;
        
        // Update the visual
        GetComponent<SpriteRenderer>().sprite = characterSprites[choice];
        
        // Update the animations
        GetComponent<Animator>().runtimeAnimatorController = controllers[choice];
    }
}