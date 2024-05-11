using Application.IServices.External;
using Microsoft.AspNetCore.Mvc;
using REST.Exceptions;
using System.IO;
using System.Threading.Tasks;

namespace REST.Controllers
{
	[ApiController]
	[Route("images")]
	public class ImagesController : ControllerBase
	{
		private readonly IImagesService _imagesService;


		public ImagesController(IImagesService imagesService)
		{
			this._imagesService = imagesService;
		}

		[HttpGet("{imageName}")]
		public async Task<ActionResult> GetImage(string imageName)
		{
			FileInfo fileInfo = new FileInfo(imageName);

			string contentType = null;

			switch (fileInfo.Extension)
			{
				case (".png"):
					contentType = "image/png";
					break;

				case (".jpg"):
					contentType = "image/jpeg";
					break;

				case (".svg"):
					contentType = "image/svg+xml";
					break;

				default:
					PresentationLayerException exception = new PresentationLayerException(
						$"Запрошено изображение с неподдерживаемым форматом. {fileInfo.Extension}"
						);

					return new BadRequestObjectResult(exception);
			}

			byte[] image = await _imagesService.GetImageAsync(imageName);

			if (image is null)
			{
				PresentationLayerException exception = new PresentationLayerException($"Изображение с названием \"{imageName}\" не найдено.");

				return new BadRequestObjectResult(exception);
			}

			return File(image, contentType);
		}
	}
}
