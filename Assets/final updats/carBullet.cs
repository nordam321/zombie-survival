using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carBullet : MonoBehaviour{
    public float explosionRadius = 5f;
    public int explosionDamage = 25;

    private void OnTriggerEnter(Collider other){
        explosionDamage = Random.Range(20, 36);
        Explode();
    }

    void Explode(){

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders){
            if (nearbyObject.CompareTag("Senimy")){
                enimyHealth smallEnemyHealth = nearbyObject.GetComponent<enimyHealth>();
                if (smallEnemyHealth != null){
                    smallEnemyHealth.Senemyhealth -= explosionDamage;
                    Debug.Log(smallEnemyHealth.Senemyhealth);
                }
            }
            else if (nearbyObject.CompareTag("Menimy")){
                MenemyHealth mediumEnemyHealth = nearbyObject.GetComponent<MenemyHealth>();
                if (mediumEnemyHealth != null){
                    mediumEnemyHealth.Menemyhealth -= explosionDamage;
                    Debug.Log(mediumEnemyHealth.Menemyhealth);
                }
            }
            else if (nearbyObject.CompareTag("boss")){
                smallEnimySpawning bossHealth = nearbyObject.GetComponent<smallEnimySpawning>();
                if (bossHealth != null){
                    bossHealth.bossHealth -= explosionDamage;
                    Debug.Log(bossHealth.bossHealth);
                }
            }
        }

        Destroy(gameObject);
    }
}
