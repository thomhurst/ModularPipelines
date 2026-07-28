using ModularPipelines.Context;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context.Domains.Shell;
using Moq;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Context;

public class GitInformationTests : TestBase
{
    [Test]
    public async Task Repository_Info_Is_Cached()
    {
        var context = await GetService<IPipelineContext>();
        var gitInformation = context.Git().Information;
        var first = await gitInformation.GetInfoAsync();
        var second = await gitInformation.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(first).IsNotNull();
            await Assert.That(ReferenceEquals(first, second)).IsTrue();
        }
    }

    [Test]
    public async Task Resolving_Service_Does_Not_Run_Git_Commands()
    {
        var command = new Mock<ICommandContext>();
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton<ICommandContext>(command.Object));

        command.VerifyNoOtherCalls();
        await result.Host.DisposeAsync();
    }

    [Test]
    public async Task Unavailable_Git_Returns_Null()
    {
        var command = new Mock<ICommandContext>();
        command.Setup(x => x.ExecuteCommandLineTool(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("git unavailable"));
        var result = await GetService<IGitInformation>((_, services) =>
            services.AddSingleton<ICommandContext>(command.Object));

        await Assert.That(await result.T.GetInfoAsync()).IsNull();
        await result.Host.DisposeAsync();
    }
}
