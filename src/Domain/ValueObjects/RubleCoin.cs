using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.ValueObjects
{
	/// <summary>
	/// Описывает существующие рублевые монеты номиналом 1, 2, 5 и 10 рублей.
	/// </summary>
	public record RubleCoin : IComparable<RubleCoin>
	{
		public int Value { get; init; }


		public RubleCoin(int value)
		{
			switch (value)
			{
				case (1):
				case (2):
				case (5):
				case (10):
					Value = value;
					break;

				default:
					throw new UnSpecifiedCoinException($"Монеты наминалом {value} не поддерживаются.");
			}
		}

		// Logic
		public int CompareTo(RubleCoin other)
		{
			if (this.Value < other.Value) return -1;

			if (this.Value == other.Value) return 0;

			return 1;
		}

		// Operators
		public static implicit operator RubleCoin(int value) => new RubleCoin(value);

		public static implicit operator int(RubleCoin rubleCoin) => rubleCoin.Value;


		// Static
		public static readonly RubleCoin One = new RubleCoin(1);

		public static readonly RubleCoin Two = new RubleCoin(2);

		public static readonly RubleCoin Five = new RubleCoin(5);

		public static readonly RubleCoin Ten = new RubleCoin(10);

		public static readonly ReadOnlyCollection<RubleCoin> AllSupportedCoins = new RubleCoin[]
		{
			One, Two, Five, Ten
		}.AsReadOnly();
	}
}
