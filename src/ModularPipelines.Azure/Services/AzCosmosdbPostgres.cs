using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbPostgres
{
    public AzCosmosdbPostgres(
        AzCosmosdbPostgresCluster cluster,
        AzCosmosdbPostgresConfiguration configuration,
        AzCosmosdbPostgresFirewallRule firewallRule,
        AzCosmosdbPostgresRole role,
        ICommand internalCommand
    )
    {
        Cluster = cluster;
        Configuration = configuration;
        FirewallRule = firewallRule;
        Role = role;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbPostgresCluster Cluster { get; }

    public AzCosmosdbPostgresConfiguration Configuration { get; }

    public AzCosmosdbPostgresFirewallRule FirewallRule { get; }

    public AzCosmosdbPostgresRole Role { get; }

    public async Task<CommandResult> CheckNameAvailability(AzCosmosdbPostgresCheckNameAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}