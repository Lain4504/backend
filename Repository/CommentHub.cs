using BackEnd.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

public class CommentHub : Hub
{

    public async Task SendComment(long bookId, string userId, string commentContent)
    {
        try
        {
            await Clients.Group(bookId.ToString()).SendAsync("ReceiveComment", userId, commentContent);
        
        }
        catch (Exception ex)
        {
            // Log the error (you might want to use a logging framework)
            Console.WriteLine($"Error sending comment: {ex.Message}");
            // Optionally, you can throw an exception or handle it in some other way
        }
    }
    
    public override async Task OnConnectedAsync()
    {
        var bookId = Context.GetHttpContext().Request.Query["bookId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, bookId.ToString());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var bookId = Context.GetHttpContext().Request.Query["bookId"];
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, bookId.ToString());
        await base.OnDisconnectedAsync(exception);
    }
}
