using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 2f;
    public float dashSpeed = 8f;
    public float dashDuration = 1f;
    public float minDashDelay = 2f;
    public float maxDashDelay = 5f;

    public float hp;

    public GameObject[] portes;

    private float dashTimer;
    private float nextDashTime;
    private bool isDashing;
    private Vector2 dashDir;

    void Start()
    {
        SetNextDash();
        portes[0] = GameObject.Find("Sortie");
        portes[1] = GameObject.Find("Entrée");
    }

    void Update()
    {
        if (hp <= 0) 
        {
            for (int i = 0; i < portes.Length; i++)
            {
                Destroy(portes[i]);
            }
            Destroy(gameObject);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;

        Transform target = players[0].transform;

        if (!isDashing)
        {transform.position = Vector2.MoveTowards(transform.position,target.position,speed * Time.deltaTime);

            if (Time.time >= nextDashTime)
            {
                dashDir = (target.position - transform.position).normalized;
                isDashing = true;
                dashTimer = dashDuration;
            }
        }
        else
        {
            transform.position += (Vector3)(dashDir * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                SetNextDash();
            }
        }
    }

    void SetNextDash()
    {
        nextDashTime = Time.time + Random.Range(minDashDelay, maxDashDelay);
    }


    public void TakeDamage(float damage)
    {
        hp -= damage;

    }

    public void GetKnockbacked(Transform origin)
    {
    }

}