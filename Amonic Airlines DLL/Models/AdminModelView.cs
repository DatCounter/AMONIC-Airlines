﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Amonic_Airlines_CORE.Models
{
    public class AdminModelView : INotifyPropertyChanged
    {
        private User currentUser;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; private set; }

        public string LastName { get; private set; }

        public int Age { get; private set; }

        public string UserRole { get; private set; }

        public string EmailAddress { get; private set; }

        public int OfficeCode { get; private set; }

        public string OfficeName { get; set; }

        public string Color { get; private set; }

        public User CurrentUser { get => currentUser; set { currentUser = value; OnPropertyChanged(); UpdateModel(); } }

        //Конструктор, который преобразует сущность User в UserModelView для представления
        public AdminModelView(User user)
        {
            this.CurrentUser = user;
        }
        //Обновляет модель при получении текущего пользователя
        private void UpdateModel()
        {
            Name = CurrentUser.FirstName;
            LastName = CurrentUser.SecondName;
            Age = DateTime.Now.Year - CurrentUser.Birthdate.Year;
            UserRole = CurrentUser.IsAdmin ? "Administrator" : "Office user";
            EmailAddress = CurrentUser.Email;
            Color = CurrentUser.IsActive ? "#196AA6" : "IndianRed";
            OfficeCode = CurrentUser.Office;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
