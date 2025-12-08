using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("container")]
public class GcloudContainerHub
{
    public GcloudContainerHub(
        GcloudContainerHubCloudrun cloudrun,
        GcloudContainerHubClusterupgrade clusterupgrade,
        GcloudContainerHubDataplaneV2Encryption dataplaneV2Encryption,
        GcloudContainerHubFeatures features,
        GcloudContainerHubFleetobservability fleetobservability,
        GcloudContainerHubIdentityService identityService,
        GcloudContainerHubIngress ingress,
        GcloudContainerHubMemberships memberships,
        GcloudContainerHubMesh mesh,
        GcloudContainerHubMultiClusterServices multiClusterServices,
        GcloudContainerHubPolicycontroller policycontroller,
        GcloudContainerHubScopes scopes,
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

    public GcloudContainerHubCloudrun Cloudrun { get; }

    public GcloudContainerHubClusterupgrade Clusterupgrade { get; }

    public GcloudContainerHubDataplaneV2Encryption DataplaneV2Encryption { get; }

    public GcloudContainerHubFeatures Features { get; }

    public GcloudContainerHubFleetobservability Fleetobservability { get; }

    public GcloudContainerHubIdentityService IdentityService { get; }

    public GcloudContainerHubIngress Ingress { get; }

    public GcloudContainerHubMemberships Memberships { get; }

    public GcloudContainerHubMesh Mesh { get; }

    public GcloudContainerHubMultiClusterServices MultiClusterServices { get; }

    public GcloudContainerHubPolicycontroller Policycontroller { get; }

    public GcloudContainerHubScopes Scopes { get; }

    public async Task<CommandResult> Create(GcloudContainerHubCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(GcloudContainerHubDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubDeleteOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubDescribeOptions(), token);
    }

    public async Task<CommandResult> List(GcloudContainerHubListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubUpdateOptions(), token);
    }
}