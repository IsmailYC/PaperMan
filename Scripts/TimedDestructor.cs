using UnityEngine;
using System.Collections;

public class TimedDestructor : MonoBehaviour {
	public float time;

	void Start()
	{
		Destroy (gameObject, time);
	}
}
