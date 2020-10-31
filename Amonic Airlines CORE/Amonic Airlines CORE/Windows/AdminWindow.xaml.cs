using Amonic_Airlines.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<UserModelView> usersList = new List<UserModelView>();

        public List<UserModelView> UsersList { get => usersList; set { usersList = value; RaisePropertyChanged("UsersList"); } }

        public UserModelView SelectedUser { get; set; }

        public AdminWindow()
        {
            InitializeComponent();
            //Take Data to Window
            DataContext = this;
            //Take Users to DataGrid
            UsersList = UpdateListUserModelView();
            //Take offices to Combobox
            List<OfficeComboboxItem> offices = AmonicContext.GetContext().Offices.Select(o => new OfficeComboboxItem 
                                                {OfficeCode = o.OfficeCode, Name= o.Name}).ToList();
            offices.Add( new OfficeComboboxItem{OfficeCode = 0, Name = "Choose an element" });
            offices = offices.OrderBy(o => o.OfficeCode).ToList();
            OfficeCombobox.ItemsSource = offices;
        }

        private List<UserModelView> UpdateListUserModelView()
        {
            List<UserModelView> users = new List<UserModelView>();
            AmonicContext.GetContext().Users.ToList().ForEach((user) =>
            {
                var newUser = new UserModelView(user)
                {
                    OfficeName = AmonicContext.GetContext().Offices.FirstOrDefault(O => O.OfficeCode == user.Office).Name
                };
                users.Add(newUser);
            });

            return users;
        }

        private List<UserModelView> UpdateListUserModelView(int? OfficeCode)
        {
            List<UserModelView> users = new List<UserModelView>();
            AmonicContext.GetContext().Users.Where(u => u.Office == OfficeCode).ToList().ForEach((user) =>
            {
                var newUser = new UserModelView(user)
                {
                    OfficeName = AmonicContext.GetContext().Offices.FirstOrDefault(O => O.OfficeCode == user.Office).Name
                };
                users.Add(newUser);
            });

            return users;
        }

        private void CanLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
                return;
            User user = AmonicContext.GetContext().Users.FirstOrDefault(u => u.Email == SelectedUser.EmailAddress);
            if (user == null)
            {
                MessageBox.Show("Данный пользователь был удален в момент операции", "Ошибка");
            }
            else
            {
                user.IsActive = !user.IsActive;
                AmonicContext.GetContext().SaveChanges();
            }
            UsersList = UpdateListUserModelView();
        }

        private void ChangeRole_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //TODO: закрыть сессию
        }
        private void OfficeCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var currentOffice = (OfficeComboboxItem)OfficeCombobox.SelectedItem;

            if (currentOffice.OfficeCode == 0)
                return;


            UsersList = UpdateListUserModelView(currentOffice.OfficeCode);
        }
        /// <summary>
        /// To raise property changed
        /// </summary>
        /// <param name="prop">property</param>
        private void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
