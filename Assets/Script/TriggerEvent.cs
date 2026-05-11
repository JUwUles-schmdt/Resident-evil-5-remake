using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public GameObject entry;
    public GameObject exit;
    public GameObject boss;
    public Transform bossSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entry.SetActive(true);
            Instantiate(boss, bossSpawn.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
