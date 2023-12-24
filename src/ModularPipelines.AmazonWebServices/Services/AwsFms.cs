using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsFms
{
    public AwsFms(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateAdminAccount(AwsFmsAssociateAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateThirdPartyFirewall(AwsFmsAssociateThirdPartyFirewallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateResource(AwsFmsBatchAssociateResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateResource(AwsFmsBatchDisassociateResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAppsList(AwsFmsDeleteAppsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNotificationChannel(AwsFmsDeleteNotificationChannelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsDeleteNotificationChannelOptions(), token);
    }

    public async Task<CommandResult> DeletePolicy(AwsFmsDeletePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProtocolsList(AwsFmsDeleteProtocolsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceSet(AwsFmsDeleteResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateAdminAccount(AwsFmsDisassociateAdminAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsDisassociateAdminAccountOptions(), token);
    }

    public async Task<CommandResult> DisassociateThirdPartyFirewall(AwsFmsDisassociateThirdPartyFirewallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAdminAccount(AwsFmsGetAdminAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsGetAdminAccountOptions(), token);
    }

    public async Task<CommandResult> GetAdminScope(AwsFmsGetAdminScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAppsList(AwsFmsGetAppsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetComplianceDetail(AwsFmsGetComplianceDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNotificationChannel(AwsFmsGetNotificationChannelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsGetNotificationChannelOptions(), token);
    }

    public async Task<CommandResult> GetPolicy(AwsFmsGetPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetProtectionStatus(AwsFmsGetProtectionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetProtocolsList(AwsFmsGetProtocolsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceSet(AwsFmsGetResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetThirdPartyFirewallAssociationStatus(AwsFmsGetThirdPartyFirewallAssociationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetViolationDetails(AwsFmsGetViolationDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAdminAccountsForOrganization(AwsFmsListAdminAccountsForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListAdminAccountsForOrganizationOptions(), token);
    }

    public async Task<CommandResult> ListAdminsManagingAccount(AwsFmsListAdminsManagingAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListAdminsManagingAccountOptions(), token);
    }

    public async Task<CommandResult> ListAppsLists(AwsFmsListAppsListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListAppsListsOptions(), token);
    }

    public async Task<CommandResult> ListComplianceStatus(AwsFmsListComplianceStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDiscoveredResources(AwsFmsListDiscoveredResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMemberAccounts(AwsFmsListMemberAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListMemberAccountsOptions(), token);
    }

    public async Task<CommandResult> ListPolicies(AwsFmsListPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListProtocolsLists(AwsFmsListProtocolsListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListProtocolsListsOptions(), token);
    }

    public async Task<CommandResult> ListResourceSetResources(AwsFmsListResourceSetResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourceSets(AwsFmsListResourceSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsFmsListResourceSetsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsFmsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListThirdPartyFirewallFirewallPolicies(AwsFmsListThirdPartyFirewallFirewallPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAdminAccount(AwsFmsPutAdminAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAppsList(AwsFmsPutAppsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutNotificationChannel(AwsFmsPutNotificationChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutPolicy(AwsFmsPutPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutProtocolsList(AwsFmsPutProtocolsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourceSet(AwsFmsPutResourceSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsFmsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsFmsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}