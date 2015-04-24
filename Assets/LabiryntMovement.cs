using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LabiryntMovement : MonoBehaviour
{
	public Text Score;
	public Transform cammove, camrotate;
	Rigidbody body;
	int yourscore;
	bool destroy = false;
	bool k = false;
	bool teleport = false;
	int i = 0;
	GameObject fordestroy;

	// Use this for initialization
	void Start ()
	{
		body = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Exit") {
			Application.LoadLevel (0);
		}
		if (other.tag == "Player") {
			k = true;
			yourscore ++;
		}
		if (other.name == "Cube 4") {
			teleport = true;
			//Camera.main.transform.RotateAround (camrotate.position, Vector3.up, 180f);
		}
	}
	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player") {
			fordestroy = other.gameObject;
			destroy = true;
		}
	}
	// Update is called once per frame
	void Update ()
	{

		Score.text = "Score: " + yourscore;
		camrotate.position = transform.position;
		Vector3 accel = Input.acceleration;
		body.AddForce (camrotate.TransformDirection (accel.x * 30, 0, (accel.y + 0.6f) * 30));

		if (k) {
			GetComponent<MeshRenderer> ().material.color = Color.Lerp (GetComponent<MeshRenderer> ().material.color, new Color (Random.Range (0, 0.255f), Random.Range (0, 0.255f), Random.Range (0, 0.255f)), i / 100f);
			i++;
			if (i >= 100) {
				k = false;
				i = 0;
			}
		} 
		if (teleport) {
			transform.position = Vector3.Lerp (transform.position, GameObject.Find ("Cube 5").transform.position, i / 200f);
			i++;
			if (i >= 200) {
				teleport = false;
				i = 0;
			}
		}
		if (destroy) {
			fordestroy.transform.position = Vector3.Lerp (fordestroy.transform.position, new Vector3 (Random.Range (-40, 10), Random.Range (5, 20), Random.Range (-10, 10)), i / 100f);
			i++;
			if (i >= 100) {
				destroy = false;
			}
		}
		if (Input.touchCount == 1) {
			if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				Vector2 pos = Input.GetTouch (0).deltaPosition;
				if (Mathf.Abs (pos.x) > 0.4) {
					camrotate.Rotate (0, pos.x, 0, Space.World);
				}
				if (Mathf.Abs (pos.y) > 0.4) {
					cammove.Rotate (pos.y, 0, 0, Space.Self);
				}
			}
		}

	}

}
