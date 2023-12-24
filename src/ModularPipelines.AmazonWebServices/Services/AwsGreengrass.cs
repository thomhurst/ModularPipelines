using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsGreengrass
{
    public AwsGreengrass(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateRoleToGroup(AwsGreengrassAssociateRoleToGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateServiceRoleToAccount(AwsGreengrassAssociateServiceRoleToAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectorDefinition(AwsGreengrassCreateConnectorDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateConnectorDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateConnectorDefinitionVersion(AwsGreengrassCreateConnectorDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCoreDefinition(AwsGreengrassCreateCoreDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateCoreDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateCoreDefinitionVersion(AwsGreengrassCreateCoreDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeployment(AwsGreengrassCreateDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeviceDefinition(AwsGreengrassCreateDeviceDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateDeviceDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateDeviceDefinitionVersion(AwsGreengrassCreateDeviceDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFunctionDefinition(AwsGreengrassCreateFunctionDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateFunctionDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateFunctionDefinitionVersion(AwsGreengrassCreateFunctionDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroup(AwsGreengrassCreateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroupCertificateAuthority(AwsGreengrassCreateGroupCertificateAuthorityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroupVersion(AwsGreengrassCreateGroupVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLoggerDefinition(AwsGreengrassCreateLoggerDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateLoggerDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateLoggerDefinitionVersion(AwsGreengrassCreateLoggerDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResourceDefinition(AwsGreengrassCreateResourceDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateResourceDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateResourceDefinitionVersion(AwsGreengrassCreateResourceDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSoftwareUpdateJob(AwsGreengrassCreateSoftwareUpdateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubscriptionDefinition(AwsGreengrassCreateSubscriptionDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassCreateSubscriptionDefinitionOptions(), token);
    }

    public async Task<CommandResult> CreateSubscriptionDefinitionVersion(AwsGreengrassCreateSubscriptionDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnectorDefinition(AwsGreengrassDeleteConnectorDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCoreDefinition(AwsGreengrassDeleteCoreDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeviceDefinition(AwsGreengrassDeleteDeviceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFunctionDefinition(AwsGreengrassDeleteFunctionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGroup(AwsGreengrassDeleteGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoggerDefinition(AwsGreengrassDeleteLoggerDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResourceDefinition(AwsGreengrassDeleteResourceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubscriptionDefinition(AwsGreengrassDeleteSubscriptionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateRoleFromGroup(AwsGreengrassDisassociateRoleFromGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateServiceRoleFromAccount(AwsGreengrassDisassociateServiceRoleFromAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassDisassociateServiceRoleFromAccountOptions(), token);
    }

    public async Task<CommandResult> GetAssociatedRole(AwsGreengrassGetAssociatedRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetBulkDeploymentStatus(AwsGreengrassGetBulkDeploymentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectivityInfo(AwsGreengrassGetConnectivityInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectorDefinition(AwsGreengrassGetConnectorDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectorDefinitionVersion(AwsGreengrassGetConnectorDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreDefinition(AwsGreengrassGetCoreDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCoreDefinitionVersion(AwsGreengrassGetCoreDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeploymentStatus(AwsGreengrassGetDeploymentStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeviceDefinition(AwsGreengrassGetDeviceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeviceDefinitionVersion(AwsGreengrassGetDeviceDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFunctionDefinition(AwsGreengrassGetFunctionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFunctionDefinitionVersion(AwsGreengrassGetFunctionDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroup(AwsGreengrassGetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroupCertificateAuthority(AwsGreengrassGetGroupCertificateAuthorityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroupCertificateConfiguration(AwsGreengrassGetGroupCertificateConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroupVersion(AwsGreengrassGetGroupVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggerDefinition(AwsGreengrassGetLoggerDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggerDefinitionVersion(AwsGreengrassGetLoggerDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceDefinition(AwsGreengrassGetResourceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceDefinitionVersion(AwsGreengrassGetResourceDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceRoleForAccount(AwsGreengrassGetServiceRoleForAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassGetServiceRoleForAccountOptions(), token);
    }

    public async Task<CommandResult> GetSubscriptionDefinition(AwsGreengrassGetSubscriptionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSubscriptionDefinitionVersion(AwsGreengrassGetSubscriptionDefinitionVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetThingRuntimeConfiguration(AwsGreengrassGetThingRuntimeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBulkDeploymentDetailedReports(AwsGreengrassListBulkDeploymentDetailedReportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListBulkDeployments(AwsGreengrassListBulkDeploymentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListBulkDeploymentsOptions(), token);
    }

    public async Task<CommandResult> ListConnectorDefinitionVersions(AwsGreengrassListConnectorDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConnectorDefinitions(AwsGreengrassListConnectorDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListConnectorDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListCoreDefinitionVersions(AwsGreengrassListCoreDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCoreDefinitions(AwsGreengrassListCoreDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListCoreDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListDeployments(AwsGreengrassListDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeviceDefinitionVersions(AwsGreengrassListDeviceDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeviceDefinitions(AwsGreengrassListDeviceDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListDeviceDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListFunctionDefinitionVersions(AwsGreengrassListFunctionDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFunctionDefinitions(AwsGreengrassListFunctionDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListFunctionDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListGroupCertificateAuthorities(AwsGreengrassListGroupCertificateAuthoritiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroupVersions(AwsGreengrassListGroupVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroups(AwsGreengrassListGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListGroupsOptions(), token);
    }

    public async Task<CommandResult> ListLoggerDefinitionVersions(AwsGreengrassListLoggerDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLoggerDefinitions(AwsGreengrassListLoggerDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListLoggerDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListResourceDefinitionVersions(AwsGreengrassListResourceDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResourceDefinitions(AwsGreengrassListResourceDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListResourceDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListSubscriptionDefinitionVersions(AwsGreengrassListSubscriptionDefinitionVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSubscriptionDefinitions(AwsGreengrassListSubscriptionDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsGreengrassListSubscriptionDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsGreengrassListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetDeployments(AwsGreengrassResetDeploymentsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBulkDeployment(AwsGreengrassStartBulkDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopBulkDeployment(AwsGreengrassStopBulkDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsGreengrassTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsGreengrassUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectivityInfo(AwsGreengrassUpdateConnectivityInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectorDefinition(AwsGreengrassUpdateConnectorDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCoreDefinition(AwsGreengrassUpdateCoreDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDeviceDefinition(AwsGreengrassUpdateDeviceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFunctionDefinition(AwsGreengrassUpdateFunctionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGroup(AwsGreengrassUpdateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGroupCertificateConfiguration(AwsGreengrassUpdateGroupCertificateConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLoggerDefinition(AwsGreengrassUpdateLoggerDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceDefinition(AwsGreengrassUpdateResourceDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubscriptionDefinition(AwsGreengrassUpdateSubscriptionDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateThingRuntimeConfiguration(AwsGreengrassUpdateThingRuntimeConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}