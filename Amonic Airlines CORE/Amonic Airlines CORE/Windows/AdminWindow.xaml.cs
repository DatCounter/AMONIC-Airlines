using Amonic_Airlines.Models;
using Amonic_Airlines_CORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {

        private AddUserWindow addUserWindow;
        private List<AdminModelView> usersList = new List<AdminModelView>();
        private EditUserWindow editUserWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<AdminModelView> UsersList { get => usersList; set { usersList = value; RaisePropertyChanged("UsersList"); } }

        public AdminModelView SelectedUser { get; set; }

        public AdminWindow()
        {
            InitializeComponent();

            //Take Data to Window
            DataContext = this;
            //Take Users to DataGrid
            UsersList = UpdateListUserModelView();
            //Take offices to Combobox
            List<OfficeComboboxItem> offices = AmonicContext.GetContext().Offices.Select(o => new OfficeComboboxItem
            {
                OfficeCode = o.OfficeCode,
                Name = o.Name
            }).ToList();
            offices.Add(new OfficeComboboxItem { OfficeCode = 0, Name = "Choose an element" });

            offices = offices.OrderBy(o => o.OfficeCode).ToList();
            OfficeCombobox.ItemsSource = offices;
        }

        private List<AdminModelView> UpdateListUserModelView()
        {
            List<AdminModelView> users = new List<AdminModelView>();
            AmonicContext.GetContext().Users.ToList().ForEach((user) =>
            {
                var newUser = new AdminModelView(user)
                {
                    OfficeName = AmonicContext.GetContext().Offices.FirstOrDefault(O => O.OfficeCode == user.Office).Name
                };
                users.Add(newUser);
            });

            return users;
        }


        private List<AdminModelView> UpdateListUserModelView(int? OfficeCode)
        {
            List<AdminModelView> users = new List<AdminModelView>();
            AmonicContext.GetContext().Users.Where(u => u.Office == OfficeCode).ToList().ForEach((user) =>
            {
                var newUser = new AdminModelView(user)
                {
                    OfficeName = AmonicContext.GetContext().Offices.FirstOrDefault(O => O.OfficeCode == user.Office).Name
                };
                users.Add(newUser);
            });

            return users;
        }

        private void CanLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null || UsersList.FirstOrDefault(u => u == SelectedUser) == null)
            {
                MessageBox.Show("Выберете пользователя!");
                return;
            }

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
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберете пользователя!");
                return;
            }
            editUserWindow = new EditUserWindow(SelectedUser);
            IsEnabled = false;
            editUserWindow.Closed += AddUserWindow_Closed;
            editUserWindow.Show();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            addUserWindow = new AddUserWindow(OfficeCombobox.Items
                                                            .SourceCollection
                                                            .Cast<OfficeComboboxItem>()
                                                            .ToList());

            IsEnabled = false;
            addUserWindow.Closed += AddUserWindow_Closed;
            addUserWindow.Show();
        }

        private void AddUserWindow_Closed(object sender, EventArgs e)
        {
            OfficeCombobox.SelectedIndex = 0;
            UsersList = UpdateListUserModelView();
            IsEnabled = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (addUserWindow != null)
            {
                addUserWindow.Close();
            }

            if (e != null)
            {
                Close();
            }
            //TODO: закрыть сессию
        }
        private void OfficeCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var currentOffice = (OfficeComboboxItem)OfficeCombobox.SelectedItem;

            if (currentOffice.OfficeCode == 0)
            {
                UsersList = UpdateListUserModelView();
            }
            else
            {
                UsersList = UpdateListUserModelView(currentOffice.OfficeCode);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Exit_Click(sender, null);
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
