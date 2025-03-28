
using MauiApp1.model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class UserService
    {
        private readonly string _usersFileName = "users.json";
        private List<User> _users;

        public async Task<List<User>> LoadUsersAsync(bool forceReload = false)
        {
            if (!forceReload && _users != null)
            {
                return _users;
            }

            try
            {
                var appDataPath = Path.Combine(FileSystem.AppDataDirectory, _usersFileName);

                if (!File.Exists(appDataPath))
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync(_usersFileName);
                    using var reader = new StreamReader(stream);
                    var jsonData = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        await File.WriteAllTextAsync(appDataPath, jsonData);
                    }
                }

                var json = await File.ReadAllTextAsync(appDataPath);
                _users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                return _users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading users: {ex.Message}");
                return new List<User>();
            }
        }

        // เปลี่ยนจาก Username เป็น Email
        public async Task<User> GetUserByEmailAsync(string email, bool forceReload = false)
        {
            var users = await LoadUsersAsync(forceReload);
            return users.FirstOrDefault(u => u.Email == email); // ใช้ Email แทน Username
        }

        public async Task<bool> AddUserAsync(User newUser)
        {
            var users = await LoadUsersAsync();

            // ตรวจสอบว่า Email ซ้ำกันหรือไม่
            if (users.Any(u => u.Email == newUser.Email))  // เปลี่ยนจาก Username เป็น Email
            {
                return false; // หากซ้ำให้คืนค่า false
            }

            users.Add(newUser);
            return await SaveUsersAsync(users);
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var users = await LoadUsersAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == updatedUser.Email); // เปลี่ยนจาก Username เป็น Email

            if (existingUser == null)
            {
                return false;
            }

            // อัปเดตข้อมูลผู้ใช้
            existingUser.Password = updatedUser.Password;
            existingUser.ProfilePicture = updatedUser.ProfilePicture;

            return await SaveUsersAsync(users);
        }

        private async Task<bool> SaveUsersAsync(List<User> users)
        {
            try
            {
                var json = JsonConvert.SerializeObject(users, Formatting.Indented);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _usersFileName);
                await File.WriteAllTextAsync(filePath, json);

                _users = await LoadUsersAsync(true); // บังคับโหลดใหม่จากไฟล์

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving users: {ex.Message}");
                return false;
            }
        }
    }
}
