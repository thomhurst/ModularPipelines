using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsWorkspacesWeb
{
    public AwsWorkspacesWeb(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateBrowserSettings(AwsWorkspacesWebAssociateBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateIpAccessSettings(AwsWorkspacesWebAssociateIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateNetworkSettings(AwsWorkspacesWebAssociateNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTrustStore(AwsWorkspacesWebAssociateTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateUserAccessLoggingSettings(AwsWorkspacesWebAssociateUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateUserSettings(AwsWorkspacesWebAssociateUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateBrowserSettings(AwsWorkspacesWebCreateBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIdentityProvider(AwsWorkspacesWebCreateIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIpAccessSettings(AwsWorkspacesWebCreateIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkSettings(AwsWorkspacesWebCreateNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePortal(AwsWorkspacesWebCreatePortalOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebCreatePortalOptions(), token);
    }

    public async Task<CommandResult> CreateTrustStore(AwsWorkspacesWebCreateTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserAccessLoggingSettings(AwsWorkspacesWebCreateUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserSettings(AwsWorkspacesWebCreateUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteBrowserSettings(AwsWorkspacesWebDeleteBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIdentityProvider(AwsWorkspacesWebDeleteIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpAccessSettings(AwsWorkspacesWebDeleteIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkSettings(AwsWorkspacesWebDeleteNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePortal(AwsWorkspacesWebDeletePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrustStore(AwsWorkspacesWebDeleteTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserAccessLoggingSettings(AwsWorkspacesWebDeleteUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserSettings(AwsWorkspacesWebDeleteUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateBrowserSettings(AwsWorkspacesWebDisassociateBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateIpAccessSettings(AwsWorkspacesWebDisassociateIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateNetworkSettings(AwsWorkspacesWebDisassociateNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTrustStore(AwsWorkspacesWebDisassociateTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateUserAccessLoggingSettings(AwsWorkspacesWebDisassociateUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateUserSettings(AwsWorkspacesWebDisassociateUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBrowserSettings(AwsWorkspacesWebGetBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityProvider(AwsWorkspacesWebGetIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpAccessSettings(AwsWorkspacesWebGetIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkSettings(AwsWorkspacesWebGetNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPortal(AwsWorkspacesWebGetPortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPortalServiceProviderMetadata(AwsWorkspacesWebGetPortalServiceProviderMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrustStore(AwsWorkspacesWebGetTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrustStoreCertificate(AwsWorkspacesWebGetTrustStoreCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserAccessLoggingSettings(AwsWorkspacesWebGetUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserSettings(AwsWorkspacesWebGetUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBrowserSettings(AwsWorkspacesWebListBrowserSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListBrowserSettingsOptions(), token);
    }

    public async Task<CommandResult> ListIdentityProviders(AwsWorkspacesWebListIdentityProvidersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIpAccessSettings(AwsWorkspacesWebListIpAccessSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListIpAccessSettingsOptions(), token);
    }

    public async Task<CommandResult> ListNetworkSettings(AwsWorkspacesWebListNetworkSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListNetworkSettingsOptions(), token);
    }

    public async Task<CommandResult> ListPortals(AwsWorkspacesWebListPortalsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListPortalsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsWorkspacesWebListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrustStoreCertificates(AwsWorkspacesWebListTrustStoreCertificatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrustStores(AwsWorkspacesWebListTrustStoresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListTrustStoresOptions(), token);
    }

    public async Task<CommandResult> ListUserAccessLoggingSettings(AwsWorkspacesWebListUserAccessLoggingSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListUserAccessLoggingSettingsOptions(), token);
    }

    public async Task<CommandResult> ListUserSettings(AwsWorkspacesWebListUserSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesWebListUserSettingsOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsWorkspacesWebTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsWorkspacesWebUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateBrowserSettings(AwsWorkspacesWebUpdateBrowserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIdentityProvider(AwsWorkspacesWebUpdateIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIpAccessSettings(AwsWorkspacesWebUpdateIpAccessSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNetworkSettings(AwsWorkspacesWebUpdateNetworkSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePortal(AwsWorkspacesWebUpdatePortalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrustStore(AwsWorkspacesWebUpdateTrustStoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserAccessLoggingSettings(AwsWorkspacesWebUpdateUserAccessLoggingSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserSettings(AwsWorkspacesWebUpdateUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}