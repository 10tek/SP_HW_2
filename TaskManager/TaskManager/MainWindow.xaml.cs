﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                var result = MessageBox.Show("Вы точно хотите удалить?","Уведомление", MessageBoxButton.YesNo);
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
            return;
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

        private void openProcessBtnClick(object sender, RoutedEventArgs e)
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
                        MessageBox.Show($"new priority class: {myProcess.PriorityClass}");
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
    }
}
