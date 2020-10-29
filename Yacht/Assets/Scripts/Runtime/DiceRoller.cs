using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CQ.MiniGames
{
	public class DiceRoller : MonoBehaviour
	{
		public GameObject dice;
		public Vector3 startPos;
		Rigidbody body;
		public float value; 
		public float qValue; 

		void Awake()
		{
			body = dice.GetComponent<Rigidbody>();
		}

		public void Roll()
		{
			
			dice.transform.position = startPos;
			dice.transform.rotation = UnityEngine.Random.rotation;
			body.velocity =
				new Vector3(Random.Range(0, value), Random.Range(-value, value), Random.Range(0, value));
			body.angularVelocity = new Vector3(Random.Range(0, qValue), Random.Range(0, qValue), Random.Range(0, qValue));
		}

		void CreateAnim()
		{
			AnimationClip clip = new AnimationClip();
		}
	}
}