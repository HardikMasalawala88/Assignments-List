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
    
    public partial class CreateBucket : Window
    {
        private MainWindow m_parent;
        public CreateBucket(MainWindow parent)
        {
            InitializeComponent();
            m_parent = parent;
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

        private void btnBackToMail_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            m_parent.Show();
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBucketName.Text))
            {
                S3Service.CreateBucket(txtBucketName.Text);
                MessageBox.Show("Bucket Created successfully");
                txtBucketName.Text = string.Empty;
                refreshBucketListFromS3();
                //if (result != null && result.HttpStatusCode == System.Net.HttpStatusCode.OK) 
                //{ 
                //    refreshBucketListFromS3();
                //}
            }
            else {
                MessageBox.Show("Enter Bucket Name");
            }
        }

        #region Class

        private void refreshBucketListFromS3()
        {
            var BucketListing = S3Service.transformBucketListFromS3();
            List<BucketList> bucketLists = new List<BucketList>();
            dataGridBucketList.ItemsSource = null;
            dataGridBucketList.ItemsSource = BucketListing.Result;
            dataGridBucketList.Items.Refresh();
        }

        #endregion
    }
}
