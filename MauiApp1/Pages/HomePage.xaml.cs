using MauiAppPro.ViewsModel;
using System.Diagnostics; // เพิ่ม namespace สำหรับ Debug.WriteLine

namespace MauiAppPro.Pages;

[QueryProperty(nameof(UserId), "userId")]
public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;
    private string _userId = string.Empty;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public string UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnUserIdReceived(value);
        }
    }

    private void OnUserIdReceived(string userId)
    {
        Debug.WriteLine($"HomePage: Received userId: {userId}");
        Debug.WriteLine($"HomePage: userId before LoadData: {userId}");

        if (string.IsNullOrEmpty(userId))
        {
            Debug.WriteLine("HomePage: userId is null or empty. No data loaded.");
            return;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!string.IsNullOrEmpty(_userId))
        {
            Debug.WriteLine($"HomePage: OnAppearing with userId: {_userId}");
            _viewModel.LoadData(_userId);
            UpdateUserInfoDisplay();
        }
    }
    
    private void UpdateUserInfoDisplay()
    {
        Debug.WriteLine("HomePage: Updating UI with user information");
    }
}