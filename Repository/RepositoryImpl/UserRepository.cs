﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using BackEnd.DTO.Request;
using Microsoft.AspNetCore.Identity;

namespace BackEnd.Repository.RepositoryImpl
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;

        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string Email)
        {
            var user = await _context.Users
                .Where(u => u.Email == Email)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.Password,
                    u.Phone,
                    u.Dob,
                    u.Address,
                    u.Role,
                    u.State,
                    u.Gender

                })
                .SingleOrDefaultAsync();

            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return null;
            }

            // Xử lý các trường có thể null
            return new User
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                Dob = user.Dob,
                Address = user.Address,
                Role = user.Role,
                State = user.State,
                Gender = user.Gender
            };
        }

        public async Task<User> GetByIDAsync(long id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id) // Giả sử Id là khóa chính của người dùng
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Password,
                    u.Email,
                    u.Phone,
                    u.Dob,
                    u.Address,
                    u.Gender,
                    u.Role
                })
                .SingleOrDefaultAsync();

            if (user == null)
            {
               return null; // Hoặc có thể ném một ngoại lệ tùy thuộc vào yêu cầu
            }

            // Return the User object with the retrieved fields
            return new User
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                Dob = user.Dob,
                Address = user.Address,
                Gender = user.Gender,
                Role = user.Role
            };
        }


        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            // Kiểm tra xem người dùng có tồn tại không bằng Email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Cập nhật các thuộc tính cần thiết
            existingUser.State = user.State;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserPassword(User user)
        {
            // Kiểm tra xem người dùng có tồn tại không bằng Email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Cập nhật các thuộc tính cần thiết
            existingUser.Password = user.Password;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserProfile(UserUpdateRequest user, long id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist.");
            }

            // Cập nhật thông tin người dùng
            existingUser.FullName = user.FullName;
            existingUser.Dob = user.Dob;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Gender = user.Gender;
            // Lưu thay đổi vào database thông qua repository
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserRoleAndState(User user, long id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist.");
            }

            // Cập nhật thông tin người dùng
            existingUser.State = user.State;
            existingUser.Role = user.Role;
            // Lưu thay đổi vào database thông qua repository
            await _context.SaveChangesAsync();
        }
        public async Task ChangePassword(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Cập nhật các thuộc tính cần thiết
            existingUser.Password = user.Password;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task DeleteUserAsync(long id)
        {
            var user = await GetByIDAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}