using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("spanner")]
public class GcloudSpannerDatabases
{
    public GcloudSpannerDatabases(
        GcloudSpannerDatabasesDdl ddl,
        GcloudSpannerDatabasesRoles roles,
        GcloudSpannerDatabasesSessions sessions,
        ICommand internalCommand
    )
    {
        Ddl = ddl;
        Roles = roles;
        Sessions = sessions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudSpannerDatabasesDdl Ddl { get; }

    public GcloudSpannerDatabasesRoles Roles { get; }

    public GcloudSpannerDatabasesSessions Sessions { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudSpannerDatabasesAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudSpannerDatabasesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudSpannerDatabasesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudSpannerDatabasesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteSql(GcloudSpannerDatabasesExecuteSqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudSpannerDatabasesGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudSpannerDatabasesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudSpannerDatabasesListOptions(), token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudSpannerDatabasesRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(GcloudSpannerDatabasesRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudSpannerDatabasesSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudSpannerDatabasesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}