using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Commands;

public class CommandOutputTruncationTests : TestBase
{
    [Test]
    [Arguments(5, true, 64)]
    [Arguments(0, true, 64)]
    [Arguments(5, false, 64)]
    [Arguments(5, true, CommandExecutionOptions.DefaultMaxCapturedOutputLength)]
    public async Task Truncation_Masks_Secret_Fragments_In_Failures_And_Preserves_Raw_Results(
        int exitCode, bool throwOnFailure, int limit)
    {
        const string secret = "EDGE-secret!";
        var paddingLength = (limit / 2) - secret.Length + 1;
        var logger = new Mock<ICommandLogger>();
        var (command, pipeline) = await GetService<ICommandContext>(services => services.AddSingleton(logger.Object));
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);
        var execution = command.ExecuteCommandLineToolAsync(
            new PowerShellScriptOptions(
                $"$value = ('P' * {paddingLength}) + '{secret}' + ('M' * {limit - 2}) + '{secret}' + ('T' * {paddingLength}); "
                + $"[Console]::Out.Write($value); [Console]::Error.Write($value); exit {exitCode}"),
            new CommandExecutionOptions { MaxCapturedOutputLength = limit, ThrowOnNonZeroExitCode = throwOnFailure });
        var expected = new string('P', paddingLength) + "**********" + Environment.NewLine
            + $"... [truncated {limit} characters] ..." + Environment.NewLine
            + "**********" + new string('T', paddingLength);

        if (exitCode == 0 || !throwOnFailure)
        {
            var result = await execution;
            await Assert.That(result.StandardOutput).StartsWith(new string('P', paddingLength) + secret[..^1]);
            await Assert.That(result.StandardOutput).EndsWith(secret[1..] + new string('T', paddingLength));
            await Assert.That(result.StandardError).IsEqualTo(result.StandardOutput);
        }
        else
        {
            var failure = await Assert.That(async () => { await execution; }).Throws<CommandException>();
            await Assert.That(failure!.Result.StandardOutput).IsEqualTo(expected);
            await Assert.That(failure.Result.StandardError).IsEqualTo(expected);
            await Assert.That(failure.Result.StandardOutputTruncatedCharacters).IsEqualTo(limit);
            await Assert.That(failure.Result.StandardErrorTruncatedCharacters).IsEqualTo(limit);
        }

        logger.Verify(instance => instance.LogCommandCompletion(
            It.IsAny<CommandLineToolOptions>(), It.IsAny<CommandExecutionOptions>(), It.IsAny<string>(),
            exitCode, It.IsAny<TimeSpan?>(), expected, expected, It.IsAny<string>()), Times.Once());
    }

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
