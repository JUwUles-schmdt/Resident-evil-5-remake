using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [HideInInspector]public bool isActive = false;

    public float enemySpawnCd;
    private float timer;
    public GameObject zombie;
    public GameObject croco;

    private void Start()
    {
        timer = enemySpawnCd;
    }
    void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int crocos = UnityEngine.Random.Range(1, 3);
            for (int i=0; i<crocos; i++)
            {
                SpawnEnemy(croco);
            }

            int zombies = UnityEngine.Random.Range(3, 6);
            for (int i = 0; i < zombies; i++)
            {
                SpawnEnemy(zombie);
            }
            timer = enemySpawnCd;


        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector3 camPos = cam.transform.position;

        float left = camPos.x - width;
        float right = camPos.x + width;
        float top = camPos.y + height;
        float bottom = camPos.y - height;

        float margin = 2f;

        Vector3 spawnPos;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: 
                spawnPos = new Vector3(
                    left - margin,
                    Random.Range(bottom, top),
                    0
                );
                break;

            case 1: 
                spawnPos = new Vector3(
                    right + margin,
                    Random.Range(bottom, top),
                    0
                );
                break;

            case 2: 
                spawnPos = new Vector3(
                    Random.Range(left, right),
                    top + margin,
                    0
                );
                break;

            default: 
                spawnPos = new Vector3(
                    Random.Range(left, right),
                    bottom - margin,
                    0
                );
                break;
        }

        Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);
    }
}
