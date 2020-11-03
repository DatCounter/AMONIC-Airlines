using Amonic_Airlines_CORE.Models;
using Amonic_Airlines.Models;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window, INotifyPropertyChanged
    {
        private AdminModelView selectedUser;
        public AdminModelView SelectedUser { get => selectedUser; private set { selectedUser = value; RaisePropertyChanged(); } }

        public EditUserWindow(AdminModelView SelectedUser)
        {
            InitializeComponent();
            this.SelectedUser = SelectedUser;
            DataContext = this.SelectedUser;
            var _ = selectedUser.UserRole == "Office user" ? isUser.IsChecked = true : isAdmin.IsChecked = true;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите сохранить изменения?", "Вопрос",
                                MessageBoxButton.YesNo, MessageBoxImage.Question)
                                == MessageBoxResult.Yes)
            {
                var userToChangeRole = AmonicContext.GetContext().Users.FirstOrDefault(u => u.Email == SelectedUser.EmailAddress);
                AmonicContext.GetContext().Attach(userToChangeRole);
                userToChangeRole.IsAdmin = (bool)isUser.IsChecked ? false : true;
                AmonicContext.GetContext().SaveChangesAsync();
                MessageBox.Show("Изменения сохранены");
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из этого меню?",
                                "Вопрос",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes || e != null)
            {
                Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
