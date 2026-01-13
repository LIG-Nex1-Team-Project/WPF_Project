using BojRankApp.Model;
using BojRankApp.Service;
using System.Diagnostics;
using System.Globalization;
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
    public class TagColorConverter : IValueConverter
    {
        // 15가지 파스텔톤 색상 팔레트 (배경색, 글자/테두리색)
        private readonly List<(string Back, string Fore)> _colors = new()
        {
            ("#E3F2FD", "#1565C0"), ("#E8F5E9", "#2E7D32"), ("#FFF3E0", "#EF6C00"),
            ("#F3E5F5", "#7B1FA2"), ("#FFEBEE", "#C62828"), ("#E0F7FA", "#00838F"),
            ("#FFF8E1", "#F9A825"), ("#FCE4EC", "#AD1457"), ("#ECEFF1", "#455A64"),
            ("#F1F8E9", "#558B2F"), ("#EDE7F6", "#512DA8"), ("#EFEBE9", "#4E342E"),
            ("#FAFAFA", "#212121"), ("#F9FBE7", "#827717"), ("#E0F2F1", "#00695C")
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string tagName) return Brushes.Transparent;

            // 태그 이름의 해시코드(고유 숫자)를 이용해서 항상 같은 색상을 배정
            int index = Math.Abs(tagName.GetHashCode()) % _colors.Count;
            var colorPair = _colors[index];

            string colorCode = (parameter as string) == "Fore" ? colorPair.Fore : colorPair.Back;

            return new BrushConverter().ConvertFromString(colorCode) as Brush ?? Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    // MainWindow.xaml.cs 에 추가

    public class TierColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string tierName) return Brushes.Transparent;

            string colorCode;
            bool isForeground = (parameter as string) == "Fore";

            // 티어별 색상 정의
            switch (tierName)
            {
                case "Bronze":
                    // 브론즈: 갈색 계열
                    colorCode = isForeground ? "#AD5600" : "#FFF4E6";
                    break;
                case "Silver":
                    // 실버: 짙은 회색
                    colorCode = isForeground ? "#384C5E" : "#F4F6F8";
                    break;
                case "Gold":
                    // 골드: 황금색 계열
                    colorCode = isForeground ? "#EC9A00" : "#FFF9DB";
                    break;
                case "Platinum":
                    // 플래티넘: 민트색 계열
                    colorCode = isForeground ? "#00C78B" : "#E6FCF5";
                    break;
                case "Diamond":
                    // 다이아몬드: 하늘색 계열
                    colorCode = isForeground ? "#00B4FC" : "#E3F2FD";
                    break;
                case "Ruby":
                    // 루비: 붉은색 계열
                    colorCode = isForeground ? "#FF0062" : "#FFF0F6";
                    break;
                case "Master":
                    // 마스터: 보라색 계열
                    colorCode = isForeground ? "#B300B3" : "#F8F0FC";
                    break;
                default: // Unrated 등
                    colorCode = isForeground ? "#495057" : "#F1F3F5";
                    break;
            }

            return new BrushConverter().ConvertFromString(colorCode) as Brush ?? Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}