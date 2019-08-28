using System;
using UnityEngine;

[Serializable]
public struct CharacterInScene
{
    public string CharacterName;
    public GameObject CharacterModel;
    public Material HeaderMaterial;
}

internal class CharacterManager
{
    internal CharacterInScene Character { get; private set; }
    internal int CurrentScore { get; set; }

    internal CharacterManager(CharacterInScene characterInScene)
    {
        Character = characterInScene;
        CurrentScore = 0;
    }

    internal void LoadAnimation(string characterAnimation)
    {
        AnimationClip animationClip = Resources.Load<AnimationClip>($"Animations Legacy/m01@{characterAnimation}");
        if (animationClip != null)
        {
            Character.CharacterModel.GetComponent<Animation>().AddClip(animationClip, characterAnimation);
            Character.CharacterModel.GetComponent<Animation>().CrossFade(characterAnimation);
        }
    }
}