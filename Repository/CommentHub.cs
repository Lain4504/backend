using BackEnd.Models;
using Microsoft.AspNetCore.SignalR;

public class CommentHub : Hub
{
    private readonly BookStoreContext _context;
    public CommentHub(BookStoreContext context)
    {
        _context = context;
    }

    public async Task SendComment(long bookId, long userId, string commentContent)
    {
        var newFeedBack = new Feedback(){
            BookId = bookId,
            UserId = userId,
            Comment = commentContent,
            CreatedAt = DateTime.Now,
            State = "active"
        };

        await _context.Feedbacks.AddAsync(newFeedBack);
        await _context.SaveChangesAsync();

        await Clients.Group(bookId.ToString()).SendAsync("ReceiveComment", userId, commentContent);
    }
}