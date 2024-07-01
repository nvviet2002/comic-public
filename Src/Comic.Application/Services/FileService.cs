using Comic.Application.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using Comic.Domain.Exceptions;
using Comic.Domain.Common;

namespace Comic.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadImageAsync(IFormFile postedFile, string fileName, string webRootDir)
        {

            string path = this._environment.WebRootPath + webRootDir;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, fileName + ".jpg");
            var tempImage = Image.FromStream(postedFile.OpenReadStream());
            if (tempImage == null)
            {
                throw new AppException("Lỗi đọc ảnh");
            }

            tempImage.Save(filePath,ImageFormat.Jpeg);
            return $"{webRootDir}/{fileName}.jpg";

        }

        //waste
        private void CompressImage()
        {
            //compress image
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, 40l);
            myEncoderParameters.Param[0] = myEncoderParameter;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        // end waste

        public string GetFileUrl(string path)
        {
            return AppSetting.FileRootUrl + path;
        }

        public async Task DeleteFileAsync(string path)
        {
            string pathFile = _environment.WebRootPath + path;
            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }
        }


        public async Task CreateFolderAsync(string folderName, string webRootDir)
        {
            string path = _environment.WebRootPath + Path.Combine(webRootDir, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public async Task DeleteFolderAsync(string folderName, string webRootDir)
        {
            string path = _environment.WebRootPath + Path.Combine(webRootDir, folderName);
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }


    }
}
