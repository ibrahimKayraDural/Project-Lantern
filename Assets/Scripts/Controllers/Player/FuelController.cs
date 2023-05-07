using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelController : MonoBehaviour
{
    public event EventHandler<bool> Event_OnFuelHasDepleted;

    [SerializeField] Slider FuelSlider;

    [SerializeField] float MaxFuel = 50;
    [SerializeField] float RegenDefaultPerSecond = 0.25f;
    [SerializeField] float SpendDefaultPerSecond = 1f;
    [SerializeField] float RegenHinderPercent = 50;
    [SerializeField] float RegenStartCooldown = 1;
    [SerializeField] float RegainLightPercent = 50;

    public float FuelRegainValue => MaxFuel * RegainLightPercent / 100;

    public float FuelLevel => _currentFuelLevel;
    float _currentFuelLevel;
    public bool FuelIsDepleted => _fuelHasDepleted;
    bool _fuelHasDepleted;
    public bool EnemyAreInOrange => _enemyAreInOrange;
    bool _enemyAreInOrange;

    Dictionary<string, float> depleteMultipliers = new Dictionary<string, float>();
    List<string> UniqueNames = new List<string>();

    float regenStartTargetTime = float.MinValue;
    bool redAreaInUse;

    void Start()
    {
        _currentFuelLevel = MaxFuel;

        if (SliderExists())
        { 
            FuelSlider.maxValue = MaxFuel;
            SetFuelSliderValue(MaxFuel);
        }
        else
        { Debug.LogError("No Fuel Slider Has Attached"); }
    }
    void Update()
    {
        //eðer kýzýlsa bide en son basýmýndan 1 sn sora ayrýca ýþýklar gelince (sonuncusu tahminen)
        if (redAreaInUse && FuelLevel > 0 && _fuelHasDepleted == false)
        {
            FuelSpend(Time.deltaTime);
            regenStartTargetTime = Time.time + RegenStartCooldown;
        }
        else if (regenStartTargetTime <= Time.time)
        {
            FuelRegen(Time.deltaTime);
        }
    }
    public void AddFuel(float amount)
    {
        _currentFuelLevel = Mathf.Min(_currentFuelLevel + amount, MaxFuel);
        RefreshSliderValue();

        if (FuelLevel > FuelRegainValue)
        {
            FuelRegained();
        }
    }
    public void RemoveFuel(float amount)
    {
        _currentFuelLevel = Mathf.Max(_currentFuelLevel - amount, 0);
        RefreshSliderValue();

        if (FuelLevel <= 0)
        {
            FuelHasDepletedMethod();
        }
    }
    void FuelHasDepletedMethod()
    {
        _fuelHasDepleted = true;
        Event_OnFuelHasDepleted?.Invoke(this, true);
    }
    void FuelRegained()
    {
        _fuelHasDepleted = false;
        Event_OnFuelHasDepleted?.Invoke(this, false);
    }
    void SetFuelSliderValue(float setTo)
    {
        if (SliderExists() == false) return;

        FuelSlider.value = Mathf.Clamp(setTo, 0, FuelSlider.maxValue);
    }
    void RefreshSliderValue()
    {
        if (SliderExists() == false) return;

        FuelSlider.value = FuelLevel;
    }
    bool SliderExists()
    {
        if (FuelSlider != null) return true;
        return false;
    }
    void FuelRegen(float deltaTime)
    {
        float NetValue = RegenDefaultPerSecond * deltaTime;
        NetValue = EnemyAreInOrange ? NetValue * RegenHinderPercent / 100 : NetValue;
        AddFuel(NetValue);
    }
    void FuelSpend(float deltaTime)
    {
        RemoveFuel(SpendDefaultPerSecond * deltaTime);
    }
    public void SetEnemyAreInOrange(bool setTo) => _enemyAreInOrange = setTo;
    public void SetRedAreaInUse(bool setTo) => redAreaInUse = setTo;
}
