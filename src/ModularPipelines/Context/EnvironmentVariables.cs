using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace ModularPipelines.Context;

internal partial class EnvironmentVariables : IEnvironmentVariablesContext
{
    private const string PathVariableName = "PATH";

    private static readonly Lock PathLock = new();

    private static StringComparer Comparer => OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;

    public string? Get(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public IReadOnlyDictionary<string, string?> GetAll(
        EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariables(target)
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value?.ToString(), Comparer);
    }

    public void Set(string name, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Get(PathVariableName, target)?.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
    }

    public void AddToPath(string pathToAdd)
    {
        AddToPath(pathToAdd, EnvironmentVariableTarget.Process);
    }

    public void AddToPath(string pathToAdd, EnvironmentVariableTarget target)
    {
        AddToPath(pathToAdd, prepend: false, target);
    }

    public void AddToPath(string pathToAdd, bool prepend, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        ArgumentNullException.ThrowIfNull(pathToAdd);

        lock (PathLock)
        {
            if (target == EnvironmentVariableTarget.Process)
            {
                Set(PathVariableName, CombinePaths(Get(PathVariableName), pathToAdd, prepend));
                return;
            }

            if (target is not (EnvironmentVariableTarget.User or EnvironmentVariableTarget.Machine))
            {
                throw new ArgumentOutOfRangeException(nameof(target), target, "Invalid environment variable target.");
            }

            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException("User and machine PATH updates are only supported on Windows.");
            }

            using (var environmentKey = target == EnvironmentVariableTarget.User
                ? Registry.CurrentUser.CreateSubKey("Environment", writable: true)
                : Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", writable: true))
            {
                if (environmentKey is null)
                {
                    throw new InvalidOperationException("The Windows environment registry key could not be opened.");
                }

                AddToRegistryPath(environmentKey, pathToAdd, prepend);
            }

            // Match Environment.SetEnvironmentVariable's best-effort notification after persisting the change.
            SendMessageTimeout(new IntPtr(0xffff), 0x001a, UIntPtr.Zero, "Environment", 0, 1000, out _);
        }
    }

    [SupportedOSPlatform("windows")]
    internal static void AddToRegistryPath(RegistryKey environmentKey, string pathToAdd, bool prepend)
    {
        var value = environmentKey.GetValue(PathVariableName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
        var kind = value is null ? RegistryValueKind.ExpandString : environmentKey.GetValueKind(PathVariableName);
        if (kind is not (RegistryValueKind.String or RegistryValueKind.ExpandString))
        {
            throw new InvalidOperationException("The Windows PATH registry value must be a string.");
        }

        environmentKey.SetValue(PathVariableName, CombinePaths(value as string, pathToAdd, prepend), kind);
    }

    private static string CombinePaths(string? existingPath, string pathToAdd, bool prepend)
    {
        var existingPaths = existingPath?.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
        var newPaths = pathToAdd.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
        var paths = prepend ? newPaths.Concat(existingPaths) : existingPaths.Concat(newPaths);
        return string.Join(Path.PathSeparator, paths.Distinct(Comparer));
    }

    [SupportedOSPlatform("windows")]
    [LibraryImport("user32.dll", EntryPoint = "SendMessageTimeoutW", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static partial IntPtr SendMessageTimeout(
        IntPtr window,
        uint message,
        UIntPtr wParam,
        string lParam,
        uint flags,
        uint timeout,
        out UIntPtr result);
}
