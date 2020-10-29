using Amonic_Airlines.Models;
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
        public event PropertyChangedEventHandler PropertyChanged;

        private List<UserModelView> usersList = new List<UserModelView>();

        public List<UserModelView> UsersList { get => usersList; set { usersList = value; RaisePropertyChanged("UsersList"); } }



        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;
            UsersList = UpdateListUserModelView();
            var offices = AmonicContext.GetContext().Offices.Select(o => new { o.OfficeCode, o.Name }).ToList();
            offices.Add( new {OfficeCode = 0, Name = "Choose an element" });
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

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            //TODO: закрыть сессию
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
