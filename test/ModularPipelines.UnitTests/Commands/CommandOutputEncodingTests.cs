using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.ProcessTestHost;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Commands;

public class CommandOutputEncodingTests : TestBase
{
    [Test]
    public async Task Defaults_To_Utf8_For_Both_Streams()
    {
        const string output = "café 東京 🚀";
        const string error = "échec 東京 🚀";
        var command = await GetService<ICommandContext>();

        var result = await command.ExecuteCommandLineToolAsync(CreateOutputCommand(Encoding.UTF8, output, error));

        await Assert.That(new CommandExecutionOptions().OutputEncoding).IsEqualTo(Encoding.UTF8);
        await Assert.That(result.StandardOutput).IsEqualTo(output);
        await Assert.That(result.StandardError).IsEqualTo(error);
    }

    [Test]
    [Arguments(1252)]
    [Arguments(850)]
    [Arguments(1201)]
    public async Task Uses_Command_Encoding_For_Both_Streams(int codePage)
    {
        var encoding = CodePagesEncodingProvider.Instance.GetEncoding(codePage) ?? Encoding.GetEncoding(codePage);
        var output = string.Concat(Enumerable.Repeat("café Grüße", 1000));
        const string error = "déjà vu";
        var command = await GetService<ICommandContext>();

        var result = await command.ExecuteCommandLineToolAsync(
            CreateOutputCommand(encoding, output, error),
            new CommandExecutionOptions { OutputEncoding = encoding });

        await Assert.That(result.StandardOutput).IsEqualTo(output);
        await Assert.That(result.StandardError).IsEqualTo(error);
    }

    [Test]
    public async Task Byte_Order_Mark_Overrides_Command_Encoding()
    {
        const string output = "café 東京";
        const string error = "échec 東京";
        var command = await GetService<ICommandContext>();

        var result = await command.ExecuteCommandLineToolAsync(
            CreateOutputCommand(Encoding.Unicode, output, error, includePreamble: true),
            new CommandExecutionOptions { OutputEncoding = Encoding.Latin1 });

        await Assert.That(result.StandardOutput).IsEqualTo(output);
        await Assert.That(result.StandardError).IsEqualTo(error);
    }

    [Test]
    public async Task Masks_Decoded_Secrets_In_Both_Streams()
    {
        const string secret = "café-secret";
        var messages = new ConcurrentQueue<string>();
        var (command, pipeline) = await GetService<ICommandContext>(services =>
        {
            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            services.AddLogging(builder => builder.AddProvider(new RecordingLoggerProvider(messages)));
        });
        pipeline.Services.GetRequiredService<ISecretRegistry>().AddSecret(secret);

        var result = await command.ExecuteCommandLineToolAsync(
            CreateOutputCommand(Encoding.Latin1, $"output:{secret}", $"error:{secret}"),
            new CommandExecutionOptions { OutputEncoding = Encoding.Latin1 });
        await pipeline.DisposeAsync();

        await Assert.That(result.StandardOutput).IsEqualTo($"output:{secret}");
        await Assert.That(result.StandardError).IsEqualTo($"error:{secret}");
        // Observe masking at the logger boundary. The file provider's bounded disposal
        // wait does not guarantee that queued entries have reached disk under load.
        var log = string.Join(Environment.NewLine, messages);
        await Assert.That(log).Contains("output:********");
        await Assert.That(log).Contains("error:********");
        await Assert.That(log).DoesNotContain(secret);
    }

    private sealed class RecordingLoggerProvider(ConcurrentQueue<string> messages) : ILoggerProvider, ILogger
    {
        public ILogger CreateLogger(string categoryName) => this;

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            messages.Enqueue(formatter(state, exception));
        }

        public void Dispose()
        {
        }
    }

    private static CommandLineToolOptions CreateOutputCommand(Encoding encoding, string output, string error, bool includePreamble = false)
    {
        var preamble = includePreamble ? encoding.GetPreamble() : [];
        var runtimeConfiguration = Path.ChangeExtension(typeof(CommandOutputEncodingTests).Assembly.Location, ".runtimeconfig.json");
        return new CommandLineToolOptions("dotnet")
        {
            Arguments =
            [
                "exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, "write-bytes",
                Convert.ToBase64String([.. preamble, .. encoding.GetBytes(output)]),
                Convert.ToBase64String([.. preamble, .. encoding.GetBytes(error)]),
            ],
        };
    }
}
