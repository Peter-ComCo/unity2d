﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;
    


    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;	
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
	downRotation = Quaternion.Euler(0, 0, -90);
	forwardRotation = Quaternion.Euler(0, 0, 35);
	game = GameManager.Instance;
	rigidbody.simulated = false;
    }

    void OnEnable() {
	GameManager.OnGameStarted += OnGameStarted;
	GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable() {
	GameManager.OnGameStarted -= OnGameStarted;
	GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }

    void OnGameStarted() {
	rigidbody.velocity = Vector3.zero;
	rigidbody.simulated = true;
    }

    void OnGameOverConfirmed() {
	transform.localPosition = startPos;
	transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
	if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0)) {
        //Time.timeScale += 1;
		transform.rotation = forwardRotation;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);	

	    }
        if (game.GameOver == false)
        {

            OnPlayerScored();

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
	
	if (col.gameObject.tag == "DeadZone") {
		rigidbody.simulated = false;
		OnPlayerDied();	
	}
    }	
}
