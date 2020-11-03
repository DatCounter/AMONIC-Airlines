using System;
using System.Collections.Generic;
using Amonic_Airlines_CORE.Models;
using System.Linq;
using System.Security.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Amonic_Airlines
{
    public class Authorize
    {
        public User User { get; }

        public Authorize(List<User> Users, string Email, string Password, ref int CountInvalidAuthAttempts)
        {
            if (Users.Count == 0)
                return;

            var currentUser = Users.FirstOrDefault(u => u.Email == Email);

            if (string.IsNullOrEmpty(Email.Trim()) || string.IsNullOrEmpty(Password.Trim()))
            {
                CountInvalidAttempts(ref CountInvalidAuthAttempts);
            }

            if (currentUser is null)
            {
                CountInvalidAttempts(ref CountInvalidAuthAttempts);
            }

            if (currentUser.Password != Password)
            {
                CountInvalidAttempts(ref CountInvalidAuthAttempts);
            }

            if (currentUser.IsActive == false)
                throw new MemberAccessException($"Пользователь {currentUser.FirstName} заблокирован администратором");

            User = currentUser;
        }

        private void CountInvalidAttempts(ref int InvalidAuthAttempts)
        {
            InvalidAuthAttempts++;
            if (InvalidAuthAttempts % 3 == 0)
            {
                InvalidAuthAttempts = 0;
                throw new ValidationException("Вы ввели более трёх раз неверно логин или пароль, повторите через 10 секунд");
            }
            throw new AuthenticationException("Логин или пароль не верны");
        }
    }
}
