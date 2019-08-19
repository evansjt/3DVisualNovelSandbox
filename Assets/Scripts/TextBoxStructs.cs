using System;
using UnityEngine;
using TMPro;

public class TextBoxStructs
{
    [Serializable]
    public struct CharacterInScene
    {
	    public string CharacterName;
	    public GameObject CharacterModel;
	    public Material HeaderMaterial;
    }

    [Serializable]
    public struct HeaderTextBox
    {
	    public GameObject headerTextBoxObject;
	    public TextMeshProUGUI headerText;
    }

    [Serializable]
    public struct DialogueTextBox
    {
	    public GameObject dialogueTextBoxObject;
	    public TextMeshProUGUI dialogueText;
    }

    public struct DialogueLine
    {
        internal string CharacterName { get; set; }
	    internal string DialogueText { get; set; }
	    internal string CharacterAnimation { get; set; }
    }

    public struct CameraDelta
    {
        internal Vector3 PositionDelta { get; set; }
        internal Vector3 RotationDelta { get; set; }
    }
}


