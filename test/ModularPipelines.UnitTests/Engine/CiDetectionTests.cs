using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Enums;
using ModularPipelines.Requirements;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

/// <summary>
/// Verifies that every CI check (<see cref="OnCI"/>, <see cref="OnLocal"/>, <c>Require.Ci</c>,
/// <c>Require.LocalEnvironment</c> and <see cref="IBuildSystemContext.IsBuildServer"/>) shares one definition.
/// </summary>
public class CiDetectionTests
{
    [Test]
    [Arguments(null, null, false)]
    [Arguments("true", null, true)]
    [Arguments("1", null, true)]
    [Arguments("false", null, false)]
    [Arguments("FALSE", null, false)]
    [Arguments("0", null, false)]
    [Arguments("", null, false)]
    [Arguments(null, "GITHUB_ACTIONS", true)]
    [Arguments("false", "GITHUB_ACTIONS", true)]
    [Arguments("false", "TF_BUILD", true)]
    public async Task All_Ci_Checks_Agree(string? ciValue, string? buildSystemVariable, bool expectedCi)
    {
        var context = CreateContext(ciValue, buildSystemVariable);

        var onCi = await new OnCI().EvaluateAsync(context, CancellationToken.None);
        var onLocal = await new OnLocal().EvaluateAsync(context, CancellationToken.None);
        var requireCi = await Require.Ci().EvaluateAsync(context, CancellationToken.None);
        var requireLocal = await Require.LocalEnvironment().EvaluateAsync(context, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(context.Environment.BuildSystem.IsBuildServer).IsEqualTo(expectedCi);
            await Assert.That(context.IsRunningInCI()).IsEqualTo(expectedCi);
            await Assert.That(context.IsRunningLocally()).IsEqualTo(!expectedCi);
            await Assert.That(onCi).IsEqualTo(expectedCi);
            await Assert.That(onLocal).IsEqualTo(!expectedCi);
            await Assert.That(requireCi.IsSatisfied).IsEqualTo(expectedCi);
            await Assert.That(requireLocal.IsSatisfied).IsEqualTo(!expectedCi);
        }
    }

    [Test]
    public async Task Detected_Build_System_Is_Exposed_Through_Is()
    {
        var buildSystem = CreateContext(null, "TEAMCITY_VERSION").Environment.BuildSystem;

        using (Assert.Multiple())
        {
            await Assert.That(buildSystem.Current).IsEqualTo(BuildSystem.TeamCity);
            await Assert.That(buildSystem.Is(BuildSystem.TeamCity)).IsTrue();
            await Assert.That(buildSystem.Is(BuildSystem.GitHubActions)).IsFalse();
        }
    }

    [Test]
    public async Task Generic_Ci_Variable_Does_Not_Imply_A_Known_Build_System()
    {
        var buildSystem = CreateContext("true", null).Environment.BuildSystem;

        using (Assert.Multiple())
        {
            await Assert.That(buildSystem.IsBuildServer).IsTrue();
            await Assert.That(buildSystem.Current).IsEqualTo(BuildSystem.Unknown);
        }
    }

    private static IPipelineContext CreateContext(string? ciValue, string? buildSystemVariable)
    {
        var variables = new Mock<IEnvironmentVariablesContext>();
        variables
            .Setup(x => x.Get("CI", It.IsAny<EnvironmentVariableTarget>()))
            .Returns(ciValue);
        if (buildSystemVariable is not null)
        {
            variables
                .Setup(x => x.Get(buildSystemVariable, It.IsAny<EnvironmentVariableTarget>()))
                .Returns("true");
        }

        var buildSystem = new BuildSystemContext(new BuildSystemDetector(variables.Object));
        var environment = new Mock<IEnvironmentContext>();
        environment.SetupGet(x => x.BuildSystem).Returns(buildSystem);
        environment.SetupGet(x => x.Variables).Returns(variables.Object);
        var context = new Mock<IPipelineContext>();
        context.SetupGet(x => x.Environment).Returns(environment.Object);
        return context.Object;
    }
}
