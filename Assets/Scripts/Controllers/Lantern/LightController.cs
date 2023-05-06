using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Light2D outerLight;
    [SerializeField] Light2D innerLight;
    [SerializeField] CircleCollider2D outerCollider;
    [SerializeField] CircleCollider2D innerCollider;
    [SerializeField] LayerMask EnemyLayers;
    [SerializeField] FuelController fuelController;

    [Header("Values")]
    [SerializeField] float outerRadius = 7f;
    [SerializeField] float innerRadius = 2f;
    [SerializeField] Color outerColor = Color.white;
    [SerializeField] Color innerColor = Color.white;
    [SerializeField] float outerIntensity = 1f;
    [SerializeField] float innerIntensity = .8f;

    [SerializeField] float OuterColliderTweak = 0f;
    [SerializeField] float InnerColliderTweak = 0f;

    List<Entity> EnemiesInOuterArea = new List<Entity>();
    List<Entity> EnemiesInInnerArea = new List<Entity>();


    void OnValidate()
    {
        outerLight.pointLightOuterRadius = outerRadius;
        outerLight.pointLightInnerRadius = innerRadius;
        innerLight.pointLightOuterRadius = innerRadius + .1f;
        outerLight.color = outerColor;
        innerLight.color = innerColor;
        outerLight.intensity = outerIntensity;
        innerLight.intensity = innerIntensity;

        outerCollider.radius = outerRadius + OuterColliderTweak;
        innerCollider.radius = innerRadius + InnerColliderTweak;
    }
    void Start()
    {
        fuelController.Event_OnFuelHasDepleted += ToggleLights;
    }
    void Update()//AAAAAAAAAAAAAAAAAAAAAAA CLEAN THIS CODE PLS FOR THE LOVE OF GOD
    {
        List<Collider2D> outer_overlappedColliders = new List<Collider2D>();
        List<Collider2D> inner_overlappedColliders = new List<Collider2D>();

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = EnemyLayers;
        filter.useLayerMask = true;

        List<Entity> newList = new List<Entity>();

        outerCollider.OverlapCollider(filter, outer_overlappedColliders);
        foreach(Collider2D collider in outer_overlappedColliders)
        {
            if (collider.gameObject.TryGetComponent(out Entity outEntity) == false)
            { return; }
            Entity enemyEntity = outEntity;

            newList.Add(enemyEntity);
        }
        foreach(Entity e in newList)
        {
            if(EnemiesInOuterArea.Contains(e) == false)
            {
                EntityEnteredOuter(e);
            }
        }
        foreach(Entity e in EnemiesInOuterArea)
        {
            if(newList.Contains(e) == false)
            {
                EntityExitedOuter(e);
            }
        }

        EnemiesInOuterArea = newList;
        newList = new List<Entity>();

        innerCollider.OverlapCollider(filter, inner_overlappedColliders);
        foreach (Collider2D collider in inner_overlappedColliders)
        {
            if (collider.gameObject.TryGetComponent(out Entity outEntity) == false)
            { return; }
            Entity enemyEntity = outEntity;

            newList.Add(enemyEntity);
        }
        foreach (Entity e in newList)
        {
            if (EnemiesInInnerArea.Contains(e) == false)
            {
                EntityEnteredInner(e);
            }
        }
        foreach (Entity e in EnemiesInInnerArea)
        {
            if (newList.Contains(e) == false)
            {
                EntityExitedInner(e);
            }
        }
        EnemiesInInnerArea = newList;
    }

    public void ToggleLights(object sender, bool depleted)
    {
        bool toggleOn = !depleted;

        outerCollider.enabled = toggleOn;
        innerCollider.enabled = toggleOn;
        outerLight.enabled = toggleOn;
        innerLight.enabled = toggleOn;
    }

    private static void EntityEnteredOuter(Entity entity)
    {
        //Debug.Log(entity + " has entered outer");
    }

    private static void EntityExitedOuter(Entity entity)
    {
        //Debug.Log(entity + " has exited outer");
    }

    void EntityEnteredInner(Entity entity)
    {
        //Debug.Log(entity + " has entered inner");
        entity.EnteredOrange();

        if (fuelController != null) 
        {
            fuelController.SetEnemyAreInOrange(true);
        }
    }

    void EntityExitedInner(Entity entity)
    {
        //Debug.Log(entity + " has exited inner");

        entity.SetSpeedToDefault();

        if (fuelController != null)
        {
            if (EnemiesInInnerArea.Count > 0)
            { fuelController.SetEnemyAreInOrange(false); }
        }
    }
}
