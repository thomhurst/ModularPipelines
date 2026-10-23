using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class DetachedHeadBranchTests : TestBase
{
    [Test]
    [Arguments("TF_BUILD", "BUILD_SOURCEBRANCH", "refs/heads/feature/topic", "feature/topic")]
    [Arguments("GITLAB_CI", "CI_COMMIT_REF_NAME", "feature/topic", "feature/topic")]
    [Arguments("GITHUB_ACTIONS", "GITHUB_REF_NAME", "feature/topic", "feature/topic")]
    [Arguments("JENKINS_URL", "BRANCH_NAME", "feature/topic", "feature/topic")]
    [Arguments("BITBUCKET_BUILD_NUMBER", "BITBUCKET_BRANCH", "feature/topic", "feature/topic")]
    [Arguments("TRAVIS", "TRAVIS_BRANCH", "feature/topic", "feature/topic")]
    [Arguments("APPVEYOR", "APPVEYOR_REPO_BRANCH", "feature/topic", "feature/topic")]
    public async Task Detached_Head_Uses_Detected_Providers_Branch(
        string marker, string variable, string value, string expected)
    {
        var (context, logger) = await CreateContext(new()
        {
            [marker] = "true",
            [variable] = value,
        });

        var info = await context.Tools.Git.Information.GetRequiredInfoAsync();

        await Assert.That(info.BranchName).IsEqualTo(expected);
        await Assert.That(logger.Invocations.Any(IsWarning)).IsFalse();
        await Assert.That(await new RunIfBranchAttribute(expected).EvaluateAsync(context, default)).IsTrue();
        await Assert.That(await new RunOnlyOnBranchAttribute(expected).EvaluateAsync(context, default)).IsTrue();
        await Assert.That(await new SkipIfBranchAttribute(expected).EvaluateAsync(context, default)).IsTrue();
        await Assert.That(await new RunIfBranchStartsWithAttribute("feature/").EvaluateAsync(context, default)).IsTrue();
        await Assert.That(await new RunOnlyIfBranchStartsWithAttribute("feature/").EvaluateAsync(context, default)).IsTrue();
        await Assert.That(await new RunIfBranchAttribute("main").EvaluateAsync(context, default)).IsFalse();
        await Assert.That(await new SkipIfBranchAttribute("main").EvaluateAsync(context, default)).IsFalse();
    }

    [Test]
    [Arguments("TF_BUILD", "SYSTEM_PULLREQUEST_SOURCEBRANCH", "BUILD_SOURCEBRANCH", "refs/pull/42/merge")]
    [Arguments("GITHUB_ACTIONS", "GITHUB_HEAD_REF", "GITHUB_REF_NAME", "42/merge")]
    [Arguments("GITLAB_CI", "CI_MERGE_REQUEST_SOURCE_BRANCH_NAME", "CI_COMMIT_REF_NAME", "refs/merge-requests/42/merge")]
    [Arguments("JENKINS_URL", "CHANGE_BRANCH", "BRANCH_NAME", "PR-42")]
    [Arguments("TRAVIS", "TRAVIS_PULL_REQUEST_BRANCH", "TRAVIS_BRANCH", "main")]
    [Arguments("APPVEYOR", "APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH", "APPVEYOR_REPO_BRANCH", "main")]
    public async Task Pull_Request_Uses_Source_Branch(
        string marker, string sourceVariable, string refVariable, string mergeRef)
    {
        var (context, _) = await CreateContext(new()
        {
            [marker] = "true",
            [sourceVariable] = "refs/heads/feature/topic",
            [refVariable] = mergeRef,
        });

        var info = await context.Tools.Git.Information.GetRequiredInfoAsync();

        await Assert.That(info.BranchName).IsEqualTo("feature/topic");
    }

    [Test]
    [Arguments("TF_BUILD", "SYSTEM_PULLREQUEST_SOURCEBRANCH", "BUILD_SOURCEBRANCH")]
    [Arguments("GITHUB_ACTIONS", "GITHUB_HEAD_REF", "GITHUB_REF_NAME")]
    [Arguments("GITLAB_CI", "CI_MERGE_REQUEST_SOURCE_BRANCH_NAME", "CI_COMMIT_REF_NAME")]
    [Arguments("JENKINS_URL", "CHANGE_BRANCH", "BRANCH_NAME")]
    [Arguments("TRAVIS", "TRAVIS_PULL_REQUEST_BRANCH", "TRAVIS_BRANCH")]
    [Arguments("APPVEYOR", "APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH", "APPVEYOR_REPO_BRANCH")]
    public async Task Blank_Source_Branch_Falls_Back_To_Branch_Variable(
        string marker, string sourceVariable, string branchVariable)
    {
        var (context, _) = await CreateContext(new()
        {
            [marker] = "true",
            [sourceVariable] = "  ",
            [branchVariable] = "main",
        });

        var info = await context.Tools.Git.Information.GetRequiredInfoAsync();

        await Assert.That(info.BranchName).IsEqualTo("main");
    }

    [Test]
    public async Task Local_Branch_Takes_Precedence_Over_Ci_Branch()
    {
        var (context, logger) = await CreateContext(new()
        {
            ["GITHUB_ACTIONS"] = "true",
            ["GITHUB_HEAD_REF"] = "feature/topic",
        }, "local-branch");

        var info = await context.Tools.Git.Information.GetRequiredInfoAsync();

        await Assert.That(info.BranchName).IsEqualTo("local-branch");
        await Assert.That(logger.Invocations.Any(IsWarning)).IsFalse();
    }

    [Test]
    public async Task AppVeyor_Non_Tag_Build_Uses_Branch()
    {
        var (context, _) = await CreateContext(new()
        {
            ["APPVEYOR"] = "true",
            ["APPVEYOR_REPO_TAG"] = "false",
            ["APPVEYOR_REPO_BRANCH"] = "main",
        });

        await Assert.That((await context.Tools.Git.Information.GetRequiredInfoAsync()).BranchName).IsEqualTo("main");
    }

    [Test]
    [Arguments(null)]
    [Arguments("GITHUB_ACTIONS")]
    public async Task Unrelated_Provider_Branch_Variables_Are_Ignored(string? marker)
    {
        var variables = new Dictionary<string, string?>
        {
            ["BUILD_SOURCEBRANCH"] = "refs/heads/main",
            ["CI_COMMIT_REF_NAME"] = "main",
            ["GITHUB_HEAD_REF"] = "main",
            ["GITHUB_REF_NAME"] = "main",
            ["BRANCH_NAME"] = "main",
            ["BITBUCKET_BRANCH"] = "main",
            ["TRAVIS_BRANCH"] = "main",
            ["APPVEYOR_REPO_BRANCH"] = "main",
        };
        if (marker is not null)
        {
            variables[marker] = "true";
            variables.Remove("GITHUB_HEAD_REF");
            variables.Remove("GITHUB_REF_NAME");
        }

        var (context, logger) = await CreateContext(variables);
        var information = context.Tools.Git.Information;

        await Assert.That((await information.GetRequiredInfoAsync()).BranchName).IsNull();
        await information.GetInfoAsync();

        var warning = logger.Invocations.Single(IsWarning);
        await Assert.That(warning.Arguments[2]?.ToString()).Contains("branch");
    }

    [Test]
    [Arguments("TF_BUILD", "BUILD_SOURCEBRANCH", "refs/tags/main", null, null)]
    [Arguments("TF_BUILD", "BUILD_SOURCEBRANCH", "refs/pull/42/merge", null, null)]
    [Arguments("TF_BUILD", "BUILD_SOURCEBRANCH", "refs/heads/", null, null)]
    [Arguments("GITLAB_CI", "CI_COMMIT_REF_NAME", "main", "CI_COMMIT_TAG", "main")]
    [Arguments("GITHUB_ACTIONS", "GITHUB_REF_NAME", "main", "GITHUB_REF_TYPE", "tag")]
    [Arguments("JENKINS_URL", "BRANCH_NAME", "main", "TAG_NAME", "main")]
    [Arguments("BITBUCKET_BUILD_NUMBER", "BITBUCKET_BRANCH", "main", "BITBUCKET_TAG", "main")]
    [Arguments("TRAVIS", "TRAVIS_BRANCH", "main", "TRAVIS_TAG", "main")]
    [Arguments("APPVEYOR", "APPVEYOR_REPO_BRANCH", "main", "APPVEYOR_REPO_TAG", "true")]
    [Arguments("APPVEYOR", "APPVEYOR_REPO_BRANCH", "main", "APPVEYOR_REPO_TAG_NAME", "main")]
    [Arguments("BITBUCKET_BUILD_NUMBER", "BITBUCKET_BRANCH", "  ", null, null)]
    [Arguments("TRAVIS", "TRAVIS_BRANCH", null, null, null)]
    [Arguments("APPVEYOR", "APPVEYOR_REPO_BRANCH", "refs/tags/main", null, null)]
    [Arguments("GITHUB_ACTIONS", "GITHUB_REF_NAME", "  ", null, null)]
    [Arguments("GITHUB_ACTIONS", "GITHUB_REF_NAME", null, null, null)]
    public async Task Missing_Branches_And_Tags_Remain_Unknown_And_Warn(
        string marker, string variable, string? value, string? tagVariable, string? tagValue)
    {
        var variables = new Dictionary<string, string?>
        {
            [marker] = "true",
            [variable] = value,
        };
        if (tagVariable is not null)
        {
            variables[tagVariable] = tagValue;
        }

        var (context, logger) = await CreateContext(variables);
        var information = context.Tools.Git.Information;

        await Assert.That((await information.GetRequiredInfoAsync()).BranchName).IsNull();
        await information.GetInfoAsync();
        await Assert.That(logger.Invocations.Count(IsWarning)).IsEqualTo(1);
        await Assert.That(await new RunIfBranchAttribute("main").EvaluateAsync(context, default)).IsFalse();
        await Assert.That(await new SkipIfBranchAttribute("main").EvaluateAsync(context, default)).IsFalse();
    }

    private async Task<(IPipelineContext Context, Mock<ILogger> Logger)> CreateContext(
        Dictionary<string, string?> variables, string branch = "")
    {
        var environment = new Mock<IEnvironmentVariablesContext>();
        environment.Setup(x => x.Get(It.IsAny<string>(), EnvironmentVariableTarget.Process))
            .Returns((string name, EnvironmentVariableTarget _) => variables.GetValueOrDefault(name));
        var logger = new Mock<ILogger>();
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
        var command = new Mock<ICommandContext>();
        command.Setup(x => x.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CommandLineToolOptions options, CommandExecutionOptions? _, CancellationToken _) =>
                CommandResult.Ok(options switch
                {
                    GitRevParseOptions { ShowToplevel: true } => TestContext.WorkingDirectory,
                    GitBranchOptions { ShowCurrent: true } => branch,
                    CommandLineToolOptions { Tool: "git" } => "origin/main",
                    _ => string.Empty,
                }));
        var runner = new Mock<IGitCommandRunner>();
        var result = await GetService<IPipelineContext>(services =>
        {
            services.AddSingleton(environment.Object);
            services.AddSingleton<ILogger<GitInformation>>(new Logger<GitInformation>(loggerFactory.Object));
            services.AddSingleton(command.Object);
            services.AddSingleton(runner.Object);
        });
        return (result.T, logger);
    }

    private static bool IsWarning(Moq.IInvocation invocation) =>
        invocation.Method.Name == nameof(ILogger.Log) &&
        invocation.Arguments[0] is LogLevel.Warning;
}

