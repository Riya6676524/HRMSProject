using System;
using System.Text;

namespace HRMSUtility
{
    public static class Base64Helper
    {
        public static string Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static string Decode(string base64Text)
        {
            if (string.IsNullOrWhiteSpace(base64Text))
                return null;

            return Encoding.UTF8.GetString(Convert.FromBase64String(base64Text));
        }
    }
}

