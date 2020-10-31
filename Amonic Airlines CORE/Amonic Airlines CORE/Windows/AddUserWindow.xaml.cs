using Amonic_Airlines.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow(List<OfficeComboboxItem> offices)
        {
            InitializeComponent();
            ComboboxOffice.ItemsSource = offices;
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var context = AmonicContext.GetContext();
            string resultMessage = "";

            OfficeComboboxItem office = (OfficeComboboxItem)ComboboxOffice.SelectedItem;

            //check all
            if (string.IsNullOrEmpty(TBEmail.Text))
                resultMessage += "Почта не заполнена\n";
            if (string.IsNullOrEmpty(TBFName.Text))
                resultMessage += "Имя не полнено\n";
            if (string.IsNullOrEmpty(TBLName.Text))
                resultMessage += "Фамилия не заполнена\n";
            if (office is null || office.OfficeCode == 0)
                resultMessage += "Офис не выбран\n";
            if (string.IsNullOrEmpty(Password.Password))
                resultMessage += "Пароль не задан\n";
            if (Calendar.SelectedDate is null || DateTime.Now.Year - Calendar.SelectedDate.Value.Year < 18)
                resultMessage += "Дата не выбрана или вам нет 18-и\n";

            if (context.Users.FirstOrDefault(u => u.Email == TBEmail.Text.Trim()) != null)
                resultMessage += "Указанная почта уже существует\n";

            if (!string.IsNullOrEmpty(resultMessage))
            {
                MessageBox.Show(resultMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //save all
            context.Users.Add(new User
            {
                Email = TBEmail.Text.Trim(),
                Password = Password.Password,
                FirstName = TBFName.Text.Trim(),
                SecondName = TBLName.Text.Trim(),
                Office = office.OfficeCode,
                Birthdate = Calendar.SelectedDate.Value,
                IsAdmin = false,
                IsActive = true
            });

            context.SaveChangesAsync();
            MessageBox.Show("Дааные успешно сохранены!");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
