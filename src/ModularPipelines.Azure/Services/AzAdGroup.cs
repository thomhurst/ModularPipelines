using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad")]
public class AzAdGroup
{
    public AzAdGroup(
        AzAdGroupMember member,
        AzAdGroupOwner owner,
        ICommand internalCommand
    )
    {
        Member = member;
        Owner = owner;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAdGroupMember Member { get; }

    public AzAdGroupOwner Owner { get; }

    public async Task<CommandResult> Create(AzAdGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAdGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMemberGroups(AzAdGroupGetMemberGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAdGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAdGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

