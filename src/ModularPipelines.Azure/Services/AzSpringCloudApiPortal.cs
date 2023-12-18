using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudApiPortal
{
    public AzSpringCloudApiPortal(
        AzSpringCloudApiPortalCustomDomain customDomain,
        ICommand internalCommand
    )
    {
        CustomDomain = customDomain;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudApiPortalCustomDomain CustomDomain { get; }

    public async Task<CommandResult> Clear(AzSpringCloudApiPortalClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudApiPortalShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringCloudApiPortalUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

