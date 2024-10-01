public enum OrderState
{
    Order,
    Cart,
    Processing,
    Confirmed,
    Shipping,
    Canceled,
    Returned,
    Completed
}

public static class OrderStateExtensions
{
    private static readonly Dictionary<OrderState, string> VietnameseNames = new Dictionary<OrderState, string>
    {
        { OrderState.Order, "Đặt hàng" },
        { OrderState.Cart, "Giỏ hàng" },
        { OrderState.Processing, "Đang xử lý" },
        { OrderState.Confirmed, "Đã xác nhận" },
        { OrderState.Shipping, "Đang giao hàng" },
        { OrderState.Canceled, "Đã hủy" },
        { OrderState.Returned, "Đã trả lại" },
        { OrderState.Completed, "Đã hoàn thành" }
    };

    public static string GetVietnameseName(this OrderState orderState)
    {
        return VietnameseNames[orderState];
    }
}
