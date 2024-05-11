using Application.IServices.External;
using LocalImages.Exceptions;
using LocalImages.Options;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace LocalImages.ImagesService
{
	public class LocalImagesService : IImagesService
	{
		public string DirectoryPath { get; init; }


		public LocalImagesService(IOptions<LocalImagesServiceOptions> options)
		{
			if (string.IsNullOrWhiteSpace(options.Value.DirectoryPath))
			{
				throw new LocalImagesOptionsConfugeredException($"Переменная {nameof(DirectoryPath)} is null");
			}

			this.DirectoryPath = options.Value.DirectoryPath;
		}


		public void DeleteImage(string imageName)
		{
			string path = Path.Combine(DirectoryPath, imageName);

			File.Delete(path);
		}

		public async Task<byte[]> GetImageAsync(string imageName)
		{
			byte[] image = null;

			string path = Path.Combine(DirectoryPath, imageName);

			image = await File.ReadAllBytesAsync(path);

			return image;
		}

		public async Task SaveImageAsync(byte[] image, string imageName)
		{
			string path = Path.Combine(DirectoryPath, imageName);

			await File.WriteAllBytesAsync(path, image);
		}
	}
}
