namespace ModularPipelines.FileSystem;

internal static class FileCopyMetadata
{
    internal static bool UsesUnixPermissions(IFileSystemProvider provider) =>
        !OperatingSystem.IsWindows() && ReferenceEquals(provider, SystemFileSystemProvider.Instance);

    internal static void CopyUnixPermissions(IFileSystemProvider provider, Stream sourceStream, Stream destinationStream)
    {
        if (!OperatingSystem.IsWindows() && UsesUnixPermissions(provider))
        {
            const UnixFileMode permissions = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
                | UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute
                | UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute;

            // Never transfer setuid, setgid, or sticky bits to a potentially differently owned copy.
            // Use the open files so replacing either path cannot redirect the metadata operation.
            File.SetUnixFileMode(((FileStream) destinationStream).SafeFileHandle,
                File.GetUnixFileMode(((FileStream) sourceStream).SafeFileHandle) & permissions);
        }
    }
}
