﻿using ModularPipelines.Context;
using Moq;

namespace ModularPipelines.UnitTests;

public class BuildSystemDetectorTests : TestBase
{
    private readonly Mock<IEnvironmentVariables> _environmentVariables;
    
    private readonly BuildSystemDetector _buildSystemDetector;

    public BuildSystemDetectorTests()
    {
        _environmentVariables = new Mock<IEnvironmentVariables>();
        _buildSystemDetector = new BuildSystemDetector(_environmentVariables.Object);
    }

    [Test]
    public void When_No_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_False()
    {
        Assert.That(_buildSystemDetector.IsKnownBuildAgent, Is.False);
    }
    
    [TestCase("TF_BUILD")]
    [TestCase("TEAMCITY_VERSION")]
    [TestCase("GITHUB_ACTIONS")]
    [TestCase("JENKINS_URL")]
    [TestCase("GITLAB_CI")]
    [TestCase("BITBUCKET_BUILD_NUMBER")]
    [TestCase("TRAVIS")]
    [TestCase("APPVEYOR")]
    public void When_Known_BuildAgent_Variable_Then_IsKnownBuildAgent_Returns_True(string environmentVariableName)
    {
        _environmentVariables
            .Setup(x => x.GetEnvironmentVariable(environmentVariableName, It.IsAny<EnvironmentVariableTarget>()))
            .Returns("dummy value");
        
        Assert.That(_buildSystemDetector.IsKnownBuildAgent, Is.True);
    }

    [Test]
    public void Each_Property_Returns_Result()
    {
        Assert.That(_buildSystemDetector.IsRunningOnBitbucket, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnJenkins, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnAzurePipelines, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnTeamCity, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnGitHubActions, Is.True.Or.False);
        Assert.That(_buildSystemDetector.IsRunningOnAppVeyor, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnGitLab, Is.False);
        Assert.That(_buildSystemDetector.IsRunningOnTravisCI, Is.False);
    }
}