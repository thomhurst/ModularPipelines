namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudEmptyValueTests
{
    [Test]
    [Arguments("An empty string means that the default behavior is used.", true)]
    [Arguments("EMPTY STRING means default behavior.", true)]
    [Arguments("An empty string is not allowed.", false)]
    [Arguments("An empty string means an invalid setting.", false)]
    [Arguments("The mapping rules to use.", false)]
    public async Task Only_Explicit_Default_Reset_Descriptions_Allow_Empty_Values(string description, bool expected)
    {
        var help = $$"""
            NAME
                gcloud example update - Update settings.
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                --value=VALUE
                    {{description}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        await Assert.That(command.Options.Single().AllowsEmptyValue).IsEqualTo(expected);
    }

    [Test]
    public async Task Inherited_Group_Prose_Does_Not_Allow_Empty_Option_Values()
    {
        const string help = """
            NAME
                gcloud example update - Update settings.
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                Arguments for configuration:
                    An empty string means that the default behavior is used.

                    --value=VALUE
                        The mapping rules to use.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var option = command.Options.Single();
        await Assert.That(option.Description).Contains("An empty string");
        await Assert.That(option.AllowsEmptyValue).IsFalse();
    }
}
