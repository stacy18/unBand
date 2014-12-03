﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Cargo.Client;
using Microsoft.Live;
using Microsoft.Live.Desktop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using unBand.BandHelpers;
using unBand.Cloud;
using unBand.CloudHelpers;

namespace unBand.pages
{
    /// <summary>
    /// Interaction logic for MyBandPage.xaml
    /// </summary>
    public partial class ActivityLogPage : UserControl
    {

        private BandManager _band;
        ProgressDialogController _progressDialog;

        public List<BandEventBase> Events { get; set; }

        public ActivityLogPage()
        {
            InitializeComponent();

            _band = BandManager.Instance;

            this.DataContext = BandCloudManager.Instance;
        }

        private void btnExportLast100_Click(object sender, RoutedEventArgs e)
        {
            ExportEvents(100);
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            ExportEvents();            
        }

        private async void ExportEvents(int? count = null)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.FileName = "band_export.csv";
            saveDialog.DefaultExt = ".csv";

            var result = saveDialog.ShowDialog();

            if (result == true)
            {
                _progressDialog = await ((MetroWindow)(Window.GetWindow(this))).ShowProgressAsync("Exporting Data", "...");
                _progressDialog.SetCancelable(true); // TODO: this needs to be implemented. No event?
                _progressDialog.SetProgress(0);

                var progressIndicator = new Progress<BandCloudExportProgress>(ReportProgress);

                await BandCloudManager.Instance.ExportEventsSummary(saveDialog.FileName, count, progressIndicator);

                _progressDialog.CloseAsync();
            }
        }

        void ReportProgress(BandCloudExportProgress value)
        {
            System.Diagnostics.Debug.WriteLine("Export Progress: " + ((double)(value.ExportedEventsCount) / value.TotalEventsToExport) * 100 + "%");
            _progressDialog.SetProgress(((double)(value.ExportedEventsCount) / value.TotalEventsToExport));
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            ExportEvents();
        }
    }
}