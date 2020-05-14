using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace webapi.Utils
{
    public class CloudinaryManager
    {
        Cloudinary cloudinary;

        public CloudinaryManager()
        {
            Account account = new Account(
            "de1xlc3s1",
            "962536298833484",
            "DxjiGuYYI51ID6x8IpwkFpx2QFY");

            cloudinary = new Cloudinary(account);

        }
        public string UploadImage(string image)
        {
            string dataImageBase64 = @image;
            string pattern = "data:.*?base64,";
            string base64string = Regex.Replace(dataImageBase64, pattern, "");
            Stream imageStream = new MemoryStream(Convert.FromBase64String(base64string));

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription("Picture", imageStream)
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.Uri.ToString();
            }

            return string.Empty;

        }
    }
}
