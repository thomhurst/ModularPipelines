using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLicenseManagerUserSubscriptions
{
    public AwsLicenseManagerUserSubscriptions(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateUser(AwsLicenseManagerUserSubscriptionsAssociateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterIdentityProvider(AwsLicenseManagerUserSubscriptionsDeregisterIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateUser(AwsLicenseManagerUserSubscriptionsDisassociateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIdentityProviders(AwsLicenseManagerUserSubscriptionsListIdentityProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerUserSubscriptionsListIdentityProvidersOptions(), token);
    }

    public async Task<CommandResult> ListInstances(AwsLicenseManagerUserSubscriptionsListInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerUserSubscriptionsListInstancesOptions(), token);
    }

    public async Task<CommandResult> ListProductSubscriptions(AwsLicenseManagerUserSubscriptionsListProductSubscriptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUserAssociations(AwsLicenseManagerUserSubscriptionsListUserAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterIdentityProvider(AwsLicenseManagerUserSubscriptionsRegisterIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartProductSubscription(AwsLicenseManagerUserSubscriptionsStartProductSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopProductSubscription(AwsLicenseManagerUserSubscriptionsStopProductSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIdentityProviderSettings(AwsLicenseManagerUserSubscriptionsUpdateIdentityProviderSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}