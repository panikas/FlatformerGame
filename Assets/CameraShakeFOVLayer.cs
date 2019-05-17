using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeFOVLayer : CameraShakeLayer
{
    [Header("Camera field of view")]
    [SerializeField]
    [Range(-0.2f, 0.2f)]
    float fieldOfViewMulti = 0.05f;

    float startFOV;
    float resetFOV;

    private void Start()
    {
        useY = false;
        useZ = false;

        Camera cam = Camera.main;

        if (!cam) cam = GetComponentInChildren<Camera>();

        resetFOV = cam.fieldOfView;

        onStart.AddListener(() => startFOV = cam.fieldOfView);

        onLoop.AddListener((x, y, z) =>
        {
            cam.fieldOfView = startFOV + (startFOV * fieldOfViewMulti * x);
        });

        onReset.AddListener(() =>
        {
            if (startFOV != resetFOV) startFOV = resetFOV;
            cam.fieldOfView = resetFOV;
        });
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        useY = false;
        useZ = false;
    }

#endif
}
