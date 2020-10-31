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
        private readonly List<ActivityUser> activitiesUser;
        private string welcomeName;
        private string timeSpent;
        private string numberOfCrashes;
        private readonly TimeSpan timeSpanNow = new TimeSpan(0, 0, 0, 0);

        public List<ActivityUser> ActivitiesUser => activitiesUser;
        public List<string> Color { get; set; }

        public string WelcomeName { get => welcomeName; set { welcomeName = value; RaisePropertyChanged(); } }

        public string TimeSpent
        {
            get => timeSpent; set { timeSpent = value; RaisePropertyChanged(); }
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
            //step 1: определить все активности пользователя
            activitiesUser = AmonicContext.GetContext().ActivityUser.
                        Where(AU => AU.Email == currentUser.Email).
                        OrderBy(AU => AU.LoginDate).ToList();
            UpdateAllData();
        }

        private void UpdateAllData()
        {
            Color = new List<string>();
            foreach (var activity in activitiesUser)
            {
                Color.Add(activity.LogoutDate == null ? "Red" : "White");
            }
            RaisePropertyChanged(nameof(ActivitiesUser));
            //step 2: дать свойствам жизнь

            WelcomeName = $"Hi, {currentUser.FirstName}, Welcome to " +
                            $"AMONIC Airlines";

            //Общее время, которое провёл пользователь в системе за последние 30 дней
            if (timeSpanNow.TotalSeconds == 0)
            {
                var List = activitiesUser.Where(UA =>
                    DateTime.Now.Subtract(UA.LoginDate) < new TimeSpan(30, 0, 0, 0)
                ).ToList();
                List.ForEach((UA) =>
                {
                    timeSpanNow.Add(UA.TimeSpent.Value);
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
            timeSpanNow.Add(new TimeSpan(0, 0, 1));
            TimeSpent = $"Time spent on system: {timeSpanNow}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
