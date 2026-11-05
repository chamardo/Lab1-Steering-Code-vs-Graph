using UnityEngine;

public class FlockController : MonoBehaviour
{
    public GameObject boidPrefab;
    [Range(1, 200)] public int boidCount = 30;
    [Range(0, 10)] public int leaderCount = 1;
    public float spawnRadius = 10f;
    public float speed = 3f;
    public float leaderSpeed = 3.5f;
    public float neighbourDistance = 4f;
    public float separationDistance = 1.5f;
    public float areaRadius = 20f;

    [Range(0f, 10f)] public float separationWeight = 3f;
    [Range(0f, 10f)] public float alignmentWeight = 1f;
    [Range(0f, 10f)] public float cohesionWeight = 1f;
    [Range(0f, 10f)] public float leaderWeight = 2f;
    [Range(0f, 20f)] public float avoidWeight = 10f;
    public float boundaryWeight = 5f;

    public LayerMask obstacleMask;
    public float avoidDistance = 3f;
    public Color leaderColor = Color.red;

    [HideInInspector] public Boid[] boids;

    int spawnedCount;
    int spawnedLeaders;

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if (boidCount != spawnedCount || leaderCount != spawnedLeaders)
            Spawn();
    }

    void Spawn()
    {
        if (boids != null)
            foreach (Boid b in boids)
                if (b != null) Destroy(b.gameObject);

        boids = new Boid[boidCount];

        for (int i = 0; i < boidCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 position = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            boids[i] = Instantiate(boidPrefab, position, rotation, transform).GetComponent<Boid>();
            boids[i].controller = this;
            boids[i].isLeader = i < leaderCount;

            if (boids[i].isLeader)
                foreach (Renderer r in boids[i].GetComponentsInChildren<Renderer>())
                    r.material.color = leaderColor;
        }

        spawnedCount = boidCount;
        spawnedLeaders = leaderCount;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
