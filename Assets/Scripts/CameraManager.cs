﻿using System.Collections;
using System.Globalization;
using UnityEngine;

internal class CameraManager : MonoBehaviour
{
    private bool translating;
    private bool rotating;
    internal Vector3 PositionDelta { get; private set; }
    internal Vector3 RotationDelta { get; private set; }

    internal void InitializeCameraDelta(string[] stringsOfTranslation, string[] stringsOfRotation)
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

    internal void ChangeCameraLocation(float speed)
    {
        if (PositionDelta != Vector3.zero)
        {
            _ = StartCoroutine(TranslateCamera(speed));
        }
        if (RotationDelta != Vector3.zero)
        {
            _ = StartCoroutine(RotateCamera(speed));
        }
    }

    private IEnumerator TranslateCamera(float duration)
    {
        if (translating)
        {
            yield break;
        }
        translating = true;

        Vector3 currentPos = Camera.main.transform.position;
        Vector3 newPos = new Vector3
        {
            x = currentPos.x + PositionDelta.x,
            y = currentPos.y + PositionDelta.y,
            z = currentPos.z + PositionDelta.z
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

    private IEnumerator RotateCamera(float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = Camera.main.transform.rotation;
        Quaternion newRot = Quaternion.Euler(currentRot.eulerAngles.x + RotationDelta.x,
                                             currentRot.eulerAngles.y + RotationDelta.y,
                                             currentRot.eulerAngles.z + RotationDelta.z);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            Camera.main.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }
}