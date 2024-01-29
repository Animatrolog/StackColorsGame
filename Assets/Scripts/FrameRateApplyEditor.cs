#if UNITY_EDITOR
using UnityEngine;

public class FrameRateApplyEditor : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
#endif