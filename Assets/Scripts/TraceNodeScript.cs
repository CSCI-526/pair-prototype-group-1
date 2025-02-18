using UnityEngine;

public class TraceNodeScript : MonoBehaviour
{
    private float untriggerableTimer = 0.5f;

    void Update()
    {
        untriggerableTimer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (untriggerableTimer <= 0.0f) {
            TraceSpawnerManager traceSpawnerManager = GameObject.Find("TraceSpawner").GetComponent<TraceSpawnerManager>();
            traceSpawnerManager.CollectTrace();
            Destroy(gameObject);
        }
    }
}
