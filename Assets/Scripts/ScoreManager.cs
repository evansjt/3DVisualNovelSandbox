using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

internal class ScoreManager : ILineManager
{
    private List<CharacterManager> characterManagers;

    public ScoreManager(List<CharacterManager> characterManagers)
    {
        this.characterManagers = characterManagers;
    }

    public void Action(string[] lineBlock)
    {
        foreach (var characterManager in characterManagers)
        {
            if (lineBlock[1] == characterManager.Character.CharacterName)
            {
                int scoreInc = int.Parse(lineBlock[2], new NumberFormatInfo { NegativeSign = "-" });
                characterManager.CurrentScore += scoreInc;
            }
        }
    }
}