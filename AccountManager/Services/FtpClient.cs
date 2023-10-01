using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AccountManager.Services
{
    public static class FtpClient
    {
        private static string _host = @"ftp://someWebSite.somee.com/www.someWebSite.somee.com";
        private static string _user = "dfi_named_ion";
        private static string _password = "Kyter133";


        private static bool CheckConnection()
        {
            try
            {
                return new Ping().Send(@"www.google.com").Status == IPStatus.Success;
            }
            catch (Exception)
            { return false; }
        }

        public static string GetEncryptToken()
        {
            return ReadData("ENCRYPT_TOKEN.txt");
        }

        //public static string GetCredentials()
        //{
        //    return ReadData("CREDENTIALS.txt");
        //}

        private static string ReadData(string fileName)
        {
            try
            {
                if (!CheckConnection())
                    return null;

                string remotePath = $@"{_host}/{fileName}";

                FtpWebRequest _ftpRequest = (FtpWebRequest)FtpWebRequest.Create(remotePath);
                _ftpRequest.Credentials = new NetworkCredential(_user, _password);
                _ftpRequest.UseBinary = true;

                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();

                Stream _ftpStream = _ftpResponse.GetResponseStream();

                int _bufferSize = 2048;
                byte[] buffer = new byte[_bufferSize];
                int bytes = _ftpStream.Read(buffer, 0, _bufferSize);

                _ftpStream.Close();
                _ftpResponse.Close();
                _ftpRequest = null;

                return Encoding.UTF8.GetString(buffer); ;
            }
            catch (Exception)
            { return null; }
        }
    }
}