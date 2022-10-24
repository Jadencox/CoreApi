using Core.Utility.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Jwt.Identity.Core
{
    public class DoCarePassWordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        public static string MD5_String(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var data = Encoding.Default.GetBytes(str);
            var result = md5.ComputeHash(data);
            var ret = "";
            for (var i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        public override string HashPassword(TUser user, string password)
        {
            Guard.ArgumentNotNullOrEmpty(password, "Password");
            Guard.ArgumentNotNull(user, "User");
            return HashPassword(password);

        }

        public override string HashPassword(string password)
        {
            Guard.ArgumentNotNullOrEmpty(password, "Password");
            return MD5_String(password).ToUpper();
        }

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            Guard.ArgumentNotNullOrEmpty(hashedPassword, "hashedPassword");
            Guard.ArgumentNotNullOrEmpty(providedPassword, "providedPassword");

            var providedPasswordHash = HashPassword(providedPassword);

            if (providedPasswordHash.Length == 0)
                return PasswordVerificationResult.Failed;

            if (string.Compare(hashedPassword, providedPasswordHash) == 0)
                return PasswordVerificationResult.Success;
            else
                return PasswordVerificationResult.Failed;           

        }
    }
}
