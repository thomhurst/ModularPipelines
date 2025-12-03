using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container")]
public class GcloudContainerFleet
{
    public GcloudContainerFleet(
        GcloudContainerFleetCloudrun cloudrun,
        GcloudContainerFleetClusterupgrade clusterupgrade,
        GcloudContainerFleetDataplaneV2Encryption dataplaneV2Encryption,
        GcloudContainerFleetFeatures features,
        GcloudContainerFleetFleetobservability fleetobservability,
        GcloudContainerFleetIdentityService identityService,
        GcloudContainerFleetIngress ingress,
        GcloudContainerFleetMemberships memberships,
        GcloudContainerFleetMesh mesh,
        GcloudContainerFleetMultiClusterServices multiClusterServices,
        GcloudContainerFleetPolicycontroller policycontroller,
        GcloudContainerFleetScopes scopes,
        ICommand internalCommand
    )
    {
        Cloudrun = cloudrun;
        Clusterupgrade = clusterupgrade;
        DataplaneV2Encryption = dataplaneV2Encryption;
        Features = features;
        Fleetobservability = fleetobservability;
        IdentityService = identityService;
        Ingress = ingress;
        Memberships = memberships;
        Mesh = mesh;
        MultiClusterServices = multiClusterServices;
        Policycontroller = policycontroller;
        Scopes = scopes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerFleetCloudrun Cloudrun { get; }

    public GcloudContainerFleetClusterupgrade Clusterupgrade { get; }

    public GcloudContainerFleetDataplaneV2Encryption DataplaneV2Encryption { get; }

    public GcloudContainerFleetFeatures Features { get; }

    public GcloudContainerFleetFleetobservability Fleetobservability { get; }

    public GcloudContainerFleetIdentityService IdentityService { get; }

    public GcloudContainerFleetIngress Ingress { get; }

    public GcloudContainerFleetMemberships Memberships { get; }

    public GcloudContainerFleetMesh Mesh { get; }

    public GcloudContainerFleetMultiClusterServices MultiClusterServices { get; }

    public GcloudContainerFleetPolicycontroller Policycontroller { get; }

    public GcloudContainerFleetScopes Scopes { get; }

    public async Task<CommandResult> Create(GcloudContainerFleetCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(GcloudContainerFleetDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetDeleteOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerFleetDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetDescribeOptions(), token);
    }

    public async Task<CommandResult> List(GcloudContainerFleetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetUpdateOptions(), token);
    }
}