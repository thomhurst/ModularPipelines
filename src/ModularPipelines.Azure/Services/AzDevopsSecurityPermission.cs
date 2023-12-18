using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security")]
public class AzDevopsSecurityPermission
{
    public AzDevopsSecurityPermission(
        AzDevopsSecurityPermissionNamespace @namespace,
        ICommand internalCommand
    )
    {
        Namespace = @namespace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDevopsSecurityPermissionNamespace Namespace { get; }

    public async Task<CommandResult> List(AzDevopsSecurityPermissionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reset(AzDevopsSecurityPermissionResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetAll(AzDevopsSecurityPermissionResetAllOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDevopsSecurityPermissionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzDevopsSecurityPermissionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}