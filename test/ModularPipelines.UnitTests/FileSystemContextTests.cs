using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class FileSystemContextTests : TestBase
{
    [Test]
    public async Task Move_File()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        context.MoveFile(file, newLocation);

        Assert.Multiple(() =>
        {
            Assert.That(file.Path).Is.EqualTo(newLocation);
            Assert.That(file.OriginalPath).Is.Not.EqualTo(newLocation);
            Assert.That(new File(file.OriginalPath).Exists).Is.False();
            Assert.That(file.Exists).Is.True();
        });
    }

    [Test]
    public async Task Copy_File()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFile(file, newLocation);

        Assert.Multiple(() =>
        {
            Assert.That(file.Path).Is.Not.EqualTo(newLocation);
            Assert.That(file.OriginalPath).Is.Not.EqualTo(newLocation);
            Assert.That(newFile.Path).Is.EqualTo(newLocation);
            Assert.That(newFile.OriginalPath).Is.EqualTo(newLocation);
            Assert.That(new File(file.OriginalPath).Exists).Is.True();
            Assert.That(file.Exists).Is.True();
        });
    }

    [Test]
    public async Task File_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();

        Assert.That(context.FileExists(file)).Is.True();

        file.Delete();

        Assert.That(context.FileExists(file)).Is.False();
    }

    [Test]
    public async Task File_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();

        Assert.Multiple(() =>
        {
            Assert.That(context.GetFileAttributes(file).ToString()).Is.Not.Null();
            Assert.That(context.GetNewTemporaryFilePath()).Is.Not.Null().And.Is.Not.Empty();
            Assert.That(context.FileExists(file)).Is.True();
        });
    }

    [Test]
    public async Task Move_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        context.MoveFolder(folder, newLocation);

        Assert.Multiple(() =>
        {
            Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            Assert.That(folder.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            Assert.That(new Folder(folder.OriginalPath).Exists).Is.False();
            Assert.That(folder.Exists).Is.True();
        });
    }

    [Test]
    public async Task Copy_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFolder(folder, newLocation);

        Assert.Multiple(() =>
        {
            Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            Assert.That(folder.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            Assert.That(newFile.Path.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            Assert.That(newFile.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            Assert.That(new Folder(folder.OriginalPath).Exists).Is.True();
            Assert.That(folder.Exists).Is.True();
        });
    }

    [Test]
    public async Task Folder_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();

        Assert.That(context.FolderExists(folder)).Is.True();

        folder.Delete();

        Assert.That(context.FolderExists(folder)).Is.False();
    }

    [Test]
    public async Task Folder_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();

        Assert.Multiple(() =>
        {
            Assert.That(context.GetFolderAttributes(folder).ToString()).Is.Not.Null();
            Assert.That(context.CreateTemporaryFolder()).Is.Not.Null();
            Assert.That(context.FolderExists(folder)).Is.True();
        });
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}