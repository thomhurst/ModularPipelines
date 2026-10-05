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

    protected override async Task<CliCommandResult> ExecuteHelpCommandAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        if (commandPath.Length != 2 || commandPath[1] != "test")
        {
            return await base.ExecuteHelpCommandAsync(commandPath, cancellationToken).ConfigureAwait(false);
        }

        // Repository global.json can select Microsoft.Testing.Platform and hide VSTest
        // switches. Keep the selected SDK, but query its default runner in isolation.
        var version = await GetVersionAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Cannot isolate dotnet test help without the selected SDK version.");
        var directory = Directory.CreateTempSubdirectory("dotnet-test-help-");
        try
        {
            // Resolve sdk.paths against the original invocation directory before running help in isolation.
            var settings = await CreateIsolatedSdkSettingsAsync(Environment.CurrentDirectory, version, cancellationToken).ConfigureAwait(false);
            await File.WriteAllTextAsync(Path.Combine(directory.FullName, "global.json"), settings, cancellationToken).ConfigureAwait(false);
            return await ExecuteAndRecordHelpCommandAsync(commandPath, ExecutablePath, GetHelpArguments(commandPath),
                cancellationToken, directory.FullName).ConfigureAwait(false);
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

    internal static async Task<string> CreateIsolatedSdkSettingsAsync(string workingDirectory, string version, CancellationToken cancellationToken)
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
        }, IsolatedSdkJsonOptions);
    }
}
