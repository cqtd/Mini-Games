using System;
using UnityEngine;

namespace CQ.MiniGames
{
	using Yacht.Gameplay;
	
	public static class DiceResolver
	{
		public static Vector3 GetLocalRotation(this Enums.DiceFace upperFace, Enums.DiceValue value)
		{
			switch (upperFace)
			{
				case Enums.DiceFace.FORWARD:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.zero;
						case Enums.DiceValue.TWO:
							return Vector3.right * 90f;
						case Enums.DiceValue.THREE:
							return Vector3.up * -90f;
						case Enums.DiceValue.FOUR:
							return Vector3.up * 90f;
						case Enums.DiceValue.FIVE:
							return Vector3.right * -90f;
						case Enums.DiceValue.SIX:
							return Vector3.up * 180f;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
				case Enums.DiceFace.BACKWARD:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.up * 180f;
						case Enums.DiceValue.TWO:
							return Vector3.right * -90f;
						case Enums.DiceValue.THREE:
							return Vector3.up * 90f;
						case Enums.DiceValue.FOUR:
							return Vector3.up * -90f;
						case Enums.DiceValue.FIVE:
							return Vector3.right * 90f;
						case Enums.DiceValue.SIX:
							return Vector3.zero;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
					
				case Enums.DiceFace.RIGHT:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.up * 90f;
						case Enums.DiceValue.TWO:
							return Vector3.forward * -90f;
						case Enums.DiceValue.THREE:
							return Vector3.zero;
						case Enums.DiceValue.FOUR:
							return Vector3.up * 180f;
						case Enums.DiceValue.FIVE:
							return Vector3.forward * 90f;
						case Enums.DiceValue.SIX:
							return Vector3.up * -90f;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
				case Enums.DiceFace.LEFT:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.up * -90f;
						case Enums.DiceValue.TWO:
							return Vector3.forward * 90f;
						case Enums.DiceValue.THREE:
							return Vector3.up * 180f;
						case Enums.DiceValue.FOUR:
							return Vector3.zero;							
						case Enums.DiceValue.FIVE:
							return Vector3.forward * -90f;
						case Enums.DiceValue.SIX:
							return Vector3.up * 90f;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
				
				case Enums.DiceFace.TOP:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.right * -90f;
						case Enums.DiceValue.TWO:
							return Vector3.zero;
						case Enums.DiceValue.THREE:
							return Vector3.forward * 90f;
						case Enums.DiceValue.FOUR:
							return Vector3.forward * -90f;
						case Enums.DiceValue.FIVE:
							return Vector3.right * 180f;
						case Enums.DiceValue.SIX:
							return Vector3.right * 90f;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
				case Enums.DiceFace.BOTTOM:
				{
					switch (value)
					{
						case Enums.DiceValue.ONE:
							return Vector3.right * 90f;
						case Enums.DiceValue.TWO:
							return Vector3.right * 180f;
						case Enums.DiceValue.THREE:
							return Vector3.forward * -90f;
						case Enums.DiceValue.FOUR:
							return Vector3.forward * 90f;
						case Enums.DiceValue.FIVE:
							return Vector3.zero;
						case Enums.DiceValue.SIX:
							return Vector3.right * -90f;

						case Enums.DiceValue.NONE:
						default:
							throw new ArgumentOutOfRangeException(nameof(value), value, null);
					}
				}
					
				default:
					throw new ArgumentOutOfRangeException(nameof(upperFace), upperFace, null);
			}
		}

		public static Enums.DiceFace GetResult(Transform origin, float maxDistance = 1.414f)
		{
			if (Physics.Raycast(origin.position, origin.up, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.BOTTOM;
			}

			if (Physics.Raycast(origin.position, -origin.up, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.TOP;
			}
			
			if (Physics.Raycast(origin.position, origin.forward, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.BACKWARD;
			}
			
			if (Physics.Raycast(origin.position, -origin.forward, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.FORWARD;
			}
			
			if (Physics.Raycast(origin.position, origin.right, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.LEFT;
			}
			
			if (Physics.Raycast(origin.position, -origin.right, 1.5f, Settings.Physics.groundLayer.value))
			{
				return Enums.DiceFace.RIGHT;
			}

#if UNITY_EDITOR
			Debug.LogError("This must not be undefined", origin.gameObject);
#endif
			
			return Enums.DiceFace.UNDEFINED;
		}
	}
}