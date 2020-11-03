using Amonic_Airlines_CORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Amonic_Airlines.Models
{
    public class UserModelView : INotifyPropertyChanged
    {
        private readonly User currentUser;
        private List<ActivityUserView> activitiesUser;
        private string welcomeName;
        private string timeSpent;
        private string numberOfCrashes;
        private TimeSpan timeSpanNow = new TimeSpan(0, 0, 0, 0);

        public List<ActivityUserView> ActivitiesUser
        {
            get => activitiesUser; private set { activitiesUser = value; }
        }

        public string WelcomeName { get => welcomeName; set { welcomeName = value; RaisePropertyChanged();} }

        public string TimeSpent
        {
            get => timeSpent; set { timeSpent = value; RaisePropertyChanged(); RaisePropertyChanged(); }
        }

        public string NumberOfCrashes { get => numberOfCrashes; set { numberOfCrashes = value; RaisePropertyChanged(); } }

        public UserModelView(User user)
        {
            currentUser = user;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateAllData();
        }

        private void UpdateAllData()
        {
            ActivitiesUser = new List<ActivityUserView>();
            //step 1: определить все активности пользователя
            AmonicContext.GetContext().ActivityUser.
                        Where(AU => AU.Email == currentUser.Email).
                        OrderBy(AU => AU.LoginDate).ToList().ForEach((actvivity) =>
                        {
                            ActivitiesUser.Add(new ActivityUserView(actvivity));
                        });
            RaisePropertyChanged(nameof(ActivitiesUser));
            //step 2: дать свойствам жизнь

            WelcomeName = $"Hi, {currentUser.FirstName}, Welcome to " +
                            $"AMONIC Airlines";

            //Общее время, которое провёл пользователь в системе за последние 30 дней
            if (timeSpanNow.TotalSeconds == 0)
            {
                var List = activitiesUser.Where(UA =>
                    DateTime.Now.Subtract(UA.LoginDate).TotalDays < new TimeSpan(30, 0, 0, 0).TotalDays
                ).ToList();
                List.ForEach((UA) =>
                {
                    timeSpanNow = timeSpanNow.Add(UA.TimeSpent.Value);
                });

            }
            //Количество крашей
            var CountOfCrashes = activitiesUser
                            .Where(UA => !string.IsNullOrEmpty(UA.UnsuccessfulLogoutReason)
                            || UA.LogoutDate is null || UA.TimeSpent is null)
                            .Count();

            NumberOfCrashes = $"Number of crashes: {CountOfCrashes}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeSpanNow = timeSpanNow.Add(new TimeSpan(0, 0, 1));
            TimeSpent = $"Time spent on system: {timeSpanNow}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
