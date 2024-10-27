using ModularPipelines.Context;
using ModularPipelines.TestHelpers;

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
        await Assert.That(result).IsEqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_List_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        Environment.SetEnvironmentVariable(guid, "Foo bar!");

        var context = await GetService<IEnvironmentContext>();

        var result = context.EnvironmentVariables.GetEnvironmentVariables();
        await Assert.That(result).IsNotNull().And.IsAssignableTo(typeof(IDictionary<string, string>));
        await Assert.That(result[guid]).IsEqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_Set_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        var context = await GetService<IEnvironmentContext>();

        context.EnvironmentVariables.SetEnvironmentVariable(guid, "Foo bar!");

        var result = Environment.GetEnvironmentVariable(guid);
        await Assert.That(result).IsEqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_Add_To_Path()
    {
        var context = await GetService<IEnvironmentContext>();

        var directoryToAdd = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N"));

        var path = context.EnvironmentVariables.GetPath();
        await Assert.That(path).IsNotEmpty();
        await Assert.That(path).DoesNotContain(directoryToAdd);

        context.EnvironmentVariables.AddToPath(directoryToAdd);

        path = context.EnvironmentVariables.GetPath();
        await Assert.That(path).Contains(directoryToAdd);
    }

    [Test]
    public async Task Assert_Values_Populated()
    {
        var context = await GetService<IEnvironmentContext>();
        
        using (Assert.Multiple())
        {
            await Assert.That(context.ContentDirectory).IsNotNull();
            await Assert.That(context.OperatingSystem.ToString()).IsNotNull();
            await Assert.That(context.OperatingSystemVersion.ToString()).IsNotNull();
            await Assert.That(context.Is64BitOperatingSystem).IsTrue().Or.IsFalse();
            await Assert.That(context.WorkingDirectory).IsNotNull();
            await Assert.That(context.AppDomainDirectory).IsNotNull();
            await Assert.That(context.GetFolder(Environment.SpecialFolder.LocalApplicationData)).IsNotNull();
            await Assert.That(context.EnvironmentName).IsNotNull();
        }
    }
}