namespace ModularPipelines.FileSystem;

internal static class FileCopyMetadata
{
    internal static void CopyUnixPermissions(IFileSystemProvider provider, string sourcePath, string destinationPath)
    {
        if (!OperatingSystem.IsWindows() && ReferenceEquals(provider, SystemFileSystemProvider.Instance))
        {
            const UnixFileMode permissions = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
                | UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute
                | UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute;

            // Never transfer setuid, setgid, or sticky bits to a potentially differently owned copy.
            File.SetUnixFileMode(destinationPath, File.GetUnixFileMode(sourcePath) & permissions);
        }
    }
}
