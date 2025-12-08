using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering")]
public class AzPeeringService
{
    public AzPeeringService(
        AzPeeringServiceCountry country,
        AzPeeringServiceLocation location,
        AzPeeringServicePrefix prefix,
        AzPeeringServiceProvider provider,
        ICommand internalCommand
    )
    {
        Country = country;
        Location = location;
        Prefix = prefix;
        Provider = provider;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPeeringServiceCountry Country { get; }

    public AzPeeringServiceLocation Location { get; }

    public AzPeeringServicePrefix Prefix { get; }

    public AzPeeringServiceProvider Provider { get; }

    public async Task<CommandResult> Create(AzPeeringServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPeeringServiceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPeeringServiceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPeeringServiceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPeeringServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzPeeringServiceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}