using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsWorkspaces
{
    public AwsWorkspaces(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateConnectionAlias(AwsWorkspacesAssociateConnectionAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateIpGroups(AwsWorkspacesAssociateIpGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWorkspaceApplication(AwsWorkspacesAssociateWorkspaceApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeIpRules(AwsWorkspacesAuthorizeIpRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyWorkspaceImage(AwsWorkspacesCopyWorkspaceImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectClientAddIn(AwsWorkspacesCreateConnectClientAddInOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectionAlias(AwsWorkspacesCreateConnectionAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIpGroup(AwsWorkspacesCreateIpGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateStandbyWorkspaces(AwsWorkspacesCreateStandbyWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTags(AwsWorkspacesCreateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUpdatedWorkspaceImage(AwsWorkspacesCreateUpdatedWorkspaceImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkspaceBundle(AwsWorkspacesCreateWorkspaceBundleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkspaceImage(AwsWorkspacesCreateWorkspaceImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWorkspaces(AwsWorkspacesCreateWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteClientBranding(AwsWorkspacesDeleteClientBrandingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnectClientAddIn(AwsWorkspacesDeleteConnectClientAddInOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnectionAlias(AwsWorkspacesDeleteConnectionAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpGroup(AwsWorkspacesDeleteIpGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsWorkspacesDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWorkspaceBundle(AwsWorkspacesDeleteWorkspaceBundleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDeleteWorkspaceBundleOptions(), token);
    }

    public async Task<CommandResult> DeleteWorkspaceImage(AwsWorkspacesDeleteWorkspaceImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeployWorkspaceApplications(AwsWorkspacesDeployWorkspaceApplicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterWorkspaceDirectory(AwsWorkspacesDeregisterWorkspaceDirectoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccount(AwsWorkspacesDescribeAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeAccountOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountModifications(AwsWorkspacesDescribeAccountModificationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeAccountModificationsOptions(), token);
    }

    public async Task<CommandResult> DescribeApplicationAssociations(AwsWorkspacesDescribeApplicationAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeApplications(AwsWorkspacesDescribeApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeApplicationsOptions(), token);
    }

    public async Task<CommandResult> DescribeBundleAssociations(AwsWorkspacesDescribeBundleAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClientBranding(AwsWorkspacesDescribeClientBrandingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeClientProperties(AwsWorkspacesDescribeClientPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnectClientAddIns(AwsWorkspacesDescribeConnectClientAddInsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnectionAliasPermissions(AwsWorkspacesDescribeConnectionAliasPermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnectionAliases(AwsWorkspacesDescribeConnectionAliasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeConnectionAliasesOptions(), token);
    }

    public async Task<CommandResult> DescribeImageAssociations(AwsWorkspacesDescribeImageAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeIpGroups(AwsWorkspacesDescribeIpGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeIpGroupsOptions(), token);
    }

    public async Task<CommandResult> DescribeTags(AwsWorkspacesDescribeTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkspaceAssociations(AwsWorkspacesDescribeWorkspaceAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkspaceBundles(AwsWorkspacesDescribeWorkspaceBundlesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeWorkspaceBundlesOptions(), token);
    }

    public async Task<CommandResult> DescribeWorkspaceDirectories(AwsWorkspacesDescribeWorkspaceDirectoriesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeWorkspaceDirectoriesOptions(), token);
    }

    public async Task<CommandResult> DescribeWorkspaceImagePermissions(AwsWorkspacesDescribeWorkspaceImagePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkspaceImages(AwsWorkspacesDescribeWorkspaceImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeWorkspaceImagesOptions(), token);
    }

    public async Task<CommandResult> DescribeWorkspaceSnapshots(AwsWorkspacesDescribeWorkspaceSnapshotsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeWorkspaces(AwsWorkspacesDescribeWorkspacesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeWorkspacesOptions(), token);
    }

    public async Task<CommandResult> DescribeWorkspacesConnectionStatus(AwsWorkspacesDescribeWorkspacesConnectionStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesDescribeWorkspacesConnectionStatusOptions(), token);
    }

    public async Task<CommandResult> DisassociateConnectionAlias(AwsWorkspacesDisassociateConnectionAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateIpGroups(AwsWorkspacesDisassociateIpGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWorkspaceApplication(AwsWorkspacesDisassociateWorkspaceApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportClientBranding(AwsWorkspacesImportClientBrandingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportWorkspaceImage(AwsWorkspacesImportWorkspaceImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableManagementCidrRanges(AwsWorkspacesListAvailableManagementCidrRangesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MigrateWorkspace(AwsWorkspacesMigrateWorkspaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyAccount(AwsWorkspacesModifyAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesModifyAccountOptions(), token);
    }

    public async Task<CommandResult> ModifyCertificateBasedAuthProperties(AwsWorkspacesModifyCertificateBasedAuthPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyClientProperties(AwsWorkspacesModifyClientPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySamlProperties(AwsWorkspacesModifySamlPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifySelfservicePermissions(AwsWorkspacesModifySelfservicePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyWorkspaceAccessProperties(AwsWorkspacesModifyWorkspaceAccessPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyWorkspaceCreationProperties(AwsWorkspacesModifyWorkspaceCreationPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyWorkspaceProperties(AwsWorkspacesModifyWorkspacePropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ModifyWorkspaceState(AwsWorkspacesModifyWorkspaceStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootWorkspaces(AwsWorkspacesRebootWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebuildWorkspaces(AwsWorkspacesRebuildWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterWorkspaceDirectory(AwsWorkspacesRegisterWorkspaceDirectoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreWorkspace(AwsWorkspacesRestoreWorkspaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeIpRules(AwsWorkspacesRevokeIpRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartWorkspaces(AwsWorkspacesStartWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopWorkspaces(AwsWorkspacesStopWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateWorkspaces(AwsWorkspacesTerminateWorkspacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectClientAddIn(AwsWorkspacesUpdateConnectClientAddInOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectionAliasPermission(AwsWorkspacesUpdateConnectionAliasPermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRulesOfIpGroup(AwsWorkspacesUpdateRulesOfIpGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWorkspaceBundle(AwsWorkspacesUpdateWorkspaceBundleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWorkspacesUpdateWorkspaceBundleOptions(), token);
    }

    public async Task<CommandResult> UpdateWorkspaceImagePermission(AwsWorkspacesUpdateWorkspaceImagePermissionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}