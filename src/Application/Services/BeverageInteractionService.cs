using Application.DTO;
using Application.DTO.BeverageInteraction;
using Application.Exceptions;
using Application.Exceptions.CRUD;
using Application.IServices;
using Database.IUnitWork;
using Domain.Entities;
using Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Application.Services
{
	public class BeverageInteractionService : IBeverageInteractionService
	{
		protected IUnitWork _unitWork;


		public BeverageInteractionService(IUnitWork unitWork)
		{
			this._unitWork = unitWork;
		}


		#region Публичные методы

		public async Task<ChangeResponse> AcceptOrderAsync(DrinkOrderRequest request)
		{
			List<RubleCoin> coinsBuffered = new List<RubleCoin>(request.Coins); // Введенные покупателем монеты

			int balance = coinsBuffered.Sum(coin => coin.Value);


			Drink drink = await GetDrinkAsync(drinkID: request.drinkID, balance: balance);

			await VerifyAvailabilityCoins(coinsBuffered.Distinct());


			int changeAmount = balance - drink.Cost; // Сумма сдачи

			List<RubleCoin> coinsReadyForChange = new List<RubleCoin>(); // монеты которые выдадутся в качестве сдачи


			using (ITransaction transaction = await _unitWork.BeginTransactionAsync())
			{

				if (changeAmount > 0)
				{
					List<RubleCoin> coinsTakedFromBuffered = TakeCoinsForChangeFromBuffered(ref changeAmount, ref coinsBuffered);

					coinsReadyForChange.AddRange(coinsTakedFromBuffered);
				}

				if (changeAmount > 0)
				{
					int numberOneRuble = await GetNumberSelectedCoinAsync(RubleCoin.One);

					int numberTwoRuble = await GetNumberSelectedCoinAsync(RubleCoin.Two);

					int numberFiveRuble = await GetNumberSelectedCoinAsync(RubleCoin.Five);

					int numberTenRuble = await GetNumberSelectedCoinAsync(RubleCoin.Ten);


					bool isEnoughCoins = CalculateNumberCoinsNeedTakeFromStorage(ref changeAmount,

						numberTenRuble, numberFiveRuble,
						numberTwoRuble, numberOneRuble,

						out int shouldTakeOneRuble, out int shouldTakeTwoRuble,
						out int shouldTakeFiveRuble, out int shouldTakeTenRuble
						);


					if (isEnoughCoins is false) throw new ApplicationLayerException("В автомате не хватает средств для выдачи сдачи.");


					List<RubleCoin> coinsTakedFromStorage = await TakeCoinsForChangeFromStorage(
						shouldTakeOneRuble, shouldTakeTwoRuble,
						shouldTakeFiveRuble, shouldTakeTenRuble
					);

					coinsReadyForChange.AddRange(coinsTakedFromStorage);
				}


				// Обрабатываем заказ

				List<AcceptedCoin> acceptedCoins = new List<AcceptedCoin>();

				foreach (RubleCoin coin in coinsBuffered)
				{
					acceptedCoins.Add(new AcceptedCoin(coin));
				}

				drink.Count--;


				await _unitWork.AcceptedCoinRepository.CreateAsync(acceptedCoins);

				await _unitWork.DrinkRepository.UpdateAsync(drink);


				await transaction.CommitAsync();
			}


			return new ChangeResponse(coinsReadyForChange.ToArray());
		}

		public async Task<AllDrinksResponse> GetAllDrinksAsync()
		{
			List<Drink> drinks = await _unitWork.DrinkRepository.SelectAllAsync(drink => true);

			List<BlockedDrink> blockedDrinks = await _unitWork.BlockedDrinkRepository.SelectAllAsync(drink => true);


			List<DrinkDTO> drinkResponses = new List<DrinkDTO>();

			foreach (Drink drink in drinks)
			{
				bool isBlocked = false;

				if (blockedDrinks.Where(blockedDrink => blockedDrink.DrinkID == drink.ID).Count() > 0)
				{
					isBlocked = true;
				}

				DrinkDTO drinkResponse = new DrinkDTO(
					ID: drink.ID,
					Title: drink.Title,
					ImageName: drink.ImageName,
					Cost: drink.Cost,
					Count: drink.Count,
					isBlocked: isBlocked
					);

				drinkResponses.Add(drinkResponse);
			}


			return new AllDrinksResponse(drinkResponses.ToArray());
		}

		public async Task<AllBlockedCoinsResponse> GetBlockedCoinsAsync()
		{
			List<BlockedCoin> coins = await _unitWork.BlockedCoinRepository.SelectAllAsync(coin => true);

			return new AllBlockedCoinsResponse(coins.Select(blockedCoin => blockedCoin.RubleCoin).ToArray());
		}

		#endregion

		/// <summary>
		/// Получаем выбранный напиток на основе id, со всеми соответствующими проверками.
		/// </summary>
		/// <param name="drinkID"></param>
		/// <param name="balance"></param>
		/// <returns></returns>
		/// <exception cref="DoesNotExist"></exception>
		/// <exception cref="ApplicationLayerException"></exception>
		protected async Task<Drink> GetDrinkAsync(long drinkID, int balance)
		{
			Drink drink = await _unitWork.DrinkRepository.SelectFirstAsync(drink => drink.ID == drinkID);

			if (drink is null) throw new DoesNotExist($"Напитка с идентификатором {drinkID} не существует.");

			bool isBlockedDrink = await _unitWork.BlockedDrinkRepository.SelectFirstAsync(
				blockedDrink => blockedDrink.DrinkID == drink.ID) != null;

			if (isBlockedDrink) throw new ApplicationLayerException($"Напиток \"{drink.Title}\" не доступен для продажи.");

			if (drink.Count <= 0) throw new ApplicationLayerException($"Выбранный напиток : \"{drink.Title}\" — закончился.");

			if (balance < drink.Cost) throw new ApplicationLayerException($"Недостаточно средств для покупки напитка \"{drink.Title}\".");


			return drink;
		}

		/// <summary>
		/// Проверяет, что переданные монеты <paramref name="coins"/> не заблокированы.
		/// </summary>
		/// <param name="coins"></param>
		/// <returns></returns>
		/// <exception cref="ApplicationLayerException"></exception>
		protected async Task VerifyAvailabilityCoins(IEnumerable<RubleCoin> coins)
		{
			foreach (RubleCoin coin in coins)
			{
				bool isBlockedCoin = await _unitWork.BlockedCoinRepository.SelectFirstAsync(
					blockedCoin => blockedCoin.RubleCoin == coin) != null;

				if (isBlockedCoin) throw new ApplicationLayerException($"Монеты номиналом {coin.Value} в данный момент не принимаются.");
			}
		}

		/// <summary>
		/// Выбирает нужные монеты для выдачи сдачи.<br/> 
		/// <br>
		/// Каждую выбранную нужную монету удаляет из буфера монет <paramref name="coinsBuffered"/> 
		/// и добавлят в список монет готовых к выдаче <paramref name="coinsReadyForChange"/>. <br/>
		/// После чего уменьшает сумму сдачи, которую нужно выдать <paramref name="changeAmount"/>.
		/// </br>
		/// </summary>
		/// <param name="changeAmount"></param>
		/// <param name="coinsBuffered"></param>
		/// <param name="coinsReadyForChange"></param>
		/// <returns>Возращает список монет, взятых из буффера.</returns>
		protected List<RubleCoin> TakeCoinsForChangeFromBuffered(ref int changeAmount, ref List<RubleCoin> coinsBuffered)
		{
			if (changeAmount <= 0) throw new ApplicationLayerException("Попытка посчитать монеты для сдачи, при её отсутствии.");


			List<RubleCoin> coinsReadyForChange = new List<RubleCoin>();

			coinsBuffered.Sort();

			for (int i = coinsBuffered.Count - 1; i >= 0; i--)
			{
				if (coinsBuffered[i] <= changeAmount)
				{
					coinsReadyForChange.Add(coinsBuffered[i]); // готовим монету к выдаче

					changeAmount -= coinsBuffered[i]; // уменьшаем сумму сдачи

					coinsBuffered.RemoveAt(i); // удаляем монету с буфера
				}

				if (changeAmount <= 0) break;
			}

			return coinsReadyForChange;
		}

		/// <summary>
		/// <para>
		/// Высчитывает количество каких монеты нужно еще взять, на основе данных о количестве монет в хранилище. <br/>
		/// Попутно уменьшает сумму сдачи <paramref name="changeAmount"/>.
		/// </para>
		/// </summary>
		/// <param name="changeAmount">Сумма сдачи.</param>
		/// <param name="numberTenRuble">Количество 10 рублей в хранилище.</param>
		/// <param name="numberFiveRuble">Количество 5 рублей в хранилище.</param>
		/// <param name="numberTwoRuble">Количество 2 рублей в хранилище.</param>
		/// <param name="numberOneRuble">Количество 1 рублей в хранилище.</param>
		/// <param name="shouldTakeOneRuble">Количество 1 рублей, которые нужно взять с хранилища.</param>
		/// <param name="shouldTakeTwoRuble">Количество 2 рублей, которые нужно взять с хранилища.</param>
		/// <param name="shouldTakeFiveRuble">Количество 5 рублей, которые нужно взять с хранилища.</param>
		/// <param name="shouldTakeTenRuble">Количество 10 рублей, которые нужно взять с хранилища.</param>
		/// <returns>В случае, если имеющихся монет не хватает для выдачи сдачи, возвращает <b>false</b>.</returns>
		protected bool CalculateNumberCoinsNeedTakeFromStorage(
			ref int changeAmount,

			int numberTenRuble, int numberFiveRuble,
			int numberTwoRuble, int numberOneRuble,

			out int shouldTakeOneRuble, out int shouldTakeTwoRuble,
			out int shouldTakeFiveRuble, out int shouldTakeTenRuble)
		{
			if (changeAmount <= 0) throw new ApplicationLayerException("Попытка посчитать монеты для сдачи, при её отсутствии.");


			shouldTakeOneRuble = 0;
			shouldTakeTwoRuble = 0;
			shouldTakeFiveRuble = 0;
			shouldTakeTenRuble = 0;

			while (true)
			{
				if (numberTenRuble > 0 && changeAmount >= RubleCoin.Ten)
				{
					numberTenRuble--;

					shouldTakeTenRuble++;

					changeAmount -= RubleCoin.Ten;
				}
				else if (numberFiveRuble > 0 && changeAmount >= RubleCoin.Five)
				{
					numberFiveRuble--;

					shouldTakeFiveRuble++;

					changeAmount -= RubleCoin.Five;
				}
				else if (numberTwoRuble > 0 && changeAmount >= RubleCoin.Two)
				{
					numberTwoRuble--;

					shouldTakeTwoRuble++;

					changeAmount -= RubleCoin.Two;
				}
				else if (numberOneRuble > 0 && changeAmount >= RubleCoin.One)
				{
					numberOneRuble--;

					shouldTakeOneRuble++;

					changeAmount -= RubleCoin.One;
				}
				else
				{
					return false;
				}

				if (changeAmount == 0) return true;
			}
		}

		/// <summary>
		/// Забирает ( удаляет ) с хранилища автомата нужные монеты для сдачи.
		/// </summary>
		/// <param name="changeAmount"></param>
		/// <param name="coinsBuffered"></param>
		/// <returns>Возращает монеты взятые из хранилища.</returns>
		protected async Task<List<RubleCoin>> TakeCoinsForChangeFromStorage(
			int shouldTakeOneRuble, int shouldTakeTwoRuble,
			int shouldTakeFiveRuble, int shouldTakeTenRuble)
		{
			List<RubleCoin> coinsReadyForChange = new List<RubleCoin>();

			// Монеты которые мы достанем с хранилища автомата
			List<AcceptedCoin> withdrawalCoins = new List<AcceptedCoin>();

			if (shouldTakeTenRuble > 0)
			{
				List<AcceptedCoin> _acceptedCoins = await GetPortionSelectedCoinAsync(RubleCoin.Ten, shouldTakeTenRuble);

				withdrawalCoins.AddRange(_acceptedCoins);
			}
			if (shouldTakeFiveRuble > 0)
			{
				List<AcceptedCoin> _acceptedCoins = await GetPortionSelectedCoinAsync(RubleCoin.Five, shouldTakeFiveRuble);

				withdrawalCoins.AddRange(_acceptedCoins);
			}
			if (shouldTakeTwoRuble > 0)
			{
				List<AcceptedCoin> _acceptedCoins = await GetPortionSelectedCoinAsync(RubleCoin.Two, shouldTakeTwoRuble);

				withdrawalCoins.AddRange(_acceptedCoins);
			}
			if (shouldTakeOneRuble > 0)
			{
				List<AcceptedCoin> _acceptedCoins = await GetPortionSelectedCoinAsync(RubleCoin.One, shouldTakeOneRuble);

				withdrawalCoins.AddRange(_acceptedCoins);
			}

			// Выводим монеты из хранилища и возвращаем их

			foreach (AcceptedCoin acceptedCoin in withdrawalCoins)
			{
				coinsReadyForChange.Add(acceptedCoin.RubleCoin);
			}

			await _unitWork.AcceptedCoinRepository.DeleteAsync(withdrawalCoins);


			return coinsReadyForChange;
		}

		protected async Task<List<AcceptedCoin>> GetPortionSelectedCoinAsync(RubleCoin coin, int take)
		{
			return await _unitWork.AcceptedCoinRepository.SelectAllAsync(
					acceptedCoin => acceptedCoin.RubleCoin == coin,
					take: take,
					skip: 0);
		}

		protected async Task<int> GetNumberSelectedCoinAsync(RubleCoin coin)
		{
			return await _unitWork.AcceptedCoinRepository.CountAsync(
				acceptedCoin => acceptedCoin.RubleCoin == coin
				);
		}
	}
}
