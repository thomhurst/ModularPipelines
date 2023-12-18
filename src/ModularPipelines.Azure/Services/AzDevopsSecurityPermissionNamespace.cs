using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission")]
public class AzDevopsSecurityPermissionNamespace
{
    public AzDevopsSecurityPermissionNamespace(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzDevopsSecurityPermissionNamespaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsSecurityPermissionNamespaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevopsSecurityPermissionNamespaceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}