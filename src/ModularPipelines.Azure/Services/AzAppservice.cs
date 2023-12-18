using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzAppservice
{
    public AzAppservice(
        AzAppserviceAse ase,
        AzAppserviceDomain domain,
        AzAppserviceHybridConnection hybridConnection,
        AzAppserviceKube kube,
        AzAppservicePlan plan,
        AzAppserviceVnetIntegration vnetIntegration,
        ICommand internalCommand
    )
    {
        Ase = ase;
        Domain = domain;
        HybridConnection = hybridConnection;
        Kube = kube;
        Plan = plan;
        VnetIntegration = vnetIntegration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAppserviceAse Ase { get; }

    public AzAppserviceDomain Domain { get; }

    public AzAppserviceHybridConnection HybridConnection { get; }

    public AzAppserviceKube Kube { get; }

    public AzAppservicePlan Plan { get; }

    public AzAppserviceVnetIntegration VnetIntegration { get; }

    public async Task<CommandResult> ListLocations(AzAppserviceListLocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}