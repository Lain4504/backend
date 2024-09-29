using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Repository.RepositoryImpl
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BookStoreContext _context;

        public PublisherRepository(BookStoreContext context)
        {
            _context = context;
        }

        // Phương thức xóa Publisher theo id
        public async Task DeletePublisherAsync(long id)
        {
            // Tìm publisher theo id
            var publisher = await _context.Publishers.FindAsync(id);
            // Kiểm tra xem có bất kỳ sách nào có publisherId trùng với publisherId cần xóa hay không
            var qr = await (from b in _context.Books
                            where b.PublisherId == id // Giả sử id là id của publisher cần xóa
                            select b).FirstOrDefaultAsync();

            if (qr != null)
            {
                // Có sách liên kết với publisher này, không thể xóa
                throw new Exception("khong xoa duoc");
            }

            else
            {
                // Nếu không tìm thấy publisher
                if (publisher == null)
                {
                    throw new InvalidOperationException($"Publisher with Id {id} not found.");
                }
                // Xóa publisher
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }



        }

        // Phương thức lấy tất cả các publisher
        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        // Phương thức lấy Publisher theo id
        public async Task<Publisher?> GetPublisherByIdAsync(long id)
        {
            // Tìm Publisher, nếu không có trả về null
            return await _context.Publishers.FindAsync(id);
        }

        // Phương thức lưu Publisher mới
        public async Task SavePublisherAsync(Publisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException(nameof(publisher), "Publisher cannot be null");
            }

            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();
        }

        // Phương thức cập nhật tên của Publisher theo id
        public async Task UpdatePublisherAsync(long id, string newName)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                throw new InvalidOperationException($"Publisher with Id {id} not found.");
            }

            publisher.Name = newName;
            await _context.SaveChangesAsync();
        }
    }
}
