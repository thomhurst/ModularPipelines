using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Commands;

public class CommandOutputTruncationTests : TestBase
{
    [Test]
    [Arguments(10, 0, false)]
    [Arguments(10, 5, false)]
    [Arguments(10, 5, true)]
    [Arguments(16, 0, false)]
    [Arguments(0, 0, false)]
    [Arguments(-1, 0, false)]
    public async Task Reports_Omitted_Characters_Per_Stream(int limit, int exitCode, bool throwOnFailure)
    {
        var logger = new Mock<ICommandLogger>();
        var service = await GetService<ICommandContext>(services => services.AddSingleton(logger.Object));
        var execution = service.T.ExecuteCommandLineToolAsync(
            new PowerShellScriptOptions($"[Console]::Out.Write('0123456789abcdef'); [Console]::Error.Write('012345678901'); exit {exitCode}"),
            new CommandExecutionOptions { MaxCapturedOutputLength = limit, ThrowOnNonZeroExitCode = throwOnFailure });
        CommandResult result;
        if (throwOnFailure)
        {
            var failure = await Assert.That(async () => { await execution; }).Throws<CommandException>();
            result = failure!.Result;
        }
        else
        {
            result = await execution;
        }

        var outputOmitted = limit > 0 ? Math.Max(0, 16 - limit) : 0;
        var errorOmitted = limit > 0 ? Math.Max(0, 12 - limit) : 0;
        await Assert.That(result.StandardOutputTruncatedCharacters).IsEqualTo(outputOmitted);
        await Assert.That(result.StandardErrorTruncatedCharacters).IsEqualTo(errorOmitted);
        await Assert.That(result.StandardOutput.Contains("[truncated", StringComparison.Ordinal)).IsEqualTo(outputOmitted > 0);
        await Assert.That(result.StandardError.Contains("[truncated", StringComparison.Ordinal)).IsEqualTo(errorOmitted > 0);
        logger.Verify(instance => instance.LogOutputTruncation(It.IsAny<CommandLineToolOptions>(), It.IsAny<CommandExecutionOptions>(), outputOmitted, errorOmitted, limit),
            outputOmitted + errorOmitted > 0 ? Times.Once() : Times.Never());
    }
}
