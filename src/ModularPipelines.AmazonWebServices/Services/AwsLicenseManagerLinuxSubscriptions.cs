using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> GetServiceSettings(AwsLicenseManagerLinuxSubscriptionsGetServiceSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsGetServiceSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLinuxSubscriptionInstances(AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionInstancesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionInstancesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLinuxSubscriptions(AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerLinuxSubscriptionsListLinuxSubscriptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateServiceSettings(AwsLicenseManagerLinuxSubscriptionsUpdateServiceSettingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}