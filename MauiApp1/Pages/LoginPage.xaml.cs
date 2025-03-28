
using MauiApp1.Services;
using MauiApp1.ViewsModel;
namespace MauiAppPro.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewsModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();  // ต้องมีการเรียก InitializeComponent ที่นี่
            var userService = new UserService();  // สร้าง instance ของ UserService
            _viewModel = new LoginViewsModel(userService); // Inject UserService เข้าไปใน ViewModel
            this.BindingContext = _viewModel;  // กำหนด BindingContext ใน C#
        }

        // เมื่อกดปุ่ม Login
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await _viewModel.Login(); // ใช้คำสั่ง Login ที่ถูกต้องใน ViewModel
        }
    }
}
