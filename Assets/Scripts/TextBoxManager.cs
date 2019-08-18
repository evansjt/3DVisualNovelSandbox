using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static TextBoxStructs;

public class TextBoxManager : MonoBehaviour
{
    public List<CharacterInScene> charactersInScene;

    public HeaderTextBox headerTextBox;

    public DialogueTextBox dialogueTextBox;

    public TextAsset textFile;

    public int endAtLine;

    private int currentLineIndex;
    private string[] textLines;
    private DialogueLine line;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        LoadNewDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            LoadNewDialogue();
        }
    }

    private void LoadNewDialogue()
    {
        if (currentLineIndex == endAtLine)
        {
            currentLineIndex = 0;
            EditorApplication.isPlaying = false;
            Application.Quit();
        }
        if (textLines.Length > 0)
        {
            DeclareDialogueLine();
            DisplayNewDialogue();
        }
    }

    private void DeclareDialogueLine()
    {
        string[] lineBlock = textLines[currentLineIndex].Split('|');
        line = new DialogueLine
        {
            CharacterName = lineBlock[0],
            DialogueText = lineBlock[1],
            CharacterAnimation = lineBlock[2]
        };
    }

    private void DisplayNewDialogue()
    {
        CharacterInScene character = charactersInScene.Find(c => c.CharacterName == line.CharacterName);

        CreateHeader(character);
        dialogueTextBox.dialogueText.text = line.DialogueText;

        if (line.CharacterAnimation != "")
        {
            CreateAnimation(character);
        }
    }

    private void CreateAnimation(CharacterInScene character)
    {
        string path = $"Assets/TaichiCharacterPack/Resources/Taichi/Animations Legacy/m01@{line.CharacterAnimation}.fbx";

        Object[] assetRepresentationsAtPath = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
        foreach (Object assetRepresentation in assetRepresentationsAtPath)
        {
            AnimationClip animationClip = assetRepresentation as AnimationClip;
            if (animationClip != null)
            {
                character.CharacterModel.GetComponent<Animation>().AddClip(animationClip, line.CharacterAnimation);
                character.CharacterModel.GetComponent<Animation>().CrossFade(line.CharacterAnimation);
            }
        }
    }

    private void CreateHeader(CharacterInScene character)
    {
        headerTextBox.headerText.text = line.CharacterName;
        headerTextBox.headerTextBoxObject.GetComponent<MeshRenderer>().material = character.HeaderMaterial;
    }
}
