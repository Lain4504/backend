﻿using Azure;
using BackEnd.Models;
using BackEnd.Repository;
using BackEnd.Util;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Service.ServiceImpl
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book?> GetBookByIdAsync(long id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(long id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book>> FindBooksByTitleAsync(string title)
        {
            return await _bookRepository.FindByTitleAsync(title);
        }

        public async Task AddBookToCollectionAsync(long bookId, long collectionId)
        {
            await _bookRepository.AddBookToCollectionAsync(bookId, collectionId);
        }

        public Task<PaginatedList<Book>> GetAllBooksAsync(int page, int size, string sortBy, bool isAscending)
        {
            return _bookRepository.GetAllBooksAsync(page, size, sortBy, isAscending);
        }

        public async Task<IEnumerable<Book>> GetBooksByCollectionAsync(int? collectionId, string sortBy, string sortOrder)
        {
            var booksQuery = _bookRepository.GetBooks();

            if (collectionId.HasValue)
            {
                booksQuery = _bookRepository.GetBooksByCollection(collectionId.Value);
            }

            booksQuery = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? booksQuery.OrderBy(b => EF.Property<object>(b, sortBy))
                : booksQuery.OrderByDescending(b => EF.Property<object>(b, sortBy));

            return await booksQuery.Include(b => b.Images).ToListAsync();
        }
    }
}
