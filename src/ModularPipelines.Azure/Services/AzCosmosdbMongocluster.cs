using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbMongocluster
{
    public AzCosmosdbMongocluster(
        AzCosmosdbMongoclusterFirewall firewall,
        ICommand internalCommand
    )
    {
        Firewall = firewall;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbMongoclusterFirewall Firewall { get; }

    public async Task<CommandResult> Create(AzCosmosdbMongoclusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbMongoclusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbMongoclusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbMongoclusterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzCosmosdbMongoclusterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbMongoclusterUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}