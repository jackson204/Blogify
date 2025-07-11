namespace Blogify.ClientApi.ViewModel;

/// <summary>
/// 關於頁面 ViewModel
/// </summary>
public class AboutViewModel
{
    /// <summary>
    /// 網站標題
    /// </summary>
    public string SiteTitle { get; set; } = "MyBlog";

    /// <summary>
    /// 網站描述
    /// </summary>
    public string SiteDescription { get; set; } = "專業的技術部落格平台，分享最新的程式設計知識和開發經驗";

    /// <summary>
    /// 關於內容（HTML 格式）
    /// </summary>
    public string AboutContent { get; set; } = "";

    /// <summary>
    /// 網站統計資料
    /// </summary>
    public Dictionary<string, int> Statistics { get; set; } = new();

    /// <summary>
    /// 團隊成員列表
    /// </summary>
    public List<TeamMemberViewModel> TeamMembers { get; set; } = new();

    /// <summary>
    /// 聯絡郵箱
    /// </summary>
    public string ContactEmail { get; set; } = "contact@myblog.com";

    /// <summary>
    /// 建立日期
    /// </summary>
    public DateTime EstablishedDate { get; set; } = new DateTime(2024, 1, 1);
}

/// <summary>
/// 團隊成員 ViewModel
/// </summary>
public class TeamMemberViewModel
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 職位
    /// </summary>
    public string Role { get; set; } = "";

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// 頭像 URL
    /// </summary>
    public string Avatar { get; set; } = "";

    /// <summary>
    /// 社交媒體連結
    /// </summary>
    public Dictionary<string, string> SocialLinks { get; set; } = new();
}
