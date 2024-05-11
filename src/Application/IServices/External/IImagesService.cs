using System.Threading.Tasks;

namespace Application.IServices.External
{
	public interface IImagesService
	{
		string DirectoryPath { get; } // рабочая директория

		Task SaveImageAsync(byte[] image, string imageName);

		Task<byte[]> GetImageAsync(string imageName);

		void DeleteImage(string imageName);
	}
}
