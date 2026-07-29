using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
    Task<CommandResult> AccessListPackagesAsync(NpmAccessListPackagesOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessListCollaboratorsAsync(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessGetStatusAsync(NpmAccessGetStatusOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessSetAsync(NpmAccessSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> AccessGrantAsync(NpmAccessGrantOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> AccessRevokeAsync(NpmAccessRevokeOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> AdduserAsync(NpmAdduserOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> AuditAsync(NpmAuditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> BugsAsync(NpmBugsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheAddAsync(NpmCacheAddOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheCleanAsync(NpmCacheCleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheLsAsync(NpmCacheLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheVerifyAsync(NpmCacheVerifyOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CiAsync(NpmCiOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CompletionAsync(NpmCompletionOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigSetAsync(NpmConfigSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigSetRegistryAsync(NpmConfigSetRegistryOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigGetAsync(NpmConfigGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigDeleteAsync(NpmConfigDeleteOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigListAsync(NpmConfigListOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigEditAsync(NpmConfigEditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigFixAsync(NpmConfigFixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DedupeAsync(NpmDedupeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DeprecateAsync(NpmDeprecateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> DiffAsync(NpmDiffOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DocsAsync(NpmDocsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> DoctorAsync(NpmDoctorOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> EditAsync(NpmEditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ExecAsync(NpmExecOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ExecCAsync(NpmExecCOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ExplainAsync(NpmExplainOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ExploreAsync(NpmExploreOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> FundAsync(NpmFundOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> HelpAsync(NpmHelpOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookAddAsync(NpmHookAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookLsAsync(NpmHookLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> HookRmAsync(NpmHookRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookUpdateAsync(NpmHookUpdateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> InitAsync(NpmInitOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> InstallAsync(NpmInstallOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> LinkAsync(NpmLinkOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> LoginAsync(NpmLoginOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> LogoutAsync(NpmLogoutOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> LsAsync(NpmLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgSetAsync(NpmOrgSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgRmAsync(NpmOrgRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgLsAsync(NpmOrgLsOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OutdatedAsync(NpmOutdatedOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerAddAsync(NpmOwnerAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerRmAsync(NpmOwnerRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerLsAsync(NpmOwnerLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PackAsync(NpmPackOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PingAsync(NpmPingOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgSetAsync(NpmPkgSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgGetAsync(NpmPkgGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgDeleteAsync(NpmPkgDeleteOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgFixAsync(NpmPkgFixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PrefixAsync(NpmPrefixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileEnable2faAsync(NpmProfileEnable2faOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileDisable2faAsync(NpmProfileDisable2faOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileGetAsync(NpmProfileGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileSetAsync(NpmProfileSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PruneAsync(NpmPruneOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PublishAsync(NpmPublishOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> QueryAsync(NpmQueryOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> RebuildAsync(NpmRebuildOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> RepoAsync(NpmRepoOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> RestartAsync(NpmRestartOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> RootAsync(NpmRootOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> RunAsync(NpmRunOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> SbomAsync(NpmSbomOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> SearchAsync(NpmSearchOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ShrinkwrapAsync(NpmShrinkwrapOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> StarAsync(NpmStarOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> StarsAsync(NpmStarsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> StartAsync(NpmStartOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> StopAsync(NpmStopOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamCreateAsync(NpmTeamCreateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamDestroyAsync(NpmTeamDestroyOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamAddAsync(NpmTeamAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamRmAsync(NpmTeamRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamLsAsync(NpmTeamLsOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TestAsync(NpmTestOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenListAsync(NpmTokenListOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenRevokeAsync(NpmTokenRevokeOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenCreateAsync(NpmTokenCreateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UninstallAsync(NpmUninstallOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UnpublishAsync(NpmUnpublishOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UnstarAsync(NpmUnstarOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> UpdateAsync(NpmUpdateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> VersionAsync(NpmVersionOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ViewAsync(NpmViewOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> WhoamiAsync(NpmWhoamiOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> NpxAsync(NpxOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> NpxCAsync(NpxCOptions? options = default, CancellationToken cancellationToken = default);
}