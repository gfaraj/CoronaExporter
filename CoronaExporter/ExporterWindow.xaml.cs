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

namespace Squadventure.Corona
{
	/// <summary>
	/// Interaction logic for ExporterWindow.xaml
	/// </summary>
	public partial class ExporterWindow : Window
	{
		public ExporterWindow()
		{
			InitializeComponent();
		}

		private void browseAppButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.SelectedPath = appPathText.Text;

				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					appPathText.Text = dialog.SelectedPath;
				}
			}
		}

		private void browseOutputButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				dialog.SelectedPath = outputPathText.Text;

				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					outputPathText.Text = dialog.SelectedPath;
				}
			}
		}

		private void exportButton_Click(object sender, RoutedEventArgs e)
		{
			exportButton.IsEnabled = false;

			var exporter = new Exporter();
			try
			{
				exporter.AppDirectory = appPathText.Text;
				exporter.ImageSuffix = imageSuffixText.Text;
				exporter.TargetPath = outputPathText.Text;

				exporter.Export();
			}
			finally
			{
				exportButton.IsEnabled = true;
			}
		}
	}
}
