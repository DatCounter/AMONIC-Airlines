using System;
using System.Collections.Generic;
using Amonic_Airlines.Models;
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
            var currentUser = Users.FirstOrDefault(u => u.Email == Email);
            //TODO: Сделать обработку на пустые поля,
            //      При правильном входе обнулить CountInvalidAuthAttempts
            //      Понять, почему не сохраняются данные

            if (currentUser is null)
            {
                CountInvalidAuthAttempts++;
                if (CountInvalidAuthAttempts % 3 == 0)
                {
                    CountInvalidAuthAttempts = 0;
                    throw new ValidationException("Вы ввели более трёх раз неверно логин или пароль, повторите через 10 секунд");
                }
                throw new AuthenticationException("Логин или пароль не верны");
            }

            if (currentUser.Password != Password)
            {
                CountInvalidAuthAttempts++;
                if (CountInvalidAuthAttempts % 3 == 0)
                {
                    CountInvalidAuthAttempts = 0;
                    throw new ValidationException("Вы ввели более трёх раз неверно логин или пароль, повторите через 10 секунд");
                }
                throw new AuthenticationException("Логин или пароль не верны");
            }

            if (currentUser.IsActive == false)
                throw new MemberAccessException($"Пользователь {currentUser.FirstName} заблокирован администратором");

            User = currentUser;
        }
    }
}
