using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security")]
public class AzDevopsSecurityGroup
{
    public AzDevopsSecurityGroup(
        AzDevopsSecurityGroupMembership membership,
        ICommand internalCommand
    )
    {
        Membership = membership;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDevopsSecurityGroupMembership Membership { get; }

    public async Task<CommandResult> Create(AzDevopsSecurityGroupCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsSecurityGroupCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzDevopsSecurityGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevopsSecurityGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsSecurityGroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevopsSecurityGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzDevopsSecurityGroupUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}