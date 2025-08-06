using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public event Action DownTouched;
    public event Action UpTouched;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DownTouched?.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UpTouched?.Invoke();
        }
    }
}
