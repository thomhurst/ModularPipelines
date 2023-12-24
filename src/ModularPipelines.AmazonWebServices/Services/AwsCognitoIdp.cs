using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCognitoIdp
{
    public AwsCognitoIdp(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddCustomAttributes(AwsCognitoIdpAddCustomAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminAddUserToGroup(AwsCognitoIdpAdminAddUserToGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminConfirmSignUp(AwsCognitoIdpAdminConfirmSignUpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminCreateUser(AwsCognitoIdpAdminCreateUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminDeleteUser(AwsCognitoIdpAdminDeleteUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminDeleteUserAttributes(AwsCognitoIdpAdminDeleteUserAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminDisableProviderForUser(AwsCognitoIdpAdminDisableProviderForUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminDisableUser(AwsCognitoIdpAdminDisableUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminEnableUser(AwsCognitoIdpAdminEnableUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminForgetDevice(AwsCognitoIdpAdminForgetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminGetDevice(AwsCognitoIdpAdminGetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminGetUser(AwsCognitoIdpAdminGetUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminInitiateAuth(AwsCognitoIdpAdminInitiateAuthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminLinkProviderForUser(AwsCognitoIdpAdminLinkProviderForUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminListDevices(AwsCognitoIdpAdminListDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminListGroupsForUser(AwsCognitoIdpAdminListGroupsForUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminListUserAuthEvents(AwsCognitoIdpAdminListUserAuthEventsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminRemoveUserFromGroup(AwsCognitoIdpAdminRemoveUserFromGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminResetUserPassword(AwsCognitoIdpAdminResetUserPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminRespondToAuthChallenge(AwsCognitoIdpAdminRespondToAuthChallengeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminSetUserMfaPreference(AwsCognitoIdpAdminSetUserMfaPreferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminSetUserPassword(AwsCognitoIdpAdminSetUserPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminSetUserSettings(AwsCognitoIdpAdminSetUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminUpdateAuthEventFeedback(AwsCognitoIdpAdminUpdateAuthEventFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminUpdateDeviceStatus(AwsCognitoIdpAdminUpdateDeviceStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminUpdateUserAttributes(AwsCognitoIdpAdminUpdateUserAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AdminUserGlobalSignOut(AwsCognitoIdpAdminUserGlobalSignOutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateSoftwareToken(AwsCognitoIdpAssociateSoftwareTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCognitoIdpAssociateSoftwareTokenOptions(), token);
    }

    public async Task<CommandResult> ChangePassword(AwsCognitoIdpChangePasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmDevice(AwsCognitoIdpConfirmDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmForgotPassword(AwsCognitoIdpConfirmForgotPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfirmSignUp(AwsCognitoIdpConfirmSignUpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroup(AwsCognitoIdpCreateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIdentityProvider(AwsCognitoIdpCreateIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResourceServer(AwsCognitoIdpCreateResourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserImportJob(AwsCognitoIdpCreateUserImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserPool(AwsCognitoIdpCreateUserPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserPoolClient(AwsCognitoIdpCreateUserPoolClientOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUserPoolDomain(AwsCognitoIdpCreateUserPoolDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGroup(AwsCognitoIdpDeleteGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIdentityProvider(AwsCognitoIdpDeleteIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceServer(AwsCognitoIdpDeleteResourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUser(AwsCognitoIdpDeleteUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserAttributes(AwsCognitoIdpDeleteUserAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserPool(AwsCognitoIdpDeleteUserPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserPoolClient(AwsCognitoIdpDeleteUserPoolClientOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUserPoolDomain(AwsCognitoIdpDeleteUserPoolDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeIdentityProvider(AwsCognitoIdpDescribeIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeResourceServer(AwsCognitoIdpDescribeResourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRiskConfiguration(AwsCognitoIdpDescribeRiskConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserImportJob(AwsCognitoIdpDescribeUserImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserPool(AwsCognitoIdpDescribeUserPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserPoolClient(AwsCognitoIdpDescribeUserPoolClientOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeUserPoolDomain(AwsCognitoIdpDescribeUserPoolDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ForgetDevice(AwsCognitoIdpForgetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ForgotPassword(AwsCognitoIdpForgotPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCsvHeader(AwsCognitoIdpGetCsvHeaderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevice(AwsCognitoIdpGetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroup(AwsCognitoIdpGetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityProviderByIdentifier(AwsCognitoIdpGetIdentityProviderByIdentifierOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLogDeliveryConfiguration(AwsCognitoIdpGetLogDeliveryConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSigningCertificate(AwsCognitoIdpGetSigningCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUiCustomization(AwsCognitoIdpGetUiCustomizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUser(AwsCognitoIdpGetUserOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserAttributeVerificationCode(AwsCognitoIdpGetUserAttributeVerificationCodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUserPoolMfaConfig(AwsCognitoIdpGetUserPoolMfaConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GlobalSignOut(AwsCognitoIdpGlobalSignOutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitiateAuth(AwsCognitoIdpInitiateAuthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDevices(AwsCognitoIdpListDevicesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroups(AwsCognitoIdpListGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListIdentityProviders(AwsCognitoIdpListIdentityProvidersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourceServers(AwsCognitoIdpListResourceServersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCognitoIdpListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUserImportJobs(AwsCognitoIdpListUserImportJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUserPoolClients(AwsCognitoIdpListUserPoolClientsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUserPools(AwsCognitoIdpListUserPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCognitoIdpListUserPoolsOptions(), token);
    }

    public async Task<CommandResult> ListUsers(AwsCognitoIdpListUsersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUsersInGroup(AwsCognitoIdpListUsersInGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResendConfirmationCode(AwsCognitoIdpResendConfirmationCodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RespondToAuthChallenge(AwsCognitoIdpRespondToAuthChallengeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeToken(AwsCognitoIdpRevokeTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetLogDeliveryConfiguration(AwsCognitoIdpSetLogDeliveryConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetRiskConfiguration(AwsCognitoIdpSetRiskConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetUiCustomization(AwsCognitoIdpSetUiCustomizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetUserMfaPreference(AwsCognitoIdpSetUserMfaPreferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetUserPoolMfaConfig(AwsCognitoIdpSetUserPoolMfaConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetUserSettings(AwsCognitoIdpSetUserSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SignUp(AwsCognitoIdpSignUpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartUserImportJob(AwsCognitoIdpStartUserImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopUserImportJob(AwsCognitoIdpStopUserImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCognitoIdpTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCognitoIdpUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAuthEventFeedback(AwsCognitoIdpUpdateAuthEventFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDeviceStatus(AwsCognitoIdpUpdateDeviceStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGroup(AwsCognitoIdpUpdateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIdentityProvider(AwsCognitoIdpUpdateIdentityProviderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceServer(AwsCognitoIdpUpdateResourceServerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserAttributes(AwsCognitoIdpUpdateUserAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserPool(AwsCognitoIdpUpdateUserPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserPoolClient(AwsCognitoIdpUpdateUserPoolClientOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUserPoolDomain(AwsCognitoIdpUpdateUserPoolDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifySoftwareToken(AwsCognitoIdpVerifySoftwareTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyUserAttribute(AwsCognitoIdpVerifyUserAttributeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}