using Domain.Exceptions;


namespace Domain.Entities
{
	/// <summary>
	/// Описывает напиток.
	/// </summary>
	public class Drink : IEntityIdentifier<long>
	{
		public long ID { get; private set; }

		public string Title { get; private set; }

		public string ImageName { get; private set; }


		private int _cost;
		public int Cost
		{
			get
			{
				return _cost;
			}
			set
			{
				if (value < 0) throw new DomainLayerException("Стоимость напитка не может быть отрицательным числом.");

				_cost = value;
			}
		}

		private int _count;
		public int Count
		{
			get
			{
				return _count;
			}
			set
			{
				if (value < 0) throw new DomainLayerException("Количество напитков не может быть отрицательным числом.");

				_count = value;
			}
		}


		// Logic
		private Drink() { }

		public Drink(string title, string imageName, int count, int cost)
		{
			Title = title;

			ImageName = imageName;

			Count = count;

			Cost = cost;
		}

		public void Update(string title = null, string imageName = null, int? count = null, int? cost = null)
		{
			if (title is not null) Title = title;

			if (imageName is not null) ImageName = imageName;

			if (count is not null) Count = count ?? Count;

			if (cost is not null) Cost = cost ?? Cost;
		}
	}
}
