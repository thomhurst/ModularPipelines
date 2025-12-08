using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("iam")]
public class GcloudIamWorkforcePools
{
    public GcloudIamWorkforcePools(
        GcloudIamWorkforcePoolsOperations operations,
        GcloudIamWorkforcePoolsProviders providers,
        GcloudIamWorkforcePoolsSubjects subjects,
        ICommand internalCommand
    )
    {
        Operations = operations;
        Providers = providers;
        Subjects = subjects;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamWorkforcePoolsOperations Operations { get; }

    public GcloudIamWorkforcePoolsProviders Providers { get; }

    public GcloudIamWorkforcePoolsSubjects Subjects { get; }

    public async Task<CommandResult> Create(GcloudIamWorkforcePoolsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCredConfig(GcloudIamWorkforcePoolsCreateCredConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoginConfig(GcloudIamWorkforcePoolsCreateLoginConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudIamWorkforcePoolsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudIamWorkforcePoolsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudIamWorkforcePoolsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudIamWorkforcePoolsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudIamWorkforcePoolsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(GcloudIamWorkforcePoolsUndeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudIamWorkforcePoolsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}