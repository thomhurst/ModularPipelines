using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.TestHelpers;
using Moq;
using TUnit.Assertions;
using TUnit.Core;

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
    public void When_No_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_False()
    {
        Assert.That(_buildSystemDetector.IsKnownBuildAgent).Is.False();
    }

    [TestWithData("TF_BUILD")]
    [TestWithData("TEAMCITY_VERSION")]
    [TestWithData("GITHUB_ACTIONS")]
    [TestWithData("JENKINS_URL")]
    [TestWithData("GITLAB_CI")]
    [TestWithData("BITBUCKET_BUILD_NUMBER")]
    [TestWithData("TRAVIS")]
    [TestWithData("APPVEYOR")]
    public void When_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_True(string environmentVariableName)
    {
        _environmentVariables
            .Setup(x => x.GetEnvironmentVariable(environmentVariableName, It.IsAny<EnvironmentVariableTarget>()))
            .Returns("dummy value");

        Assert.That(_buildSystemDetector.IsKnownBuildAgent).Is.True();
    }

    [Test]
    public void Each_Property_Returns_Result()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_buildSystemDetector.IsRunningOnBitbucket).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnJenkins).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnAzurePipelines).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnTeamCity).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnGitHubActions).Is.True().Or.Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnAppVeyor).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnGitLab).Is.False();
            Assert.That(_buildSystemDetector.IsRunningOnTravisCI).Is.False();
        });
    }

    [TestWithData("TF_BUILD", BuildSystem.AzurePipelines)]
    [TestWithData("TEAMCITY_VERSION", BuildSystem.TeamCity)]
    [TestWithData("GITHUB_ACTIONS", BuildSystem.GitHubActions)]
    [TestWithData("JENKINS_URL", BuildSystem.Jenkins)]
    [TestWithData("GITLAB_CI", BuildSystem.GitLab)]
    [TestWithData("BITBUCKET_BUILD_NUMBER", BuildSystem.Bitbucket)]
    [TestWithData("TRAVIS", BuildSystem.TravisCI)]
    [TestWithData("APPVEYOR", BuildSystem.AppVeyor)]
    [TestWithData("blah", BuildSystem.Unknown)]
    public void Expected_Build_Agent(string environmentVariableName, BuildSystem expectedBuildSystem)
    {
        _environmentVariables
            .Setup(x => x.GetEnvironmentVariable(environmentVariableName, It.IsAny<EnvironmentVariableTarget>()))
            .Returns("dummy value");

        Assert.That(_buildSystemDetector.GetCurrentBuildSystem()).Is.EqualTo(expectedBuildSystem);
    }
}