using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour, ILineManager
{
    private float strength;
    private float _remainingShakeTime;
    private Vector3 _initialCameraPosition;

    public void Action(string[] lineBlock)
    {
        strength = float.Parse(lineBlock[1]);
        _remainingShakeTime = float.Parse(lineBlock[2]);
        enabled = true;
    }
    private void Awake()
    {
        _initialCameraPosition = transform.localPosition;
        enabled = false;
    }
    private void Update()
    {
        if (_remainingShakeTime <= 0)
        {
            transform.localPosition = _initialCameraPosition;
            enabled = false;
        }

        transform.Translate(Random.insideUnitCircle * strength);

        _remainingShakeTime -= Time.deltaTime;
    }
}
