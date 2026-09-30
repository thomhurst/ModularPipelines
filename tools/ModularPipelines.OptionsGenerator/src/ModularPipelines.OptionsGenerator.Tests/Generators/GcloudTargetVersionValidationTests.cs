using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Gcloud_Lustre_Target_Version_Is_Independent_Of_Nested_Squash_Bundle()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "587.0.0",
            "gcloud-lustre-instances-create.txt"));
        var command = (await new GcloudResourceArgumentTests.TestScraper().Parse(["gcloud", "lustre", "instances", "create"], help))!;
        // --target-version is a standalone optional flag in SYNOPSIS; only the squash bundle
        // shares the "must be specified if any of the other arguments" requirement.
        await Assert.That(command.RequiredAlternativeGroups.Any(group => group.PropertyNames.Contains("TargetVersion"))).IsFalse();
        var groups = command.RequiredAlternativeGroups
            .Where(group => group.PropertyNames.Contains("DefaultSquashMode"))
            .ToList();
        await ValidateCapturedGroups(command, groups,
        [
            ("", true),
            ("TargetVersion", true),
            ("TargetVersion,DefaultSquashMode", true),
            ("TargetVersion,DefaultSquashGid", false),
            ("DefaultSquashMode", true),
            ("DefaultSquashMode,DefaultSquashGid", true),
            ("DefaultSquashGid", false),
        ], additionalProperties: ["TargetVersion"]);
    }
}
