using REST.DTO;
using System.IO;
using System.Threading.Tasks;

namespace REST.Mapping
{
	public static class AddDrinkRequestMapping
	{
		public static async Task<Application.DTO.BeverageMaintenance.AddDrinkRequest> MapToApplicationDTO(this AddDrinkRequest request)
		{
			byte[] image;

			using (MemoryStream stream = new MemoryStream())
			{
				await request.Image.CopyToAsync(stream);

				image = stream.ToArray();
			}

			FileInfo imageInfo = new FileInfo(request.Image.FileName);

			return new Application.DTO.BeverageMaintenance.AddDrinkRequest(
				Title: request.Title,
				Image: new Application.DTO.ImageDTO(image, imageInfo.Extension),
				Cost: request.Cost,
				Count: request.Count);
		}
	}
}
