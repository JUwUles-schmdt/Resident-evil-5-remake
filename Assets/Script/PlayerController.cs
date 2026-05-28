using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;


public class PlayerController : MonoBehaviour
{
    [SerializeField]private float speed;
    private float vitesse;
    private float ms = 1;
    private float sprint = 1;


    private Vector2 move;
    private Vector2 look;
    private bool aim;
    private bool marcheArriere;
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Weapons[] getWeapons;
    [SerializeField]private Weapons[] weapons;
    private Weapons currentWeapon;
    private float switchCd=0.3f;
    private float currentCd;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float knifeCd=1f;
    public GameController gc;

    [Header ("Prefabs")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField]private GameObject laser;
    public GameObject bloodPrefab;
    public GameObject flashLight;


    private bool isReloading=false;


    public TMPro.TextMeshProUGUI ammoUi;
    public PlayerInput playerInput;




    private void Start()
    {

        

        flashLight.SetActive(false);
        gc = FindObjectOfType<GameController>();
        if (gc.playerNumber == 0)
        {
            gc.playerNumber += 1;
            gc.cc.AddPlayer(transform);
            transform.position = gc.spawn.position;
        }
        else if (gc.playerNumber == 1) 
        {
            gc.cc.AddPlayer(transform);
            transform.position = gc.cc.players[0].position;
            flashLight.SetActive(gc.cc.players[0].gameObject.GetComponent<PlayerController>().flashLight.activeSelf);
        }
        vitesse = speed;
        laser.SetActive(false);
        currentWeapon = weapons[0];
        currentCd = currentWeapon.cd;

        for (int i = 0; i < getWeapons.Length; i++)
        {
            weapons[i] = getWeapons[i].Clone();
        }
        currentWeapon = weapons[0];


    }
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.canceled) return;
        look = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
            aim = true;

        if (context.canceled)
            aim = false;
    }



    private void FixedUpdate()
    {
        Vector2 dir = move.normalized * vitesse * ms * sprint;

        Vector2 nextPos = rb.position + dir * Time.fixedDeltaTime;

        Vector3 vp = Camera.main.WorldToViewportPoint(nextPos);

        if (vp.x > 0.05f && vp.x < 0.95f &&
            vp.y > 0.05f && vp.y < 0.95f)
        {
            rb.linearVelocity = dir;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }


    void Update()
    {
        ammoUi.SetText(currentWeapon.mag+"/"+currentWeapon.reserve);
        float angleRad = rb.rotation * Mathf.Deg2Rad;
        Vector2 test = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        float dot = Vector2.Dot(move.normalized,test);

        if (dot < -0.5f)
        {
            marcheArriere = true;
            ms = 0.5f;
        }
        else
        {
            marcheArriere=false;
            ms = 1;
        }

        
        
        
        
        if (look.magnitude > 0.7f)
        {
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
        else if (move.magnitude > 0.7f)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }



        HandleAim();
        switchCd -= Time.deltaTime;
        currentCd -=Time.deltaTime;
        knifeCd -= Time.deltaTime;
    }

    private void HandleAim()
    {
        if (aim&& !isReloading)
        {
            vitesse = (speed/4);
            laser.SetActive(true);
        }
        else
        {
            vitesse = speed;
            laser.SetActive(false);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started&&!aim&&!marcheArriere&& !isReloading)
            sprint = 2;

        if (context.canceled)
            sprint = 1;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started&&currentCd<0&& !isReloading)
        {
            if (aim && currentWeapon.mag > 0)
            {
                if (currentWeapon.name == "Shotgun"|| currentWeapon.name == "Shotgun(Clone)")
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float angle = Random.Range(-15, 15);
                        Quaternion rot = Quaternion.Euler(0, 0, angle);
                        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * rot);
                        bullet.GetComponent<BulletManager>().damage = currentWeapon.damage;
                    }
                    currentCd = currentWeapon.cd;
                }
                if (currentWeapon.name == "Sniper"|| currentWeapon.name == "Sniper(Clone)")
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    bullet.GetComponent<BulletManager>().damage = currentWeapon.damage;
                    currentCd = currentWeapon.cd;
                }
                if (currentWeapon.name == "Pistol" || currentWeapon.name == "Pistol(Clone)")
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    bullet.GetComponent<BulletManager>().damage = currentWeapon.damage;
                    currentCd = currentWeapon.cd;
                }
                currentWeapon.mag -= 1;
            }
            else if (knifeCd<0f&& !isReloading)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, 2f);


                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        hit.GetComponent<EnemyController>()?.TakeDamage(20);
                        Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
                        hit.GetComponent<EnemyController>()?.GetKnockbacked(attackPoint);

                    }
                }
                knifeCd = 1f;
            }
        }
    }


    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.started&&switchCd<0&& !isReloading)
        {
            if (currentWeapon == weapons[0])
            {
                weapons[0] = currentWeapon;
                currentWeapon = weapons[1];
                switchCd = 0.3f;
            }
            else
            {
                weapons[1] = currentWeapon;
                currentWeapon = weapons[0];
                switchCd = 0.3f;
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context) 
    {
        if (context.started && currentWeapon.mag < currentWeapon.maxMag&& currentWeapon.reserve > 0)
        {
            StartCoroutine(reload());
        }
    }

    IEnumerator reload()
    {
        if (!isReloading)
        {
            isReloading = true;
            if (currentWeapon.name!="Shotgun" && currentWeapon.name != "Shotgun(Clone)")
            yield return new WaitForSeconds(currentWeapon.reloadTime);
            while (currentWeapon.maxMag!= currentWeapon.mag && currentWeapon.reserve>0) 
            {
                currentWeapon.mag += 1;
                currentWeapon.reserve -= 1;
                if (currentWeapon.name == "Shotgun" || currentWeapon.name == "Shotgun(Clone)")
                    yield return new WaitForSeconds(currentWeapon.reloadTime);
            }
            isReloading = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AmmoPistol"))
        {
            int ammo = UnityEngine.Random.Range(5, 22);
            if (currentWeapon == weapons[0])
            {
                currentWeapon.reserve += ammo;
            }
            else
            {
                weapons[0].reserve += ammo;
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("AmmoShotgun") && weapons[1].name == "Shotgun(Clone)")
        {
            int ammo = UnityEngine.Random.Range(2, 8);
            if (currentWeapon == weapons[1])
            {
                currentWeapon.reserve += ammo;
            }
            else
            {
                weapons[1].reserve += ammo;
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("AmmoSniper") && weapons[1].name == "Sniper(Clone)")
        {
            int ammo = UnityEngine.Random.Range(2, 8);
            if (currentWeapon == weapons[1])
            {
                currentWeapon.reserve += ammo;
            }
            else
            {
                weapons[1].reserve += ammo;
            }
            Destroy(collision.gameObject);
        }
    }

    public void stock()
    {
        getWeapons[0] = weapons[0];
        getWeapons[1] = weapons[1];
    }

    public void toggleLight(bool light)
    {
        flashLight.SetActive(light);
    }




    public void OnPause()
    {
        FindAnyObjectByType<GameController>().pause();
    }

}