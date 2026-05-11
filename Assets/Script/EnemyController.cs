using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class EnemyController : MonoBehaviour
{
    enum CurrentState { Chasing, Wandering, Dead }

    private CurrentState state = CurrentState.Wandering;

    [SerializeField] private float Hp;
    [SerializeField] private Rigidbody2D rb;

    public Vector2 target;
    public float speed;
    public float chasingSpeed;
    public float maxDistance;

    public GameObject[] drops;

    bool isKnocked;
    bool isWanderingRoutine;
    public bool isDying;

    Transform player;

    private void Start()
    {
        setNewDestination();
    }

    private void Update()
    {
        if (isDying) return;
        if (Hp <= 0)
        {
            state = CurrentState.Dead;

            if (!isDying)
            {
                isDying = true;
                StartCoroutine(die());
            }
            return;
        }
        

        player = GetClosestPlayer();

        if (player != null && Vector2.Distance(transform.position, player.position) < maxDistance)
        {
            state = CurrentState.Chasing;
            target = player.position;
        }

        if (isKnocked) return;

        if (state == CurrentState.Wandering)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.deltaTime));

            if (!isWanderingRoutine &&
                Vector2.Distance(transform.position, target) < 0.2f)
            {
                StartCoroutine(newTarget());
            }
        }
        else if (state == CurrentState.Chasing)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, target, chasingSpeed * Time.deltaTime));
        }


        
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
        if (state != CurrentState.Chasing) 
        {

            player = GetClosestPlayer();
            maxDistance = Mathf.Infinity;
            state = CurrentState.Chasing;

        }

    }

    public void GetKnockbacked(Transform origin)
    {
        if (isKnocked) return;
        StartCoroutine(KnockbackRoutine(origin));
    }

    IEnumerator KnockbackRoutine(Transform origin)
    {
        isKnocked = true;

        Vector2 dir = (transform.position - origin.position).normalized;
        rb.linearVelocity = dir * 5f;

        yield return new WaitForSeconds(0.2f);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    IEnumerator newTarget()
    {
        isWanderingRoutine = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));

        setNewDestination();

        isWanderingRoutine = false;
    }

    IEnumerator die()
    {
        int rdm = UnityEngine.Random.Range(1, 10);
        if (rdm > 6)
        {
            int loot = UnityEngine.Random.Range(0, drops.Length);
            Instantiate(drops[loot], transform.position, Quaternion.identity);
        }


        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        CircleCollider2D CircleCollider2D = GetComponent<CircleCollider2D>();
        CircleCollider2D.isTrigger = true;

        while (sr.color.a > 0)
        {
            Color c = sr.color;
            c.a -= 0.1f;
            sr.color = c;

            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }

    private void setNewDestination()
    {
        target = (Vector2)transform.position +
                 new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));

        state = CurrentState.Wandering;
    }

    Transform GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            if (p == null) continue;

            float dist = Vector2.Distance(transform.position, p.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = p.transform;
            }
        }

        return closest;
    }



    
}