using ModularPipelines.NerdbankGitVersioning.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.NerdbankGitVersioning.UnitTests.Attributes;

public class NbgvOptionsTests
{
    [Test]
    public async Task GetVersion_Renders_Commit_And_Explicit_Boolean()
    {
        var arguments = BuildArguments(new NbgvGetVersionOptions
        {
            CommitIsh = "HEAD~1",
            Project = "src/App",
            Metadata = "ci",
            PublicRelease = false,
        });

        await AssertArguments(arguments,
        [
            "HEAD~1",
            "--project", "src/App",
            "--metadata", "ci",
            "--public-release=false",
        ]);
    }

    [Test]
    public async Task Cloud_Renders_Flags_And_Repeated_Definitions()
    {
        var arguments = BuildArguments(new NbgvCloudOptions
        {
            AllVars = true,
            SkipCloudBuildNumber = true,
            Define = ["Name=Value", "Channel=stable"],
        });

        await AssertArguments(arguments,
        [
            "--all-vars",
            "--skip-cloud-build-number",
            "--define", "Name=Value",
            "--define", "Channel=stable",
        ]);
    }

    [Test]
    public async Task PrepareRelease_Renders_Tag_And_CamelCase_Options()
    {
        var arguments = BuildArguments(new NbgvPrepareReleaseOptions
        {
            Tag = "v2.0",
            NextVersion = "2.1",
            VersionIncrement = "minor",
            WhatIf = true,
        });

        await AssertArguments(arguments,
        [
            "v2.0",
            "--nextVersion", "2.1",
            "--versionIncrement", "minor",
            "--what-if",
        ]);
    }
}
