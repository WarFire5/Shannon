namespace Shannon;

static class FileLocator
{
    /// <summary>Ищет файл, поднимаясь вверх от текущего каталога запуска.</summary>
    public static string? FindFileUpwards(string fileName)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null)
        {
            string candidate = Path.Combine(dir.FullName, fileName);
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        return null;
    }
}