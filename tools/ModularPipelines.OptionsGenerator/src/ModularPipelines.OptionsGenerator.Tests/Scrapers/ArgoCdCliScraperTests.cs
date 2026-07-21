using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class ArgoCdCliScraperTests
{
    [Test]
    public async Task Appset_Uses_ApplicationSet_Identifier()
    {
        var scraper = new TestArgoCdCliScraper();

        await Assert.That(scraper.NormalizeIdentifier("appset")).IsEqualTo("ApplicationSet");
        await Assert.That(scraper.NormalizeIdentifier("app")).IsEqualTo("App");
    }

    private sealed class TestArgoCdCliScraper : ArgoCdCliScraper
    {
        public TestArgoCdCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<ArgoCdCliScraper>.Instance)
        {
        }

        public string NormalizeIdentifier(string commandPart) => NormalizeCommandIdentifier(commandPart);
    }
}
