namespace ITI_MVC_Project.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string subfolder);
        void DeleteFile(string imageUrl, string subfolder);
    }
}
