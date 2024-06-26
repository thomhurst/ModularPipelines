using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.TestHelpers;
using Moq;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests;

public class BuildSystemDetectorTests : TestBase
{
    private readonly Mock<IEnvironmentVariables> _environmentVariables;

    private readonly IBuildSystemDetector _buildSystemDetector;

    public BuildSystemDetectorTests()
    {
        _environmentVariables = new Mock<IEnvironmentVariables>();
        _buildSystemDetector = new BuildSystemDetector(_environmentVariables.Object);
    }

    [Test]
    public async Task When_No_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_False()
    {
        await Assert.That(_buildSystemDetector.IsKnownBuildAgent).Is.False();
    }

    [DataDrivenTest]
    [Arguments("TF_BUILD")]
    [Arguments("TEAMCITY_VERSION")]
    [Arguments("GITHUB_ACTIONS")]
    [Arguments("JENKINS_URL")]
    [Arguments("GITLAB_CI")]
    [Arguments("BITBUCKET_BUILD_NUMBER")]
    [Arguments("TRAVIS")]
    [Arguments("APPVEYOR")]
    public async Task When_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_True(string environmentVariableName)
    {
        _environmentVariables
            .Setup(x => x.GetEnvironmentVariable(environmentVariableName, It.IsAny<EnvironmentVariableTarget>()))
            .Returns("dummy value");
        await Assert.That(_buildSystemDetector.IsKnownBuildAgent).Is.True();
    }

    [Test]
    public async Task Each_Property_Returns_Result()
    {
        await using (Assert.Multiple())
        {
            await Assert.That(_buildSystemDetector.IsRunningOnBitbucket).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnJenkins).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnAzurePipelines).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnTeamCity).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnGitHubActions).Is.True().Or.Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnAppVeyor).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnGitLab).Is.False();
            await Assert.That(_buildSystemDetector.IsRunningOnTravisCI).Is.False();
        }
    }

    [DataDrivenTest]
    [Arguments("TF_BUILD", BuildSystem.AzurePipelines)]
    [Arguments("TEAMCITY_VERSION", BuildSystem.TeamCity)]
    [Arguments("GITHUB_ACTIONS", BuildSystem.GitHubActions)]
    [Arguments("JENKINS_URL", BuildSystem.Jenkins)]
    [Arguments("GITLAB_CI", BuildSystem.GitLab)]
    [Arguments("BITBUCKET_BUILD_NUMBER", BuildSystem.Bitbucket)]
    [Arguments("TRAVIS", BuildSystem.TravisCI)]
    [Arguments("APPVEYOR", BuildSystem.AppVeyor)]
    [Arguments("blah", BuildSystem.Unknown)]
    public async Task Expected_Build_Agent(string environmentVariableName, BuildSystem expectedBuildSystem)
    {
        _environmentVariables
            .Setup(x => x.GetEnvironmentVariable(environmentVariableName, It.IsAny<EnvironmentVariableTarget>()))
            .Returns("dummy value");
        await Assert.That(_buildSystemDetector.GetCurrentBuildSystem()).Is.EqualTo(expectedBuildSystem);
    }
}