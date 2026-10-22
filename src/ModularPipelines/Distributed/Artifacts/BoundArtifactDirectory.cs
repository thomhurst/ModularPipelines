using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.Distributed.Artifacts;

/// <summary>
/// Binds host filesystem operations to opened directories instead of trusting a prior path check.
/// Custom filesystem providers do not use this implementation. The host's trusted anchor may
/// contain system aliases; every directory below it and every opened input is checked without
/// following links. Windows holds the ancestor chain against replacement; Unix uses relative IO
/// against the original directory even if an ancestor is renamed after it has been opened.
/// </summary>
internal sealed partial class BoundArtifactDirectory : IDisposable
{
    private const uint OpenReparsePoint = 0x00200000;
    private const uint BackupSemantics = 0x02000000;
    private readonly List<SafeFileHandle> _directories = [];
    private bool _disposed;

    private BoundArtifactDirectory(string fullPath)
    {
        FullPath = fullPath;
    }

    public string FullPath { get; private set; }

    private SafeFileHandle DirectoryHandle => _directories[^1];

    public static BoundArtifactDirectory Open(string directoryPath, bool create, string? trustedRoot = null)
    {
        if (!OperatingSystem.IsWindows()
            && (!(OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                || RuntimeInformation.ProcessArchitecture is not (Architecture.X64 or Architecture.Arm64)))
        {
            throw new PlatformNotSupportedException("Bound artifact IO requires Windows or x64/arm64 Linux or macOS.");
        }

        var fullPath = Path.GetFullPath(directoryPath);
        var anchor = ArtifactContextImpl.GetTrustedValidationRoot(fullPath, trustedRoot) ?? Path.GetPathRoot(fullPath)!;
        // A configured restore workspace can be new. Bind its nearest existing ancestor,
        // then create each missing descendant relative to the validated directory chain.
        while (create && !Directory.Exists(anchor))
        {
            anchor = Path.GetDirectoryName(Path.TrimEndingDirectorySeparator(anchor))
                ?? throw new DirectoryNotFoundException($"Cannot bind artifact directory '{directoryPath}'.");
        }

        var result = new BoundArtifactDirectory(fullPath);
        try
        {
            // Resolve the trusted anchor once, as Unix open() does. All Windows IO then
            // uses its physical path, never the original alias or alias target path.
            if (OperatingSystem.IsWindows())
            {
                var followedAnchor = CheckHandle(CreateFile(anchor, 0x80, 1, 0, 3, BackupSemantics, 0), anchor, directory: true);
                result._directories.Add(followedAnchor);
                var relativeToAnchor = Path.GetRelativePath(anchor, fullPath);
                anchor = GetFinalWindowsPath(followedAnchor);
                // Extended Windows paths preserve literal '.' segments; do not append one.
                fullPath = relativeToAnchor == "." ? anchor : Path.Combine(anchor, relativeToAnchor);
                result.FullPath = fullPath;
            }

            var walkRoot = OperatingSystem.IsWindows() ? Path.GetPathRoot(fullPath)! : anchor;
            result._directories.Add(OperatingSystem.IsWindows()
                ? OpenWindowsDirectory(walkRoot)
                : CheckHandle(OpenUnix(anchor, UnixDirectoryFlags & ~UnixNoFollow), anchor, directory: true));
            var currentPath = walkRoot;
            var relative = Path.GetRelativePath(walkRoot, fullPath);
            foreach (var segment in relative.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
            {
                if (segment == ".")
                {
                    continue;
                }

                currentPath = Path.Combine(currentPath, segment);
                SafeFileHandle handle;
                if (OperatingSystem.IsWindows())
                {
                    // Each parent is locked before creating/opening its child. A concurrently
                    // inserted reparse point is opened itself and rejected below.
                    if (create)
                    {
                        Directory.CreateDirectory(currentPath);
                    }

                    handle = OpenWindowsDirectory(currentPath);
                }
                else
                {
                    // EEXIST is safe: openat still validates the entry after mkdirat.
                    if (create && MakeDirectoryAt(result.DirectoryHandle, segment, 0x1ff) != 0
                        && Marshal.GetLastPInvokeError() != 17)
                    {
                        throw NativeError(currentPath);
                    }

                    handle = CheckHandle(OpenAt(result.DirectoryHandle, segment, UnixDirectoryFlags), currentPath, directory: true);
                }

                result._directories.Add(handle);
                if (OperatingSystem.IsWindows() && currentPath.Equals(anchor, StringComparison.OrdinalIgnoreCase)
                    && !GetFinalWindowsPath(result._directories[0]).Equals(anchor, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("The trusted artifact directory moved while its ancestors were being bound.");
                }
            }

            return result;
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }

    public FileStream OpenRead(string leaf)
    {
        ValidateLeaf(leaf);
        var path = Path.Combine(FullPath, leaf);
        var handle = OperatingSystem.IsWindows()
            ? CheckHandle(CreateFile(path, 0x80000000, 1, 0, 3, OpenReparsePoint, 0), path)
            : CheckHandle(OpenAt(DirectoryHandle, leaf, UnixCloseOnExec | UnixNoFollow | UnixNonBlocking), path);
        try
        {
            if ((File.GetAttributes(handle) & (FileAttributes.ReparsePoint | FileAttributes.Directory)) != 0)
            {
                throw new IOException($"Artifact input '{path}' must be a file and cannot be a link.");
            }

            if (!OperatingSystem.IsWindows())
            {
                // O_NONBLOCK keeps openat from waiting on a FIFO. Clear it before
                // FileStream detects IsAsync: Unix asynchronous pipe reads ignore seek
                // offsets, which otherwise breaks ZIP reads on this regular file.
                var result = OperatingSystem.IsMacOS() && RuntimeInformation.ProcessArchitecture == Architecture.Arm64
                    ? SetFileFlagsMacArm64(handle, 4, 0, 0, 0, 0, 0, 0, 0)
                    : SetFileFlags(handle, 4, 0); // F_SETFL, no mutable status flags.
                if (result == -1)
                {
                    throw NativeError(path);
                }
            }

            var stream = new ArtifactFileStream(handle, FileAccess.Read, path);
            if (!stream.CanSeek)
            {
                // O_NONBLOCK prevents a FIFO from blocking before this handle validation.
                stream.Dispose();
                throw new IOException($"Artifact input '{path}' must support seeking.");
            }

            return stream;
        }
        catch
        {
            handle.Dispose();
            throw;
        }
    }

    public FileStream CreateNew(string leaf, UnixFileMode? mode = null)
    {
        ValidateLeaf(leaf);
        var path = Path.Combine(FullPath, leaf);
        var permissions = mode.HasValue ? (uint) mode.Value : 0x1b6u; // 0666, subject to the process umask.
        var handle = OperatingSystem.IsWindows()
            ? CheckHandle(CreateFile(path, 0x40000000, 1, 0, 1, OpenReparsePoint, 0), path)
            : CheckHandle(CreateAt(DirectoryHandle, leaf, 1 | UnixCloseOnExec | UnixNoFollow | UnixCreate | UnixExclusive,
                permissions), path);
        try
        {
            return new ArtifactFileStream(handle, FileAccess.Write, path);
        }
        catch
        {
            handle.Dispose();
            throw;
        }
    }

    public void Move(string temporaryLeaf, string destinationLeaf)
    {
        ValidateLeaf(temporaryLeaf);
        ValidateLeaf(destinationLeaf);
        if (OperatingSystem.IsWindows())
        {
            File.Move(Path.Combine(FullPath, temporaryLeaf), Path.Combine(FullPath, destinationLeaf), overwrite: true);
        }
        else if (RenameAt(DirectoryHandle, temporaryLeaf, DirectoryHandle, destinationLeaf) != 0)
        {
            throw NativeError(destinationLeaf);
        }
    }

    public void Delete(string leaf)
    {
        ValidateLeaf(leaf);
        if (OperatingSystem.IsWindows())
        {
            File.Delete(Path.Combine(FullPath, leaf));
        }
        else if (UnlinkAt(DirectoryHandle, leaf, 0) != 0 && Marshal.GetLastPInvokeError() != 2)
        {
            throw NativeError(leaf);
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        for (var index = _directories.Count - 1; index >= 0; index--)
        {
            _directories[index].Dispose();
        }
    }

    private void ValidateLeaf(string leaf)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (string.IsNullOrEmpty(leaf) || leaf is "." or ".."
            || leaf.IndexOfAny(['/', '\0']) >= 0
            || (OperatingSystem.IsWindows() && (leaf.IndexOfAny(['\\', ':']) >= 0 || leaf.EndsWith('.') || leaf.EndsWith(' '))))
        {
            throw new ArgumentException("Artifact IO requires a single file name.", nameof(leaf));
        }
    }

    private static unsafe string GetFinalWindowsPath(SafeFileHandle handle)
    {
        var length = GetFinalPathName(handle, null, 0, 0);
        if (length == 0)
        {
            throw NativeError("trusted artifact root");
        }

        while (true)
        {
            var buffer = new char[checked((int) length)];
            fixed (char* pointer = buffer)
            {
                var written = GetFinalPathName(handle, pointer, length, 0);
                if (written == 0)
                {
                    throw NativeError("trusted artifact root");
                }

                if (written < length)
                {
                    return new string(buffer, 0, checked((int) written));
                }

                length = written;
            }
        }
    }

    private static SafeFileHandle OpenWindowsDirectory(string path)
    {
        var handle = CheckHandle(CreateFile(path, 0x80, 1, 0, 3,
            BackupSemantics | OpenReparsePoint, 0), path, directory: true);
        try
        {
            var attributes = File.GetAttributes(handle);
            if ((attributes & FileAttributes.Directory) == 0
                || (attributes & FileAttributes.ReparsePoint) != 0)
            {
                throw new IOException($"Artifact directory '{path}' cannot be a link or a file.");
            }

            return handle;
        }
        catch
        {
            handle.Dispose();
            throw;
        }
    }

    private static SafeFileHandle CheckHandle(SafeFileHandle handle, string path, bool directory = false)
    {
        if (!handle.IsInvalid)
        {
            return handle;
        }

        var error = Marshal.GetLastPInvokeError();
        handle.Dispose();
        throw OpenError(path, error, directory);
    }

    private static SafeFileHandle CheckHandle(int descriptor, string path, bool directory = false)
    {
        // open/openat return int, not a pointer-sized HANDLE. In particular, sign-extend
        // -1 explicitly instead of interpreting unspecified upper return-register bits.
        if (descriptor == -1)
        {
            throw OpenError(path, Marshal.GetLastPInvokeError(), directory);
        }

        return new SafeFileHandle(descriptor, ownsHandle: true);
    }

    private static Exception OpenError(string path, int error, bool directory)
    {
        if (error == 2 || (OperatingSystem.IsWindows() && error == 3))
        {
            return directory ? new DirectoryNotFoundException($"Artifact directory '{path}' was not found.")
                : new FileNotFoundException("Artifact file was not found.", path);
        }

        return NativeError(path, error);
    }

    private static IOException NativeError(string path, int? error = null)
    {
        var exception = new Win32Exception(error ?? Marshal.GetLastPInvokeError());
        return new IOException($"Cannot access artifact path '{path}': {exception.Message}", exception);
    }

    // Linux arm64 deliberately uses different directory/no-follow bits from x64.
    private static int UnixNoFollow => OperatingSystem.IsMacOS() ? 0x100
        : RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? 0x8000 : 0x20000;

    private static int UnixCloseOnExec => OperatingSystem.IsMacOS() ? 0x1000000 : 0x80000;

    private static int UnixNonBlocking => OperatingSystem.IsMacOS() ? 4 : 0x800;

    private static int UnixCreate => OperatingSystem.IsMacOS() ? 0x200 : 0x40;

    private static int UnixExclusive => OperatingSystem.IsMacOS() ? 0x800 : 0x80;

    private static int UnixDirectoryFlags => UnixCloseOnExec | UnixNoFollow
        // O_SEARCH on macOS and O_PATH on Linux bind directories without requiring
        // read permission. O_DIRECTORY also rejects a symlink opened with O_PATH.
        | (OperatingSystem.IsMacOS() ? 0x40100000
            : 0x200000 | (RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? 0x4000 : 0x10000));

    private static int CreateAt(SafeFileHandle directory, string leaf, int flags, uint mode)
    {
        // Apple arm64 passes variadic arguments on the stack. Fill the unused integer
        // registers so the openat mode argument occupies its required stack slot.
        return OperatingSystem.IsMacOS() && RuntimeInformation.ProcessArchitecture == Architecture.Arm64
            ? OpenAtMacArm64(directory, leaf, flags, 0, 0, 0, 0, 0, mode)
            : OpenAtWithMode(directory, leaf, flags, mode);
    }

    [LibraryImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static unsafe partial uint GetFinalPathName(SafeFileHandle handle, char* buffer, uint length, uint flags);

    [LibraryImport("kernel32.dll", EntryPoint = "CreateFileW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static partial SafeFileHandle CreateFile(string path, uint access, uint share, nint security, uint disposition, uint flags, nint template);

    [LibraryImport("libc", EntryPoint = "open", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int OpenUnix(string path, int flags);

    [LibraryImport("libc", EntryPoint = "openat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int OpenAt(SafeFileHandle directory, string leaf, int flags);

    [LibraryImport("libc", EntryPoint = "openat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int OpenAtWithMode(SafeFileHandle directory, string leaf, int flags, uint mode);

    [LibraryImport("libc", EntryPoint = "openat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int OpenAtMacArm64(SafeFileHandle directory, string leaf, int flags,
        nint unused3, nint unused4, nint unused5, nint unused6, nint unused7, uint mode);

    [LibraryImport("libc", EntryPoint = "fcntl", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SetFileFlags(SafeFileHandle handle, int command, int flags);

    // fcntl's third argument is variadic, so Apple arm64 requires a stack slot.
    [LibraryImport("libc", EntryPoint = "fcntl", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SetFileFlagsMacArm64(SafeFileHandle handle, int command,
        nint unused2, nint unused3, nint unused4, nint unused5, nint unused6, nint unused7, int flags);

    [LibraryImport("libc", EntryPoint = "mkdirat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int MakeDirectoryAt(SafeFileHandle directory, string leaf, uint mode);

    [LibraryImport("libc", EntryPoint = "renameat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int RenameAt(SafeFileHandle sourceDirectory, string source, SafeFileHandle destinationDirectory, string destination);

    [LibraryImport("libc", EntryPoint = "unlinkat", StringMarshalling = StringMarshalling.Utf8, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int UnlinkAt(SafeFileHandle directory, string leaf, int flags);

    // Handle-based FileStream constructors otherwise report "[Unknown]". Preserve the
    // ordinary stream name for store diagnostics and temporary-archive cleanup checks.
    private sealed class ArtifactFileStream(SafeFileHandle handle, FileAccess access, string path) : FileStream(handle, access)
    {
        public override string Name => path;
    }
}
