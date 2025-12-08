using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("metastore")]
public class GcloudMetastoreServices
{
    public GcloudMetastoreServices(
        GcloudMetastoreServicesBackups backups,
        GcloudMetastoreServicesExport export,
        GcloudMetastoreServicesImport import,
        ICommand internalCommand
    )
    {
        Backups = backups;
        Export = export;
        Import = import;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudMetastoreServicesBackups Backups { get; }

    public GcloudMetastoreServicesExport Export { get; }

    public GcloudMetastoreServicesImport Import { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudMetastoreServicesAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AlterMetadataResourceLocation(GcloudMetastoreServicesAlterMetadataResourceLocationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AlterTableProperties(GcloudMetastoreServicesAlterTablePropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudMetastoreServicesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudMetastoreServicesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudMetastoreServicesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudMetastoreServicesGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudMetastoreServicesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudMetastoreServicesListOptions(), token);
    }

    public async Task<CommandResult> MoveTableToDatabase(GcloudMetastoreServicesMoveTableToDatabaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> QueryMetadata(GcloudMetastoreServicesQueryMetadataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudMetastoreServicesRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(GcloudMetastoreServicesRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudMetastoreServicesSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudMetastoreServicesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}