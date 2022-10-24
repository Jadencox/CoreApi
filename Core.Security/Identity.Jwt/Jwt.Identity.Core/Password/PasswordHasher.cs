using System;
using System.Collections.Generic;
using System.Text;
using Core.Utility.Common;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace Jwt.Identity.Core
{
    /// <summary>
    /// Implements the standard Identity password hashing.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user.</typeparam>
    public class PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        /* =======================
      * HASHED PASSWORD FORMATS
      * ======================
      * Version 3:
      * PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations.
      * Format: { 0x01, prf (UInt32), iter count (UInt32), salt length (UInt32), salt, subkey }
      * (All UInt32s are stored big-endian.)
      */

        private readonly int _iterCount = 10000;

        /// <summary>
        /// Implements the standard Identity password hashing.
        /// </summary>
        /// <typeparam name="TUser">The type used to represent a user.</typeparam>
        public virtual string HashPassword(TUser user, string password)
        {
            Guard.ArgumentNotNullOrEmpty(password, "Password");
            Guard.ArgumentNotNull(user, "User");
            return Convert.ToBase64String(HashPasswordV3(password));
        }

        public virtual string HashPassword(string password)
        {
            Guard.ArgumentNotNullOrEmpty(password, "Password");
            
            return Convert.ToBase64String(HashPasswordV3(password));
        }

        private byte[] HashPasswordV3(string password)
        {
            var saltSize = 128 / 8;
            byte[] salt = new byte[saltSize];
            //RandomNumberGenerator rng.GetBytes(salt);
            var numBytesRequested = 256 / 8;
            var prf = KeyDerivationPrf.HMACSHA256;
            byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, _iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.</returns>
        /// <remarks>Implementations of this method should be time consistent.</remarks>
        public virtual PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            Guard.ArgumentNotNullOrEmpty(hashedPassword, "hashedPassword");
            Guard.ArgumentNotNullOrEmpty(providedPassword, "providedPassword");     

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
                return PasswordVerificationResult.Failed;
        
            int embeddedIterCount;
            if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount))
            {
                // If this hasher was configured with a higher iteration count, change the entry now.
                return (embeddedIterCount < _iterCount)
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
           
            
        }

        private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount)
        {
            iterCount = default(int);

            try
            {
                // Read header information
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = hashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

       
    }
}
