using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Light2D outerLight;
    [SerializeField] Light2D innerLight;
    [SerializeField] Light2D redLight;
    [SerializeField] CircleCollider2D outerCollider;
    [SerializeField] CircleCollider2D innerCollider;
    [SerializeField] LayerMask EnemyLayers;
    [SerializeField] FuelController fuelController;

    [Header("Values")]
    [SerializeField] float lanternDamagePerSeconds = 1f;
    [SerializeField] float OuterColliderTweak = 0f;
    [SerializeField] float InnerColliderTweak = 0f;
    [SerializeField] float redTweak = 0f;

    [Header("More Values")]
    [SerializeField] float outerRadius = 7f;
    [SerializeField] float innerRadius = 2f;
    [SerializeField] float redRadius = 1f;
    [SerializeField] Color outerColor = Color.white;
    [SerializeField] Color innerColor = Color.white;
    [SerializeField] Color redColor = Color.red;
    [SerializeField] float outerIntensity = 1f;
    [SerializeField] float innerIntensity = .8f;
    [SerializeField] float redIntensity = 2f;

    LightColorType light_ColorType = LightColorType.Default;

    List<Entity> EnemiesInOuterArea = new List<Entity>();
    List<Entity> EnemiesInInnerArea = new List<Entity>();

    float RedCastRadius => redRadius + redTweak;
    bool lightsAreEnabled;

    void OnValidate()
    {
        outerLight.pointLightOuterRadius = outerRadius;
        outerLight.pointLightInnerRadius = innerRadius;
        innerLight.pointLightOuterRadius = innerRadius + .1f;
        redLight.pointLightOuterRadius = redRadius;

        outerLight.color = outerColor;
        innerLight.color = innerColor;
        redLight.color = redColor;

        outerLight.intensity = outerIntensity;
        innerLight.intensity = innerIntensity;
        redLight.intensity = redIntensity;

        outerCollider.radius = outerRadius + OuterColliderTweak;
        innerCollider.radius = innerRadius + InnerColliderTweak;
    }
    void Start()
    {
        fuelController.Event_OnFuelHasDepleted += ToggleLights;
    }
    void Update()//AAAAAAAAAAAAAAAAAAAAAAA CLEAN THIS CODE PLS FOR THE LOVE OF GOD
    {
        //Adds entities to outer and inner entity lists (above)
        //also invokes enter and exit methods for entities (below)
        #region HandwrittenTriggerEnterExit
        List<Collider2D> outer_overlappedColliders = new List<Collider2D>();
        List<Collider2D> inner_overlappedColliders = new List<Collider2D>();

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = EnemyLayers;
        filter.useLayerMask = true;

        List<Entity> newList = new List<Entity>();

        outerCollider.OverlapCollider(filter, outer_overlappedColliders);
        foreach (Collider2D collider in outer_overlappedColliders)
        {
            if (collider.gameObject.TryGetComponent(out Entity outEntity) == false)
            { return; }
            Entity enemyEntity = outEntity;

            newList.Add(enemyEntity);
        }
        foreach (Entity e in newList)
        {
            if (EnemiesInOuterArea.Contains(e) == false)
            {
                EntityEnteredOuter(e);
            }
        }
        foreach (Entity e in EnemiesInOuterArea)
        {
            if (newList.Contains(e) == false)
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
        #endregion

        bool redUse = Input.GetMouseButton(0);

        fuelController.SetRedAreaInUse(redUse);
        UseRedArea(redUse);
    }

    public void ToggleLights(object sender, bool depleted)
    {
        lightsAreEnabled = !depleted;

        outerCollider.enabled = lightsAreEnabled;
        innerCollider.enabled = lightsAreEnabled;
        outerLight.enabled = lightsAreEnabled;
        innerLight.enabled = lightsAreEnabled;

        if(depleted)
        {
            UseRedArea(false);
        }
    }
    public void UseRedArea(bool doUse)
    {
        if (doUse && lightsAreEnabled == false) return;

        redLight.enabled = doUse;

        if (doUse)
        {
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, RedCastRadius, EnemyLayers);
            foreach (Collider2D col in overlappedColliders)
            {
                if (col.TryGetComponent(out Entity entity))
                {
                    entity.IsInRed(lanternDamagePerSeconds * Time.deltaTime);
                }
            }
        }
    }
    public void SetOuterLightColor(Color colorToSet, LightColorType type)
    {
        light_ColorType = type;
        outerLight.color = colorToSet;

        RefreshCollidedEntities();
    }
    public void ResetOuterLightColor()
    {
        light_ColorType = LightColorType.Default;
        outerLight.color = outerColor;

        RefreshCollidedEntities();
    }

    void RefreshCollidedEntities()
    {
        foreach(Entity e in EnemiesInOuterArea)
        {
            EntityEnteredOuter(e);
        }
        foreach (Entity e in EnemiesInInnerArea)
        {
            EntityEnteredInner(e);
        }
    }

    private static void EntityEnteredOuter(Entity entity)//Also enters here when salt is enabled
    {
        //Debug.Log(entity + " has entered outer");
    }

    private static void EntityExitedOuter(Entity entity)
    {
        //Debug.Log(entity + " has exited outer");
    }

    void EntityEnteredInner(Entity entity)//Also enters here when salt is enabled
    {
        entity.EnteredOrange(light_ColorType);

        if (fuelController != null) 
        {
            fuelController.SetEnemyAreInOrange(true);
        }
    }

    void EntityExitedInner(Entity entity)
    {
        entity.ExitedOrange();

        if (fuelController != null)
        {
            if (EnemiesInInnerArea.Count > 0)
            { fuelController.SetEnemyAreInOrange(false); }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RedCastRadius);
    }
}
