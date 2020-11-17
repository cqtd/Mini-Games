using UnityEngine;

namespace CQ.MiniGames
{
	public class DiceRollTest : MonoBehaviour
	{
		public GameObject dicePrefab = default;
		public Transform startPosMarker = default;

		public float minOffset = -1;
		public float maxOffset = 1;

		public float minForce = 2;
		public float maxForce = 10;

		public float minAngular = 0;
		public float maxAnguler = 360;

		GameObject[] dices;
		
		void Awake()
		{
			dices = new GameObject[5];
			for (int i = 0; i < 5; i++)
			{
				dices[i] = Instantiate(dicePrefab);
			}
			
			Roll();
		}

		Vector3 GetRandomOffset(float min, float max)
		{
			return new Vector3(Random.Range(min, max), Random.Range(min, max),Random.Range(min, max));
		}

		[ContextMenu("Roll")]
		public void Roll()
		{
			foreach (GameObject dice in dices)
			{
				dice.transform.position = startPosMarker.position + GetRandomOffset(minOffset, maxOffset);
				var rb = dice.GetComponent<Rigidbody>();
				
				rb.useGravity = true;
				rb.angularVelocity = GetRandomOffset(minAngular, maxAnguler);
				rb.velocity = startPosMarker.forward * Random.Range(minForce, maxForce);
			}
		}
	}
}