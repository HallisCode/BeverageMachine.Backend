using Application.DTO;
using Application.DTO.BeverageMaintenance;
using Application.Exceptions.CRUD;
using Application.IServices;
using Application.IServices.External;
using Database.IUnitWork;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
	public class BeverageMaintetanceService : IBeverageMaintetanceService
	{
		private readonly IUnitWork _unitWork;

		private readonly IImagesService _imagesService;


		public BeverageMaintetanceService(IUnitWork unitWork, IImagesService imagesService)
		{
			this._unitWork = unitWork;

			this._imagesService = imagesService;
		}


		public async Task<DrinkDTO> AddDrinkAsync(AddDrinkRequest request)
		{
			Drink drink = await _unitWork.DrinkRepository.SelectFirstLikeTitleAsync(request.Title);

			if (drink is not null) throw new AlreadyExistException($"Напиток с названием \"{drink.Title}\" уже существует.");

			// Объединяем сохранине картинки и напитка в одну транзакцию
			using (ITransaction transaction = await _unitWork.BeginTransactionAsync())
			{
				// Название для картинки {GUID}.extension
				string imageName = Guid.NewGuid().ToString() + request.Image.Extension;

				await _imagesService.SaveImageAsync(request.Image.Image, imageName);

				drink = new Drink(
					title: request.Title,
					imageName: imageName,
					count: request.Count,
					cost: request.Cost);

				await _unitWork.DrinkRepository.CreateAsync(drink);


				await transaction.CommitAsync();
			}

			return new DrinkDTO(
				ID: drink.ID,
				Title: drink.Title,
				ImageName: drink.ImageName,
				Cost: drink.Cost,
				Count: drink.Count,
				isBlocked: false);
		}

		public async Task LockUnlockDrinkAsync(LockUnlockDrinkRequest request)
		{
			BlockedDrink blockedDrink = await _unitWork.BlockedDrinkRepository.SelectFirstAsync(drink => drink.DrinkID == request.drinkID);

			if (blockedDrink is null)
			{
				Drink drink = await _unitWork.DrinkRepository.SelectFirstAsync(drink => drink.ID == request.drinkID);

				if (drink is null) throw new DoesNotExist($"Напитка с идентификатором {request.drinkID} не существует.");


				blockedDrink = new BlockedDrink(request.drinkID);

				await _unitWork.BlockedDrinkRepository.CreateAsync(blockedDrink);
			}
			else
			{
				await _unitWork.BlockedDrinkRepository.DeleteAsync(blockedDrink);
			}
		}

		public async Task DeleteDrinkAsync(DeleteDrinkRequest request)
		{
			Drink drink = await _unitWork.DrinkRepository.SelectFirstAsync(drink => drink.ID == request.drinkID);

			if (drink is null) throw new DoesNotExist($"Напитка с идентификатором {request.drinkID} не существует.");

			// Объединяем удаление изображения и напитка в одну тразакцию
			using (ITransaction transaction = await _unitWork.BeginTransactionAsync())
			{
				_imagesService.DeleteImage(drink.ImageName);

				await _unitWork.DrinkRepository.DeleteAsync(drink);


				await transaction.CommitAsync();
			}
		}

		public async Task LockUnlockCoinAsync(LockUnlockCoinRequest request)
		{
			BlockedCoin blockedCoin = await _unitWork.BlockedCoinRepository.SelectFirstAsync(coin => coin.RubleCoin == request.Coin);

			if (blockedCoin is null)
			{
				blockedCoin = new BlockedCoin(request.Coin);

				await _unitWork.BlockedCoinRepository.CreateAsync(blockedCoin);
			}
			else
			{
				await _unitWork.BlockedCoinRepository.DeleteAsync(blockedCoin);
			}
		}

		public async Task UpdateDrinkAsync(UpdateDrinkRequest request)
		{
			Drink drink = await _unitWork.DrinkRepository.SelectFirstAsync(drink => drink.ID == request.ID);

			if (drink is null) throw new DoesNotExist($"Напитка с идентификатором {request.ID} не существует.");


			Drink drinkother = await _unitWork.DrinkRepository.SelectFirstLikeTitleAsync(request.Title);

			if (drinkother is not null) throw new AlreadyExistException($"Напиток с названием \"{drink.Title}\" уже существует.");


			// Объединяем обновление картинки и напитка в одну транзакцию.
			using (ITransaction transaction = await _unitWork.BeginTransactionAsync())
			{
				string imageName = null;

				if (request.Image is not null)
				{
					_imagesService.DeleteImage(drink.ImageName);

					// Название для картинки {GUID}.extension
					imageName = Guid.NewGuid().ToString() + request.Image.Extension;

					await _imagesService.SaveImageAsync(request.Image.Image, imageName);
				}

				drink.Update(title: request.Title,
					count: request.Count,
					imageName: imageName,
					cost: request.Cost
				);

				await _unitWork.DrinkRepository.UpdateAsync(drink);


				await transaction.CommitAsync();
			}
		}

		public async Task<NumberCoinsDTO> ChangeNumberCoinsAsync(ChangeNumberCoinsRequest request)
		{
			int numberOneRuble = 0;
			int numberTwoRuble = 0;
			int numberFiveRuble = 0;
			int numberTenRuble = 0;


			using (ITransaction transaction = await _unitWork.BeginTransactionAsync())
			{
				if (request.NumberOneRuble is not null)
				{
					numberOneRuble = await AdjustNumberCoinsAsync(RubleCoin.One, (int)request.NumberOneRuble);
				}
				else
				{
					numberOneRuble = await GetNumberSelectedCoinAsync(RubleCoin.One);
				}

				if (request.NumberTwoRuble is not null)
				{
					numberTwoRuble = await AdjustNumberCoinsAsync(RubleCoin.Two, (int)request.NumberTwoRuble);
				}
				else
				{
					numberTwoRuble = await GetNumberSelectedCoinAsync(RubleCoin.Two);
				}

				if (request.NumberFiveRuble is not null)
				{
					numberFiveRuble = await AdjustNumberCoinsAsync(RubleCoin.Five, (int)request.NumberFiveRuble);
				}
				else
				{
					numberFiveRuble = await GetNumberSelectedCoinAsync(RubleCoin.Five);
				}

				if (request.numberTenRuble is not null)
				{
					numberTenRuble = await AdjustNumberCoinsAsync(RubleCoin.Ten, (int)request.numberTenRuble);
				}
				else
				{
					numberTenRuble = await GetNumberSelectedCoinAsync(RubleCoin.Ten);
				}

				await transaction.CommitAsync();
			}

			return new NumberCoinsDTO(
				NumberOneRuble: numberOneRuble,
				NumberTwoRuble: numberTwoRuble,
				NumberFiveRuble: numberFiveRuble,
				numberTenRuble: numberTenRuble);
		}

		public async Task<NumberCoinsDTO> GetNumberCoinsAsync()
		{
			int numberOneRuble = await GetNumberSelectedCoinAsync(RubleCoin.One);

			int numberTwoRuble = await GetNumberSelectedCoinAsync(RubleCoin.Two);

			int numberFiveRuble = await GetNumberSelectedCoinAsync(RubleCoin.Five);

			int numberTenRuble = await GetNumberSelectedCoinAsync(RubleCoin.Ten);

			return new NumberCoinsDTO(
				NumberOneRuble: numberOneRuble,
				NumberTwoRuble: numberTwoRuble,
				NumberFiveRuble: numberFiveRuble,
				numberTenRuble: numberTenRuble);
		}

		/// <summary>
		/// Подгоняет количество монет в хранилище автомата, согласно заданному числу <paramref name="adjustableNumber"/>. <br/>
		/// Благодаря удалению / добавлению монет ( логично ).
		/// </summary>
		/// <param name="coinType"></param>
		/// <param name="adjustableNumber"></param>
		/// <returns>Количество заданных монет <paramref name="coinType"/> в автомате.</returns>
		protected async Task<int> AdjustNumberCoinsAsync(RubleCoin coinType, int adjustableNumber)
		{
			int numberCoins = await GetNumberSelectedCoinAsync(coinType);

			int delta = adjustableNumber - numberCoins;

			int actualNumberCoins = numberCoins;

			if (delta > 0)
			{
				List<AcceptedCoin> coins = new List<AcceptedCoin>();

				for (int i = 0; i < delta; i++)
				{
					coins.Add(new AcceptedCoin(coinType));
				}

				await _unitWork.AcceptedCoinRepository.CreateAsync(coins);

				actualNumberCoins = numberCoins + delta;
			}
			else if (delta < 0)
			{
				List<AcceptedCoin> storageCoins = await _unitWork.AcceptedCoinRepository.SelectAllAsync(
					coin => coin.RubleCoin == coinType, Math.Abs(delta), 0);

				await _unitWork.AcceptedCoinRepository.DeleteAsync(storageCoins);

				actualNumberCoins = numberCoins - Math.Abs(delta);
			}

			return actualNumberCoins;
		}

		protected async Task<int> GetNumberSelectedCoinAsync(RubleCoin coin)
		{
			return await _unitWork.AcceptedCoinRepository.CountAsync(
				acceptedCoin => acceptedCoin.RubleCoin == coin
				);
		}
	}
}
