using System.Globalization;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Git;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using File = System.IO.File;

namespace ModularPipelines.UnitTests;

public class FolderTests : TestBase
{
    [Test]
    public async Task CleanFiles()
    {
        var folder = CreateRandomFolder();

        foreach (var fileName in Enumerable.Range(0, 10)
                     .Select(x => Guid.NewGuid().ToString("N") + ".txt"))
        {
            await File.WriteAllTextAsync(Path.Combine(folder, fileName), "Foo bar!");
        }

        Assert.That(folder.ListFiles().ToList(), Has.Count.EqualTo(10));

        folder.Clean();

        Assert.That(folder.ListFiles().ToList(), Has.Count.EqualTo(0));
    }

    [Test]
    public void CleanFolders()
    {
        var folder = CreateRandomFolder();

        foreach (var folderName in Enumerable.Range(0, 10)
                     .Select(x => Guid.NewGuid().ToString("N")))
        {
            Directory.CreateDirectory(Path.Combine(folder, folderName));
        }

        Assert.That(folder.ListFolders().ToList(), Has.Count.EqualTo(10));

        folder.Clean();

        Assert.That(folder.ListFolders().ToList(), Has.Count.EqualTo(0));
    }

    [Test]
    public async Task FindFile()
    {
        var git = await GetService<IGit>();

        var readme = git.RootDirectory.FindFile(x => x.Name == "README.md");

        Assert.That(readme, Is.Not.Null);
        Assert.That(readme!.Exists, Is.True);
    }

    [Test]
    public async Task FindFolder()
    {
        var git = await GetService<IGit>();

        var src = git.RootDirectory.FindFolder(x => x.Name == "src");

        Assert.That(src, Is.Not.Null);
        Assert.That(src!.Exists, Is.True);
    }

    [Test]
    public void Delete()
    {
        var folder = CreateRandomFolder();

        Assert.That(folder.Exists, Is.True);

        folder.Delete();

        Assert.That(folder.Exists, Is.False);
    }

    [Test]
    public async Task MoveTo()
    {
        var folder = CreateRandomFolder();

        var folder2 = new Folder(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        foreach (var fileName in Enumerable.Range(0, 10)
                     .Select(x => Guid.NewGuid().ToString("N") + ".txt"))
        {
            await File.WriteAllTextAsync(Path.Combine(folder, fileName), "Foo bar!");
        }

        Assert.Multiple(() =>
        {
            Assert.That(folder.Exists, Is.True);
            Assert.That(folder.ListFiles().ToList(), Has.Count.EqualTo(10));
            Assert.That(folder2.Exists, Is.False);
        });

        folder.MoveTo(folder2);

        Assert.Multiple(() =>
        {
            Assert.That(new Folder(folder.OriginalPath).Exists, Is.False);
            Assert.That(folder.Exists, Is.True);
            Assert.That(folder2.Exists, Is.True);
            Assert.That(folder2.ListFiles().ToList(), Has.Count.EqualTo(10));
        });
    }

    [Test]
    public async Task CopyTo()
    {
        var folder = CreateRandomFolder();

        var folder2 = new Folder(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));

        foreach (var fileName in Enumerable.Range(0, 10)
                     .Select(x => Guid.NewGuid().ToString("N") + ".txt"))
        {
            await File.WriteAllTextAsync(Path.Combine(folder, fileName), "Foo bar!");
        }

        Assert.Multiple(() =>
        {
            Assert.That(folder.Exists, Is.True);
            Assert.That(folder.ListFiles().ToList(), Has.Count.EqualTo(10));
            Assert.That(folder2.Exists, Is.False);
        });

        folder.CopyTo(folder2);

        Assert.Multiple(() =>
        {
            Assert.That(folder.Exists, Is.True);
            Assert.That(folder.ListFiles().ToList(), Has.Count.EqualTo(10));
            Assert.That(folder2.Exists, Is.True);
            Assert.That(folder2.ListFiles().ToList(), Has.Count.EqualTo(10));
        });
    }

    [Test]
    public void Data_Is_Populated()
    {
        var folder = CreateRandomFolder();

        Assert.Multiple(() =>
        {
            Assert.That(folder.Exists, Is.True);
            Assert.That(folder.Attributes.ToString(), Is.Not.Null.Or.Empty);
            Assert.That(folder.Path, Is.Not.Null.Or.Empty);
            Assert.That(folder.OriginalPath, Is.Not.Null.Or.Empty);
            Assert.That(folder.Extension, Is.Not.Null.Or.Empty);
            Assert.That(folder.Parent?.ToString(), Is.Not.Null.Or.Empty);
            Assert.That(folder.Root.ToString(), Is.Not.Null.Or.Empty);
            Assert.That(folder.CreationTime.ToString(CultureInfo.InvariantCulture), Is.Not.Null.Or.Empty);
            Assert.That(folder.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture), Is.Not.Null.Or.Empty);
            Assert.That(folder.Hidden, Is.False);
            Assert.That(folder.Name, Is.Not.Null.Or.Empty);
        });
    }

    [Test]
    public void CreateFolder()
    {
        var folder = new Folder(Path.GetRandomFileName());

        Assert.That(folder.Exists, Is.False);

        folder.Create();

        Assert.That(folder.Exists, Is.True);
    }

    [Test]
    public void CreateFile()
    {
        var folder = CreateRandomFolder();

        var fileBeforeCreation = folder.GetFile("Foo.txt");

        Assert.That(fileBeforeCreation.Exists, Is.False);

        var file = folder.CreateFile("Foo.txt");

        Assert.That(file.Exists, Is.True);
    }

    [Test]
    public void CreateSubfolder()
    {
        var folder = CreateRandomFolder();

        var folderBeforeCreation = folder.GetFolder("Foo");

        Assert.That(folderBeforeCreation.Exists, Is.False);

        var subfolder = folder.CreateFolder("Foo");

        Assert.Multiple(() =>
        {
            Assert.That(subfolder.Exists, Is.True);
            Assert.That(subfolder.Path, Is.Not.EqualTo(folder.Path));
            Assert.That(subfolder.Parent, Is.EqualTo(folder));
        });
    }

    [Test]
    public void Null_FileInfo_Implicit_Cast()
    {
        DirectoryInfo? directoryInfo = null;

        Folder? file = directoryInfo;

        Assert.That(file, Is.Null);
    }

    [Test]
    public void Null_String_Implicit_Cast()
    {
        string? fileInfo = null;

        Folder? file = fileInfo;

        Assert.That(file, Is.Null);
    }

    [Test]
    public void FileInfo_Implicit_Cast()
    {
        var directoryInfo = new DirectoryInfo(Path.GetTempFileName());

        Folder file = directoryInfo;

        Assert.That(file, Is.Not.Null);
    }

    [Test]
    public void String_Implicit_Cast()
    {
        var path = Path.GetTempFileName();

        Folder file = path!;

        Assert.That(file, Is.Not.Null);
    }

    [Test]
    [WindowsOnlyTest]
    public void Attributes()
    {
        var folder = CreateRandomFolder();

        Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden), Is.False);

        folder.Attributes = FileAttributes.Hidden;

        Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden), Is.True);
    }

    [Test]
    public void EqualityTrue()
    {
        var path = Path.GetRandomFileName();
        var folder = new Folder(path);
        var folder2 = new Folder(path);

        Assert.Multiple(() =>
        {
            Assert.That(folder, Is.EqualTo(folder2));
            Assert.That(folder.GetHashCode(), Is.EqualTo(folder2.GetHashCode()));
            Assert.That(folder == folder2, Is.True);
            Assert.That(folder != folder2, Is.False);
        });
    }

    [Test]
    public void EqualityFalse()
    {
        var folder = new Folder(Path.GetRandomFileName());
        var folder2 = new Folder(Path.GetRandomFileName());

        Assert.Multiple(() =>
        {
            Assert.That(folder, Is.Not.EqualTo(folder2));
            Assert.That(folder.GetHashCode(), Is.Not.EqualTo(folder2.GetHashCode()));
            Assert.That(folder == folder2, Is.False);
            Assert.That(folder != folder2, Is.True);
        });
    }

    [Test]
    public void AssertExists()
    {
        var folder = (Folder?) CreateRandomFolder();

        Assert.DoesNotThrow(() => folder.AssertExists());
    }

    [Test]
    public void AssertExists_ThrowsWhenNotExists()
    {
        Folder folder = ModularPipelines.FileSystem.File.GetNewTemporaryFilePath().Path!;

        Assert.Throws<DirectoryNotFoundException>(() => folder.AssertExists());
    }

    [Test]
    public void AssertExists_ThrowsWhenNull()
    {
        var folder = null as Folder;

        Assert.Throws<DirectoryNotFoundException>(() => folder.AssertExists());
    }

    private static Folder CreateRandomFolder()
    {
        var tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempFolderPath);

        return new Folder(tempFolderPath);
    }
}