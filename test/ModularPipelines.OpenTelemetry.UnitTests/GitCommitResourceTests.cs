using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.TestHelpers;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry.UnitTests;

[TUnit.Core.NotInParallel]
public class GitCommitResourceTests
{
    private static readonly string[] CommitVariables =
    [
        "GITHUB_SHA", "CI_COMMIT_SHA", "BUILD_SOURCEVERSION", "BITBUCKET_COMMIT",
        "CODEBUILD_RESOLVED_SOURCE_VERSION", "TRAVIS_COMMIT", "APPVEYOR_REPO_COMMIT",
    ];

    [Test]
    [Arguments("GITHUB_SHA")]
    [Arguments("CI_COMMIT_SHA")]
    [Arguments("BUILD_SOURCEVERSION")]
    [Arguments("BITBUCKET_COMMIT")]
    [Arguments("CODEBUILD_RESOLVED_SOURCE_VERSION")]
    [Arguments("TRAVIS_COMMIT")]
    [Arguments("APPVEYOR_REPO_COMMIT")]
    public Task Ci_Commit_Is_Added_To_The_Resource(string variable) =>
        WithCommitEnvironment(new() { [variable] = "commit-sha" }, async () =>
            await Assert.That(await ReadCommitResource()).IsEqualTo("commit-sha"));

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    public Task Existing_Commit_Precedence_Is_Preserved(int firstVariable)
    {
        var variables = CommitVariables.Select((name, index) => (name, index))
            .ToDictionary(pair => pair.name, pair => pair.index < firstVariable ? "  " : pair.name);
        return WithCommitEnvironment(variables, async () =>
            await Assert.That(await ReadCommitResource()).IsEqualTo(CommitVariables[firstVariable]));
    }

    [Test]
    public Task Missing_Commit_Is_Not_Added_To_The_Resource() =>
        WithCommitEnvironment([], async () => await Assert.That(await ReadCommitResource()).IsNull());

    private static async Task WithCommitEnvironment(Dictionary<string, string> variables, Func<Task> test)
    {
        var previous = CommitVariables.ToDictionary(name => name, Environment.GetEnvironmentVariable);
        try
        {
            foreach (var name in CommitVariables)
            {
                Environment.SetEnvironmentVariable(name, variables.GetValueOrDefault(name));
            }

            await test();
        }
        finally
        {
            foreach (var (name, value) in previous)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private static async Task<string?> ReadCommitResource()
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddModule<TestModule>();
        builder.AddOpenTelemetry();
        await using var pipeline = await builder.BuildAsync();
        var provider = pipeline.Services.GetRequiredService<TracerProvider>();
        return provider.GetResource().Attributes.FirstOrDefault(pair => pair.Key == "vcs.ref.head.revision").Value as string;
    }

    private sealed class TestModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
