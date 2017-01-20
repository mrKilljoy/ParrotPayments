using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotPaymentsApp.Models;

namespace ParrotPaymentsApp.Infrastructure
{
    /// <summary>
    /// Класс-обертка для управления регистрацией пользователей.
    /// </summary>
    public class AuthManager : IDisposable
    {
        private Models.AuthContext _cnt;
        private UserManager<ParrotUser> _mng;

        public AuthManager()
        {
            _cnt = new Models.AuthContext();
            _mng = new UserManager<ParrotUser>(new UserStore<ParrotUser>(_cnt));
        }

        public void Dispose()
        {
            ((IDisposable)_cnt).Dispose();
        }

        public async Task<IdentityResult> RegisterUser(Models.UserModel userModel)
        {
            ParrotUser user = new ParrotUser
            {
                UserName = userModel.Email, // т.к. поиск идет по имени юзера
                DecorativeName = userModel.Username // для демонстрации
            };

            var result = await _mng.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<ParrotUser> FindUser(string userName, string password)
        {
            ParrotUser user = await _mng.FindAsync(userName, password);

            return user;
        }

        /// <summary>
        /// Получить публичное имя пользователя.
        /// </summary>
        /// <param name="user_login">Логин пользователя.</param>
        /// <returns></returns>
        public async Task<string> GetPublicUsername(string user_login)
        {
            ParrotUser user = await _mng.FindByNameAsync(user_login);

            return user.DecorativeName;
        }

        /// <summary>
        /// Узнать текущий баланс счета пользователя.
        /// </summary>
        /// <param name="user_login">Логин пользователя.</param>
        /// <returns></returns>
        public async Task<int> GetUserBalance(string user_login)
        {
            ParrotUser user = await _mng.FindByNameAsync(user_login);
            
            return user.AccountBalance;
        }
    }
}