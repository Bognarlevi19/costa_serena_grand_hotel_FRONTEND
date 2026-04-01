using Microsoft.AspNetCore.Http;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class ImageStorageService
    {
        private readonly IWebHostEnvironment _environment;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        public ImageStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string?> SaveSingleAsync(IFormFile? file, string folderName, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                return null;

            return await SaveInternalAsync(file, folderName, cancellationToken);
        }

        public async Task<List<string>> SaveManyAsync(IEnumerable<IFormFile>? files, string folderName, CancellationToken cancellationToken = default)
        {
            var result = new List<string>();

            if (files == null)
                return result;

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var path = await SaveInternalAsync(file, folderName, cancellationToken);
                result.Add(path);
            }

            return result;
        }

        private async Task<string> SaveInternalAsync(IFormFile file, string folderName, CancellationToken cancellationToken)
        {
            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
                throw new Exception("Csak .jpg, .jpeg, .png vagy .webp képfájl tölthető fel.");

            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("Egy kép mérete legfeljebb 5 MB lehet.");

            var safeFolder = Path.Combine(_environment.WebRootPath, "kepek", folderName);
            Directory.CreateDirectory(safeFolder);

            var fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(safeFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return $"/kepek/{folderName}/{fileName}".Replace("\\", "/");
        }
    }
}