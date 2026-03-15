namespace ITI_MVC_Project.Services
{
    public class FileService : IFileService
    {
        private readonly string _wwwRootPath;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _wwwRootPath = webHostEnvironment.WebRootPath;
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string subfolder)
        {
            if (imageFile is null)
                throw new ArgumentNullException(nameof(imageFile));

            var folderPath = Path.Combine(_wwwRootPath, "images", subfolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var extension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            // Return a relative URL usable in <img src="...">
            return $"/images/{subfolder}/{fileName}";
        }

        public void DeleteFile(string imageUrl, string subfolder)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            // imageUrl is like /images/products/guid.jpg — extract just the filename
            var fileName = Path.GetFileName(imageUrl);
            var fullPath = Path.Combine(_wwwRootPath, "images", subfolder, fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
