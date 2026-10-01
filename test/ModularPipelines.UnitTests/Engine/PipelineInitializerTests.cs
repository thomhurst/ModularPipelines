using System.Collections;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors;
using ModularPipelines.UnitTests.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineInitializerTests
{
    [Test]
    public async Task EnvironmentVariablesTable_Does_Not_Add_Blank_Rows()
    {
        var variables = new Hashtable
        {
            ["FIRST"] = "one",
            ["SECOND"] = "two",
        };

        var table = PipelineInitializer.CreateEnvironmentVariablesTable(
            variables,
            value => value);

        await Assert.That(table.Rows).Count().IsEqualTo(2);
    }

    [Test]
    public async Task BuildSystemDetection_Logs_Once_When_Only_CI_Variable_Marks_Build_Server()
    {
        var output = LogBuildSystemDetection(("CI", "1"));

        await Assert.That(output).Contains("Build System: Unknown");
        await Assert.That(output)
            .Contains("Treating this run as CI because CI=1 and no known build agent was detected.");
        await Assert.That(CountOccurrences(output, "Treating this run as CI")).IsEqualTo(1);
    }

    [Test]
    public async Task BuildSystemDetection_Does_Not_Log_CI_Variable_When_Known_Agent_Detected()
    {
        var output = LogBuildSystemDetection(("CI", "true"), ("GITHUB_ACTIONS", "true"));

        await Assert.That(output).Contains("Build System: GitHubActions (detected from GITHUB_ACTIONS)");
        await Assert.That(output).DoesNotContain("Treating this run as CI");
    }

    [Test]
    public async Task BuildSystemDetection_Does_Not_Log_CI_Variable_When_CI_Is_False()
    {
        var output = LogBuildSystemDetection(("CI", "false"));

        await Assert.That(output).DoesNotContain("Treating this run as CI");
    }

    private static string LogBuildSystemDetection(params (string Name, string Value)[] variables)
    {
        var environmentVariables = new Mock<IEnvironmentVariablesContext>();
        foreach (var (name, value) in variables)
        {
            environmentVariables
                .Setup(context => context.Get(name, It.IsAny<EnvironmentVariableTarget>()))
                .Returns(value);
        }

        var output = new StringBuilder();
        PipelineInitializer.LogBuildSystemDetection(
            new StringLogger<PipelineInitializer>(output),
            new BuildSystemDetector(environmentVariables.Object));
        return output.ToString();
    }

    private static int CountOccurrences(string value, string search)
    {
        var count = 0;
        var index = 0;
        while ((index = value.IndexOf(search, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += search.Length;
        }

        return count;
    }
}
