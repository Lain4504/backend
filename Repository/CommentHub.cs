using BackEnd.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

public class CommentHub : Hub
{
    private readonly BookStoreContext _context;

    public CommentHub(BookStoreContext context)
    {
        _context = context;
    }

    public async Task SendComment(long bookId, long userId, string commentContent)
    {
        try
        {
            var newFeedBack = new Feedback
            {
                BookId = bookId,
                UserId = userId,
                Comment = commentContent,
                CreatedAt = DateTime.Now,
                State = "active"
            };

            await _context.Feedbacks.AddAsync(newFeedBack);
            await _context.SaveChangesAsync();

            await Clients.Group(bookId.ToString()).SendAsync("ReceiveComment", newFeedBack);
        }
        catch (Exception ex)
        {
            // Log the error (you might want to use a logging framework)
            Console.WriteLine($"Error sending comment: {ex.Message}");
            // Optionally, you can throw an exception or handle it in some other way
        }
    }


    public async Task JoinBookRoom(string bookId)
    {
        if (string.IsNullOrEmpty(bookId))
        {
            throw new ArgumentException("Book ID cannot be null or empty");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, bookId);
    }

    public async Task LeaveBookRoom(string bookId)
    {
        if (string.IsNullOrEmpty(bookId))
        {
            throw new ArgumentException("Book ID cannot be null or empty");
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, bookId);
    }
}
