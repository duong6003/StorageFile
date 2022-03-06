using System.Net.Http.Headers;

namespace FileStorageSupplier.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile? file);

        Task<string> UploadFileAsync(IEnumerable<IFormFile>? files);

        Task<string> UpdateFileAsync(IFormFile? file, string path);
        Task RemoveFileAsync(string path);
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task RemoveFileAsync(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }

        public async Task<string> UpdateFileAsync(IFormFile? file, string path)
        {
            if (File.Exists(path))
            {
                string filename = ContentDispositionHeaderValue
                                .Parse(file!.ContentDisposition)
                                .FileName!
                                .Trim('"');
                string folder = Path.GetDirectoryName(path)!;
                File.Delete(path);
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                return filePath;
            }
            return null!;
        }

        public async Task<string> UploadFileAsync(IFormFile? file)
        {
            string filename = ContentDispositionHeaderValue
                                .Parse(file!.ContentDisposition)
                                .FileName!
                                .Trim('"');

            string imageFolder = Path.Combine(Path.DirectorySeparatorChar.ToString(),"uploaded", "files", DateTime.Now.ToString("yyyyMMdd"));
            string folder = _webHostEnvironment.WebRootPath + imageFolder;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, filename);
            using (FileStream fs = File.Create(filePath))
            {
                await file.CopyToAsync(fs);
                await fs.FlushAsync();
            }
            return filePath;
        }

        public async Task<string> UploadFileAsync(IEnumerable<IFormFile>? files)
        {
            string url = "";
            foreach (IFormFile file in files!)
            {
                string result = await UploadFileAsync(file);
                url = url + result + ";";
            }
            url = url.Substring(0, url.Length - 1);
            return url;
        }
    }
}