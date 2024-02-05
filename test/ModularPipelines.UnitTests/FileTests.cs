using System.Globalization;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using TUnit.Assertions;
using TUnit.Core;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class FileTests : TestBase
{
    [Test]
    public async Task Delete()
    {
        var file = await CreateRandomFile();
        await Assert.That(file.Exists).Is.True();

        file.Delete();
        await Assert.That(file.Exists).Is.False();
    }

    [Test]
    public async Task MoveTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        await Assert.Multiple(() =>
        {
            Assert.That(file.Exists).Is.True();
            Assert.That(file2.Exists).Is.False();
        });

        file.MoveTo(file2);
        await Assert.Multiple(() =>
        {
            Assert.That(new File(file.OriginalPath).Exists).Is.False();

            Assert.That(file.Exists).Is.True();
            Assert.That(file2.Exists).Is.True();
        });
    }

    [Test]
    public async Task Data_Is_Populated()
    {
        var file = await CreateRandomFile();
        await Assert.Multiple(() =>
        {
            Assert.That(file.Exists).Is.True();
            Assert.That(file.Attributes.ToString()).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.Path).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.OriginalPath).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.Extension).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.Folder.ToString()).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.CreationTime.ToString(CultureInfo.InvariantCulture)).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture)).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.Hidden).Is.False();
            Assert.That(file.Name).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.NameWithoutExtension).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(file.IsReadOnly).Is.False();
        });
    }

    [Test]
    public async Task CopyTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        await Assert.Multiple(() =>
        {
            Assert.That(file.Exists).Is.True();
            Assert.That(file2.Exists).Is.False();
        });

        file.CopyTo(file2);
        await Assert.Multiple(() =>
        {
            Assert.That(file.Exists).Is.True();
            Assert.That(file2.Exists).Is.True();
        });
    }

    [Test]
    public void CreateFile()
    {
        var file = File.GetNewTemporaryFilePath();
        await Assert.That(file.Exists).Is.False();

        file.Create();
        await Assert.That(file.Exists).Is.True();
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

        Assert.Multiple(() =>
        {
            Assert.That(plainText).Is.Empty();
            Assert.That(lines).Is.Empty();
            Assert.That(bytes).Is.Empty();
            Assert.That(stream.Length).Is.Zero();
        });
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

        Assert.Multiple(() =>
        {
            Assert.That(plainText).Is.EqualTo($"Hello{Environment.NewLine}world");
            Assert.That(lines).Has.Count().EqualTo(2);
            Assert.That(lines[0]).Is.EqualTo("Hello");
            Assert.That(lines[1]).Is.EqualTo("world");
            Assert.That(bytes).Is.Not.Empty();
            Assert.That(stream.Length).Is.Not.Zero();
        });
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
        await Assert.Multiple(() =>
        {
            Assert.That(plainText).Is.EqualTo($"Hello{Environment.NewLine}world{Environment.NewLine}");
            Assert.That(lines).Has.Count().EqualTo(2);
            Assert.That(lines[0]).Is.EqualTo("Hello");
            Assert.That(lines[1]).Is.EqualTo("world");
        });
    }

    [Test]
    public async Task ReadWriteBytesFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync("Hello world!"u8.ToArray());

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).Is.EqualTo("Hello world!");
    }

    [Test]
    public async Task WriteStreamFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new MemoryStream("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).Is.EqualTo("Hello world!");
    }

    [Test]
    public async Task ReadWriteReadOnlyMemoryBytesFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new ReadOnlyMemory<byte>("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).Is.EqualTo("Hello world!");
    }

    [Test]
    public async Task ReadWriteStreamFile()
    {
        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(new MemoryStream("Hello world!"u8.ToArray()));

        var plainText = await file.ReadAsync();
        await Assert.That(plainText).Is.EqualTo("Hello world!");
    }

    [Test]
    public void Null_FileInfo_Implicit_Cast()
    {
        FileInfo? fileInfo = null;

        File? file = fileInfo;
        await Assert.That(file).Is.Null();
    }

    [Test]
    public void Null_String_Implicit_Cast()
    {
        string? fileInfo = null;

        File? file = fileInfo;
        await Assert.That(file).Is.Null();
    }

    [Test]
    public void FileInfo_Implicit_Cast()
    {
        var fileInfo = new FileInfo(Path.GetTempFileName());

        File file = fileInfo;
        await Assert.That(file).Is.Not.Null();
    }

    [Test]
    public void String_Implicit_Cast()
    {
        var fileInfo = Path.GetTempFileName();

        File file = fileInfo!;
        await Assert.That(file).Is.Not.Null();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Attributes()
    {
        var file = await CreateRandomFile();
        await Assert.That(file.Attributes.HasFlag(FileAttributes.Hidden)).Is.False();

        file.Attributes = FileAttributes.Hidden;
        await Assert.That(file.Attributes.HasFlag(FileAttributes.Hidden)).Is.True();
    }

    [Test]
    public void EqualityTrue()
    {
        var path = Path.GetRandomFileName();
        var file = new File(path);
        var file2 = new File(path);
        await Assert.Multiple(() =>
        {
            Assert.That(file).Is.EqualTo(file2);
            Assert.That(file.GetHashCode()).Is.EqualTo(file2.GetHashCode());
            Assert.That(file == file2).Is.True();
            Assert.That(file != file2).Is.False();
        });
    }

    [Test]
    public void EqualityFalse()
    {
        var file = new File(Path.GetRandomFileName());
        var file2 = new File(Path.GetRandomFileName());
        await Assert.Multiple(() =>
        {
            Assert.That(file).Is.Not.EqualTo(file2);
            Assert.That(file.GetHashCode()).Is.Not.EqualTo(file2.GetHashCode());
            Assert.That(file == file2).Is.False();
            Assert.That(file != file2).Is.True();
        });
    }

    [TestWithData("**/Nest2/**/*.txt")]
    [TestWithData("**/blah.txt")]
    [TestWithData("**/Blah.txt")]
    [TestWithData("**/Nest1/Nest2/Nest3/Nest4/Nest5/*.txt")]
    public void GlobTests(string globPattern)
    {
        var workingDirectory = new Folder(Environment.CurrentDirectory);
        var files = workingDirectory.GetFiles(globPattern).ToList();
        await Assert.That(files).Has.Count().EqualTo(1);
        Assert.That(files[0].Name).Is.EqualTo("Blah.txt");
    }

    [Test]
    public void GlobTest2()
    {
        var folder = new Folder(Environment.CurrentDirectory)
            .FindFolder(x => x.Name == "Nest5")!;

        var files = folder.GetFiles("Blah.txt").ToList();

        Assert.That(files).Has.Count().EqualTo(1);
        Assert.That(files[0].Name).Is.EqualTo("Blah.txt");
    }

    [Test]
    public async Task AssertExists()
    {
        var file = (File?) await CreateRandomFile();
        await Assert.That(() => file.AssertExists()).Throws.Nothing();
    }

    [Test]
    public void AssertExists_ThrowsWhenNotExists()
    {
        var file = File.GetNewTemporaryFilePath();
        await Assert.That(() => file.AssertExists()).Throws.TypeOf<FileNotFoundException>();
    }

    [Test]
    public void AssertExists_ThrowsWhenNull()
    {
        var file = null as File;
        await Assert.That(() => file.AssertExists()).Throws.TypeOf<FileNotFoundException>();
    }

    [Test]
    public async Task MoveTo_Folder()
    {
        var file = await CreateRandomFile();
        file.MoveTo(new Folder(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}