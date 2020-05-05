using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FilePath { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            var currentProcesses = Process.GetProcesses().ToList();
            processesDG.ItemsSource = currentProcesses;
        }

        private void ProcessesDGSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы точно хотите удалить?", "Уведомление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel) return;
                using (var selectedProcess = processesDG.SelectedItem as Process)
                {
                    var processes = Process.GetProcessesByName(selectedProcess.ProcessName);
                    foreach (var process in processes)
                    {
                        process.Kill();
                    }
                }
                processesDG.ItemsSource = Process.GetProcesses().ToList();
            }
            catch
            {
                MessageBox.Show("Отказано в доступе");
            }
        }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        private void OpenProcessHighPriorityBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OpenFileDialog())
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = FilePath;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        myProcess.PriorityClass = ProcessPriorityClass.High;
                        MessageBox.Show($"new priority class: {myProcess.PriorityClass}. ID: {myProcess.Id}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenProcessNormalPriorityBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OpenFileDialog())
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = FilePath;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        if (myProcess.PriorityClass == ProcessPriorityClass.RealTime)
                        {
                            myProcess.PriorityClass = ProcessPriorityClass.Normal;
                        }
                        MessageBox.Show($"new priority class: {myProcess.PriorityClass}. ID: {myProcess.Id}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenProcessCheckPriorityBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OpenFileDialog())
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = FilePath;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        myProcess.PriorityClass = ProcessPriorityClass.RealTime;
                        MessageBox.Show($"new priority class: {myProcess.PriorityClass}. ID: {myProcess.Id}");
                    }
                }
                else
                {
                    MessageBox.Show("Файл не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseProcessByIdBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                if (!int.TryParse(idTB.Text, out id))
                {
                    MessageBox.Show("Введите ЦЕЛОЕ ЧИСЛО!");
                    return;
                }

                var result = MessageBox.Show("Вы точно хотите удалить?", "Уведомление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel) return;
                var process = Process.GetProcessById(id);
                process.Kill();
                processesDG.ItemsSource = Process.GetProcesses().ToList();
                idTB.Text = "Enter ID";
            }
            catch
            {
                MessageBox.Show("Отказано в доступе");
            }
        }
    }
}
