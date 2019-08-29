using System;
using System.Collections;
using UnityEngine;

internal class CharacterManager : MonoBehaviour
{
    [SerializeField] public string CharacterName;
    [SerializeField] public Material HeaderMaterial;
    internal int CurrentScore { get; set; }

    private bool translating;
    private bool rotating;
    private bool animating;

    internal void ChangeCharacterLocation(Vector3 positionDelta, Vector3 rotationDelta, string endAnimation, float speed)
    {
        if (positionDelta != Vector3.zero)
        {
            _ = StartCoroutine(TranslateCharacter(positionDelta, speed));
        }
        if (rotationDelta != Vector3.zero)
        {
            _ = StartCoroutine(RotateCharacter(rotationDelta, speed));
        }
        AnimationClip endAnimationClip = Resources.Load<AnimationClip>($"Animations Legacy/m01@{endAnimation}");
        if (endAnimationClip != null)
        {
            gameObject.GetComponent<Animation>().AddClip(endAnimationClip, endAnimation);
            _ = StartCoroutine(AnimationCoroutine(endAnimationClip, speed));
        }
    }

    private IEnumerator TranslateCharacter(Vector3 positionDelta, float duration)
    {
        if (translating)
        {
            yield break;
        }
        translating = true;

        Vector3 currentPos = gameObject.transform.position;
        Vector3 newPos = new Vector3
        {
            x = currentPos.x + positionDelta.x,
            y = currentPos.y + positionDelta.y,
            z = currentPos.z + positionDelta.z
        };

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(currentPos, newPos, counter / duration);
            yield return null;
        }
        translating = false;
    }

    private IEnumerator RotateCharacter(Vector3 rotationDelta, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = gameObject.transform.rotation;
        Quaternion newRot = Quaternion.Euler(currentRot.eulerAngles.x + rotationDelta.x,
                                             currentRot.eulerAngles.y + rotationDelta.y,
                                             currentRot.eulerAngles.z + rotationDelta.z);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObject.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    internal IEnumerator AnimationCoroutine(AnimationClip animationClip, float duration)
    {
        if (animating)
        {
            yield break;
        }
        animating = true;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<Animation>().CrossFade(animationClip.name);
        animating = false;
    }

    internal void LoadAndPlayAnimation(string characterAnimation)
    {
        AnimationClip animationClip = Resources.Load<AnimationClip>($"Animations Legacy/m01@{characterAnimation}");
        if (animationClip != null)
        {
            gameObject.GetComponent<Animation>().AddClip(animationClip, characterAnimation);
            gameObject.GetComponent<Animation>().CrossFade(characterAnimation);
        }
    }
}