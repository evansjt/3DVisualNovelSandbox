using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

internal struct Option
{
    internal string OptionText { get; set; }
    internal int ScoreIncrement { get; set; }
    internal int StartAtLine { get; set; }
    internal int EndAtLine { get; set; }
    internal int JumpToLine { get; set; }
}

internal class InputPromptManager
{
    private readonly List<Option> optionsList;
    internal Option SelectedOption { get; private set; }
    internal GameObject InputPromptBox { get; private set; }
    public bool InputPromptEnabled { get; private set; }

    internal InputPromptManager(GameObject inputPromptBox)
    {
        InputPromptBox = inputPromptBox;
        optionsList = new List<Option>();
    }

    internal void InitializeInputPrompt(string[] lineBlock)
    {
        for (int i = 1; i < lineBlock.Length; i++)
        {
            string[] optionBlock = lineBlock[i].Split('=');
            int scoreInc = int.Parse(optionBlock[1], new NumberFormatInfo { NegativeSign = "-" });
            int startAtLine = int.Parse(optionBlock[2]) - 1;
            int endAtLine = int.Parse(optionBlock[3]) - 1;
            int jumpToLine = int.Parse(optionBlock[4]) - 1;
            optionsList.Add(new Option
            {
                OptionText = optionBlock[0],
                ScoreIncrement = scoreInc,
                StartAtLine = startAtLine,
                EndAtLine = endAtLine,
                JumpToLine = jumpToLine
            });
        }

        ShuffleList(optionsList);

        for (int i = 0; i < InputPromptBox.transform.childCount; i++)
        {
            InputPromptBox.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = optionsList[i].OptionText;
        }
    }

    internal void SetInputPromptEnabled(bool isEnabled)
    {
        InputPromptEnabled = isEnabled;
        if (InputPromptBox != null)
        {
            InputPromptBox.GetComponent<Renderer>().enabled = isEnabled;
        }
        for (int i = 0; i < InputPromptBox.transform.childCount; i++)
        {
            InputPromptBox.transform.GetChild(i).gameObject.GetComponentInChildren<Renderer>().enabled = isEnabled;
            InputPromptBox.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = isEnabled;
        }
    }

    internal IEnumerator GetSelection()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                string selection = null;
                for (int i = 0; i < InputPromptBox.transform.childCount; i++)
                {
                    selection = InputPromptBox.transform.GetChild(i).GetComponent<OptionListener>().GetSelectedOptionText();
                    if (selection != null)
                    {
                        SelectedOption = optionsList.Find(s => s.OptionText == selection);
                        SetInputPromptEnabled(false);
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}