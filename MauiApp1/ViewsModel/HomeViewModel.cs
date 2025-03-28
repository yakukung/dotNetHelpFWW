using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using MauiApp1.model;
using MauiApp1.Services;
using System;
using System.Linq;

namespace MauiAppPro.ViewsModel;

public partial class HomeViewModel : ObservableObject
{
    private readonly UserService _userService;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private int year;

    [ObservableProperty]
    private string major = string.Empty;

    [ObservableProperty]
    private string studentId = string.Empty;

    [ObservableProperty]
    private string profilePicture = string.Empty;

    public HomeViewModel()
    {
        _userService = new UserService();
    }

    public async void LoadData(string userId)
    {
        Debug.WriteLine($"HomeViewModel: LoadData started with userId: {userId}");

        if (!string.IsNullOrEmpty(userId))
        {
            try
            {
                // Load all users
                var users = await _userService.LoadUsersAsync();
                
                if (users != null && users.Count > 0)
                {
                    Debug.WriteLine($"HomeViewModel: Users list count: {users.Count}");
                    
                    // Find only the logged-in user
                    var user = users.FirstOrDefault(u => u.StudentId?.Trim().ToLower() == userId.Trim().ToLower());

                    if (user != null)
                    {
                        Debug.WriteLine($"HomeViewModel: User found: {user.FirstName} {user.LastName}");

                        try
                        {
                            // Set user properties with null checking
                            FullName = (user.FirstName != null && user.LastName != null) 
                                ? $"{user.FirstName} {user.LastName}" 
                                : "ไม่พบชื่อ";
                            
                            // Fix for the Year property - simple cast from long to int
                            Year = (int)user.Year;
                            
                            Major = user.Major ?? "ไม่พบสาขา";
                            
                            StudentId = user.StudentId ?? "ไม่พบรหัสนิสิต";
                            
                            // Set profile picture if available
                            ProfilePicture = !string.IsNullOrEmpty(user.ProfilePicture) 
                                ? user.ProfilePicture 
                                : "dotnet_bot.png";
                            
                            // Log only the logged-in user information
                            Debug.WriteLine("========== LOGGED IN USER ==========");
                            Debug.WriteLine($"ชื่อ-นามสกุล: {FullName}");
                            Debug.WriteLine($"รหัสนิสิต: {StudentId}");
                            Debug.WriteLine($"ชั้นปี: {Year}");
                            Debug.WriteLine($"สาขา: {Major}");
                            Debug.WriteLine($"ที่อยู่ภาพ: {ProfilePicture}");
                            Debug.WriteLine("===================================");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"HomeViewModel: Error processing user data: {ex.Message}");
                            SetDefaultValues();
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"HomeViewModel: User with ID {userId} not found in the users list.");
                        SetDefaultValues();
                    }
                }
                else
                {
                    Debug.WriteLine("HomeViewModel: Users list is empty or null.");
                    SetDefaultValues();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HomeViewModel: Error loading user data: {ex.Message}");
                SetDefaultValues();
            }
        }
        else
        {
            Debug.WriteLine("HomeViewModel: userId is null or empty.");
            SetDefaultValues();
        }
    }

    [RelayCommand]
    public async Task NavigateToProfilePage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.ProfilePage), new Dictionary<string, object> { { "userId", StudentId } });
    }

    [RelayCommand]
    public async Task NavigateToCurrentTermRegistrationPage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.CurrentTermRegistrationPage), new Dictionary<string, object> { { "userId", StudentId } });
    }

    [RelayCommand]
    public async Task NavigateToSearchCoursesPage()
    {
        await Shell.Current.GoToAsync(nameof(Pages.SearchCoursesPage), new Dictionary<string, object> { { "userId", StudentId } });
    }
    
    private void SetDefaultValues()
    {
        FullName = "ไม่พบผู้ใช้";
        Year = 0;
        Major = "ไม่พบสาขา";
        StudentId = "ไม่พบรหัสนิสิต";
        ProfilePicture = "dotnet_bot.png";
    }
}