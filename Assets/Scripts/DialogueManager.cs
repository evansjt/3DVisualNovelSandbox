using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour, ILineManager
{
    public List<CharacterInScene> charactersInScene;
    public GameObject headerTextBox;
    public GameObject dialogueTextBox;
    public GameObject inputPromptBox;
    public TextAsset textFile;

    private string[] textLines;
    private string[] currentLineBlock;
    private int currentLineIndex;
    private int endAtLine;
    private int jumpToLine;
    private List<CharacterManager> characterManagers;
    private Dictionary<string, ILineManager> lineManagers;

    // Start is called before the first frame update
    void Start()
    {
        InitializeTextLines(0);

        InitializeLineManagers();

        HandleCurrentLine();
    }

    private void InitializeTextLines(int currentLine)
    {
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        currentLineIndex = currentLine;
        endAtLine = 0;
        jumpToLine = 0;
    }

    private void InitializeLineManagers()
    {
        lineManagers = new Dictionary<string, ILineManager>();
        characterManagers = new List<CharacterManager>();
        foreach (var character in charactersInScene)
        {
            characterManagers.Add(new CharacterManager(character));
        }

        lineManagers.Add("CAMERA", Camera.main.GetComponent<CameraManager>());
        lineManagers.Add("INPUT", new InputPromptManager(inputPromptBox));
        lineManagers.Add("TEXT", new TextBoxManager(headerTextBox, dialogueTextBox, characterManagers));
        lineManagers.Add("SCORE", new ScoreManager(characterManagers));
        lineManagers.Add("JUMP", this);
    }

    // Update is called once per frame
    void Update()
    {
        if (((InputPromptManager)lineManagers["INPUT"]).InputPromptEnabled)
        {
            _ = StartCoroutine(((InputPromptManager)lineManagers["INPUT"]).GetSelection());
            currentLineIndex = ((InputPromptManager)lineManagers["INPUT"]).SelectedOption.StartAtLine - 1;
            endAtLine = ((InputPromptManager)lineManagers["INPUT"]).SelectedOption.EndAtLine;
            jumpToLine = ((InputPromptManager)lineManagers["INPUT"]).SelectedOption.JumpToLine - 1;
        }

        if (Input.GetMouseButtonDown(0) || (currentLineBlock[0] != "TEXT" && currentLineBlock[0] != "INPUT"))
        {
            currentLineIndex++;
            HandleCurrentLine();
        }
    }

    private void HandleCurrentLine()
    {
        if (currentLineIndex == textLines.Length)
        {
            EditorApplication.isPlaying = false;
            Application.Quit();
            return;
        }
        if (textLines.Length > 0)
        {
            AdvanceLine();
        }
        if (currentLineIndex != 0 && currentLineIndex == endAtLine)
        {
            currentLineIndex = jumpToLine;
        }
    }

    private void AdvanceLine()
    {
        currentLineBlock = textLines[currentLineIndex].Split('|');
        foreach (var manager in lineManagers)
        {
            if (currentLineBlock[0] == manager.Key)
            {
                manager.Value.Action(currentLineBlock);
                break;
            }
        }
    }

    public void Action(string[] lineBlock)
    {
        textFile = Resources.Load<TextAsset>($"Text/{lineBlock[1]}");
        InitializeTextLines(int.Parse(lineBlock[2])-1);
        HandleCurrentLine();
    }
}
