using ModularPipelines.Hadolint.Enums;
using ModularPipelines.Hadolint.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Hadolint.UnitTests.Attributes;

public class HadolintOptionsTests
{
    [Test]
    public async Task Execute_Renders_Dockerfiles_Enums_Collections_And_Flags()
    {
        var arguments = BuildArguments(new HadolintExecuteOptions
        {
            Dockerfiles = ["Dockerfile", "build/Dockerfile"],
            Format = HadolintFormat.Sarif,
            Error = ["DL3006", "DL3020"],
            Ignore = ["DL3008"],
            NoColor = true,
            FailureThreshold = HadolintFailureThreshold.Warning,
        });

        await AssertArguments(arguments,
        [
            "Dockerfile",
            "build/Dockerfile",
            "--no-color",
            "--format", "sarif",
            "--error", "DL3006",
            "--error", "DL3020",
            "--ignore", "DL3008",
            "--failure-threshold", "warning",
        ]);
    }

    [Test]
    public async Task Format_Renders_Underscored_Wire_Value()
    {
        var arguments = BuildArguments(new HadolintExecuteOptions
        {
            Format = HadolintFormat.GitlabCodeclimate,
        });

        await AssertArguments(arguments,
            ["--format", "gitlab_codeclimate"]);
    }

    [Test]
    public async Task Format_Renders_Junit_Wire_Value()
    {
        var arguments = BuildArguments(new HadolintExecuteOptions
        {
            Format = HadolintFormat.Junit,
        });

        await AssertArguments(arguments, ["--format", "junit"]);
    }
}
