using System.Globalization;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class FileTests : TestBase
{
    [Test]
    public async Task Delete()
    {
        var file = await CreateRandomFile();

        Assert.That(file.Exists, Is.True);
        
        file.Delete();
        
        Assert.That(file.Exists, Is.False);
    }

    [Test]
    public async Task MoveTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        
        Assert.Multiple(() =>
        {
            Assert.That(file.Exists, Is.True);
            Assert.That(file2.Exists, Is.False);
        });
        
        file.MoveTo(file2);
        
        Assert.Multiple(() =>
        {
            Assert.That(new File(file.OriginalPath).Exists, Is.False);

            Assert.That(file.Exists, Is.True);
            Assert.That(file2.Exists, Is.True);
        });
    }

    [Test]
    public async Task Data_Is_Populated()
    {
        var file = await CreateRandomFile();
        
        Assert.Multiple(() =>
        {
            Assert.That(file.Exists, Is.True);
            Assert.That(file.Attributes.ToString(), Is.Not.Null.Or.Empty);
            Assert.That(file.Path, Is.Not.Null.Or.Empty);
            Assert.That(file.OriginalPath, Is.Not.Null.Or.Empty);
            Assert.That(file.Extension, Is.Not.Null.Or.Empty);
            Assert.That(file.Folder?.ToString(), Is.Not.Null.Or.Empty);
            Assert.That(file.CreationTime.ToString(CultureInfo.InvariantCulture), Is.Not.Null.Or.Empty);
            Assert.That(file.LastWriteTimeUtc.ToString(CultureInfo.InvariantCulture), Is.Not.Null.Or.Empty);
            Assert.That(file.Hidden, Is.False);
            Assert.That(file.Name, Is.Not.Null.Or.Empty);
            Assert.That(file.NameWithoutExtension, Is.Not.Null.Or.Empty);
            Assert.That(file.IsReadOnly, Is.False);
        });
    }
    
    [Test]
    public async Task CopyTo()
    {
        var file = await CreateRandomFile();

        var file2 = new File(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        
        Assert.Multiple(() =>
        {
            Assert.That(file.Exists, Is.True);
            Assert.That(file2.Exists, Is.False);
        });
        
        file.CopyTo(file2);
        
        Assert.Multiple(() =>
        {
            Assert.That(file.Exists, Is.True);
            Assert.That(file2.Exists, Is.True);
        });
    }

    private static async Task<File> CreateRandomFile()
    {
        var path = File.GetNewTemporaryFilePath();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}