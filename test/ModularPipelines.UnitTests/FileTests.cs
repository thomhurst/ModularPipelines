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
            Assert.That(file.Exists, Is.False);
            Assert.That(file2.Exists, Is.True);
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
        var path = Path.GetTempFileName();

        await System.IO.File.WriteAllTextAsync(path, "Foo bar!");

        return new File(path);
    }
}