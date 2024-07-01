using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile postedFile, string fileName, string webRootDir);
        Task DeleteFileAsync(string path);
        Task CreateFolderAsync(string folderName, string webRootDir);
        Task DeleteFolderAsync(string folderName, string webRootDir);
        string GetFileUrl(string path);
    }
}