using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenemyHealth : MonoBehaviour{
    public int Menemyhealth = 5;
    public player P;
    public int randomSelection = 0;
    public GameObject HealthObject;
    public GameObject AmmoObject;
    public GameObject BoostObject;
    public Transform target;
    public NavMeshAgent agent;
    public float detectionRange = 20f;
    public float randomWalkRadius = 50f;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        float distance = Vector3.Distance(target.position, transform.position);
        
        if (distance <= detectionRange || P.isShooting){
            agent.SetDestination(target.position);
            agent.speed = 2.5f;
            
        } else {
            if (!agent.hasPath || agent.remainingDistance < 0.5f){
                agent.speed = 1f;
                Vector3 randomDirection = Random.insideUnitSphere * randomWalkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, randomWalkRadius, 1);
                Vector3 finalPosition = hit.position;

                agent.SetDestination(finalPosition);
            }
        }
        
        if (Menemyhealth<=0){
            smallEnimySpawning.score+=10;
            Destroy(gameObject);
            randomSelection = Random.Range(0,3);
            if (randomSelection == 0){
                GiftSpawning(HealthObject);
            } else if (randomSelection == 1){
                GiftSpawning(AmmoObject);
            } else if (randomSelection == 2){
                GiftSpawning(BoostObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("playerBullet")){
            Destroy(other.gameObject);
            Menemyhealth -= P.playerDamage*player.playerboost;
            Debug.Log("MEDIUM enimy health is: " + Menemyhealth);
        }
    }

    void GiftSpawning(GameObject RandomgameObject){
        GameObject giftSpawned = Instantiate(RandomgameObject, transform.position, Quaternion.identity);
        Rigidbody rb = giftSpawned.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }
}
