using UnityEngine;
using UnityEngine.AI;

public class SeekFleeAgent : MonoBehaviour
{
    public GameObject target;
    public float safetyDistance = 10f;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > safetyDistance)
            Seek(target.transform.position);
        else
            Flee(target.transform.position);
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position -  fleeVector);
    }
}
