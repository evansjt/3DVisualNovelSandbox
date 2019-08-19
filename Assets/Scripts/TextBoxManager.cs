﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using static TextBoxStructs;
using Object = System.Object;

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
    private CameraDelta cameraDelta;
    private bool translating;
    private bool rotating;

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

        AdvanceLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentLineIndex++;
            AdvanceLine();
        }
    }

    private void AdvanceLine()
    {
        if (currentLineIndex == endAtLine)
        {
            currentLineIndex = 0;
            EditorApplication.isPlaying = false;
            Application.Quit();
        }
        if (textLines.Length > 0)
        {
            string[] lineBlock = textLines[currentLineIndex].Split('|');
            if (lineBlock[0] == "camera")
            {
                cameraDelta = new CameraDelta
                {
                    PositionDelta = ParseStringArrayToVector3(lineBlock[1].Split(',')),
                    RotationDelta = ParseStringArrayToVector3(lineBlock[2].Split(','))
                };
                ChangeCameraLocation(0.75f);
            }
            else
            {
                DeclareDialogueLine(lineBlock);
            }
        }
    }

    private Vector3 ParseStringArrayToVector3(string[] positions)
    {
        var fmt = new NumberFormatInfo
        {
            NegativeSign = "-"
        };

        Vector3 newVector = new Vector3
        {
            x = float.Parse(positions[0], fmt),
            y = float.Parse(positions[1], fmt),
            z = float.Parse(positions[2], fmt)
        };
        return newVector;
    }

    private void ChangeCameraLocation(float speed)
    {
        if (cameraDelta.PositionDelta != Vector3.zero)
        {
            StartCoroutine(TranslateCamera(speed));
        }
        if (cameraDelta.RotationDelta != Vector3.zero)
        {
            StartCoroutine(RotateCamera(speed));
        }

        currentLineIndex++;
        AdvanceLine();
    }

    IEnumerator TranslateCamera(float duration)
    {
        if (translating)
        {
            yield break;
        }
        translating = true;

        Vector3 currentPos = Camera.main.transform.position;
        Vector3 newPos = new Vector3
        {
            x = currentPos.x + cameraDelta.PositionDelta.x,
            y = currentPos.y + cameraDelta.PositionDelta.y,
            z = currentPos.z + cameraDelta.PositionDelta.z
        };

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            Camera.main.transform.position = Vector3.Lerp(currentPos, newPos, counter / duration);
            yield return null;
        }
        translating = false;
    }

    IEnumerator RotateCamera(float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = Camera.main.transform.rotation;
        Quaternion newRot = Quaternion.Euler(currentRot.eulerAngles.x + cameraDelta.RotationDelta.x,
                                             currentRot.eulerAngles.y + cameraDelta.RotationDelta.y,
                                             currentRot.eulerAngles.z + cameraDelta.RotationDelta.z);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            Camera.main.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    private void DeclareDialogueLine(string[] lineBlock)
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
        CharacterInScene character = charactersInScene.Find(c => c.CharacterName == line.CharacterName);

        CreateHeader(character);
        dialogueTextBox.dialogueText.text = line.DialogueText;

        if (line.CharacterAnimation != "")
        {
            LoadAnimation(character);
        }
    }

    private void LoadAnimation(CharacterInScene character)
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
