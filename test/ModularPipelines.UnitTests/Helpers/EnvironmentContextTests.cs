using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Helpers;

public class EnvironmentContextTests : TestBase
{
    [Test]
    public async Task Can_Read_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");
        
        Environment.SetEnvironmentVariable(guid, "Foo bar!");
        
        var context = await GetService<IEnvironmentContext>();
        
        var result = context.EnvironmentVariables.GetEnvironmentVariable(guid);
        
        Assert.That(result, Is.EqualTo("Foo bar!"));
    }
    
    [Test]
    public async Task Can_List_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");
        
        Environment.SetEnvironmentVariable(guid, "Foo bar!");
        
        var context = await GetService<IEnvironmentContext>();
        
        var result = context.EnvironmentVariables.GetEnvironmentVariables();
        
        Assert.That(result, Is.Not.Null.And.AssignableTo<IDictionary<string, string>>());
        Assert.That(result[guid], Is.EqualTo("Foo bar!"));
    }
    
    [Test]
    public async Task Can_Set_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");
        
        var context = await GetService<IEnvironmentContext>();

        context.EnvironmentVariables.SetEnvironmentVariable(guid, "Foo bar!");
            
        var result = Environment.GetEnvironmentVariable(guid);
        
        Assert.That(result, Is.EqualTo("Foo bar!"));
    }
    
    [Test]
    public async Task Can_Add_To_Path()
    {
        var context = await GetService<IEnvironmentContext>();

        var directoryToAdd = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N"));
        
        var path = context.EnvironmentVariables.GetPath();
        
        Assert.That(path, Is.Not.Empty);
        Assert.That(path, Does.Not.Contain(directoryToAdd));
        
        context.EnvironmentVariables.AddToPath(directoryToAdd);
            
        path = context.EnvironmentVariables.GetPath();
        
        Assert.That(path, Does.Contain(directoryToAdd));
    }

    [Test]
    public async Task Assert_Values_Populated()
    {
        var context = await GetService<IEnvironmentContext>();
        
        Assert.Multiple(() =>
        {
            Assert.That(context.ContentDirectory, Is.Not.Null);
            Assert.That(context.OperatingSystem.ToString(), Is.Not.Null);
            Assert.That(context.OperatingSystemVersion.ToString(), Is.Not.Null);
            Assert.That(context.Is64BitOperatingSystem, Is.True.Or.False);
            Assert.That(context.WorkingDirectory, Is.Not.Null);
            Assert.That(context.AppDomainDirectory, Is.Not.Null);
            Assert.That(context.GetFolder(Environment.SpecialFolder.LocalApplicationData), Is.Not.Null);
            Assert.That(context.EnvironmentName, Is.Not.Null);
        });
    }
}