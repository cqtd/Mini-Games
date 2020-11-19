using System;
using UnityEngine;

namespace CQ.MiniGames
{
	using Yacht;
	
	public static class DiceResolver
	{
		public static Vector3 GetLocalRotation(this Enums.DiceFace upside, Enums.DiceValue value)
		{
			switch (upside)
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
					throw new ArgumentOutOfRangeException(nameof(upside), upside, null);
			}
			return Vector3.zero;
		}
	}
}