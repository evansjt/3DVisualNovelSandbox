using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal struct DialogueLine
{
    internal string CharacterName { get; set; }
    internal string DialogueText { get; set; }
    internal string CharacterAnimation { get; set; }
}

internal class TextBoxManager : ILineManager
{

    private DialogueLine line;
    internal GameObject HeaderTextBox { get; private set; }
    internal GameObject DialogueTextBox { get; private set; }
    internal List<CharacterManager> CharacterManagers { get; set; }

    internal TextBoxManager(GameObject headerTextBox, GameObject dialogueTextBox, List<CharacterManager> characterManagers)
    {
        HeaderTextBox = headerTextBox;
        DialogueTextBox = dialogueTextBox;
        CharacterManagers = characterManagers;
    }

    public void Action(string[] lineBlock)
    {
        DeclareDialogueLine(lineBlock);
    }

    internal void DeclareDialogueLine(string[] lineBlock)
    {
        line = new DialogueLine
        {
            CharacterName = lineBlock[1],
            DialogueText = lineBlock[2],
            CharacterAnimation = lineBlock[3]
        };
        DisplayNewDialogue();
    }

    private void DisplayNewDialogue()
    {
        CharacterManager characterManager = CharacterManagers.Find(c => c.CharacterName == line.CharacterName);

        CreateHeader(characterManager);

        ChangeTextInTextBox(DialogueTextBox, line.DialogueText);

        if (line.CharacterAnimation != "")
        {
            characterManager.LoadAndPlayAnimation(line.CharacterAnimation);
        }
    }

    private void CreateHeader(CharacterManager character)
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