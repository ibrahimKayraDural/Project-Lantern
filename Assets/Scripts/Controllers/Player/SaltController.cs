using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaltController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] SaltSlider saltSlider;
    [SerializeField] LightController lightController;
    [SerializeField] TextMeshProUGUI debugText;

    [Header("Values")]
    [SerializeField] float saltDuration = 5;
    [SerializeField] float saltCooldown = 5;
    [SerializeField] Color[] colors;

    public Color CurrentColor { get; private set; }
    public bool isActivated { get; private set; }
    public float SaltAmount { get; private set; } = 1;


    int currentColorindex;

    //float end_TargetTime = float.MaxValue;
    //float nextUse_TargetTime;

    void Start()
    {
        CurrentColor = colors[currentColorindex];
        TrySetDebugTextWithColor(GetColorName(CurrentColor), CurrentColor);
    }
    void Update()
    {
        //if (Input.GetButtonDown("Salt_Activate") && nextUse_TargetTime < Time.time)//Left Shift
        //{
        //    ActivateSalts();

        //    end_TargetTime = Time.time + saltDuration;
        //    nextUse_TargetTime = float.MaxValue;
        //}

        //if (end_TargetTime < Time.time)
        //{
        //    DeactivateSalts();

        //    end_TargetTime = float.MaxValue;
        //    nextUse_TargetTime = Time.time + saltCooldown;
        //}

        if (Input.GetButtonDown("Salt_Activate") && isActivated == false && SaltAmount == 1)
        {
            ActivateSalts();
        }

        if (isActivated)
        {
            SaltAmount -= Time.deltaTime / saltDuration;

            if (SaltAmount <= 0)
            {
                SaltAmount = 0;
                DeactivateSalts();
            }
        }
        else
        {
            SaltAmount += Time.deltaTime / saltCooldown;
            SaltAmount = Mathf.Min(1, SaltAmount);
        }


        if (isActivated == false)
        {
            if (Input.GetButtonDown("Salt_Next"))//E
            {
                if (currentColorindex + 1 >= colors.Length) currentColorindex = 0;
                else currentColorindex++;

                CurrentColor = colors[currentColorindex];

                TryChangeFillImage(CurrentColor);
                //TrySetDebugTextWithColor(GetColorName(CurrentColor), CurrentColor);
            }
            else if (Input.GetButtonDown("Salt_Previous"))//Q
            {
                if (currentColorindex - 1 < 0) currentColorindex = colors.Length - 1;
                else currentColorindex--;

                CurrentColor = colors[currentColorindex];

                TryChangeFillImage(CurrentColor);
                //TrySetDebugTextWithColor(GetColorName(CurrentColor), CurrentColor);
            }
        }

        saltSlider.SetSliderValueZeroToOne(SaltAmount);
    }

    void DeactivateSalts()

    {
        isActivated = false;

        TryResetLightColor();
    }
    void ActivateSalts()
    {
        isActivated = true;

        CurrentColor = colors[currentColorindex];
        TrySetLightColor(CurrentColor);
    }
    LightColorType GetColorType(Color color)
    {
        float maxComp = color.maxColorComponent;

        if (color.r == maxComp) return LightColorType.Red;
        if (color.g == maxComp) return LightColorType.Green;
        if (color.b == maxComp) return LightColorType.Blue;
        else return LightColorType.Default;
    }
    bool TrySetLightColor(Color color)
    {
        if (lightController == null) return false;

        lightController.SetOuterLightColor(color, GetColorType(color));
        return true;
    }
    bool TryResetLightColor()
    {
        if (lightController == null) return false;

        lightController.ResetOuterLightColor();
        return true;
    }
    bool TryChangeFillImage(Color color)
    {
        if (saltSlider == null) return false;

        LightColorType type = GetColorType(color);

        saltSlider.ChangeSaltFillImage(type);
        return true;
    }

    #region DebugStuff
    string GetColorName(Color color)
    {
        string colorName = "Unknown";

        if (color.r == 1 && color.g == 1 && color.b == 1)
        {
            colorName = "White";
        }
        else if (color.r == 0 && color.g == 0 && color.b == 0)
        {
            colorName = "Black";
        }
        else
        {
            float maxColor = color.maxColorComponent;

            if (color.r == maxColor) colorName = "Red";
            else if (color.g == maxColor) colorName = "Green";
            else colorName = "Blue";
        }

        return colorName;
    }
    bool TrySetDebugText(string value)
    {
        if (debugText == null) return false;

        debugText.text = value;
        return true;
    }
    bool TrySetDebugTextWithColor(string value, Color color)
    {
        if (debugText == null) return false;

        debugText.text = value;
        debugText.color = color;
        return true;
    }
    #endregion
}