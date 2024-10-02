using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingWithShooting : MonoBehaviour{
    public Transform Player;
    private float turnSpeed = 500.0f;
    public float bulletSpeed = 60f;
    public GameObject PlayerCamera;
    public GameObject CarCamera;
    public GameObject Car;
    public int carammo = 15;
    public bool carcanShoot = true;
    public GameObject bulletObject;
    public bool isDriving = false;
    public float explosionRadius = 5f;
    public int explosionDamage = 20;
    // public GameObject explosionEffect;

    void Start() {
        CarCamera.SetActive(false);
    }

    void Update() {
        float ToDrive = Vector3.Distance(transform.position, Player.position);
        if (ToDrive <= 3f) {
            if (Input.GetKeyDown(KeyCode.E)) {
                ToggleDriveMode();
            }
        }

        if (isDriving) {
            DriveCar();
        }

        if(Input.GetMouseButton(0) && carammo>0 && carcanShoot && isDriving){
            StartCoroutine(ShootWithDelay());
        }
    }

    IEnumerator ShootWithDelay(){
        carcanShoot = false;

        Shooting();
        carammo--;

        yield return new WaitForSeconds(1f);

        carcanShoot = true;
    }

    void Shooting(){
        float forwardOffset = 1.5f;
        Vector3 forwardDirection = transform.forward * forwardOffset;
        Vector3 up = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 spawnPosition = up + forwardDirection;
        GameObject bullet = Instantiate(bulletObject, spawnPosition, Quaternion.identity);
        Rigidbody rb = bullet.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = transform.forward * bulletSpeed;
        carBullet bulletScript = bullet.AddComponent<carBullet>();
        bulletScript.explosionRadius = explosionRadius;
        bulletScript.explosionDamage = explosionDamage;
        // bulletScript.explosionEffect = explosionEffect;
        Destroy(bullet, 5f);
    }

    void ToggleDriveMode() {
        isDriving = !isDriving;

        if (isDriving) {
            Player.SetParent(Car.transform);
            Player.localPosition = new Vector3(0, 1, 0);
            Player.localRotation = Quaternion.identity;
            Player.gameObject.SetActive(false);
            
            PlayerCamera.SetActive(false);
            CarCamera.SetActive(true);
        } else {
            Player.SetParent(null);
            Player.gameObject.SetActive(true);

            PlayerCamera.SetActive(true);
            CarCamera.SetActive(false);
        }
    }

    void DriveCar() {
        float move = Input.GetAxis("Vertical") * Time.deltaTime * 10f;
        Car.transform.Translate(Vector3.forward * move);
        float turn = Input.GetAxis("Horizontal") * Time.deltaTime * 50f;
        Car.transform.Rotate(Vector3.up, turn);

        if (Input.GetAxis("Mouse X") != 0){
            float cameraturn = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;

            transform.Rotate(0, cameraturn, 0, Space.World);
        }
    }
}
