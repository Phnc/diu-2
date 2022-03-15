using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{

	/// <summary>
	/// Quem a camera precisa seguir
	/// </summary>
	public Transform target;
	/// <summary>
	/// O quao suave eh o movimento da camera
	/// </summary>
	public float smoothing = 5f;

	Vector3 offset;

	void Start()
	{
		// a diferenca entre onde a camera começa e onde o jogador esta
		offset = transform.position - target.position; 
	}

	void FixedUpdate()
	{
		Vector3 targetCamPos = target.position + offset;
		// Mover a camera para uma nova posicao
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
