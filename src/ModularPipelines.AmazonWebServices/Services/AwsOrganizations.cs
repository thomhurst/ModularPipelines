using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsOrganizations
{
    public AwsOrganizations(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptHandshake(AwsOrganizationsAcceptHandshakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachPolicy(AwsOrganizationsAttachPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelHandshake(AwsOrganizationsCancelHandshakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CloseAccount(AwsOrganizationsCloseAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccount(AwsOrganizationsCreateAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGovCloudAccount(AwsOrganizationsCreateGovCloudAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOrganization(AwsOrganizationsCreateOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsCreateOrganizationOptions(), token);
    }

    public async Task<CommandResult> CreateOrganizationalUnit(AwsOrganizationsCreateOrganizationalUnitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePolicy(AwsOrganizationsCreatePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeclineHandshake(AwsOrganizationsDeclineHandshakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOrganization(AwsOrganizationsDeleteOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsDeleteOrganizationOptions(), token);
    }

    public async Task<CommandResult> DeleteOrganizationalUnit(AwsOrganizationsDeleteOrganizationalUnitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePolicy(AwsOrganizationsDeletePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourcePolicy(AwsOrganizationsDeleteResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsDeleteResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> DeregisterDelegatedAdministrator(AwsOrganizationsDeregisterDelegatedAdministratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccount(AwsOrganizationsDescribeAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCreateAccountStatus(AwsOrganizationsDescribeCreateAccountStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEffectivePolicy(AwsOrganizationsDescribeEffectivePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeHandshake(AwsOrganizationsDescribeHandshakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOrganization(AwsOrganizationsDescribeOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsDescribeOrganizationOptions(), token);
    }

    public async Task<CommandResult> DescribeOrganizationalUnit(AwsOrganizationsDescribeOrganizationalUnitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePolicy(AwsOrganizationsDescribePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResourcePolicy(AwsOrganizationsDescribeResourcePolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsDescribeResourcePolicyOptions(), token);
    }

    public async Task<CommandResult> DetachPolicy(AwsOrganizationsDetachPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAwsServiceAccess(AwsOrganizationsDisableAwsServiceAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisablePolicyType(AwsOrganizationsDisablePolicyTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAllFeatures(AwsOrganizationsEnableAllFeaturesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsEnableAllFeaturesOptions(), token);
    }

    public async Task<CommandResult> EnableAwsServiceAccess(AwsOrganizationsEnableAwsServiceAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnablePolicyType(AwsOrganizationsEnablePolicyTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InviteAccountToOrganization(AwsOrganizationsInviteAccountToOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> LeaveOrganization(AwsOrganizationsLeaveOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsLeaveOrganizationOptions(), token);
    }

    public async Task<CommandResult> ListAccounts(AwsOrganizationsListAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListAccountsOptions(), token);
    }

    public async Task<CommandResult> ListAccountsForParent(AwsOrganizationsListAccountsForParentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAwsServiceAccessForOrganization(AwsOrganizationsListAwsServiceAccessForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListAwsServiceAccessForOrganizationOptions(), token);
    }

    public async Task<CommandResult> ListChildren(AwsOrganizationsListChildrenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCreateAccountStatus(AwsOrganizationsListCreateAccountStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListCreateAccountStatusOptions(), token);
    }

    public async Task<CommandResult> ListDelegatedAdministrators(AwsOrganizationsListDelegatedAdministratorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListDelegatedAdministratorsOptions(), token);
    }

    public async Task<CommandResult> ListDelegatedServicesForAccount(AwsOrganizationsListDelegatedServicesForAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHandshakesForAccount(AwsOrganizationsListHandshakesForAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListHandshakesForAccountOptions(), token);
    }

    public async Task<CommandResult> ListHandshakesForOrganization(AwsOrganizationsListHandshakesForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListHandshakesForOrganizationOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationalUnitsForParent(AwsOrganizationsListOrganizationalUnitsForParentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListParents(AwsOrganizationsListParentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPolicies(AwsOrganizationsListPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPoliciesForTarget(AwsOrganizationsListPoliciesForTargetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRoots(AwsOrganizationsListRootsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsOrganizationsListRootsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsOrganizationsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTargetsForPolicy(AwsOrganizationsListTargetsForPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MoveAccount(AwsOrganizationsMoveAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResourcePolicy(AwsOrganizationsPutResourcePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterDelegatedAdministrator(AwsOrganizationsRegisterDelegatedAdministratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveAccountFromOrganization(AwsOrganizationsRemoveAccountFromOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsOrganizationsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsOrganizationsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOrganizationalUnit(AwsOrganizationsUpdateOrganizationalUnitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePolicy(AwsOrganizationsUpdatePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}