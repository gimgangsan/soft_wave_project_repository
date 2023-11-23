using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1920, (3 / 4) * 1920, true);
    }
}
