using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCamera : MonoBehaviour
{
	public GameObject Target;
	private Rigidbody Body;

	public Vector3 TargetMovementOffset = new Vector3(0, 0, -1);
	public Vector3 TargetLookAtOffset;

	void Awake()
    {
		Body = this.GetComponent<Rigidbody>();

	}

	void Start()
	{
		Assert.IsNotNull(Target);
	}
}
