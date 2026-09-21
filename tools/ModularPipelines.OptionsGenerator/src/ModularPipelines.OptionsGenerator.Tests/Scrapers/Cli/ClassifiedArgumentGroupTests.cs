using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Choice_Heading_Depth_Determines_Peer_Or_Nested_Group(bool nested)
    {
        var indentation = nested ? "    " : "  ";
        var section = $"""
              Options for configuring the instance.

            {indentation}--size=SIZE
            {indentation}   The instance size.

            {indentation}At most one of these can be specified:

            {indentation}  --region=REGION
            {indentation}     The region.

            {indentation}  --zone=ZONE
            {indentation}     The zone.
            """;
        var root = TestArgumentGroupScraper.ParseGroups(section);
        await Assert.That(root.Groups).Count().IsEqualTo(nested ? 1 : 2);
        var choice = nested ? root.Groups.Single().Groups.Single() : root.Groups[1];
        await Assert.That(choice.Kind).IsEqualTo(CliArgumentGroupKind.AtMostOne);
        await Assert.That(choice.Arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--region", "--zone"]);
        await Assert.That(root.Groups[0].Arguments.Single().SwitchName).IsEqualTo("--size");
    }

    [Test]
    public async Task Classifiable_Sibling_Headings_Preserve_The_Outer_Choice()
    {
        const string section = """
            Exactly one of these must be specified:
              First branch settings.
              --first=FIRST
                 The first branch value.

              At least one of these must be specified:
              --second=SECOND
                 The second branch value.

              At most one of these may be specified:
              --third=THIRD
                 The third branch value.
            """;
        var root = TestArgumentGroupScraper.ParseGroups(section);
        var choice = root.Groups.Single();
        await Assert.That(choice.Kind.HasFlag(CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne)).IsTrue();
        await Assert.That(choice.Arguments).IsEmpty();
        await Assert.That(choice.Groups).Count().IsEqualTo(3);
        await Assert.That(choice.Groups.Select(group => group.Arguments.Single().SwitchName))
            .IsEquivalentTo(["--first", "--second", "--third"]);
        await Assert.That(choice.Groups[1].Kind).IsEqualTo(CliArgumentGroupKind.AtLeastOne);
        await Assert.That(choice.Groups[2].Kind).IsEqualTo(CliArgumentGroupKind.AtMostOne);
    }

    [Test]
    [Arguments("At most one of these can be specified:", CliArgumentGroupKind.AtMostOne)]
    [Arguments("At least one of these must be specified:", CliArgumentGroupKind.AtLeastOne)]
    [Arguments("Or use these options:", CliArgumentGroupKind.Alternative)]
    [Arguments("Arguments for authentication:", CliArgumentGroupKind.Resource)]
    public async Task Narrative_Asides_Preserve_Classified_Group_Membership(string heading, CliArgumentGroupKind kind)
    {
        var section = $"""
            {heading}
              --token=TOKEN
                 Authenticate with a token.

              Note: authentication is checked before execution.
              --profile=PROFILE
                 Select a saved profile.
            """;
        var root = TestArgumentGroupScraper.ParseGroups(section);
        var group = root.Groups.Single();
        await Assert.That(group.Kind).IsEqualTo(kind);
        await Assert.That(group.Arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--token", "--profile"]);
        await Assert.That(group.Description).Contains("authentication is checked before execution");
    }

    [Test]
    [Arguments("Entraid configuration for the SQL Server instance.")]
    [Arguments("Options for configuring read pool auto scale.")]
    public async Task Configuration_Headings_After_Narrative_Start_Sibling_Groups(string heading)
    {
        var section = $"""
            Arguments for authentication:
              --token=TOKEN
                 Authenticate with a token.

              Note: authentication is checked before execution.
              {heading}

              --setting=SETTING
                 Configure the independent setting.
            """;
        var root = TestArgumentGroupScraper.ParseGroups(section);
        await Assert.That(root.Groups).Count().IsEqualTo(2);
        await Assert.That(root.Groups[0].Arguments.Single().SwitchName).IsEqualTo("--token");
        await Assert.That(root.Groups[1].Arguments.Single().SwitchName).IsEqualTo("--setting");
    }

    [Test]
    [Arguments("At most one of these can be specified:", CliArgumentGroupKind.AtMostOne)]
    [Arguments("Arguments for authentication:", CliArgumentGroupKind.Resource)]
    public async Task Configuration_Prose_Without_A_Heading_Break_Preserves_Group_Membership(
        string heading, CliArgumentGroupKind kind)
    {
        var section = $"""
            {heading}
              --token=TOKEN
                 Authenticate with a token.

              Options for authentication include OAuth and API tokens.
              --profile=PROFILE
                 Select a saved profile.
            """;
        var group = TestArgumentGroupScraper.ParseGroups(section).Groups.Single();
        await Assert.That(group.Kind).IsEqualTo(kind);
        await Assert.That(group.Arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--token", "--profile"]);
    }

    [Test]
    public async Task Explicit_Headings_Can_Start_Siblings_After_Classified_Groups()
    {
        const string section = """
            At most one of these can be specified:
              --token=TOKEN
                 Authenticate with a token.

              At least one of these must be specified:
              --profile=PROFILE
                 Select a saved profile.
            """;
        var root = TestArgumentGroupScraper.ParseGroups(section);
        await Assert.That(root.Groups).Count().IsEqualTo(2);
        await Assert.That(root.Groups[0].Kind).IsEqualTo(CliArgumentGroupKind.AtMostOne);
        await Assert.That(root.Groups[1].Kind).IsEqualTo(CliArgumentGroupKind.AtLeastOne);
    }
}
