using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject entityToSpawn;
    [SerializeField] bool destroyAfterUsage = true;

    public void SpawnEnemy()
    {
        if (entityToSpawn == null) return;
        if (entityToSpawn.TryGetComponent(out Entity outEntity) == false) return;

        GameObject go = Instantiate(entityToSpawn, transform.position, Quaternion.identity);

        if(go.TryGetComponent(out NavMeshAgent agent))
        {
            agent.Warp(transform.position);
            agent.enabled = true;
        }

        if(destroyAfterUsage)
        {
            Destroy(gameObject);
        }
    }
}
