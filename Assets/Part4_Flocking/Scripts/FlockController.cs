using UnityEngine;

public class FlockController : MonoBehaviour
{
    public GameObject boidPrefab;
    [Range(1, 200)] public int boidCount = 30;
    public float spawnRadius = 10f;

    GameObject[] boids;
    
    void Start()
    {
        boids = new GameObject[boidCount];

        for (int i = 0; i < boidCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 position = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            boids[i] = Instantiate(boidPrefab, position, rotation, transform);
        }
    }

    void Update()
    {
        
    }
}
