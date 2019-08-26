using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public struct CharacterInScene
{
    public string CharacterName;
    public GameObject CharacterModel;
    public Material HeaderMaterial;
}

internal class CharacterManager
{
    internal List<CharacterInScene> CharactersInScene { get; private set; }

    internal CharacterManager(List<CharacterInScene> charactersInScene)
    {
        CharactersInScene = charactersInScene;
    }

    internal void LoadAnimation(CharacterInScene character, string characterAnimation)
    {
        string path = $"Assets/TaichiCharacterPack/Resources/Taichi/Animations Legacy/m01@{characterAnimation}.fbx";

        Object[] assetRepresentationsAtPath = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
        foreach (Object assetRepresentation in assetRepresentationsAtPath)
        {
            AnimationClip animationClip = assetRepresentation as AnimationClip;
            if (animationClip != null)
            {
                character.CharacterModel.GetComponent<Animation>().AddClip(animationClip, characterAnimation);
                character.CharacterModel.GetComponent<Animation>().CrossFade(characterAnimation);
            }
        }
    }
}