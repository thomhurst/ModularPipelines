using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class GitCliScraperTests
{
    [Test]
    public async Task Tool_Definition_Preserves_The_Hand_Written_Grouped_Facade()
    {
        var scraper = new GitCliScraper(
            new StubExecutor(),
            NullLogger<GitCliScraper>.Instance);

        var tool = scraper.CreateToolDefinition();

        await Assert.That(tool.GenerateCommandFacade).IsFalse();
        await Assert.That(tool.DocumentationOutputDirectory).IsNull();
    }

    private sealed class StubExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new NotSupportedException();

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            throw new NotSupportedException();
    }
}
