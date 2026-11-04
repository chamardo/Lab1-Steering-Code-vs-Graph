using UnityEngine;
using UnityEngine.AI;

public class SeekFleeAgent : MonoBehaviour
{
    public GameObject target;
    public float fleeDistance = 10f;
    public float safetyDistance = 10f;
    NavMeshAgent agent;
    Vector3 lastTargetPosition;
    Vector3 targetVelocity;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastTargetPosition = target.transform.position;
    }

    void Update()
    {
        targetVelocity = (target.transform.position - lastTargetPosition) / Time.deltaTime;
        lastTargetPosition = target.transform.position;
        
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > safetyDistance)
            Pursue();
        else
            Evade();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position -  fleeVector.normalized * fleeDistance);
    }

    void Pursue()
    {
        float targetSpeed = targetVelocity.magnitude;

        if (targetSpeed < 0.1f)
        {
            Seek(target.transform.position);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        float lookAhead = distance / (agent.speed + targetSpeed);

        Seek(target.transform.position + targetVelocity * lookAhead);
    }

    void Evade()
    {
        float targetSpeed = targetVelocity.magnitude;

        if (targetSpeed < 0.1f)
        {
            Flee(target.transform.position);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        float lookAhead = distance / (agent.speed + targetSpeed);

        Flee(target.transform.position + targetVelocity * lookAhead);
    }
}
