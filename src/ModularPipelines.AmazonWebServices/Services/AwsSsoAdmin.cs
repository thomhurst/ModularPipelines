using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSsoAdmin
{
    public AwsSsoAdmin(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AttachCustomerManagedPolicyReferenceToPermissionSet(AwsSsoAdminAttachCustomerManagedPolicyReferenceToPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AttachManagedPolicyToPermissionSet(AwsSsoAdminAttachManagedPolicyToPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAccountAssignment(AwsSsoAdminCreateAccountAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplication(AwsSsoAdminCreateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateApplicationAssignment(AwsSsoAdminCreateApplicationAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstance(AwsSsoAdminCreateInstanceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsoAdminCreateInstanceOptions(), token);
    }

    public async Task<CommandResult> CreateInstanceAccessControlAttributeConfiguration(AwsSsoAdminCreateInstanceAccessControlAttributeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePermissionSet(AwsSsoAdminCreatePermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrustedTokenIssuer(AwsSsoAdminCreateTrustedTokenIssuerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccountAssignment(AwsSsoAdminDeleteAccountAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplication(AwsSsoAdminDeleteApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplicationAccessScope(AwsSsoAdminDeleteApplicationAccessScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplicationAssignment(AwsSsoAdminDeleteApplicationAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplicationAuthenticationMethod(AwsSsoAdminDeleteApplicationAuthenticationMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteApplicationGrant(AwsSsoAdminDeleteApplicationGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInlinePolicyFromPermissionSet(AwsSsoAdminDeleteInlinePolicyFromPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstance(AwsSsoAdminDeleteInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceAccessControlAttributeConfiguration(AwsSsoAdminDeleteInstanceAccessControlAttributeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePermissionSet(AwsSsoAdminDeletePermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePermissionsBoundaryFromPermissionSet(AwsSsoAdminDeletePermissionsBoundaryFromPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrustedTokenIssuer(AwsSsoAdminDeleteTrustedTokenIssuerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAssignmentCreationStatus(AwsSsoAdminDescribeAccountAssignmentCreationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountAssignmentDeletionStatus(AwsSsoAdminDescribeAccountAssignmentDeletionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApplication(AwsSsoAdminDescribeApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApplicationAssignment(AwsSsoAdminDescribeApplicationAssignmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApplicationProvider(AwsSsoAdminDescribeApplicationProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstance(AwsSsoAdminDescribeInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInstanceAccessControlAttributeConfiguration(AwsSsoAdminDescribeInstanceAccessControlAttributeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePermissionSet(AwsSsoAdminDescribePermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePermissionSetProvisioningStatus(AwsSsoAdminDescribePermissionSetProvisioningStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustedTokenIssuer(AwsSsoAdminDescribeTrustedTokenIssuerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachCustomerManagedPolicyReferenceFromPermissionSet(AwsSsoAdminDetachCustomerManagedPolicyReferenceFromPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DetachManagedPolicyFromPermissionSet(AwsSsoAdminDetachManagedPolicyFromPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationAccessScope(AwsSsoAdminGetApplicationAccessScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationAssignmentConfiguration(AwsSsoAdminGetApplicationAssignmentConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationAuthenticationMethod(AwsSsoAdminGetApplicationAuthenticationMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetApplicationGrant(AwsSsoAdminGetApplicationGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInlinePolicyForPermissionSet(AwsSsoAdminGetInlinePolicyForPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPermissionsBoundaryForPermissionSet(AwsSsoAdminGetPermissionsBoundaryForPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountAssignmentCreationStatus(AwsSsoAdminListAccountAssignmentCreationStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountAssignmentDeletionStatus(AwsSsoAdminListAccountAssignmentDeletionStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountAssignments(AwsSsoAdminListAccountAssignmentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountAssignmentsForPrincipal(AwsSsoAdminListAccountAssignmentsForPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAccountsForProvisionedPermissionSet(AwsSsoAdminListAccountsForProvisionedPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationAccessScopes(AwsSsoAdminListApplicationAccessScopesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationAssignments(AwsSsoAdminListApplicationAssignmentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationAssignmentsForPrincipal(AwsSsoAdminListApplicationAssignmentsForPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationAuthenticationMethods(AwsSsoAdminListApplicationAuthenticationMethodsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationGrants(AwsSsoAdminListApplicationGrantsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListApplicationProviders(AwsSsoAdminListApplicationProvidersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsoAdminListApplicationProvidersOptions(), token);
    }

    public async Task<CommandResult> ListApplications(AwsSsoAdminListApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomerManagedPolicyReferencesInPermissionSet(AwsSsoAdminListCustomerManagedPolicyReferencesInPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInstances(AwsSsoAdminListInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSsoAdminListInstancesOptions(), token);
    }

    public async Task<CommandResult> ListManagedPoliciesInPermissionSet(AwsSsoAdminListManagedPoliciesInPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPermissionSetProvisioningStatus(AwsSsoAdminListPermissionSetProvisioningStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPermissionSets(AwsSsoAdminListPermissionSetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPermissionSetsProvisionedToAccount(AwsSsoAdminListPermissionSetsProvisionedToAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSsoAdminListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrustedTokenIssuers(AwsSsoAdminListTrustedTokenIssuersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionPermissionSet(AwsSsoAdminProvisionPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutApplicationAccessScope(AwsSsoAdminPutApplicationAccessScopeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutApplicationAssignmentConfiguration(AwsSsoAdminPutApplicationAssignmentConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutApplicationAuthenticationMethod(AwsSsoAdminPutApplicationAuthenticationMethodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutApplicationGrant(AwsSsoAdminPutApplicationGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutInlinePolicyToPermissionSet(AwsSsoAdminPutInlinePolicyToPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutPermissionsBoundaryToPermissionSet(AwsSsoAdminPutPermissionsBoundaryToPermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSsoAdminTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSsoAdminUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateApplication(AwsSsoAdminUpdateApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstance(AwsSsoAdminUpdateInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstanceAccessControlAttributeConfiguration(AwsSsoAdminUpdateInstanceAccessControlAttributeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePermissionSet(AwsSsoAdminUpdatePermissionSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrustedTokenIssuer(AwsSsoAdminUpdateTrustedTokenIssuerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}