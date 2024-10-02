using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {
    //private float moveSpeed = 5f;
    //private float turnSpeed = 500.0f;
    static private int playerhealth = 200;
    public Text HealthText;
    public Text LevelText;
    static private int playerammo;
    static private int ammo = 160;
    static public int playerboost = 1;
    public int playerDamage = 10;
    private bool canShoot = true;
    static private float waitingtoshoot = 1f;
    static private float lightWaiting = 0.1f;
    static private float waitingtoreload = 5f;
    //private bool isRunning = false;
    //private float lastRunTime = 0f;
    public float cooldownTime = 10f;
    public GameObject bulletObject;
    public float bulletSpeed = 50f;
    public bool isShooting = false;
    public DrivingWithShooting D;
    private bool isreloading = false;
    private bool isDamaging = false;
    public Light l;
    public Camera playerCamera;
    public float maxShootDistance = 100f;
    public int maxDamage = 8;

    public AudioSource audioSource; // اضافة الكومبونينت تاع الصوت
    public AudioClip shootClip; // اضافة الصوت بحد ذاته

    //اضافات من اجل الانيمشن ركززز معي cleaaaan cooooodeeee

    //اول شيء من ضيف الكلاس الاساساي الذي يحتوي على الفنكشينس يلي رح نستخدمهم في كل ستايت 
    public EnemyBaseState currentState;
    public EnemtRunState eRun = new EnemtRunState();
    public EnemyAttackState eAttack = new EnemyAttackState();
    public EnemyDieState eDie = new EnemyDieState();

    public smallEnimySpawning smallEnimySpawning;
    // هون رح ضيف فاريبل من نوع بولين عشان اذا لمس البوس اللاعب تصير صح ويشتغل الانيمشين
    
    
    void Start() {
        playerammo = ammo / 2;
        l.intensity = 0;
        audioSource = GetComponent<AudioSource>();
        SwitchState(eRun);
        UpdateHealthText();
        UpdateLevelText();
    }

    void Update() {
        isShooting = false;

        if (playerammo == 0 && !isreloading) {
            StartCoroutine(reloading());
        }

        if (Input.GetMouseButton(0) && playerammo > 0 && canShoot && !D.isDriving && !isreloading) {
            playerDamage = Random.Range(1, maxDamage);
            isShooting = true;
            l.intensity = 10f;
            StartCoroutine(ShootWithDelay());
        }

        currentState.UpdateState(this);
        
    }
    public void SwitchState(EnemyBaseState state){
        currentState = state;
        currentState.EnterState(this);
    }

    IEnumerator ShootWithDelay() {
        canShoot = false;
        Shooting();
        playerammo--;

        yield return new WaitForSeconds(lightWaiting);

        l.intensity = 0;

        yield return new WaitForSeconds(waitingtoshoot);

        canShoot = true;
    }

    IEnumerator reloading() {
        isreloading = true;
        ammo /= 2;
        playerammo = ammo / 2;

        yield return new WaitForSeconds(waitingtoreload);

        isreloading = false;
    }

    IEnumerator EnemyDamage(float time) {
        isDamaging = true;

        yield return new WaitForSeconds(time);

        isDamaging = false;
    }

    void Shooting() {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, maxShootDistance)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(maxShootDistance);
        }

        Vector3 forwardOffset = transform.position + transform.forward * 1.5f + Vector3.up * 1.3f;
        Vector3 direction = (targetPoint - forwardOffset).normalized;

        GameObject bullet = Instantiate(bulletObject, forwardOffset, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = direction * bulletSpeed;
        
        Destroy(bullet, 5f);

        if (audioSource != null && shootClip != null) {
            audioSource.PlayOneShot(shootClip);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Health")) {
            Destroy(other.gameObject);
            playerhealth += 20;
            UpdateHealthText();
            Debug.Log("Your health increased 20 hp, your health is: " + playerhealth);
        } else if (other.CompareTag("ammo")) {
            Destroy(other.gameObject);
            ammo += 20;
            Debug.Log("Your ammo increased 20, your ammo amount is: " + playerammo);
        } else if (other.CompareTag("boost")) {
            Destroy(other.gameObject);
            playerboost++;
            UpdateLevelText();
            waitingtoshoot /= 2;
            maxDamage *= 2;
            Debug.Log("Your level increased 1, your damage range increased " + " and your shooting time has decreased by 50%");
        } else if (other.CompareTag("Menimy")) {
            if (playerhealth <= 0) {
                Debug.Log("You Died");
                Destroy(gameObject);
            } else if (playerhealth > 0) {
                playerhealth -= 20;
                StartCoroutine(EnemyDamage(5));
                UpdateHealthText();
                Debug.Log("Your health is: " + playerhealth);
            }
        } else if (other.CompareTag("Senimy")) {
            if (playerhealth <= 0) {
                Debug.Log("You Died");
                Destroy(gameObject);
            } else if (playerhealth > 0) {
                playerhealth -= 10;
                StartCoroutine(EnemyDamage(8));
                UpdateHealthText();
                Debug.Log("Your health is: " + playerhealth);
            }
        } else if (other.CompareTag("boss")) {
            if (playerhealth <= 0) {
                Debug.Log("You Died");
                Destroy(gameObject);
            } else if (playerhealth > 0) {
                playerhealth -= 30;
                StartCoroutine(EnemyDamage(3));
                UpdateHealthText();
                Debug.Log("Your health is: " + playerhealth);
            }
        }
    }
    void UpdateHealthText() {
        HealthText.text = string.Format("{0:00}", playerhealth);
    }
    void UpdateLevelText() {
        LevelText.text = string.Format("Level: {0}", playerboost);
    }
}
