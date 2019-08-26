using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal struct DialogueLine
{
    internal string CharacterName { get; set; }
    internal string DialogueText { get; set; }
    internal string CharacterAnimation { get; set; }
}

public class DialogueManager : MonoBehaviour
{
    public List<CharacterInScene> charactersInScene;
    public GameObject headerTextBox;
    public GameObject dialogueTextBox;
    public GameObject inputPromptBox;
    public TextAsset textFile;

    internal int currentScore;

    private int currentLineIndex;
    private string[] textLines;
    private int endAtLine;
    private int jumpToLine;
    private bool cameraMoved;
    private TextBoxManager textBoxManager;
    private CameraManager cameraManager;
    private InputPromptManager inputPromptManager;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        currentScore = 0;
        endAtLine = 0;
        jumpToLine = 0;

        textBoxManager = new TextBoxManager(headerTextBox, dialogueTextBox, charactersInScene);

        cameraManager = Camera.main.GetComponent<CameraManager>();

        inputPromptManager = new InputPromptManager(inputPromptBox);
        inputPromptManager.SetInputPromptEnabled(false);

        HandleCurrentLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputPromptManager.InputPromptEnabled)
        {
            _ = StartCoroutine(inputPromptManager.GetSelection());
            currentLineIndex = inputPromptManager.SelectedOption.StartAtLine - 1;
            currentScore += inputPromptManager.SelectedOption.ScoreIncrement;
            endAtLine = inputPromptManager.SelectedOption.EndAtLine;
            jumpToLine = inputPromptManager.SelectedOption.JumpToLine - 1;
        }
        if (Input.GetMouseButtonDown(0) || cameraMoved)
        {
            cameraMoved = false;
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
        string[] lineBlock = textLines[currentLineIndex].Split('|');
        if (lineBlock[0] == "CAMERA")
        {
            cameraManager.InitializeCameraDelta(lineBlock[1].Split(','), lineBlock[2].Split(','));
            cameraManager.ChangeCameraLocation(0.75f);
            cameraMoved = true;
        }
        else if (lineBlock[0] == "INPUT")
        {
            inputPromptManager.SetInputPromptEnabled(true);
            inputPromptManager.InitializeInputPrompt(lineBlock);
        }
        else
        {
            textBoxManager.DeclareDialogueLine(lineBlock);
        }
    }
}
