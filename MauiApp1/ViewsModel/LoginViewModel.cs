using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiAppPro.Pages;

namespace MauiApp1.ViewsModel
{
    public partial class LoginViewsModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        string email = "";

        [ObservableProperty]
        string password = "";

        // Inject UserService ผ่าน constructor
        public LoginViewsModel(UserService userService)
        {
            _userService = userService;
        }

        // เปลี่ยนระดับการเข้าถึงของฟังก์ชัน Login เป็น public
        [RelayCommand]
        public async Task Login()  // เปลี่ยนจาก private เป็น public
        {
            // อ่านข้อมูลผู้ใช้จาก UserService
            var user = await _userService.GetUserByEmailAsync(Email);

            // ตรวจสอบว่าอีเมลและรหัสผ่านตรงกับข้อมูลในระบบ
            if (user != null && user.Password == Password)
            {
                // ถ้าพบข้อมูลตรง ไปยังหน้า HomePage พร้อมส่ง userId
                Debug.WriteLine("Login successful: User authenticated");
                await GotoPage($"{nameof(HomePage)}?userId={user.StudentId}"); // ส่ง StudentId ไปยังหน้า HomePage
            }
            else
            {
                // ถ้าไม่พบข้อมูล
                Debug.WriteLine("Invalid email or password.");
                await Shell.Current.DisplayAlert("Error", "Invalid email or password.", "OK");
            }
        }

        [RelayCommand]
        async Task GotoPage(string route)
        {
            await Shell.Current.GoToAsync(route);
        }
    }
}
