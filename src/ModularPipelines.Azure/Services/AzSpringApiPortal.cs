using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring")]
public class AzSpringApiPortal
{
    public AzSpringApiPortal(
        AzSpringApiPortalCustomDomain customDomain,
        ICommand internalCommand
    )
    {
        CustomDomain = customDomain;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringApiPortalCustomDomain CustomDomain { get; }

    public async Task<CommandResult> Clear(AzSpringApiPortalClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSpringApiPortalCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringApiPortalDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringApiPortalShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringApiPortalUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}