using UnityEngine;

public class Boid : MonoBehaviour
{
    public FlockController controller;
    public bool isLeader;

    Vector3 velocity;
    Vector3 wanderTarget;

    void Start()
    {
        velocity = transform.forward * controller.speed;
        wanderTarget = transform.forward;
    }

    void Update()
    {
        Vector3 steering = Vector3.zero;

        if (isLeader)
        {
            steering += Wander();
        }
        else
        {
            steering += Separation() * controller.separationWeight;
            steering += Alignment() * controller.alignmentWeight;
            steering += Cohesion() * controller.cohesionWeight;
            steering += FollowLeader() * controller.leaderWeight;
        }

        steering += AvoidObstacles() * controller.avoidWeight;
        steering += StayInArea() * controller.boundaryWeight;

        float currentSpeed = isLeader ? controller.leaderSpeed : controller.speed;

        velocity += steering * Time.deltaTime;
        velocity.y = 0f;
        velocity = velocity.normalized * currentSpeed;

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    Vector3 Separation()
    {
        Vector3 push = Vector3.zero;

        foreach (Boid other in controller.boids)
        {
            if (other == this || other == null) continue;

            Vector3 away = transform.position - other.transform.position;
            away.y = 0f;
            float d = away.magnitude;

            if (d > 0f && d < controller.separationDistance)
                push += away.normalized / d;
        }

        return push;
    }

    Vector3 Alignment()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Boid other in controller.boids)
        {
            if (other == this || other == null) continue;

            if (Vector3.Distance(transform.position, other.transform.position) < controller.neighbourDistance)
            {
                sum += other.velocity;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        return (sum / count).normalized;
    }

    Vector3 Cohesion()
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        foreach (Boid other in controller.boids)
        {
            if (other == this || other == null) continue;

            if (Vector3.Distance(transform.position, other.transform.position) < controller.neighbourDistance)
            {
                center += other.transform.position;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        center /= count;
        Vector3 toCenter = center - transform.position;
        toCenter.y = 0f;
        return toCenter.normalized;
    }

    Vector3 FollowLeader()
    {
        Boid closest = null;
        float best = Mathf.Infinity;

        foreach (Boid other in controller.boids)
        {
            if (other == null || !other.isLeader) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if (d < best)
            {
                best = d;
                closest = other;
            }
        }

        if (closest == null) return Vector3.zero;

        Vector3 behind = closest.transform.position - closest.transform.forward * 2f;
        Vector3 toLeader = behind - transform.position;
        toLeader.y = 0f;
        return toLeader.normalized;
    }

    Vector3 Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)) * 0.5f;
        wanderTarget = wanderTarget.normalized;

        Vector3 point = transform.forward * 2f + wanderTarget;
        point.y = 0f;
        return point.normalized;
    }

    Vector3 AvoidObstacles()
    {
        if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, controller.avoidDistance, controller.obstacleMask))
        {
            Vector3 away = hit.normal;
            away.y = 0f;
            return away.normalized;
        }

        return Vector3.zero;
    }

    Vector3 StayInArea()
    {
        Vector3 toCenter = controller.transform.position - transform.position;
        toCenter.y = 0f;

        if (toCenter.magnitude > controller.areaRadius)
            return toCenter.normalized;

        return Vector3.zero;
    }
}
