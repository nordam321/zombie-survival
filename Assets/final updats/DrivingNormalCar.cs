using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingNormalCar : MonoBehaviour {
    public Transform Player;
    private float turnSpeed = 500.0f;
    public GameObject PlayerCamera;
    public GameObject CarCamera;
    public GameObject Car;

    private bool isDriving = false;

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
