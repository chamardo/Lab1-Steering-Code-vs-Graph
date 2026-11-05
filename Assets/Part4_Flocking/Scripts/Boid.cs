using UnityEngine;

public class Boid : MonoBehaviour
{
    public FlockController controller;

    Vector3 velocity;

    void Start()
    {
        velocity = transform.forward * controller.speed;
    }

    void Update()
    {
        Vector3 steering = Vector3.zero;
        steering += StayInArea();

        velocity += steering * Time.deltaTime;
        velocity.y = 0f;
        velocity = velocity.normalized * controller.speed;

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    Vector3 StayInArea()
    {
        Vector3 toCenter = controller.transform.position - transform.position;
        toCenter.y = 0f;

        if (toCenter.magnitude > controller.areaRadius)
            return toCenter.normalized * controller.boundaryWeight;

        return Vector3.zero;
    }
}
