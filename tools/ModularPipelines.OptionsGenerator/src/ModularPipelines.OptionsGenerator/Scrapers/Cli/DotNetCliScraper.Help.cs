using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class DotNetCliScraper
{
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
            var settings = JsonSerializer.Serialize(new
            {
                sdk = new { version, rollForward = "disable", allowPrerelease = true },
            });
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
}
