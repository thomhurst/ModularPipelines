using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class WindowsCommandResolverTests
{
    [Test]
    [Arguments("tool", ".exe")]
    [Arguments("tool", ".cmd")]
    [Arguments("tool.exe", ".exe")]
    [Arguments("tool.cmd", ".cmd")]
    public async Task Bare_Command_Uses_Path_Instead_Of_Working_Directory(string command, string extension)
    {
        var directory = Directory.CreateTempSubdirectory("mp resolver tests ");
        var pathDirectory = Directory.CreateDirectory(Path.Combine(directory.FullName, "bin"));
        var fileName = "tool" + extension;
        var trustedCommand = Path.Combine(pathDirectory.FullName, fileName);

        try
        {
            await File.WriteAllBytesAsync(Path.Combine(directory.FullName, fileName), []);
            await File.WriteAllBytesAsync(trustedCommand, []);

            var resolved = WindowsCommandResolver.Resolve(
                command,
                directory.FullName,
                pathDirectory.FullName,
                string.Join(Path.PathSeparator, ".EXE", ".CMD"),
                isWindows: true);

            await Assert.That(resolved).IsEqualTo(trustedCommand);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("tool", ".exe")]
    [Arguments("tool", ".cmd")]
    [Arguments("tool.exe", ".exe")]
    [Arguments("tool.cmd", ".cmd")]
    public async Task Bare_Command_Only_In_Working_Directory_Is_Not_Resolved(string command, string extension)
    {
        var directory = Directory.CreateTempSubdirectory("mp resolver tests ");

        try
        {
            await File.WriteAllBytesAsync(Path.Combine(directory.FullName, "tool" + extension), []);

            var resolved = WindowsCommandResolver.Resolve(
                command,
                directory.FullName,
                searchPath: string.Empty,
                pathExtensions: string.Join(Path.PathSeparator, ".EXE", ".CMD"),
                isWindows: true);

            await Assert.That(resolved).IsNull();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(".exe", true)]
    [Arguments(".exe", false)]
    [Arguments(".cmd", true)]
    [Arguments(".cmd", false)]
    public async Task Explicit_Relative_Command_Resolves_From_Working_Directory(string extension, bool includeExtension)
    {
        var directory = Directory.CreateTempSubdirectory("mp resolver tests ");
        var commandPath = Path.Combine(directory.FullName, "tool" + extension);

        try
        {
            await File.WriteAllBytesAsync(commandPath, []);

            var resolved = WindowsCommandResolver.Resolve(
                Path.Combine(".", includeExtension ? "tool" + extension : "tool"),
                directory.FullName,
                searchPath: string.Empty,
                pathExtensions: string.Join(Path.PathSeparator, ".EXE", ".CMD"),
                isWindows: true);

            await Assert.That(resolved).IsEqualTo(commandPath);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
