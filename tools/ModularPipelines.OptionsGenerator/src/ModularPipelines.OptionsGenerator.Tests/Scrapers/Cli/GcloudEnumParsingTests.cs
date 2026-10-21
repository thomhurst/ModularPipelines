using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudEnumParsingTests
{
    [Test]
    [Arguments("VALUE must be one of:", "IMMEDIATE, NEXT_AVAILABLE_WINDOW, or SPECIFIC-TIME", "IMMEDIATE,NEXT_AVAILABLE_WINDOW,SPECIFIC-TIME")]
    [Arguments("VALUE must be one of:", "alpha, beta, and gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, or beta", "alpha,beta")]
    [Arguments("VALUE must be one of:", "alpha, AND beta", "alpha,beta")]
    [Arguments("VALUE must be one of:", "alpha, beta, gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, or", "alpha,or")]
    [Arguments("VALUE must be one of:", "alpha, and", "alpha,and")]
    [Arguments("VALUE must be one of:", "alpha, orange, android", "alpha,orange,android")]
    [Arguments("Select a value; one of", "alpha, beta, or gamma", "alpha,beta,gamma")]
    [Arguments("Select a value; one of", "alpha, and beta", "alpha,beta")]
    [Arguments("Select a value; one of", "alpha, beta", "alpha,beta")]
    [Arguments("Select a value; one of", "alpha, or", "alpha,or")]
    public async Task Enum_Lists_Preserve_Values_Without_Conjunction_Prose(string prefix, string choices, string expected)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                 --value=VALUE
                    {{prefix}} {{choices}}.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var definition = command.Options.Single().EnumDefinition;

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(expected.Split(','));
    }

    [Test]
    public async Task Required_Reschedule_Type_Generates_Specific_Time_Enum_Value()
    {
        const string help = """
            NAME
                gcloud redis instances reschedule-maintenance - reschedule maintenance
            SYNOPSIS
                gcloud redis instances reschedule-maintenance INSTANCE --reschedule-type=RESCHEDULE_TYPE
            POSITIONAL ARGUMENTS
                 INSTANCE
                    The Redis instance to reschedule.
            REQUIRED FLAGS
                 --reschedule-type=RESCHEDULE_TYPE
                    RESCHEDULE_TYPE must be one of: IMMEDIATE, NEXT_AVAILABLE_WINDOW, or
                    SPECIFIC-TIME.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("redis instances reschedule-maintenance", help)).Single();
        var option = command.Options.Single();
        await Assert.That(option.IsRequired).IsTrue();
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["IMMEDIATE", "NEXT_AVAILABLE_WINDOW", "SPECIFIC-TIME"]);

        var generated = (await new EnumGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;

        await Assert.That(generated).Contains("[EnumValue(\"SPECIFIC-TIME\")]");
        await Assert.That(generated).DoesNotContain("[EnumValue(\"or\")]");
    }
}
