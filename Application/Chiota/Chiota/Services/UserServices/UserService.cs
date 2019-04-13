﻿using System;
using System.Text;
using System.Threading.Tasks;

using Chiota.Base;
using Chiota.Models;
using Chiota.Models.Database;
using Chiota.Services.Database;
using Chiota.Services.Database.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Pact.Palantir.Encryption;

using Tangle.Net.Entity;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chiota.Services.UserServices
{
    public class UserService
    {
        private readonly IUserFactory _userFactory;

        public UserService(IUserFactory userFactory)
        {
            this._userFactory = userFactory;
        }

        public static DbUser CurrentUser { get; private set; }

        /// <summary>
        /// Return a new created user object, which is saved in the database.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public async Task<bool> CreateNew(UserCreationProperties properties)
        {
            try
            {
                // Create the entry for secure storage to safe the encryption key for the user data.
                var salt = Seed.Random().Value;
                var encryptionKey = new EncryptionKey(properties.Password, salt);

                var user = await this._userFactory.CreateAsync(properties.Seed, properties.Name, properties.ImagePath, properties.ImageBase64, encryptionKey);

                // Set the database service for usage.
                AppBase.Database = new DatabaseService(DependencyService.Get<ISqlite>(), encryptionKey);

                // Save user in the database.
                var result = AppBase.Database.User.AddObject(user);

                if (result == null)
                {
                    return false;
                }

                var jsonString = JsonConvert.SerializeObject(new { userid = result.Id, salt = Convert.ToBase64String(Encoding.UTF8.GetBytes(salt)) });
                await SecureStorage.SetAsync(properties.Password, jsonString);

                this.SetCurrentUser(result);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The get current as.
        /// </summary>
        /// <typeparam name="T">
        /// The derived user type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T GetCurrentUserAs<T>()
          where T : DbUser
        {
            return CurrentUser as T;
        }

        /// <summary>
        /// Log in with a password as key.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> LogInAsync(string password)
        {
            try
            {
                var result = await SecureStorage.GetAsync(password);
                if (result == null)
                {
                    return false;
                }

                var json = JObject.Parse(result);
                var salt = (string)json.GetValue("salt");
                salt = Encoding.UTF8.GetString(Convert.FromBase64String(salt));
                var userid = (int)json.GetValue("userid");

                // Set the encryption key of the user.
                var encryptionKey = new EncryptionKey(password, salt);

                // Set the database service for usage.
                AppBase.Database = new DatabaseService(DependencyService.Get<ISqlite>(), encryptionKey);

                var user = AppBase.Database.User.GetObjectById(userid);
                if (user == null)
                {
                    return false;
                }

                user.EncryptionKey = encryptionKey;
                user.NtruKeyPair = NtruEncryption.Key.CreateAsymmetricKeyPair(user.Seed.ToLower(), user.PublicKeyAddress);

                this.SetCurrentUser(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The set current user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public void SetCurrentUser(DbUser user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// Update the user in the database, if the password is correct.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string password, DbUser user)
        {
            var result = await SecureStorage.GetAsync(password);
            if (result == null)
            {
                return false;
            }

            var json = JObject.Parse(result);
            var userid = (int)json.GetValue("userid");

            if (CurrentUser.Id != userid)
            {
                return false;
            }

            // Update the user.
            var valid = AppBase.Database.User.UpdateObject(user);
            if (!valid)
            {
                return false;
            }

            user.NtruKeyPair = NtruEncryption.Key.CreateAsymmetricKeyPair(user.Seed.ToLower(), user.PublicKeyAddress);

            this.SetCurrentUser(user);

            return true;
        }

        public async Task<bool> ValidatePasswordAsync(string password)
        {
            var result = await SecureStorage.GetAsync(password);
            if (result == null)
            {
                return false;
            }

            var json = JObject.Parse(result);
            var userid = (int)json.GetValue("userid");

            return CurrentUser.Id == userid;
        }
    }
}