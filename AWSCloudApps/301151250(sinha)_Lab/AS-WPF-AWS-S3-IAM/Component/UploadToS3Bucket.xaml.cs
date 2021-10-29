using AS_WPF_AWS_S3_IAM.Class;
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
using System.Windows.Shapes;

namespace AS_WPF_AWS_S3_IAM.Component
{
   
    public partial class UploadToS3Bucket : Window
    {
        private MainWindow m_parent;
        //NOT GETTING RESPONSE FROM S3 AND APPLICATION GETTING STUCK IF IT IS TRUE THEN IT GOES INSIDE IF AND FETCH BUCKET FILES
        private bool fetchBucketFiles = false;
        public UploadToS3Bucket(MainWindow parent)
        {
            InitializeComponent();
            m_parent = parent;
        }

        private void btnBackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            m_parent.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_parent.Hide();
            refreshBucketListFromS3();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            m_parent.Show();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();          
            if (result == true)
            {
                FileNameTextBox.Text = openFileDlg.FileName;
                //TextBlock1.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }
        #region Class

        private void refreshBucketListFromS3()
        {
            var BucketListing = S3Service.transformBucketListFromS3();
            List<BucketList> bucketLists = new List<BucketList>();
            comboBucketList.ItemsSource = null;
            comboBucketList.ItemsSource = BucketListing.Result.Select(x => x.BucketName);
            comboBucketList.Items.Refresh();
        }
        private void fetchBucketData(string bucketName) {
            var BucketItemListing = S3Service.ListAllItemOfBucketAsync(bucketName);
            datagridBucketFiles.ItemsSource = null;
            datagridBucketFiles.ItemsSource = BucketItemListing.Result.S3Objects.Select(x => { return new { Key = x.Key, Size = x.Size };});
            datagridBucketFiles.Items.Refresh();
        }
        
        #endregion

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var localpath = FileNameTextBox.Text;
            var fileName = FileNameTextBox.Text.Split("\\").Last();
            if (comboBucketList.SelectedValue == null)
            {
                MessageBox.Show("Please select bucket then try to upload file");
            }
            else { 
                var data = S3Service.sendMyFileToS3(localpath, comboBucketList.SelectedValue.ToString(),"", fileName);
                if (data) {
                    MessageBox.Show("File uploaded successfully on S3 Bucket Name: " + comboBucketList.SelectedValue.ToString());
                }
                if (fetchBucketFiles)//NOT GETTING RESPONSE FROM S3 AND APPLICATION GETTING STUCK IF IT IS TRUE THEN IT GOES INSIDE IF AND FETCH BUCKET FILES
                {
                    fetchBucketData(comboBucketList.SelectedValue.ToString());
                }
                
            }
        }

        private void comboBucketList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBucket = comboBucketList.SelectedValue.ToString();
            if (fetchBucketFiles) {
                fetchBucketData(selectedBucket);
            }
        }
    }
}
