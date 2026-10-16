using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Azure.Pipelines.UnitTests;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class AzurePipelineVariablesTests
{
    [Test]
    [Arguments(typeof(AzurePipelineAgentVariables))]
    [Arguments(typeof(AzurePipelineBuildVariables))]
    [Arguments(typeof(AzurePipelinePullRequestVariables))]
    public async Task VariablePropertiesDeclareNullableValues(Type variableType)
    {
        var nullability = new NullabilityInfoContext();

        foreach (var property in variableType.GetProperties())
        {
            await Assert.That(nullability.Create(property).ReadState).IsEqualTo(NullabilityState.Nullable);
        }
    }

    [Test]
    [Arguments("Agent", "BuildDirectory", "AGENT_BUILDDIRECTORY")]
    [Arguments("Agent", "ContainerMapping", "AGENT_CONTAINERMAPPING")]
    [Arguments("Agent", "HomeDirectory", "AGENT_HOMEDIRECTORY")]
    [Arguments("Agent", "Id", "AGENT_ID")]
    [Arguments("Agent", "JobName", "AGENT_JOBNAME")]
    [Arguments("Agent", "JobStatus", "AGENT_JOBSTATUS")]
    [Arguments("Agent", "MachineName", "AGENT_MACHINENAME")]
    [Arguments("Agent", "Name", "AGENT_NAME")]
    [Arguments("Agent", "OS", "AGENT_OS")]
    [Arguments("Agent", "OSArchitecture", "AGENT_OSARCHITECTURE")]
    [Arguments("Agent", "TempDirectory", "AGENT_TEMPDIRECTORY")]
    [Arguments("Agent", "ToolsDirectory", "AGENT_TOOLSDIRECTORY")]
    [Arguments("Agent", "WorkFolder", "AGENT_WORKFOLDER")]
    [Arguments("Build", "SourceBranch", "BUILD_SOURCEBRANCH")]
    [Arguments("Build", "SourceVersion", "BUILD_SOURCEVERSION")]
    [Arguments("Build", "BuildId", "BUILD_BUILDID")]
    [Arguments("PullRequest", "IsFork", "SYSTEM_PULLREQUEST_ISFORK")]
    [Arguments("PullRequest", "PullRequestId", "SYSTEM_PULLREQUEST_PULLREQUESTID")]
    [Arguments("PullRequest", "PullRequestNumber", "SYSTEM_PULLREQUEST_PULLREQUESTNUMBER")]
    [Arguments("PullRequest", "SourceBranch", "SYSTEM_PULLREQUEST_SOURCEBRANCH")]
    [Arguments("PullRequest", "SourceCommitId", "SYSTEM_PULLREQUEST_SOURCECOMMITID")]
    [Arguments("PullRequest", "SourceRepositoryUri", "SYSTEM_PULLREQUEST_SOURCEREPOSITORYURI")]
    [Arguments("PullRequest", "TargetBranch", "SYSTEM_PULLREQUEST_TARGETBRANCH")]
    [Arguments("PullRequest", "TargetBranchName", "SYSTEM_PULLREQUEST_TARGETBRANCHNAME")]
    public async Task VariablesReadCurrentEnvironment(string group, string propertyName, string variableName)
    {
        var previousValue = Environment.GetEnvironmentVariable(variableName);

        try
        {
            var variables = new AzurePipelineVariables(new AzurePipelineAgentVariables());
            object variableGroup = group switch
            {
                "Agent" => variables.Agent,
                "Build" => variables.Build,
                "PullRequest" => variables.System.PullRequest,
                _ => throw new ArgumentOutOfRangeException(nameof(group)),
            };
            var property = variableGroup.GetType().GetProperty(propertyName)!;

            Environment.SetEnvironmentVariable(variableName, null);
            await Assert.That(property.GetValue(variableGroup)).IsNull();

            Environment.SetEnvironmentVariable(variableName, "first value");
            await Assert.That(property.GetValue(variableGroup)).IsEqualTo("first value");

            Environment.SetEnvironmentVariable(variableName, "updated value");
            await Assert.That(property.GetValue(variableGroup)).IsEqualTo("updated value");
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, previousValue);
        }
    }

    [Test]
    public async Task RegistrationProvidesAllVariableGroups()
    {
        var services = new ServiceCollection();
        services.RegisterAzurePipelineContext();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var variables = scope.ServiceProvider.GetRequiredService<AzurePipelineVariables>();

        await Assert.That(variables.Agent).IsSameReferenceAs(scope.ServiceProvider.GetRequiredService<AzurePipelineAgentVariables>());
        await Assert.That(variables.Build).IsNotNull();
        await Assert.That(variables.System.PullRequest).IsNotNull();
    }
}
