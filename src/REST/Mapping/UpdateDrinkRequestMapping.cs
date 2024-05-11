using Application.DTO;
using REST.DTO;
using System.IO;
using System.Threading.Tasks;

namespace REST.Mapping
{
	public static class UpdateDrinkRequestMapping
	{
		public static async Task<Application.DTO.BeverageMaintenance.UpdateDrinkRequest> MapToApplicationDTO(this UpdateDrinkRequest request)
		{
			ImageDTO imageDTO = null;

			if (request.Image is not null)
			{
				byte[] image;

				using (MemoryStream stream = new MemoryStream())
				{
					await request.Image.CopyToAsync(stream);

					image = stream.ToArray();
				}

				FileInfo imageInfo = new FileInfo(request.Image.FileName);

				imageDTO = new ImageDTO(image, imageInfo.Extension);
			}


			return new Application.DTO.BeverageMaintenance.UpdateDrinkRequest(
				ID: request.DrinkId,
				Title: request.Title,
				Image: imageDTO,
				Cost: request.Cost,
				Count: request.Count);
		}
	}
}
