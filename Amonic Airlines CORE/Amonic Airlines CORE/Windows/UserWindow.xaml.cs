using Amonic_Airlines_CORE.Models;
using Amonic_Airlines.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly User user;

        public ActivityUser CurrentActivityUser;
        public bool ClosedByUser = false;

        public UserWindow(UserModelView userModel)
        {
            InitializeComponent();
            DataContext = userModel;
            user = userModel.currentUser;
            CreateSession();
        }

        private void CreateSession()
        {
            CurrentActivityUser = new ActivityUser()
            {
                Email = user.Email,
                LoginDate = DateTime.Now
            };
            AmonicContext.GetContext().ActivityUser.Add(CurrentActivityUser);
            AmonicContext.GetContext().SaveChanges();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            var activity = AmonicContext.GetContext().ActivityUser.FirstOrDefault(au => au.Email == CurrentActivityUser.Email && CurrentActivityUser.LoginDate == au.LoginDate);
            activity.LogoutDate = DateTime.Now;
            AmonicContext.GetContext().SaveChanges();
            ClosedByUser = true;
            this.Close();
        }
    }
}
