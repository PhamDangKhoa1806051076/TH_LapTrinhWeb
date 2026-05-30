namespace WebBanHang2.Models;

public class ErrorViewModel
{
    
    public string? RequestId { get; set; }

    // Kiểm tra xem có RequestId hay không để quyết định hiển thị lên giao diện
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
