using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private Vector3 originalPos;
	public static CameraShake instance;
	// Use this for initialization
	void Awake () {
		originalPos = transform.position;
		instance = this;
	}

	public static void Shake (float duration, float amount){
		instance.StartCoroutine (instance.coShake(duration, amount));
	}

	public IEnumerator coShake (float duration, float amount){
		float endTime = Time.time + duration;

		while (Time.time < endTime) {
			transform.localPosition = originalPos + Random.insideUnitSphere * amount;
			duration = duration - Time.deltaTime;
			yield return null;
		}
		transform.localPosition = originalPos;
	}
}
