// ViewModel/MainViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BojRankApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        // 현재 화면에 표시될 뷰모델 (UserViewModel 또는 StaticsViewModel이 담김)
        [ObservableProperty]
        private object _currentView;

        // 인스턴스를 미리 만들어둡니다.
        public UserViewModel UserVM { get; } = new();
        public StaticsViewModel StaticsVM { get; } = new();

        public MainViewModel()
        {
            // 앱이 시작될 때 첫 화면 설정
            CurrentView = UserVM;
        }

        // 화면 전환을 위한 명령들
        [RelayCommand]
        public void ShowUserView() => CurrentView = UserVM;

        [RelayCommand]
        public void ShowStaticsView() => CurrentView = StaticsVM;
    }
}