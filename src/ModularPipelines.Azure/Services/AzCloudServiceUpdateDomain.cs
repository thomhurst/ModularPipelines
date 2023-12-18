using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service")]
public class AzCloudServiceUpdateDomain
{
    public AzCloudServiceUpdateDomain(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ListUpdateDomain(AzCloudServiceUpdateDomainListUpdateDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowUpdateDomain(AzCloudServiceUpdateDomainShowUpdateDomainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceUpdateDomainShowUpdateDomainOptions(), token);
    }

    public async Task<CommandResult> WalkUpdateDomain(AzCloudServiceUpdateDomainWalkUpdateDomainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCloudServiceUpdateDomainWalkUpdateDomainOptions(), token);
    }
}