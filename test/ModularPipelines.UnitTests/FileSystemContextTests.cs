using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class FileSystemContextTests : TestBase
{
    [Test]
    public async Task Move_File()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        var originalPath = file.OriginalPath;
        var newLocation = File.GetNewTemporaryFilePath().Path;

        context.MoveFile(file, newLocation);

        using (Assert.Multiple())
        {
            // Original path should no longer exist
            await Assert.That(new File(originalPath).Exists).IsFalse();
            // New location should exist
            await Assert.That(new File(newLocation).Exists).IsTrue();
        }
    }

    [Test]
    public async Task Copy_File()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFile(file, newLocation);

        using (Assert.Multiple())
        {
            await Assert.That(file.Path).IsNotEqualTo(newLocation);
            await Assert.That(file.OriginalPath).IsNotEqualTo(newLocation);
            await Assert.That(newFile.Path).IsEqualTo(newLocation);
            await Assert.That(newFile.OriginalPath).IsEqualTo(newLocation);
            await Assert.That(new File(file.OriginalPath).Exists).IsTrue();
            await Assert.That(file.Exists).IsTrue();
        }
    }

    [Test]
    public async Task File_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        await Assert.That(context.FileExists(file)).IsTrue();

        file.Delete();
        await Assert.That(context.FileExists(file)).IsFalse();
    }

    [Test]
    public async Task File_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();

        using (Assert.Multiple())
        {
            await Assert.That(context.GetFileAttributes(file).ToString()).IsNotNull();
            await Assert.That(context.GetNewTemporaryFilePath()).IsNotNull().And.IsNotEmpty();
            await Assert.That(context.FileExists(file)).IsTrue();
        }
    }

    [Test]
    public async Task Move_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var originalPath = folder.Path;
        var newLocation = File.GetNewTemporaryFilePath().Path;

        // MoveFolder moves the folder but doesn't update the original Folder instance
        // The new Folder at the destination path is accessed by creating a new Folder instance
        context.MoveFolder(folder, newLocation);

        using (Assert.Multiple())
        {
            // Original folder instance still references the original path
            await Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).IsEqualTo(originalPath.TrimEnd('\\').TrimEnd('/'));
            // Original location no longer exists
            await Assert.That(new Folder(originalPath).Exists).IsFalse();
            // New location exists
            await Assert.That(new Folder(newLocation).Exists).IsTrue();
        }
    }

    [Test]
    public async Task Copy_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFolder(folder, newLocation);

        using (Assert.Multiple())
        {
            await Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).IsNotEqualTo(newLocation);
            await Assert.That(folder.OriginalPath.TrimEnd('\\').TrimEnd('/')).IsNotEqualTo(newLocation);
            await Assert.That(newFile.Path.TrimEnd('\\').TrimEnd('/')).IsEqualTo(newLocation);
            await Assert.That(newFile.OriginalPath.TrimEnd('\\').TrimEnd('/')).IsEqualTo(newLocation);
            await Assert.That(new Folder(folder.OriginalPath).Exists).IsTrue();
            await Assert.That(folder.Exists).IsTrue();
        }
    }

    [Test]
    public async Task Folder_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        await Assert.That(context.FolderExists(folder)).IsTrue();

        folder.Delete();
        await Assert.That(context.FolderExists(folder)).IsFalse();
    }

    [Test]
    public async Task Folder_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();

        using (Assert.Multiple())
        {
            await Assert.That(context.GetFolderAttributes(folder).ToString()).IsNotNull();
            await Assert.That(context.CreateTemporaryFolder()).IsNotNull();
            await Assert.That(context.FolderExists(folder)).IsTrue();
        }
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, TestConstants.TestString);

        return new File(path);
    }
}