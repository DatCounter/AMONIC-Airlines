using Amonic_Airlines.Models;
using Amonic_Airlines.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Amonic_Airlines.Windows
{
    /// <summary>
    /// Interaction logic for FlightSchedules.xaml
    /// </summary>
    public partial class FlightSchedules : Window, INotifyPropertyChanged
    {
        private List<FlightSchedulesModelView> schedulesList = new List<FlightSchedulesModelView>();
        public List<FlightSchedulesModelView> SchedulesList { get => schedulesList; set { schedulesList = value; } }

        public FlightSchedules()
        {
            InitializeComponent();
            AmonicContext.GetContext().FlightSchedules.ToList().ForEach((item) =>
            {
                SchedulesList.Add(new FlightSchedulesModelView(item));
            });
            ToComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)FromComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
            FromComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)ToComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
            SortByComboBox.ItemsSource = new string[] { "Date-Time", "From", "To", "Flight number", "Aircraft", "Price" };

            DataContext = this;
        }

        public void ImportChanges_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "csv Files (*.csv)|*.csv|txt Files (*.txt)|*.txt",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            if (fileDialog.ShowDialog() == true)
            {
                ImportFlightsService import = new ImportFlightsService();
                try
                {
                    var errorList = import.StartImport(fileDialog.FileName);

                    if (errorList.Count > 0)
                        foreach (var item in errorList)
                        {
                            if (MessageBox.Show("Не удалось импортировать такие данные\n" +
                                                $"{item[0]},{item[1]},{item[2]},{item[3]},{item[4]},{item[5]},{item[6]},{item[7]},{item[8]}\n" +
                                                "Продолжить вывод данных?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            {
                                break;
                            }
                        }

                }
                catch (IOException)
                {
                    MessageBox.Show("Закройте файл, который вы выбрали и повторите ещё раз");
                }
                System.Diagnostics.Debug.WriteLine(import.SuccessfulChanges + " прошедших отбор");
                System.Diagnostics.Debug.WriteLine(import.DuplicateRecords + " дубликатов");
                System.Diagnostics.Debug.WriteLine(import.RecordWithMissingFields + " ошибок");
            }
        } //TODO: Сделать реализацию импорта данных в отдельном окне

        public void ApplyFilters_Click(object sender, EventArgs args)
        {
            var resultFilter = AmonicContext.GetContext().FlightSchedules.ToList();

            if (Int32.TryParse(FlighNumberTB.Text, out int FlightNumber))
            {
                resultFilter = resultFilter.Where(FS => FS.FlightNumber == FlightNumber).ToList();
            }
            if (FromComboBox.SelectedItem != null)
            {
                int airportFromId = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == (string)FromComboBox.SelectedItem).Id;
                resultFilter = resultFilter.Where(FS => FS.FromAir == airportFromId).ToList();
            }
            if (ToComboBox.SelectedItem != null)
            {
                int airportToId = AmonicContext.GetContext().Airport.FirstOrDefault(a => a.ShortName == (string)ToComboBox.SelectedItem).Id;
                resultFilter = resultFilter.Where(FS => FS.ToAir == airportToId).ToList();
            }
            if (OutboundTime.SelectedDate.HasValue)
            {
                resultFilter = resultFilter.Where(FS => FS.DateTimeOfRace - OutboundTime.SelectedDate.Value > TimeSpan.FromDays(0)).ToList();
            }
            SchedulesList = new List<FlightSchedulesModelView>();
            resultFilter.ForEach((item) =>
            {
                SchedulesList.Add(new FlightSchedulesModelView(item));
            });
            switch (SortByComboBox.SelectedItem)
            {
                //"From", "To", "Flight number", "Aircraft", "Price" 
                case "Date-Time":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.DateTimeOfRace).ToList();
                    break;
                case "From":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.FromName).ToList();
                    break;
                case "To":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.ToName).ToList();
                    break;
                case "Flight number":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.FlightNumber).ToList();
                    break;
                case "Aircraft":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.CodeOfFlight).ToList();
                    break;
                case "Price":
                    SchedulesList = SchedulesList.OrderBy(FS => FS.EconomyPrice).ToList();
                    break;
            }

            RaisePropertyChanged(nameof(SchedulesList));
        }

        public void CancelFlight_Click(object sender, EventArgs args) //TODO: Сделать реализацию отмены рейса
        {

        }
        public void EditFlight_Click(object sender, EventArgs args) //TODO: Сделать реализацию изменении рейса
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void FromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)FromComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
            FromComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)ToComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
        }

        private void ToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)FromComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
            FromComboBox.ItemsSource = AmonicContext.GetContext().Airport.Where(A => A.ShortName != (string)ToComboBox.SelectedItem)
                                                                                    .Select(A => A.ShortName).ToList();
        }
    }
}
