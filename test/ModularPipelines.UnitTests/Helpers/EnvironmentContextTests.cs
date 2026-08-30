using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using Moq;

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

            var result = context.Variables.Get(guid);
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

            var result = context.Variables.GetAll();
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

            context.Variables.Set(guid, TestConstants.TestString);

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
            context.Variables.Set(guid, null);

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
            var path = context.Variables.GetPath();
            await Assert.That(path).IsNotEmpty();
            await Assert.That(path).DoesNotContain(directoryToAdd);

            context.Variables.AddToPath(directoryToAdd);

            path = context.Variables.GetPath();
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
            await Assert.That(context.Architecture.ToString()).IsNotNull();
            await Assert.That(context.WorkingDirectory).IsNotNull();
            await Assert.That(context.AppDomainDirectory).IsNotNull();
            await Assert.That(context.EnvironmentName).IsNotNull();
            await Assert.That(context.MachineName).IsNotEmpty();
            await Assert.That(context.UserName).IsNotNull();
            await Assert.That(context.BuildSystem).IsNotNull();
        }
    }

    [Test]
    public async Task Constructor_Rejects_Blank_Content_Root()
    {
        var hostEnvironment = new Mock<IHostEnvironment>();
        hostEnvironment.SetupGet(environment => environment.ContentRootPath).Returns(string.Empty);

        await Assert.That(() => new EnvironmentContext(
                Mock.Of<IEnvironmentVariablesContext>(),
                Mock.Of<IBuildSystemContext>(),
                new PipelineWorkingDirectory(Environment.CurrentDirectory),
                hostEnvironment.Object,
                SystemFileSystemProvider.Instance))
            .Throws<ArgumentException>();
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
