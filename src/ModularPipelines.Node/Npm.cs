using ModularPipelines.Context;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
internal class Npm : INpm
{
    private readonly ICommandContext _command;

    public Npm(ICommandContext command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> AccessListPackagesAsync(NpmAccessListPackagesOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmAccessListPackagesOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessListCollaboratorsAsync(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmAccessListCollaboratorsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessGetStatusAsync(NpmAccessGetStatusOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmAccessGetStatusOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessSetAsync(NpmAccessSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessGrantAsync(NpmAccessGrantOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessRevokeAsync(NpmAccessRevokeOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> AdduserAsync(NpmAdduserOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmAdduserOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AuditAsync(NpmAuditOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmAuditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> BugsAsync(NpmBugsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmBugsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheAddAsync(NpmCacheAddOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCacheAddOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheCleanAsync(NpmCacheCleanOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCacheCleanOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheLsAsync(NpmCacheLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCacheLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheVerifyAsync(NpmCacheVerifyOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCacheVerifyOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CiAsync(NpmCiOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCiOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CompletionAsync(NpmCompletionOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmCompletionOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigSetAsync(NpmConfigSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigSetRegistryAsync(NpmConfigSetRegistryOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigGetAsync(NpmConfigGetOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmConfigGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigDeleteAsync(NpmConfigDeleteOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigListAsync(NpmConfigListOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmConfigListOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigEditAsync(NpmConfigEditOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmConfigEditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigFixAsync(NpmConfigFixOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmConfigFixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> DedupeAsync(NpmDedupeOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmDedupeOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> DeprecateAsync(NpmDeprecateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> DiffAsync(NpmDiffOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmDiffOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> DocsAsync(NpmDocsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmDocsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> DoctorAsync(NpmDoctorOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmDoctorOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> EditAsync(NpmEditOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmEditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ExecAsync(NpmExecOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ExecCAsync(NpmExecCOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmExecCOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ExplainAsync(NpmExplainOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmExplainOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ExploreAsync(NpmExploreOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmExploreOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> FundAsync(NpmFundOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmFundOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> HelpAsync(NpmHelpOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookAddAsync(NpmHookAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookLsAsync(NpmHookLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmHookLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookRmAsync(NpmHookRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookUpdateAsync(NpmHookUpdateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> InitAsync(NpmInitOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> InstallAsync(NpmInstallOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmInstallOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> LinkAsync(NpmLinkOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmLinkOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> LoginAsync(NpmLoginOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmLoginOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> LogoutAsync(NpmLogoutOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmLogoutOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> LsAsync(NpmLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgSetAsync(NpmOrgSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgRmAsync(NpmOrgRmOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgLsAsync(NpmOrgLsOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OutdatedAsync(NpmOutdatedOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmOutdatedOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerAddAsync(NpmOwnerAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerRmAsync(NpmOwnerRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerLsAsync(NpmOwnerLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmOwnerLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PackAsync(NpmPackOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPackOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PingAsync(NpmPingOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPingOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgSetAsync(NpmPkgSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgGetAsync(NpmPkgGetOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPkgGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgDeleteAsync(NpmPkgDeleteOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgFixAsync(NpmPkgFixOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPkgFixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PrefixAsync(NpmPrefixOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPrefixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileEnable2faAsync(NpmProfileEnable2faOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmProfileEnable2faOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileDisable2faAsync(NpmProfileDisable2faOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmProfileDisable2faOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileGetAsync(NpmProfileGetOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmProfileGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileSetAsync(NpmProfileSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> PruneAsync(NpmPruneOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPruneOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PublishAsync(NpmPublishOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmPublishOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> QueryAsync(NpmQueryOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> RebuildAsync(NpmRebuildOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmRebuildOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> RepoAsync(NpmRepoOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmRepoOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> RestartAsync(NpmRestartOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> RootAsync(NpmRootOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmRootOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> RunAsync(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> SbomAsync(NpmSbomOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmSbomOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> SearchAsync(NpmSearchOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ShrinkwrapAsync(NpmShrinkwrapOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmShrinkwrapOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> StarAsync(NpmStarOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmStarOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> StarsAsync(NpmStarsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmStarsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> StartAsync(NpmStartOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> StopAsync(NpmStopOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamCreateAsync(NpmTeamCreateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamDestroyAsync(NpmTeamDestroyOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamAddAsync(NpmTeamAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamRmAsync(NpmTeamRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamLsAsync(NpmTeamLsOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TestAsync(NpmTestOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenListAsync(NpmTokenListOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmTokenListOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenRevokeAsync(NpmTokenRevokeOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenCreateAsync(NpmTokenCreateOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmTokenCreateOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> UninstallAsync(NpmUninstallOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmUninstallOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> UnpublishAsync(NpmUnpublishOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmUnpublishOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> UnstarAsync(NpmUnstarOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmUnstarOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> UpdateAsync(NpmUpdateOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmUpdateOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> VersionAsync(NpmVersionOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ViewAsync(NpmViewOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmViewOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> WhoamiAsync(NpmWhoamiOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpmWhoamiOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> NpxAsync(NpxOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> NpxCAsync(NpxCOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new NpxCOptions(), null, cancellationToken);
    }
}