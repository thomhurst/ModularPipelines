using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudServices
{
    public GcloudServices(
        GcloudServicesApiKeys apiKeys,
        GcloudServicesOperations operations,
        GcloudServicesPeeredDnsDomains peeredDnsDomains,
        GcloudServicesVpcPeerings vpcPeerings,
        ICommand internalCommand
    )
    {
        ApiKeys = apiKeys;
        Operations = operations;
        PeeredDnsDomains = peeredDnsDomains;
        VpcPeerings = vpcPeerings;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudServicesApiKeys ApiKeys { get; }

    public GcloudServicesOperations Operations { get; }

    public GcloudServicesPeeredDnsDomains PeeredDnsDomains { get; }

    public GcloudServicesVpcPeerings VpcPeerings { get; }

    public async Task<CommandResult> Disable(GcloudServicesDisableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(GcloudServicesEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudServicesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudServicesListOptions(), token);
    }
}