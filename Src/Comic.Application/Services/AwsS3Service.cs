using Comic.Application.IServices;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon;
using Comic.Domain.Common;
using Amazon.Runtime;
using Comic.Domain.Exceptions;
using System.Drawing.Imaging;
using System.Drawing;

namespace Comic.Application.Services
{
    public class AwsS3Service : IFileService
    {

        public async Task CreateFolderAsync(string folderName, string rootBucketDir)
        {
            var folderPath = $"{rootBucketDir}/{folderName}/";

            var amazonS3Config = new AmazonS3Config
            {
                ServiceURL = AppSetting.AwsServiceUrl,
            };
            var credentials = new BasicAWSCredentials(AppSetting.AwsAccessKey, AppSetting.AwsSecretKey);
            var s3Client = new AmazonS3Client(credentials, amazonS3Config);

            PutObjectRequest putObjectRequest = new PutObjectRequest()
            {
                BucketName = AppSetting.AwsBucketName,
                Key = folderPath,
            };

            await s3Client.PutObjectAsync(putObjectRequest);
        }

        public async Task DeleteFileAsync(string path)
        {
            var amazonS3Config = new AmazonS3Config
            {
                ServiceURL = AppSetting.AwsServiceUrl,
            };
            var credentials = new BasicAWSCredentials(AppSetting.AwsAccessKey, AppSetting.AwsSecretKey);
            var s3Client = new AmazonS3Client(credentials, amazonS3Config);


            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest()
            {
                BucketName = AppSetting.AwsBucketName,
                Key = path,
            };

            await s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task DeleteFolderAsync(string folderName, string rootBucketDir)
        {
            var folderPath = $"{rootBucketDir}/{folderName}/";

            var amazonS3Config = new AmazonS3Config
            {
                ServiceURL = AppSetting.AwsServiceUrl,
            };
            var credentials = new BasicAWSCredentials(AppSetting.AwsAccessKey, AppSetting.AwsSecretKey);
            var s3Client = new AmazonS3Client(credentials, amazonS3Config);


            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest()
            {
                BucketName = AppSetting.AwsBucketName,
                Key = folderPath,
            };

            await s3Client.DeleteObjectAsync(deleteObjectRequest);

        }

        public string GetFileUrl(string path)
        {
            return $"{AppSetting.AwsServiceUrl}/{AppSetting.AwsBucketName}/{path}";
        }

        public async Task<string> UploadImageAsync(IFormFile postedFile, string fileName, string rootBucketDir)
        {
            var path = $"{rootBucketDir}/{fileName}.jpg";

            var amazonS3Config = new AmazonS3Config
            {
                ServiceURL = AppSetting.AwsServiceUrl,
            };
            var credentials = new BasicAWSCredentials(AppSetting.AwsAccessKey, AppSetting.AwsSecretKey);
            var s3Client = new AmazonS3Client(credentials, amazonS3Config);

            var fileTranfer = new TransferUtility(s3Client);
            await fileTranfer.UploadAsync(postedFile.OpenReadStream(), AppSetting.AwsBucketName, path);
            return path;

        }

    }

}
