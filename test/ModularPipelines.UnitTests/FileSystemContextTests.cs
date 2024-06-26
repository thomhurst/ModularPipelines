using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions;
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Path).Is.EqualTo(newLocation);
            await Assert.That(file.OriginalPath).Is.Not.EqualTo(newLocation);
            await Assert.That(new File(file.OriginalPath).Exists).Is.False();
            await Assert.That(file.Exists).Is.True();
        }
    }

    [Test]
    public async Task Copy_File()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFile(file, newLocation);
        
        await using (Assert.Multiple())
        {
            await Assert.That(file.Path).Is.Not.EqualTo(newLocation);
            await Assert.That(file.OriginalPath).Is.Not.EqualTo(newLocation);
            await Assert.That(newFile.Path).Is.EqualTo(newLocation);
            await Assert.That(newFile.OriginalPath).Is.EqualTo(newLocation);
            await Assert.That(new File(file.OriginalPath).Exists).Is.True();
            await Assert.That(file.Exists).Is.True();
        }
    }

    [Test]
    public async Task File_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        await Assert.That(context.FileExists(file)).Is.True();

        file.Delete();
        await Assert.That(context.FileExists(file)).Is.False();
    }

    [Test]
    public async Task File_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var file = await CreateRandomFile();
        
        await using (Assert.Multiple())
        {
            await Assert.That(context.GetFileAttributes(file).ToString()).Is.Not.Null();
            await Assert.That(context.GetNewTemporaryFilePath()).Is.Not.Null().And.Is.Not.Empty();
            await Assert.That(context.FileExists(file)).Is.True();
        }
    }

    [Test]
    public async Task Move_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        context.MoveFolder(folder, newLocation);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            await Assert.That(folder.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            await Assert.That(new Folder(folder.OriginalPath).Exists).Is.False();
            await Assert.That(folder.Exists).Is.True();
        }
    }

    [Test]
    public async Task Copy_Folder()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        var newLocation = File.GetNewTemporaryFilePath().Path;

        var newFile = context.CopyFolder(folder, newLocation);
        
        await using (Assert.Multiple())
        {
            await Assert.That(folder.Path.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            await Assert.That(folder.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.Not.EqualTo(newLocation);
            await Assert.That(newFile.Path.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            await Assert.That(newFile.OriginalPath.TrimEnd('\\').TrimEnd('/')).Is.EqualTo(newLocation);
            await Assert.That(new Folder(folder.OriginalPath).Exists).Is.True();
            await Assert.That(folder.Exists).Is.True();
        }
    }

    [Test]
    public async Task Folder_Delete()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        await Assert.That(context.FolderExists(folder)).Is.True();

        folder.Delete();
        await Assert.That(context.FolderExists(folder)).Is.False();
    }

    [Test]
    public async Task Folder_Data_Populated()
    {
        var context = await GetService<IFileSystemContext>();

        var folder = Folder.CreateTemporaryFolder();
        
        await using (Assert.Multiple())
        {
            await Assert.That(context.GetFolderAttributes(folder).ToString()).Is.Not.Null();
            await Assert.That(context.CreateTemporaryFolder()).Is.Not.Null();
            await Assert.That(context.FolderExists(folder)).Is.True();
        }
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}