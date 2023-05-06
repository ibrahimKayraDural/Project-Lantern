using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelItem : MonoBehaviour, I_Pickable
{
    [SerializeField] float fuelAmount = 10;

    FuelController fuelController;

    public void OnPickup()
    {
        if (fuelController == null) return;

        fuelController.AddFuel(fuelAmount);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            fuelController = collision.GetComponentInChildren<FuelController>();

            OnPickup();
        }
    }
}
