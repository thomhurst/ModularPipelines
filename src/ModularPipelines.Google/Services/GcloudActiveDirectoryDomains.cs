using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory")]
public class GcloudActiveDirectoryDomains
{
    public GcloudActiveDirectoryDomains(
        GcloudActiveDirectoryDomainsBackups backups,
        GcloudActiveDirectoryDomainsTrusts trusts,
        ICommand internalCommand
    )
    {
        Backups = backups;
        Trusts = trusts;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudActiveDirectoryDomainsBackups Backups { get; }

    public GcloudActiveDirectoryDomainsTrusts Trusts { get; }

    public async Task<CommandResult> Create(GcloudActiveDirectoryDomainsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudActiveDirectoryDomainsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudActiveDirectoryDomainsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeLdapsSettings(GcloudActiveDirectoryDomainsDescribeLdapsSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExtendSchema(GcloudActiveDirectoryDomainsExtendSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudActiveDirectoryDomainsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudActiveDirectoryDomainsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudActiveDirectoryDomainsListOptions(), token);
    }

    public async Task<CommandResult> ResetAdminPassword(GcloudActiveDirectoryDomainsResetAdminPasswordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(GcloudActiveDirectoryDomainsRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudActiveDirectoryDomainsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudActiveDirectoryDomainsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLdapsSettings(GcloudActiveDirectoryDomainsUpdateLdapsSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}