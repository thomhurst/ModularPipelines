using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class PortablePathConverterTests
{
    [Test]
    public async Task File_Serializes_As_Relative_Path()
    {
        var gitRoot = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar);
        var options = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(gitRoot) },
        };

        var absolutePath = Path.Combine(gitRoot, "src", "MyProject", "File.cs");
        var file = new File(absolutePath);

        var json = JsonSerializer.Serialize(file, options);

        // Should be relative with forward slashes
        await Assert.That(json).IsEqualTo("\"src/MyProject/File.cs\"");
    }

    [Test]
    public async Task File_Deserializes_Relative_Path_Against_Local_Git_Root()
    {
        var gitRoot = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar);
        var options = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(gitRoot) },
        };

        var json = "\"src/MyProject/File.cs\"";
        var file = JsonSerializer.Deserialize<File>(json, options);

        var expected = Path.GetFullPath(Path.Combine(gitRoot, "src", "MyProject", "File.cs"));
        await Assert.That(file).IsNotNull();
        await Assert.That(file!.Path).IsEqualTo(expected);
    }

    [Test]
    public async Task File_Roundtrip_Cross_Platform_Simulation()
    {
        // Simulate: serialized on Windows with root D:\a\Repo, deserialized on Linux with root /home/runner/Repo
        var windowsRoot = OperatingSystem.IsWindows()
            ? @"D:\a\Repo"
            : "/tmp/windows-sim";
        var linuxRoot = OperatingSystem.IsWindows()
            ? @"C:\tmp\linux-sim"
            : "/home/runner/Repo";

        var serializeOptions = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(windowsRoot) },
        };
        var deserializeOptions = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(linuxRoot) },
        };

        // Create a file path under the "windows" root
        var windowsFilePath = Path.Combine(windowsRoot, "src", "Project.csproj");
        var file = new File(windowsFilePath);

        // Serialize (produces relative path)
        var json = JsonSerializer.Serialize(file, serializeOptions);
        await Assert.That(json).IsEqualTo("\"src/Project.csproj\"");

        // Deserialize on "linux" side (resolves against linux root)
        var deserialized = JsonSerializer.Deserialize<File>(json, deserializeOptions);
        var expected = Path.GetFullPath(Path.Combine(linuxRoot, "src", "Project.csproj"));
        await Assert.That(deserialized).IsNotNull();
        await Assert.That(deserialized!.Path).IsEqualTo(expected);
    }

    [Test]
    public async Task File_Outside_Git_Root_Serializes_As_Absolute()
    {
        var gitRoot = Path.Combine(Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar), "repo");
        var options = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(gitRoot) },
        };

        // Path outside the git root
        var outsidePath = Path.Combine(Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar), "other", "file.txt");
        var file = new File(outsidePath);

        var json = JsonSerializer.Serialize(file, options);

        // Should contain the absolute path since it's outside git root
        await Assert.That(json).Contains("other");
        await Assert.That(json).Contains("file.txt");
    }

    [Test]
    public async Task Null_File_Serializes_As_Null()
    {
        var gitRoot = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar);
        var options = new JsonSerializerOptions
        {
            Converters = { new PortableFilePathJsonConverter(gitRoot) },
        };

        var json = JsonSerializer.Serialize<File?>(null, options);
        await Assert.That(json).IsEqualTo("null");

        var deserialized = JsonSerializer.Deserialize<File?>(json, options);
        await Assert.That(deserialized).IsNull();
    }
}
