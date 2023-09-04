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
}