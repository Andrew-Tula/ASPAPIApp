using System.Text;
using System.Security.Cryptography;

namespace ASPAPI.Services {
    public static class AccountHelper {
        public static string CreateSaltKey() {
            using var randomNumber = RandomNumberGenerator.Create();
            var result = new byte[5];
            randomNumber.GetBytes(result);

            return Convert.ToBase64String(result);
        }

        public static string CreatePasswordHash(string password, string saltkey) {
            using var algorithm = HashAlgorithm.Create("SHA1");
            var buffer = Encoding.UTF8.GetBytes(string.Concat(password, saltkey));
            var hashByteArray = algorithm!.ComputeHash(buffer);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }
    }
}
