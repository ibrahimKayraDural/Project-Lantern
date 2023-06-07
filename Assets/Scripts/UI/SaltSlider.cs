using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaltSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject emission;

    [SerializeField] Sprite SaltImage_Red;
    [SerializeField] Sprite SaltImage_Green;
    [SerializeField] Sprite SaltImage_Blue;

    void Awake()
    {
        slider.maxValue = 1;
        SetSliderValueZeroToOne(1);
    }

    public void SetSliderValueZeroToOne(float value)
    {
        value = Mathf.Clamp(value, 0, 1);
        slider.value = value;

        SetEmission(value);
    }

    void SetEmission(float value)
    {
        if (emission == null) return;

        if (value == 1) emission.SetActive(true);
        else emission.SetActive(false);
    }

    public void ChangeSaltFillImage(LightColorType color)
    {
        switch(color)
        {
            case LightColorType.Default: break;
            case LightColorType.Red: fillImage.sprite = SaltImage_Red; break;
            case LightColorType.Green: fillImage.sprite = SaltImage_Green; break;
            case LightColorType.Blue: fillImage.sprite = SaltImage_Blue; break;
        }
    }
}
