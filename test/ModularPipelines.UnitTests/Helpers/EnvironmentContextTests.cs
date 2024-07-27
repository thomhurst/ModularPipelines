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
        await Assert.That(result).Is.EqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_List_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        Environment.SetEnvironmentVariable(guid, "Foo bar!");

        var context = await GetService<IEnvironmentContext>();

        var result = context.EnvironmentVariables.GetEnvironmentVariables();
        await Assert.That(result).Is.Not.Null().And.Is.AssignableTo<IDictionary<string, string>>();
        await Assert.That(result[guid]).Is.EqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_Set_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        var context = await GetService<IEnvironmentContext>();

        context.EnvironmentVariables.SetEnvironmentVariable(guid, "Foo bar!");

        var result = Environment.GetEnvironmentVariable(guid);
        await Assert.That(result).Is.EqualTo("Foo bar!");
    }

    [Test]
    public async Task Can_Add_To_Path()
    {
        var context = await GetService<IEnvironmentContext>();

        var directoryToAdd = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N"));

        var path = context.EnvironmentVariables.GetPath();
        await Assert.That(path).Is.Not.Empty();
        await Assert.That(path).Does.Not.Contain(directoryToAdd);

        context.EnvironmentVariables.AddToPath(directoryToAdd);

        path = context.EnvironmentVariables.GetPath();
        await Assert.That(path).Does.Contain(directoryToAdd);
    }

    [Test]
    public async Task Assert_Values_Populated()
    {
        var context = await GetService<IEnvironmentContext>();
        
        await using (Assert.Multiple())
        {
            await Assert.That(context.ContentDirectory).Is.Not.Null();
            await Assert.That(context.OperatingSystem.ToString()).Is.Not.Null();
            await Assert.That(context.OperatingSystemVersion.ToString()).Is.Not.Null();
            await Assert.That(context.Is64BitOperatingSystem).Is.True().Or.Is.False();
            await Assert.That(context.WorkingDirectory).Is.Not.Null();
            await Assert.That(context.AppDomainDirectory).Is.Not.Null();
            await Assert.That(context.GetFolder(Environment.SpecialFolder.LocalApplicationData)).Is.Not.Null();
            await Assert.That(context.EnvironmentName).Is.Not.Null();
        }
    }
}