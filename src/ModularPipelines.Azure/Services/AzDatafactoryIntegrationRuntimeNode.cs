using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
public class AzDatafactoryIntegrationRuntimeNode
{
    public AzDatafactoryIntegrationRuntimeNode(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzDatafactoryIntegrationRuntimeNodeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeNodeDeleteOptions(), token);
    }

    public async Task<CommandResult> GetIpAddress(AzDatafactoryIntegrationRuntimeNodeGetIpAddressOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeNodeGetIpAddressOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatafactoryIntegrationRuntimeNodeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeNodeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryIntegrationRuntimeNodeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeNodeUpdateOptions(), token);
    }
}