﻿using System.Collections;
using OnefallGames;
using UnityEngine;

public class DynamicKnifeController : MonoBehaviour {

	private TargetController targetControl = null;
	private Rigidbody2D rigid = null;
	private BoxCollider2D boxCollider = null;

	private Vector2 originalPos = Vector2.zero;
//	private float normalKnifeForceUp = 8f;
	private float lastKnifeForceUp = 25f;
//	private float lastKnideTorque = 15f;
//	private float normalKnifeTorque = 7f;
	public static bool canShoot = true;
	private float forceUp = 0;
	private float torque = 0;
	private void OnEnable()
	{
		GameManager.GameStateChanged += GameManager_GameStateChanged;
	}

	private void OnDisable()
	{
		GameManager.GameStateChanged -= GameManager_GameStateChanged;
	}

	private void GameManager_GameStateChanged(GameState obj)
	{
		if (obj == GameState.FinishState)
		{
			transform.parent = null;
			boxCollider.enabled = false;
			Vector2 forceDir = (forceUp == lastKnifeForceUp) ? Vector2.up : 
				(Vector2)(transform.position - targetControl.transform.position).normalized;
			forceDir *= forceUp;
			rigid.bodyType = RigidbodyType2D.Dynamic;
			rigid.AddForceAtPosition(forceDir, targetControl.transform.position, ForceMode2D.Impulse);
			rigid.AddTorque(torque, ForceMode2D.Impulse);
			Destroy(gameObject, 2f);

		}
	}
	float speed;

	void Start () {

		rigid = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
		targetControl = FindObjectOfType<TargetController>();
	}

	public void Init(float mySpeed, float scale) {

		speed = mySpeed;


//		GetComponent<SpriteRenderer> ().color = color;

		transform.localPosition = Vector2.zero;

		transform.localScale *= scale;

//		topRight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
	}

	public void MoveUp(float speed)
	{
		StartCoroutine(MovingUp(speed));
	}
	private IEnumerator MovingUp(float speed)
	{
		while (GameManager.Instance.GameState == GameState.Playing)
		{
			transform.position += Vector3.up * speed * Time.deltaTime;
			yield return null;


			float distance = (boxCollider.size.y / 2f) + 0.05f;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, distance);

			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("Respawn")) //Hit target
				{
					//                    SoundManager.Instance.PlaySound(SoundManager.Instance.hitTarget);

					//                    //Srt knife position
					Vector2 pos = (Vector2)hit.transform.position 
						+ Vector2.down * hit.collider.GetComponent<CircleCollider2D>().radius;
					transform.position = pos;
					//                    
					//                    //Set parent for this knife 
					//                    transform.SetParent(hit.transform);
					//
					//                    //Bouncing the target
					targetControl.BounceTarget();
					//
					//                    //Play knife hit target particle
					GameManager.Instance.PlayHitTargetParticle(pos);

					Destroy (gameObject);



					yield break;
				}
				else if (hit.collider.CompareTag("Finish"))//Hit other knife or obstacle
				{
					//Capture screenshot
					//                    ShareManager.CreateScreenshot();

					//Play sound effect
					//                    SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
					CameraShake.Shake (0.3f, 0.35f);
					Handheld.Vibrate();
					canShoot = false;
					boxCollider.enabled = false;
					rigid.bodyType = RigidbodyType2D.Dynamic;
//					rigid.AddTorque(10, ForceMode2D.Impulse);
					StartCoroutine(DisableObject());

					yield break;
				}
			}
		}
	}

	IEnumerator DisableObject()
	{
		while (gameObject.activeInHierarchy)
		{
			Vector2 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
			if (viewPortPos.x >= 1.1f || viewPortPos.x <= -0.1f || viewPortPos.y <= -0.1f || viewPortPos.y >= 1.1f)
			{
				rigid.bodyType = RigidbodyType2D.Static;
				transform.position = originalPos;
				transform.eulerAngles = Vector3.zero;
				boxCollider.enabled = true;
				gameObject.SetActive(false);

				if (GameManager.Instance.IsRevived)
				{
					GameManager.Instance.GameOver();
				}
				else
				{
//					if (AdManager.Instance.IsRewardedVideoAdReady())
//					{
//						GameManager.Instance.Revive();
//
//					}
//					else
//					{
						GameManager.Instance.GameOver();
						canShoot = true;
//					}
				}

				yield break;
			}
			yield return null;
		}
	}




	public void MoveToPosition(Vector2 pos, float time)
	{
		if (originalPos == Vector2.zero)
			originalPos = transform.position;
		StartCoroutine(MovingToPos(pos, time));
	}
	private IEnumerator MovingToPos(Vector2 pos, float time)
	{
		float t = 0;
		Vector2 startPos = transform.position;
		while (t < time)
		{
			t += Time.deltaTime;
			float factor = t / time;
			transform.position = Vector2.Lerp(startPos, pos, factor);
			yield return null;
		}
	}

	void Update () {
//		topRight = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, Screen.height));
//		Vector2 topRight = Camera.main.WorldToViewportPoint(transform.position);
		if (transform.position.y > 1.9f) {
			Destroy (gameObject);
		}
	}
}
