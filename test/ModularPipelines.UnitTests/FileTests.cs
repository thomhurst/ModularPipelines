using System.Globalization;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using TUnit.Assertions.Extensions.Numbers;
using TUnit.Assertions.Extensions.Throws;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class FileTests : TestBase
{
    [Test]
    public async Task Delete()
    {
        var file = await CreateRandomFile();
        await Assert.That(file.Exists).IsTrue();

        file.Delete();
        await Assert.That(file.Exists).IsFalse();
    }

    [Test]
    public async Task MoveTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Exists).IsTrue();
            await Assert.That(file2.Exists).IsFalse();
        }

        file.MoveTo(file2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(new File(file.OriginalPath).Exists).IsFalse();

            await Assert.That(file.Exists).IsTrue();
            await Assert.That(file2.Exists).IsTrue();
        }
    }

    [Test]
    public async Task Data_Is_Populated()
    {
        var file = await CreateRandomFile();
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Exists).IsTrue();
            await Assert.That(file.Attributes.ToString()).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.Path).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.OriginalPath).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.Extension).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.Folder?.ToString()).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.CreationTime.ToString(CultureInfo.InvariantCulture)).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture)).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.Hidden).IsFalse();
            await Assert.That(file.Name).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.NameWithoutExtension).IsNotNull().And.IsNotEmpty();
            await Assert.That(file.IsReadOnly).IsFalse();
        }
    }

    [Test]
    public async Task CopyTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Exists).IsTrue();
            await Assert.That(file2.Exists).IsFalse();
        }

        file.CopyTo(file2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Exists).IsTrue();
            await Assert.That(file2.Exists).IsTrue();
        }
    }

    [Test]
    public async Task CreateFile()
    {
        var file = File.GetNewTemporaryFilePath();
        await Assert.That(file.Exists).IsFalse();

        file.Create();
        await Assert.That(file.Exists).IsTrue();
    }

    [Test]
    public async Task ReadEmptyFile()
    {
        var file = File.GetNewTemporaryFilePath();

        file.Create();

        var plainText = await file.ReadAsync();
        var lines = await file.ReadLinesAsync();
        var bytes = await file.ReadBytesAsync();
        var stream = await file.GetStream().ToMemoryStreamAsync();

        await using (Assert.Multiple())
        {
            await Assert.That(plainText).IsEmpty();
            await Assert.That(lines).IsEmpty();
            await Assert.That(bytes).IsEmpty();
            await Assert.That(stream.Length).IsZero();
        }
    }

    [Test]
    public async Task ReadWriteFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync($"Hello{Environment.NewLine}world");

        var plainText = await file.ReadAsync();
        var lines = await file.ReadLinesAsync();
        var bytes = await file.ReadBytesAsync();
        var stream = await file.GetStream().ToMemoryStreamAsync();

        await using (Assert.Multiple())
        {
            await Assert.That(plainText).IsEqualTo($"Hello{Environment.NewLine}world");
            await Assert.That(lines).HasCount().EqualTo(2);
            await Assert.That(lines[0]).IsEqualTo("Hello");
            await Assert.That(lines[1]).IsEqualTo("world");
            await Assert.That(bytes).IsNotEmpty();
            await Assert.That(stream.Length).IsNotZero();
        }
    }

    [Test]
    public async Task ReadWriteLinesFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new[]
        {
            "Hello",
            "world",
        });

        var plainText = await file.ReadAsync();
        var lines = await file.ReadLinesAsync();
        
        await using (Assert.Multiple())
        {
            await Assert.That(plainText).IsEqualTo($"Hello{Environment.NewLine}world{Environment.NewLine}");
            await Assert.That(lines).HasCount().EqualTo(2);
            await Assert.That(lines[0]).IsEqualTo("Hello");
            await Assert.That(lines[1]).IsEqualTo("world");
        }
    }

    [Test]
    public async Task ReadWriteBytesFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync("Hello world!"u8.ToArray());

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).IsEqualTo("Hello world!");
    }

    [Test]
    public async Task WriteStreamFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new MemoryStream("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).IsEqualTo("Hello world!");
    }

    [Test]
    public async Task ReadWriteReadOnlyMemoryBytesFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new ReadOnlyMemory<byte>("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).IsEqualTo("Hello world!");
    }

    [Test]
    public async Task ReadWriteStreamFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new MemoryStream("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).IsEqualTo("Hello world!");
    }

    [Test]
    public async Task Null_FileInfo_Implicit_Cast()
    {
        FileInfo? fileInfo = null;

        File? file = fileInfo;
        await Assert.That(file).IsNull();
    }

    [Test]
    public async Task Null_String_Implicit_Cast()
    {
        string? fileInfo = null;

        File? file = fileInfo;
        await Assert.That(file).IsNull();
    }

    [Test]
    public async Task FileInfo_Implicit_Cast()
    {
        var fileInfo = new FileInfo(Path.GetTempFileName());

        File file = fileInfo;
        await Assert.That(file).IsNotNull();
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        var fileInfo = Path.GetTempFileName();

        File file = fileInfo!;
        await Assert.That(file).IsNotNull();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Attributes()
    {
        var file = await CreateRandomFile();
        await Assert.That(file.Attributes.HasFlag(FileAttributes.Hidden)).IsFalse();

        file.Attributes = FileAttributes.Hidden;
        await Assert.That(file.Attributes.HasFlag(FileAttributes.Hidden)).IsTrue();
    }

    [Test]
    public async Task EqualityTrue()
    {
        var path = Path.GetRandomFileName();
        var file = new File(path);
        var file2 = new File(path);
        
        await using (Assert.Multiple())
        {
            await Assert.That(file).IsEqualTo(file2);
            await Assert.That(file.GetHashCode()).IsEqualTo(file2.GetHashCode());
            await Assert.That(file == file2).IsTrue();
            await Assert.That(file != file2).IsFalse();
        }
    }

    [Test]
    public async Task EqualityFalse()
    {
        var file = new File(Path.GetRandomFileName());
        var file2 = new File(Path.GetRandomFileName());
        
        await using (Assert.Multiple())
        {
            await Assert.That(file).IsNotEqualTo(file2);
            await Assert.That(file.GetHashCode()).IsNotEqualTo(file2.GetHashCode());
            await Assert.That(file == file2).IsFalse();
            await Assert.That(file != file2).IsTrue();
        }
    }

    [Test]
    [Arguments("**/Nest2/**/*.txt")]
    [Arguments("**/blah.txt")]
    [Arguments("**/Blah.txt")]
    [Arguments("**/Nest1/Nest2/Nest3/Nest4/Nest5/*.txt")]
    public async Task GlobTests(string globPattern)
    {
        var workingDirectory = new Folder(TestContext.OutputDirectory).AssertExists();
        var files = workingDirectory.GetFiles(globPattern).ToList();
        await Assert.That(files).HasCount().EqualTo(1);
        await Assert.That(files[0].Name).IsEqualTo("Blah.txt");
    }

    [Test]
    public async Task GlobTest2()
    {
        var folder = new Folder(TestContext.OutputDirectory)
            .AssertExists()
            .FindFolder(x => x.Name == "Nest5")!;

        var files = folder.GetFiles("Blah.txt").ToList();
        await Assert.That(files).HasCount().EqualTo(1);
        await Assert.That(files[0].Name).IsEqualTo("Blah.txt");
    }

    [Test]
    public async Task AssertExists()
    {
        var file = (File?) await CreateRandomFile();
        await Assert.That(() => file.AssertExists()).ThrowsNothing();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNotExists()
    {
        var file = File.GetNewTemporaryFilePath();
        await Assert.That(() => file.AssertExists()).ThrowsException().OfType<FileNotFoundException>();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNull()
    {
        var file = null as File;
        await Assert.That(() => file.AssertExists()).ThrowsException().OfType<FileNotFoundException>();
    }

    [Test]
    public async Task MoveTo_Folder()
    {
        var file = await CreateRandomFile();
        file.MoveTo(new Folder(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
    }
    
    [Test]
    public async Task CopyTo_Folder()
    {
        var file = await CreateRandomFile();
        file.CopyTo(new Folder(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}