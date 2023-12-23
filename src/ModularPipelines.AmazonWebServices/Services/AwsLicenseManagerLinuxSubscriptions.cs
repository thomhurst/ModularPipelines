using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLicenseManagerLinuxSubscriptions
{
    public AwsLicenseManagerLinuxSubscriptions(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetServiceSettings(AwsLicenseManagerLinuxSubscriptionsGetServiceSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsGetServiceSettingsOptions(), token);
    }

    public async Task<CommandResult> ListLinuxSubscriptionInstances(AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionInstancesOptions(), token);
    }

    public async Task<CommandResult> ListLinuxSubscriptions(AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionsOptions(), token);
    }

    public async Task<CommandResult> UpdateServiceSettings(AwsLicenseManagerLinuxSubscriptionsUpdateServiceSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}