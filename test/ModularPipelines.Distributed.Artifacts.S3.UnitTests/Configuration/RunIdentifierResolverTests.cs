using ModularPipelines.Distributed.Artifacts.S3.Configuration;

namespace ModularPipelines.Distributed.Artifacts.S3.UnitTests.Configuration;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class RunIdentifierResolverTests
{
    [Test]
    public async Task Standard_Run_Id_Is_Used_Before_CI_Commit()
    {
        var previousRunId = Environment.GetEnvironmentVariable("MODULARPIPELINES_RUN_ID");
        var previousGitHubSha = Environment.GetEnvironmentVariable("GITHUB_SHA");

        try
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", "pipeline-run");
            Environment.SetEnvironmentVariable("GITHUB_SHA", "github-sha");

            await Assert.That(RunIdentifierResolver.Resolve(null)).IsEqualTo("pipeline-run");
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULARPIPELINES_RUN_ID", previousRunId);
            Environment.SetEnvironmentVariable("GITHUB_SHA", previousGitHubSha);
        }
    }
}
