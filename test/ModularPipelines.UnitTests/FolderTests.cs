using System.Globalization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using TUnit.Assertions.Extensions.Throws;
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

        await Assert.That(folder.ListFiles().ToList()).HasCount().EqualTo(10);

        folder.Clean();
        await Assert.That(folder.ListFiles().ToList()).HasCount().EqualTo(0);
    }

    private class FindFileModule : Module<ModularPipelines.FileSystem.File>
    {
        protected override async Task<ModularPipelines.FileSystem.File?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            
            return context.Git().RootDirectory.FindFile(x => x.Name == "README.md");
        }
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

        await Assert.That(folder.ListFolders().ToList()).HasCount().EqualTo(10);

        folder.Clean();
        await Assert.That(folder.ListFolders().ToList()).HasCount().EqualTo(0);
    }

    [Test]
    public async Task FindFile()
    {
        var git = await GetService<IGit>();

        var readme = git.RootDirectory.FindFile(x => x.Name == "README.md");
        await Assert.That(readme).IsNotNull();
        await Assert.That(readme!.Exists).IsTrue();
    }

    [Test]
    public async Task FindFileLogs()
    {
            var stringBuilder = new StringBuilder();

            await TestPipelineHostBuilder.Create()
                .ConfigureServices((_, collection) =>
                {
                    collection
                        .AddSingleton<ILogger<FindFileModule>>(
                            new StringLogger<FindFileModule>(stringBuilder))
                        .AddModule<FindFileModule>();
                })
                .ExecutePipelineAsync();

            var actualLogResult = stringBuilder.ToString().Trim();
            await Assert.That(actualLogResult).Contains("x => x.Name == \"README.md\"");
    }

    [Test]
    public async Task FindFolder()
    {
        var git = await GetService<IGit>();

        var src = git.RootDirectory.FindFolder(x => x.Name == "src");
        await Assert.That(src).IsNotNull();
        await Assert.That(src!.Exists).IsTrue();
    }

    [Test]
    public async Task Delete()
    {
        var folder = CreateRandomFolder();
        await Assert.That(folder.Exists).IsTrue();

        folder.Delete();
        await Assert.That(folder.Exists).IsFalse();
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
            await Assert.That(folder.Exists).IsTrue();
            await Assert.That(folder.ListFiles().ToList()).HasCount().EqualTo(10);
            await Assert.That(folder2.Exists).IsFalse();
        }

        folder.MoveTo(folder2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(new Folder(folder.OriginalPath).Exists).IsFalse();
            await Assert.That(folder.Exists).IsTrue();
            await Assert.That(folder2.Exists).IsTrue();
            await Assert.That(folder2.ListFiles().ToList()).HasCount().EqualTo(10);
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
            await Assert.That(folder.Exists).IsTrue();
            await Assert.That(folder.ListFiles().ToList()).HasCount().EqualTo(10);
            await Assert.That(folder2.Exists).IsFalse();
        }

        folder.CopyTo(folder2);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).IsTrue();
            await Assert.That(folder.ListFiles().ToList()).HasCount().EqualTo(10);
            await Assert.That(folder2.Exists).IsTrue();
            await Assert.That(folder2.ListFiles().ToList()).HasCount().EqualTo(10);
        }
    }

    [Test]
    public async Task Data_Is_Populated()
    {
        var folder = CreateRandomFolder();
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Exists).IsTrue();
            await Assert.That(folder.Attributes.ToString()).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.Path).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.OriginalPath).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.Extension).IsEmpty();
            await Assert.That(folder.Parent?.ToString()!).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.Root.ToString()).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.CreationTime.ToString(CultureInfo.InvariantCulture)).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture)).IsNotNull().And.IsNotEmpty();
            await Assert.That(folder.Hidden).IsFalse();
            await Assert.That(folder.Name).IsNotNull().And.IsNotEmpty();
        }
    }

    [Test]
    public async Task CreateFolder()
    {
        var folder = new Folder(Path.GetRandomFileName());

        await Assert.That(folder.Exists).IsFalse();

        folder.Create();
        await Assert.That(folder.Exists).IsTrue();
    }

    [Test]
    public async Task CreateFile()
    {
        var folder = CreateRandomFolder();

        var fileBeforeCreation = folder.GetFile("Foo.txt");
        await Assert.That(fileBeforeCreation.Exists).IsFalse();

        var file = folder.CreateFile("Foo.txt");
        await Assert.That(file.Exists).IsTrue();
    }

    [Test]
    public async Task CreateSubfolder()
    {
        var folder = CreateRandomFolder();

        var folderBeforeCreation = folder.GetFolder("Foo");
        await Assert.That(folderBeforeCreation.Exists).IsFalse();

        var subfolder = folder.CreateFolder("Foo");
        
        await using (Assert.Multiple())
        {
            await Assert.That(subfolder.Exists).IsTrue();
            await Assert.That(subfolder.Path).IsNotEqualTo(folder.Path);
            await Assert.That(subfolder.Parent).IsEqualTo(folder);
        }
    }

    [Test]
    public async Task Null_FileInfo_Implicit_Cast()
    {
        DirectoryInfo? directoryInfo = null;

        Folder? file = directoryInfo;
        await Assert.That(file).IsNull();
    }

    [Test]
    public async Task Null_String_Implicit_Cast()
    {
        string? fileInfo = null;

        Folder? file = fileInfo;
        await Assert.That(file).IsNull();
    }

    [Test]
    public async Task FileInfo_Implicit_Cast()
    {
        var directoryInfo = new DirectoryInfo(Path.GetTempFileName());

        Folder file = directoryInfo;
        await Assert.That(file).IsNotNull();
    }

    [Test]
    public async Task String_Implicit_Cast()
    {
        var path = Path.GetTempFileName();

        Folder file = path!;
        await Assert.That(file).IsNotNull();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Attributes()
    {
        var folder = CreateRandomFolder();
        await Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden)).IsFalse();

        folder.Attributes = FileAttributes.Hidden;

        await Assert.That(folder.Attributes.HasFlag(FileAttributes.Hidden)).IsTrue();
    }

    [Test]
    public async Task EqualityTrue()
    {
        var path = Path.GetRandomFileName();
        var folder = new Folder(path);
        var folder2 = new Folder(path);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder).IsEqualTo(folder2);
            await Assert.That(folder.GetHashCode()).IsEqualTo(folder2.GetHashCode());
            await Assert.That(folder == folder2).IsTrue();
            await Assert.That(folder != folder2).IsFalse();
        }
    }

    [Test]
    public async Task EqualityFalse()
    {
        var folder = new Folder(Path.GetRandomFileName());
        var folder2 = new Folder(Path.GetRandomFileName());
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder).IsNotEqualTo(folder2);
            await Assert.That(folder.GetHashCode()).IsNotEqualTo(folder2.GetHashCode());
            await Assert.That(folder == folder2).IsFalse();
            await Assert.That(folder != folder2).IsTrue();
        }
    }

    [Test]
    public async Task AssertExists()
    {
        var folder = (Folder?) CreateRandomFolder();
        await Assert.That(() => folder.AssertExists()).ThrowsNothing();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNotExists()
    {
        Folder folder = ModularPipelines.FileSystem.File.GetNewTemporaryFilePath().Path!;
        await Assert.That(() => folder.AssertExists()).ThrowsException().OfType<DirectoryNotFoundException>();
    }

    [Test]
    public async Task AssertExists_ThrowsWhenNull()
    {
        var folder = null as Folder;
        await Assert.That(() => folder.AssertExists()).ThrowsException().OfType<DirectoryNotFoundException>();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Files_User_Does_Not_Throw_Unauth_Exception()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            .GetFolder("AppData")
            ?.FindFile(x => x.Name.Contains(Guid.NewGuid().ToString()), exclude => exclude.Name.StartsWith('.'))
            ).ThrowsNothing();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Files_User_Does_Not_Throw_Unauth_Exception2()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
            ?.GetFiles(Guid.NewGuid().ToString()))
            .ThrowsNothing();
    }

    [Test, WindowsOnlyTest]
    public async Task Searching_Local_Folders_User_Does_Not_Throw_Unauth_Exception()
    {
        await Assert.That(() => new Folder(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                .GetFolder("AppData")
            ?.FindFolder(x => x.Name.Contains(Guid.NewGuid().ToString()), exclude => exclude.Name.StartsWith('.')))
            .ThrowsNothing();
    }

    private static Folder CreateRandomFolder()
    {
        var tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempFolderPath);

        return new Folder(tempFolderPath);
    }
}