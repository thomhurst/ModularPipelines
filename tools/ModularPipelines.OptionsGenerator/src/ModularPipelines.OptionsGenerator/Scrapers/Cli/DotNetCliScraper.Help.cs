using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class DotNetCliScraper
{
    private static readonly JsonSerializerOptions IsolatedSdkJsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// The <c>global.json</c> test runner whose <c>dotnet test</c> help generates the primary options.
    /// </summary>
    internal const string TestPlatformRunner = "Microsoft.Testing.Platform";

    /// <summary>
    /// The variant generated from the SDK's default runner, VSTest.
    /// </summary>
    internal const string VstestVariant = "Vstest";

    private static bool IsTestCommand(string[] commandPath) => commandPath is [_, "test"];

    /// <inheritdoc />
    protected override IReadOnlyList<string> GetCommandVariants(string[] commandPath) =>
        IsTestCommand(commandPath) ? [VstestVariant] : [];

    protected override async Task<CliCommandResult> ExecuteHelpCommandAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        if (!IsTestCommand(commandPath))
        {
            return await base.ExecuteHelpCommandAsync(commandPath, cancellationToken).ConfigureAwait(false);
        }

        return await ExecuteIsolatedTestHelpAsync(commandPath, TestPlatformRunner, CliHelpKind.Help, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task<CliCommandResult> ExecuteVariantHelpCommandAsync(
        string[] commandPath,
        string variant,
        CancellationToken cancellationToken)
    {
        if (!IsTestCommand(commandPath) || variant != VstestVariant)
        {
            return await base.ExecuteVariantHelpCommandAsync(commandPath, variant, cancellationToken).ConfigureAwait(false);
        }

        return await ExecuteIsolatedTestHelpAsync(commandPath, testRunner: null, CliHelpKind.Variant, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// The nearest global.json selects the dotnet test runner, and each runner prints different
    /// switches. Keep the selected SDK, but query help with an explicit runner in isolation.
    /// </summary>
    private async Task<CliCommandResult> ExecuteIsolatedTestHelpAsync(
        string[] commandPath,
        string? testRunner,
        CliHelpKind helpKind,
        CancellationToken cancellationToken)
    {
        var version = await GetVersionAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Cannot isolate dotnet test help without the selected SDK version.");
        var directory = Directory.CreateTempSubdirectory("dotnet-test-help-");
        try
        {
            // Resolve sdk.paths against the original invocation directory before running help in isolation.
            var settings = await CreateIsolatedSdkSettingsAsync(Environment.CurrentDirectory, version, testRunner, cancellationToken).ConfigureAwait(false);
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "global.json"), settings, cancellationToken).ConfigureAwait(false);
            return await ExecuteAndRecordHelpCommandAsync(commandPath, ExecutablePath, GetHelpArguments(commandPath),
                cancellationToken, directory.FullName, helpKind: helpKind).ConfigureAwait(false);
        }
        finally
        {
            try
            {
                directory.Delete(recursive: true);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                Logger.LogWarning(exception, "Could not delete temporary dotnet help directory {Directory}", directory.FullName);
            }
        }
    }

    internal static async Task<string> CreateIsolatedSdkSettingsAsync(
        string workingDirectory,
        string version,
        string? testRunner,
        CancellationToken cancellationToken)
    {
        string[]? paths = null;
        for (var directory = new DirectoryInfo(workingDirectory); directory is not null; directory = directory.Parent)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var globalJsonPath = Path.Combine(directory.FullName, "global.json");
            if (!File.Exists(globalJsonPath))
            {
                continue;
            }

            var contents = await File.ReadAllTextAsync(globalJsonPath, cancellationToken).ConfigureAwait(false);
            using var settings = JsonDocument.Parse(contents, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            });
            if (settings.RootElement.TryGetProperty("sdk", out var sdk) && sdk.TryGetProperty("paths", out var sdkPaths))
            {
                // Relative paths are based on the original global.json, not the temporary
                // directory. The host token must continue to identify the dotnet executable.
                paths = [.. sdkPaths.EnumerateArray().Select(path => path.GetString()!).Select(path => path == "$host$" ? path : Path.GetFullPath(path, directory.FullName))];
            }

            // SDK resolution uses the nearest global.json, even when it has no sdk.paths.
            break;
        }

        return JsonSerializer.Serialize(new
        {
            sdk = new { version, rollForward = "disable", allowPrerelease = true, paths },
            test = testRunner is null ? null : new { runner = testRunner },
        }, IsolatedSdkJsonOptions);
    }
}
