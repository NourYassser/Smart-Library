using System.Security.Cryptography;

namespace SmartLibrary.Api.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string Username { get; private set; }
        public string PinCode { get; private set; } = null!;

        private AppUser() { }

        public AppUser(string username, string pin)
        {
            var n = (username ?? string.Empty).Trim();
            if (n.Length == 0 || n.Length > 8) throw new ArgumentException("Username must be 1..8 characters.", nameof(username));
            if (pin == null || pin.Length != 6) throw new ArgumentException("Pin must be 6 characters.", nameof(pin));

            Id = Guid.NewGuid();
            Username = n;
            SetPin(pin);
        }

        public void SetPin(string pin)
        {
            if (pin == null) throw new ArgumentNullException(nameof(pin));
            if (pin.Length != 6) throw new ArgumentException("Pin must be 6 characters.", nameof(pin));

            const int iterations = 100_000;
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(pin, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            PinCode = $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPin(string pin)
        {
            if (string.IsNullOrEmpty(PinCode) || pin == null) return false;

            var parts = PinCode.Split('.');
            if (parts.Length != 3) return false;
            if (!int.TryParse(parts[0], out var iterations)) return false;

            var salt = Convert.FromBase64String(parts[1]);
            var expected = Convert.FromBase64String(parts[2]);

            using var pbkdf2 = new Rfc2898DeriveBytes(pin, salt, iterations, HashAlgorithmName.SHA256);
            var actual = pbkdf2.GetBytes(expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }


    }
}
