using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Newman.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Newman.UnitTests;

public class NewmanRunOptionsTests : TestBase
{
    [Test]
    [Arguments(nameof(NewmanRunOptions.Reporters), "--reporters", "cli,json")]
    [Arguments(nameof(NewmanRunOptions.Bail), "--bail", "failure,folder")]
    [Arguments(nameof(NewmanRunOptions.DelayRequest), "--delay-request", "100")]
    [Arguments(nameof(NewmanRunOptions.Timeout), "--timeout", "10000")]
    [Arguments(nameof(NewmanRunOptions.TimeoutRequest), "--timeout-request", "3000")]
    [Arguments(nameof(NewmanRunOptions.TimeoutScript), "--timeout-script", "500")]
    public async Task Optional_Values_Preserve_Omitted_Bare_And_Valued_Forms(
        string propertyName, string optionName, string value)
    {
        var options = new NewmanRunOptions("collection.json");
        var property = typeof(NewmanRunOptions).GetProperty(propertyName)!;

        await Assert.That(await Render(options)).IsEqualTo("newman run collection.json");

        property.SetValue(options, CliOptionValue.Bare);
        await Assert.That(await Render(options)).IsEqualTo($"newman run collection.json {optionName}");

        property.SetValue(options, (CliOptionValue) value);
        await Assert.That(await Render(options)).IsEqualTo($"newman run collection.json {optionName} {value}");
    }

    [Test]
    public async Task Repeated_Folders_And_Variables_Stay_Separate_Arguments()
    {
        var options = new NewmanRunOptions("collection.json")
        {
            Folder = ["first", "second"],
            GlobalVar = ["region=west", "retries=2"],
            EnvVar = ["name=first", "name=second"],
        };

        await Assert.That(await Render(options)).IsEqualTo(
            "newman run collection.json --folder first --folder second --global-var region=west --global-var retries=2 --env-var name=first --env-var name=second");
    }

    [Test]
    public async Task Required_Values_And_Flags_Render_After_Collection()
    {
        var options = new NewmanRunOptions("collection.json")
        {
            Environment = "environment.json",
            IterationCount = 2,
            SuppressExitCode = true,
            Silent = false,
            Color = "off",
            NoInsecureFileRead = true,
        };

        await Assert.That(await Render(options)).IsEqualTo(
            "newman run collection.json --environment environment.json --iteration-count 2 --suppress-exit-code --color off --no-insecure-file-read");
    }

    [Test]
    public async Task Credential_Values_Are_Masked_In_Command_Input()
    {
        var input = await Render(
            new NewmanRunOptions("collection.json")
            {
                PostmanApiKey = "test-api-key",
                SslClientPassphrase = "test-passphrase",
            });

        await Assert.That(input).IsEqualTo(
            "newman run collection.json --postman-api-key ********** --ssl-client-passphrase **********");
    }

    private async Task<string> Render(NewmanRunOptions options)
    {
        var (command, _) = await GetService<ICommandContext>(services =>
            services.AddSingleton<ICommandInterceptor, CaptureCommandInput>());
        return (await command.ExecuteCommandLineToolAsync(options)).CommandInput;
    }

    private sealed class CaptureCommandInput : ICommandInterceptor
    {
        public ValueTask<CommandResult> InvokeAsync(
            CommandInvocation invocation, CommandDelegate next, CancellationToken cancellationToken) =>
            ValueTask.FromResult(CommandResult.Ok() with { CommandInput = invocation.CommandInput });
    }
}
