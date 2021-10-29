using AS_WPF_AWS_S3_IAM.Component;
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

namespace AS_WPF_AWS_S3_IAM
{
   
    public partial class MainWindow : Window
    {
        CreateBucket createBucket;
        UploadToS3Bucket uploadToS3Bucket;
        public MainWindow()
        {
            InitializeComponent();
            createBucket = new CreateBucket(this);
            uploadToS3Bucket = new UploadToS3Bucket(this);
        }

        private void btnCreateBucket_Click(object sender, RoutedEventArgs e)
        {
           
            createBucket.Show();
        }

        private void btnObjectLevelOperations_Click(object sender, RoutedEventArgs e)
        {
            uploadToS3Bucket.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
