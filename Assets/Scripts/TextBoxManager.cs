using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class TextBoxManager
{
    private readonly CharacterManager characterManager;
    private DialogueLine line;
    internal GameObject HeaderTextBox { get; private set; }
    internal GameObject DialogueTextBox { get; private set; }

    internal TextBoxManager(GameObject headerTextBox, GameObject dialogueTextBox, List<CharacterInScene> charactersInScene)
    {
        HeaderTextBox = headerTextBox;
        DialogueTextBox = dialogueTextBox;
        characterManager = new CharacterManager(charactersInScene);
    }

    internal void DeclareDialogueLine(string[] lineBlock)
    {
        line = new DialogueLine
        {
            CharacterName = lineBlock[0],
            DialogueText = lineBlock[1],
            CharacterAnimation = lineBlock[2]
        };
        DisplayNewDialogue();
    }

    private void DisplayNewDialogue()
    {
        CharacterInScene character = characterManager.CharactersInScene.Find(c => c.CharacterName == line.CharacterName);

        CreateHeader(character);

        ChangeTextInTextBox(DialogueTextBox, line.DialogueText);

        if (line.CharacterAnimation != "")
        {
            characterManager.LoadAnimation(character, line.CharacterAnimation);
        }
    }

    private void CreateHeader(CharacterInScene character)
    {
        HeaderTextBox.GetComponent<MeshRenderer>().material = character.HeaderMaterial;
        ChangeTextInTextBox(HeaderTextBox, line.CharacterName);
    }

    private void ChangeTextInTextBox(GameObject thisGameObject, string toText)
    {
        GameObject textObject = thisGameObject.transform.GetChild(0).gameObject;
        textObject.GetComponent<TextMeshProUGUI>().text = toText;
    }
}