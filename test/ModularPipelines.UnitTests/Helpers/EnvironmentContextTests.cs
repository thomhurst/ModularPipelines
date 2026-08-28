using ModularPipelines.Context;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class EnvironmentContextTests : TestBase
{
    [Test]
    public async Task Can_Read_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        await RunWithEnvironmentRestored(guid, async () =>
        {
            Environment.SetEnvironmentVariable(guid, TestConstants.TestString);

            var context = await GetService<IEnvironmentContext>();

            var result = context.EnvironmentVariables.Get(guid);
            await Assert.That(result).IsEqualTo(TestConstants.TestString);
        });
    }

    [Test]
    public async Task Can_List_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        await RunWithEnvironmentRestored(guid, async () =>
        {
            Environment.SetEnvironmentVariable(guid, TestConstants.TestString);

            var context = await GetService<IEnvironmentContext>();

            var result = context.EnvironmentVariables.GetAll();
            await Assert.That(result).IsNotNull();
            await Assert.That((object) result).IsAssignableTo<IReadOnlyDictionary<string, string?>>();
            await Assert.That(result[guid]).IsEqualTo(TestConstants.TestString);
        });
    }

    [Test]
    public async Task Can_Set_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        await RunWithEnvironmentRestored(guid, async () =>
        {
            var context = await GetService<IEnvironmentContext>();

            context.EnvironmentVariables.Set(guid, TestConstants.TestString);

            var result = Environment.GetEnvironmentVariable(guid);
            await Assert.That(result).IsEqualTo(TestConstants.TestString);
        });
    }

    [Test]
    public async Task Can_Remove_Environment_Variables()
    {
        var guid = Guid.NewGuid().ToString("N");

        await RunWithEnvironmentRestored(guid, async () =>
        {
            Environment.SetEnvironmentVariable(guid, TestConstants.TestString);

            var context = await GetService<IEnvironmentContext>();
            context.EnvironmentVariables.Set(guid, null);

            await Assert.That(Environment.GetEnvironmentVariable(guid)).IsNull();
        });
    }

    [Test]
    public async Task Can_Add_To_Path()
    {
        var context = await GetService<IEnvironmentContext>();

        var directoryToAdd = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N"));

        await RunWithEnvironmentRestored("PATH", async () =>
        {
            var path = context.EnvironmentVariables.GetPath();
            await Assert.That(path).IsNotEmpty();
            await Assert.That(path).DoesNotContain(directoryToAdd);

            context.EnvironmentVariables.AddToPath(directoryToAdd);

            path = context.EnvironmentVariables.GetPath();
            await Assert.That(path).Contains(directoryToAdd);
        });
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

    private static async Task RunWithEnvironmentRestored(
        string variableName,
        Func<Task> action)
    {
        var previousValue = Environment.GetEnvironmentVariable(variableName);

        try
        {
            await action();
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, previousValue);
        }
    }
}
