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

        private void Datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid listBox = sender as DataGrid;

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

    // [수정됨] 숫자(int)와 문자열(string) 모두 처리하는 만능 컨버터
    public class TierColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tierName = "";

            // 1. 데이터가 '숫자'로 들어온 경우 (User 목록 등) -> 이름으로 변환
            if (value is int tierInt)
            {
                tierName = GetTierName(tierInt);
            }
            // 2. 데이터가 '문자'로 들어온 경우 (통계 목록 등) -> 그대로 사용
            else if (value is string name)
            {
                tierName = name;
            }

            // 색상 결정 로직
            string colorCode;
            bool isForeground = (parameter as string) == "Fore"; // 글자색인지 배경색인지 확인

            switch (tierName)
            {
                case "Bronze":
                    colorCode = isForeground ? "#AD5600" : "#FFF4E6"; break;
                case "Silver":
                    colorCode = isForeground ? "#384C5E" : "#F4F6F8"; break;
                case "Gold":
                    colorCode = isForeground ? "#EC9A00" : "#FFF9DB"; break;
                case "Platinum":
                    colorCode = isForeground ? "#00C78B" : "#E6FCF5"; break;
                case "Diamond":
                    colorCode = isForeground ? "#00B4FC" : "#E3F2FD"; break;
                case "Ruby":
                    colorCode = isForeground ? "#FF0062" : "#FFF0F6"; break;
                case "Master":
                    colorCode = isForeground ? "#B300B3" : "#F8F0FC"; break;
                default: // Unrated 또는 알 수 없는 경우
                    colorCode = isForeground ? "#495057" : "#F1F3F5"; break;
            }

            // 색상 반환 (실패 시 투명색 대신 기본 검정/흰색 반환하여 보이게 함)
            try
            {
                return new BrushConverter().ConvertFromString(colorCode) as Brush;
            }
            catch
            {
                return isForeground ? Brushes.Black : Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        // 티어 숫자 -> 이름 변환 헬퍼 함수
        private string GetTierName(int tier)
        {
            if (tier == 0) return "Unrated";
            if (tier >= 1 && tier <= 5) return "Bronze";
            if (tier >= 6 && tier <= 10) return "Silver";
            if (tier >= 11 && tier <= 15) return "Gold";
            if (tier >= 16 && tier <= 20) return "Platinum";
            if (tier >= 21 && tier <= 25) return "Diamond";
            if (tier >= 26 && tier <= 30) return "Ruby";
            if (tier >= 31) return "Master";
            return "Unknown";
        }
    }
}