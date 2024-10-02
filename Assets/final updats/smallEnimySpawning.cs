using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class smallEnimySpawning : MonoBehaviour {
    public GameObject SenimyObject;
    public GameObject MObject;
    public GameObject healthObject;
    public GameObject ammoObject;
    public GameObject boostObject;
    public int randomSelection = 0;
    static public int score = 0;
    public NavMeshAgent agent;
    public int bossHealth = 100; //دم البوس
    public player p;
    public Transform Player;
    public Transform boss1;
    static private int count = 0;
    static private int Mcount = 0;
    public float detectionRange = 20f;
    public float randomWalkRadius = 50f;
    static public bool spawned = false;
    static private float waitingtospawn = 20f;
    private bool canSpawn = true;
    //من اجل السرعه
    public float moveSpeed = 3.5f;
    //من اجل الانيمشن
    public Animator anim;

    // مسافه عشان يشتغل الِattack anim
    public float attackRange = 2f;


    void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(Player.position, transform.position);

        if (distanceToPlayer <= detectionRange) {
            agent.SetDestination(Player.position);
            agent.speed = moveSpeed; // هون لما حيشوف اللاعب رح تصير سرعته يلي رح نحددها

            // هون رح يطللع اذا قرب على اللاع عشان يشتغل الانميشين
            if (distanceToPlayer <= attackRange) {
                p.SwitchState(p.eAttack);
            } else {
                p.SwitchState(p.eRun);
            }
        } else {
            if (!agent.hasPath || agent.remainingDistance < 0.5f) {
                agent.speed = moveSpeed / 2; // هون لما مايشوف اللاعب رح تكون سرعته النصف
                Vector3 randomDirection = Random.insideUnitSphere * randomWalkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, randomWalkRadius, 1);
                Vector3 finalPosition = hit.position;

                agent.SetDestination(finalPosition);
            }
        }

        float distance = Vector3.Distance(Player.position, boss1.position);

        if (distance <= 20f || spawned) {
            spawned = true;
            spawnSmall();
            spawnM();
            if (canSpawn) {
                StartCoroutine(newSpawn(waitingtospawn));
            }
        }

        if (bossHealth <= 0) {
            Destroy(gameObject);
            score += 20;
            Debug.Log("You Won!");
        }
    }

    IEnumerator newSpawn(float a) {
        canSpawn = false;

        yield return new WaitForSeconds(a);
        count -= 5;
        Mcount -= 3;

        canSpawn = true;
    }

    void spawnSmall() {
        while (count < 5) {
            float randomX = Random.Range(-100f, 100f);
            float randomZ = Random.Range(-100f, 100f);
            Vector3 pos = new Vector3(randomX, 0, randomZ) + boss1.position;
            Quaternion q = Quaternion.identity;

            GameObject SmallEnemy = Instantiate(SenimyObject, pos, q);
            NavMeshAgent mesh = SmallEnemy.AddComponent<NavMeshAgent>();
            mesh.speed = moveSpeed; // اضافة سرعه للانم الصغار

            if (SmallEnemy != null) {
                enimyHealth enemyhealth = SmallEnemy.AddComponent<enimyHealth>();
                enemyhealth.target = Player;
                enemyhealth.agent = mesh;
                enemyhealth.HealthObject = healthObject;
                enemyhealth.AmmoObject = ammoObject;
                enemyhealth.BoostObject = boostObject;
                enemyhealth.P = p;

                Rigidbody rb = SmallEnemy.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.useGravity = false;
                }
            }
            count++;
        }
    }

    void spawnM() {
        while (Mcount < 5) {
            float randomX = Random.Range(-30f, 30f);
            float randomZ = Random.Range(-30f, 30f);
            Vector3 pos = new Vector3(randomX, 0, randomZ) + boss1.position;
            Quaternion q = Quaternion.identity;

            GameObject MEnemy = Instantiate(MObject, pos, q);
            NavMeshAgent mesh = MEnemy.AddComponent<NavMeshAgent>();
            mesh.speed = moveSpeed; // ضافة سرعه للانم الكبار

            if (MEnemy != null) {
                MenemyHealth menemyHealth = MEnemy.AddComponent<MenemyHealth>();
                menemyHealth.target = Player;
                menemyHealth.agent = mesh;
                menemyHealth.HealthObject = healthObject;
                menemyHealth.AmmoObject = ammoObject;
                menemyHealth.BoostObject = boostObject;
                menemyHealth.P = p;

                Rigidbody rb = MEnemy.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.useGravity = false;
                }
            }
            Mcount++;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("playerBullet")) {
            Destroy(other.gameObject);
            bossHealth -= p.playerDamage * player.playerboost;
            Debug.Log("enemy health is: " + bossHealth);
        }
    }
    //تجربه عشان الانيميشن اذا لمس البوس اللاعب
    // private void OnCollisionEnter(Collision collision) {
    //     if (collision.gameObject.CompareTag("Player")) {
    //         Debug.Log("touching from smallEnimy");
    //         p.BoosTouchingPlayer = true;
    //         anim.SetTrigger("AttackTrigger"); // Set the attack trigger
    //     }
    // }
}
