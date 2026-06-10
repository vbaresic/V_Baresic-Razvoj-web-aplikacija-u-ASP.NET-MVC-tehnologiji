namespace League_of_Legends_Tournament_Hosting.Services;

public static class TournamentDocumentValidator
{
    public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg", ".gif", ".txt"
    };

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "image/png",
        "image/jpeg",
        "image/gif",
        "text/plain"
    };

    public static bool IsValid(IFormFile file, out string? error)
    {
        if (file.Length > MaxFileSizeBytes)
        {
            error = $"File exceeds the maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB.";
            return false;
        }

        var extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
        {
            error = "File type is not allowed.";
            return false;
        }

        if (!AllowedContentTypes.Contains(file.ContentType))
        {
            error = "File content type is not allowed.";
            return false;
        }

        error = null;
        return true;
    }
}
