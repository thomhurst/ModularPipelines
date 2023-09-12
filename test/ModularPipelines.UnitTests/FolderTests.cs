using System.Globalization;
using ModularPipelines.FileSystem;
using ModularPipelines.Git;
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

    private static Folder CreateRandomFolder()
    {
        var tempFolderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempFolderPath);

        return new Folder(tempFolderPath);
    }
}