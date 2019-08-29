using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

internal class MoveCharacterManager : ILineManager
{
    private List<CharacterManager> characterManagers;
    private CharacterManager currentCharacter;

    internal Vector3 PositionDelta { get; private set; }
    internal Vector3 RotationDelta { get; private set; }

    public MoveCharacterManager(List<CharacterManager> characterManagers)
    {
        this.characterManagers = characterManagers;
    }

    public void Action(string[] lineBlock)
    {
        currentCharacter = characterManagers.Find(c => c.CharacterName == lineBlock[1]);
        currentCharacter.LoadAndPlayAnimation(lineBlock[5]);
        InitializeCharacterDelta(lineBlock[2].Split(','), lineBlock[3].Split(','));
        currentCharacter.ChangeCharacterLocation(PositionDelta, RotationDelta, lineBlock[6], float.Parse(lineBlock[4]));
    }

    internal void InitializeCharacterDelta(string[] stringsOfTranslation, string[] stringsOfRotation)
    {
        PositionDelta = ParseStringArrayToVector3(stringsOfTranslation);
        RotationDelta = ParseStringArrayToVector3(stringsOfRotation);
    }

    private Vector3 ParseStringArrayToVector3(string[] positions)
    {
        Vector3 newVector = new Vector3
        {
            x = float.Parse(positions[0], new NumberFormatInfo { NegativeSign = "-" }),
            y = float.Parse(positions[1], new NumberFormatInfo { NegativeSign = "-" }),
            z = float.Parse(positions[2], new NumberFormatInfo { NegativeSign = "-" })
        };
        return newVector;
    }

}