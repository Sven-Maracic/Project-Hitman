using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaling : MonoBehaviour
{
    RectTransform UI;
    [SerializeField] Camera cam;

    void Start()
    {
        UI = GetComponent<RectTransform>();
        float newRes = cam.pixelWidth / (float)cam.pixelHeight;
        UI.localScale = new Vector2(cam.pixelWidth, cam.pixelHeight);//new Vector2(UI.rect.width * newRes, UI.rect.height);
    }
}