using Amonic_Airlines.Models;
using Amonic_Airlines_CORE.Models;
using Amonic_Airlines.Windows;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Windows;
using System.Windows.Threading;

namespace Amonic_Airlines
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window, INotifyPropertyChanged
    {
        #region Event members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private members
        //Count Invalid Auth Attempts
        private int countAttempts = Properties.Default.CountAttempts;
        private int countTicks = 10;
        private Visibility visibleTicks = Visibility.Collapsed;
        private UserWindow userWindow;

        private AdminWindow adminWindow;
        #endregion

        #region Public members
        public int CountTicks { get => countTicks; set { countTicks = value; RaisePropertyChanged(); } }
        public Visibility VisibleTicks { get => visibleTicks; set { visibleTicks = value; RaisePropertyChanged(); } }
        public AdminModelView CurrentUser { get; set; }
        readonly DispatcherTimer Timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1),
        };
        #endregion

        //TODO: сделать обработчик аварийного выхода из приложения
        /// <summary>
        /// Initializator
        /// </summary>
        public AuthenticationWindow()
        {
            InitializeComponent();
            DataContext = this;
            var _username = Properties.Default.Username;
            if (!string.IsNullOrEmpty(_username))
            {
                Username.Text = _username;
            }
        }


        /// <summary>
        /// Authorize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Authorize TryAuthorize = null;
            try
            {
                TryAuthorize = new Authorize(AmonicContext.GetContext().Users.ToList(), Username.Text, Password.Password, ref countAttempts);

            }
            catch (AuthenticationException AuthEx)
            {
                MessageBox.Show(AuthEx.Message, "Ошибка входа");
                Properties.Default["CountAttempts"] = countAttempts;
                Properties.Default.Save();
            }
            catch (MemberAccessException AccEx)
            {
                MessageBox.Show(AccEx.Message, "Доступ заблокирован");
            }
            catch (ValidationException ValidEx)
            {
                MessageBox.Show(ValidEx.Message, "Большое количество неверных попыток");

                Username.IsEnabled = false;
                Password.IsEnabled = false;
                LoginButton.IsEnabled = false;

                Timer.Tick += Timer_Tick;
                VisibleTicks = Visibility.Visible;
                Timer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("База данных не найдена. Обратитесь к специалисту для решения проблемы", "ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //TO AUTHORIZE USE THIS AS ADMIN:
            //LOGIN : j.doe@amonic.com
            //Password : 123
            //
            //AS USER
            //LOGIN : test3@test
            //Password : 2020
            if (TryAuthorize != null)
            {
                Properties.Default["Username"] = Username.Text;
                Properties.Default.Save();

                //TODO: NEED TO CREATE SESSION

                this.Hide();
                if (TryAuthorize.User.IsAdmin)
                {
                    adminWindow = new AdminWindow();
                    adminWindow.Show();
                    adminWindow.Closed += NotOwnerWindow_Closed;
                }
                else
                {
                    userWindow = new UserWindow(new UserModelView(TryAuthorize.User));
                    
                    userWindow.Show();
                    userWindow.Closed += NotOwnerWindow_Closed;
                }
            }

        }

        private void NotOwnerWindow_Closed(object sender, EventArgs e)
        {
            if (sender is AuthenticationWindow)
            {
                Application.Current.Shutdown();
                return;
            }
            if (sender is UserWindow)
            {
                UserWindow obj = (UserWindow)sender;

                if (!obj.ClosedByUser)
                {
                    var activity = AmonicContext.GetContext().ActivityUser.FirstOrDefault(au => au.Email == obj.CurrentActivityUser.Email && au.LoginDate == obj.CurrentActivityUser.LoginDate);
                    activity.UnsuccessfulLogoutReason = "Power electro off";
                    AmonicContext.GetContext().SaveChanges();
                }
            }
            if (MessageBox.Show("Желаете авторизоваться под другим именем?", "Авторизация",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Password.Password = null;
                if (sender is AdminWindow)
                    adminWindow.Closed -= NotOwnerWindow_Closed;
                else if(sender is UserWindow)
                    userWindow.Closed -= NotOwnerWindow_Closed;
                this.Show();
            }
            else
            {
                this.Close();
                Application.Current.Shutdown();
            }

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NotOwnerWindow_Closed(this, e);
        }

        /// <summary>
        /// To raise property changed
        /// </summary>
        /// <param name="prop">property</param>
        private void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// every Tick use this func in the Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Timer_Tick(object sender, EventArgs eventArgs)
        {
            CountTicks--;
            if (CountTicks == 0)
            {
                CountTicks = 10;
                Timer.Stop();

                Username.IsEnabled = true;
                Password.IsEnabled = true;
                LoginButton.IsEnabled = true;
                Timer.Tick -= Timer_Tick;
                VisibleTicks = Visibility.Hidden;
            }
        }
    }
}

/*TODO: Доделать работу с таймером в юзер-окне
Проверить код/добавить чистоты и ясности для пользователя
*/
