using UnityEngine;

namespace CQ
{
	[RequireComponent(typeof(MeshRenderer))]
	public class UVMapper : MonoBehaviour
	{
		[SerializeField] int targetMaterialIndex = 0;
		[SerializeField] float multiplier = 3.0f;

		MeshRenderer meshRenderer;

		void Awake()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			meshRenderer.materials[targetMaterialIndex] = Instantiate(meshRenderer.sharedMaterials[targetMaterialIndex]);
		}

		void Update()
		{
			meshRenderer.materials[targetMaterialIndex].mainTextureScale =
				multiplier * new Vector2(transform.localScale.x, transform.localScale.z);
		}
	}
}