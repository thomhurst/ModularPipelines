namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudNegatedFlagTests
{
    [Test]
    [Arguments("--[no-]allow-unauthenticated", "Allow public requests.")]
    [Arguments("--allow-unauthenticated", "Allow public requests. Use --no-allow-unauthenticated to disable.")]
    public async Task Negatable_Boolean_Has_One_Property(string declaration, string description)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--allow-unauthenticated]
            FLAGS
                {{declaration}}
                    {{description}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var option = command.Options.Single();

        await Assert.That(option.SwitchName).IsEqualTo("--allow-unauthenticated");
        await Assert.That(option.NegatedSwitchName).IsEqualTo("--no-allow-unauthenticated");
        await Assert.That(option.CSharpType).IsEqualTo("bool?");
        await Assert.That(option.IsFlag).IsTrue();
    }
}
