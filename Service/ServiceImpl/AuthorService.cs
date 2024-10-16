﻿using BackEnd.DTO.Request;
using BackEnd.Models;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public Task<IEnumerable<Author>> GetAllAuthors() => _authorRepository.GetAllAuthorsAsync();
    public Task<Author> GetAuthorById(long id) => _authorRepository.GetAuthorByIdAsync(id);
    public Task DeleteAuthor(long id) => _authorRepository.DeleteAuthorAsync(id);
    public Task AddAuthor(Author author) => _authorRepository.AddAuthorAsync(author);
    public Task UpdateAuthor(Author author) => _authorRepository.UpdateAuthorAsync(author);
    public async Task<bool> AddBookToAuthor(long bookId, long authorId) => await _authorRepository.AddBookToAuthorAsync(bookId, authorId);

    public async Task<bool> RemoveAuthorFromBook(long bookId, long authorId) => await _authorRepository.RemoveAuthorFromBook(bookId, authorId);

        
}
