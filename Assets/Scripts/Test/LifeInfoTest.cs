using UnityEngine;
using System.Collections;

public class LifeInfoTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh>().text = GetComponentInParent<Life>().currentLife + " / " + GetComponentInParent<Life>().maximumLife;
	}
}
