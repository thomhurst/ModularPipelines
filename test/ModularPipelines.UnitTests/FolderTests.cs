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

        await Assert.That(folder.ListFiles().ToList()).Has.Count().EqualTo(10);

        folder.Clean();
        await Assert.That(folder.ListFiles().ToList()).Has.Count().EqualTo(0);
    }

    [Test]
    public async Task CleanFolders()
    {
        var folder = CreateRandomFolder();

        foreach (var folderName in Enumerable.Range(0, 10)
                     .Select(x => Guid.NewGuid().ToString("N")))
        {
            Directory.CreateDirectory(Path.Combine(folder, folderName));
        }

        await Assert.That(folder.ListFolders().ToList()).Has.Count().EqualTo(10);

        folder.Clean();
        await Assert.That(folder.ListFolders().ToList()).Has.Count().EqualTo(0);
    }

    [Test]
    public async Task FindFile()
    {
        var git = await GetService<IGit>();

        var readme = git.RootDirectory.FindFile(x => x.Name == "README.md");
        await Assert.That(readme).Is.Not.Null();
        await Assert.That(readme!.Exists).Is.True();
    }

    [Test]
    public async Task FindFolder()
    {
        var git = await GetService<IGit>();

        var src = git.RootDirectory.FindFolder(x => x.Name == "src");
        await Assert.That(src).Is.Not.Null();
        await Assert.That(src!.Exists).Is.True();
    }

    [Test]
    public async Task Delete()
    {
        var folder = CreateRandomFolder();
        await Assert.That(folder.Exists).Is.True();

        folder.Delete();
        await Assert.That(folder.Exists).Is.False();
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

        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).Is.True();
            await Assert.That(folder.ListFiles().ToList()).Has.Count().EqualTo(10);
            await Assert.That(folder2.Exists).Is.False();
        }

        folder.MoveTo(folder2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(new Folder(folder.OriginalPath).Exists).Is.False();
            await Assert.That(folder.Exists).Is.True();
            await Assert.That(folder2.Exists).Is.True();
            await Assert.That(folder2.ListFiles().ToList()).Has.Count().EqualTo(10);
        }
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

        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).Is.True();
            await Assert.That(folder.ListFiles().ToList()).Has.Count().EqualTo(10);
            await Assert.That(folder2.Exists).Is.False();
        }

        folder.CopyTo(folder2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).Is.True();
            await Assert.That(folder.ListFiles().ToList()).Has.Count().EqualTo(10);
            await Assert.That(folder2.Exists).Is.True();
            await Assert.That(folder2.ListFiles().ToList()).Has.Count().EqualTo(10);
        }
    }

    [Test]
    public async Task Data_Is_Populated()
    {
        var folder = CreateRandomFolder();
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).Is.True();
            await Assert.That(folder.Attributes.ToString()).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.Path).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.OriginalPath).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.Extension).Is.Empty();
            await Assert.That(folder.Parent?.ToString()!).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.Root.ToString()).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.CreationTime.ToString(CultureInfo.InvariantCulture)).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture)).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(folder.Hidden).Is.False();
            await Assert.That(folder.Name).Is.Not.Null().And.Is.Not.Empty();
        }
    }

    [Test]
    public async Task CreateFolder()
    {
        var folder = new Folder(Path.GetRandomFileName());

        await Assert.That(folder.Exists).Is.False();

        folder.Create();
        await Assert.That(folder.Exists).Is.True();
    }

    [Test]
    public async Task CreateFile()
    {
        var folder = CreateRandomFolder();

        var fileBeforeCreation = folder.GetFile("Foo.txt");
        await Assert.That(fileBeforeCreation.Exists).Is.False();

        var file = folder.CreateFile("Foo.txt");
        await Assert.That(file.Exists).Is.True();
    }

    [Test]
    public async Task CreateSubfolder()
    {
        var folder = CreateRandomFolder();

        var folderBeforeCreation = folder.GetFolder("Foo");
        await Assert.That(folderBeforeCreation.Exists).Is.False();

        var subfolder = folder.CreateFolder("Foo");
        
        await using (Assert.Multiple())
        {
            await Assert.That(subfolder.Exists).Is.True();
            await Assert.That(subfolder.Path).Is.Not.EqualTo(folder.Path);
            await Assert.That(subfolder.Parent).Is.EqualTo(folder);
        }
    }

    [Test]
    public async Task Null_FileInfo_Implicit_Cast()
    {
        DirectoryInfo? directoryInfo = null;

        Folder? file = directoryInfo;
        await Assert.That(file).Is.Null();
    }

    [Test]
    public async Task Null_String_Implicit_Cast()
    {
        string? fileInfo = null;

        Folder? file = fileInfo;
        await Assert.That(file).Is.Null();
    }

    [Test]
    public async Task FileInfo_Implicit_Cast()
    {
        var directoryInfo = new DirectoryInfo(Path.GetTempFileName());

        Folder file = directoryInfo;
        await Assert.That(file).Is.Not.Null();
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        var path = Path.GetTempFileName();

        Folder file = path!;
        await Assert.That(file).Is.Not.Null();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Attributes()
    {
        var folder = CreateRandomFolder();
        await Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden)).Is.False();

        folder.Attributes = FileAttributes.Hidden;

        await Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden)).Is.True();
    }

    [Test]
    public async Task EqualityTrue()
    {
        var path = Path.GetRandomFileName();
        var folder = new Folder(path);
        var folder2 = new Folder(path);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder).Is.EqualTo(folder2);
            await Assert.That(folder.GetHashCode()).Is.EqualTo(folder2.GetHashCode());
            await Assert.That(folder == folder2).Is.True();
            await Assert.That(folder != folder2).Is.False();
        }
    }

    [Test]
    public async Task EqualityFalse()
    {
        var folder = new Folder(Path.GetRandomFileName());
        var folder2 = new Folder(Path.GetRandomFileName());
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder).Is.Not.EqualTo(folder2);
            await Assert.That(folder.GetHashCode()).Is.Not.EqualTo(folder2.GetHashCode());
            await Assert.That(folder == folder2).Is.False();
            await Assert.That(folder != folder2).Is.True();
        }
    }

    [Test]
    public async Task AssertExists()
    {
        var folder = (Folder?) CreateRandomFolder();
        await Assert.That(() => folder.AssertExists()).Throws.Nothing();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNotExists()
    {
        Folder folder = ModularPipelines.FileSystem.File.GetNewTemporaryFilePath().Path!;
        await Assert.That(() => folder.AssertExists()).Throws.Exception().OfType<DirectoryNotFoundException>();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNull()
    {
        var folder = null as Folder;
        await Assert.That(() => folder.AssertExists()).Throws.Exception().OfType<DirectoryNotFoundException>();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Files_User_Does_Not_Throw_Unauth_Exception()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            .GetFolder("AppData")
            ?.FindFile(x => x.Name.Contains(Guid.NewGuid().ToString()), exclude => exclude.Name.StartsWith('.'))
            ).Throws.Nothing();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Files_User_Does_Not_Throw_Unauth_Exception2()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            ?.GetFiles(Guid.NewGuid().ToString()))
            .Throws.Nothing();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Folders_User_Does_Not_Throw_Unauth_Exception()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                .GetFolder("AppData")
            ?.FindFolder(x => x.Name.Contains(Guid.NewGuid().ToString()), exclude => exclude.Name.StartsWith('.')))
            .Throws.Nothing();
    }

    private static Folder CreateRandomFolder()
    {
        var tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempFolderPath);

        return new Folder(tempFolderPath);
    }
}