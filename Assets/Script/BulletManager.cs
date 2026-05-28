using UnityEngine;
using UnityEngine.UIElements;

public class BulletManager : MonoBehaviour
{
    public float damage;
    public float speed;
    public GameObject bloodPrefab;
    public GameObject eteincelle;
    public Renderer rend;
    private void Update()
    {
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 dir = transform.right * speed * Time.deltaTime; ;
        Vector2 nextPos = rb.position + dir * Time.deltaTime;

        Vector3 vp = Camera.main.WorldToViewportPoint(nextPos);

        if (vp.x < -0.05f || vp.x > 1.05f ||
         vp.y < -0.05f || vp.y > 1.05f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy.isDying) return;
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Enemy2"))
        {
            CroController enemy = collision.GetComponent<CroController>();
            if (enemy.isDying) return;
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            if (rend.isVisible)
            Instantiate(eteincelle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Boss"))
        {
            BossController enemy = collision.GetComponent<BossController>();
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

}
