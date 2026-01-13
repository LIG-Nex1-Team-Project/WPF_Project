using BojRankApp.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BojRankApp.Model;
using System.Diagnostics;

namespace BojRankApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BojService bojService;

        public MainWindow()
        {
            InitializeComponent();
            bojService = new BojService();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = await bojService.LoadUser("ghddmsrl100");
            Debug.WriteLine(user.Id);
            Debug.WriteLine(user.Rating);
            Debug.WriteLine(user.Tier);

            List<SolvedProblem> list = user.SolvedProblems;
            Debug.WriteLine("cnt: " + list.Count);
            foreach (var item in list) {
                Debug.Write(item.Name + " " + item.Pid + " " + item.Difficulty + " ");
                foreach(var s in item.Tags)
                {
                    Debug.Write(s + " ");
                }
                Debug.WriteLine("");
            }

        }
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null && listBox.SelectedItem is SolvedProblem problem)
            {
                string url = $"https://www.acmicpc.net/problem/{problem.Pid}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
    }
}