using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcareapis", "workspace")]
public class AzHealthcareapisWorkspacePrivateLinkResource
{
    public AzHealthcareapisWorkspacePrivateLinkResource(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzHealthcareapisWorkspacePrivateLinkResourceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspacePrivateLinkResourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspacePrivateLinkResourceShowOptions(), token);
    }
}