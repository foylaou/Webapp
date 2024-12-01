namespace webapi.Entities;

public class UserInfo 
{
    public int Id { get; set; } // 主鍵
    public string UserName { get; set; } // 使用者名稱
    public string Email { get; set; } // 電子郵件
    public string Password { get; set; } // 密碼
    public DateTime CreatedAt { get; set; } // 創建時間
}
