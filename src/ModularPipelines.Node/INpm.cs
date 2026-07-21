using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
    Task<CommandResult> AccessListPackages(NpmAccessListPackagesOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> AccessSet(NpmAccessSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> AccessGrant(NpmAccessGrantOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> AccessRevoke(NpmAccessRevokeOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheAdd(NpmCacheAddOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheClean(NpmCacheCleanOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheLs(NpmCacheLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> CacheVerify(NpmCacheVerifyOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Ci(NpmCiOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Completion(NpmCompletionOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigSet(NpmConfigSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigSetRegistry(NpmConfigSetRegistryOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigGet(NpmConfigGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigDelete(NpmConfigDeleteOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigList(NpmConfigListOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigEdit(NpmConfigEditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ConfigFix(NpmConfigFixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Deprecate(NpmDeprecateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Exec(NpmExecOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> ExecC(NpmExecCOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Help(NpmHelpOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookAdd(NpmHookAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookLs(NpmHookLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> HookRm(NpmHookRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> HookUpdate(NpmHookUpdateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Init(NpmInitOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgSet(NpmOrgSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgRm(NpmOrgRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OrgLs(NpmOrgLsOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Outdated(NpmOutdatedOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerAdd(NpmOwnerAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerRm(NpmOwnerRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> OwnerLs(NpmOwnerLsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgSet(NpmPkgSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgGet(NpmPkgGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgDelete(NpmPkgDeleteOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> PkgFix(NpmPkgFixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileGet(NpmProfileGetOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> ProfileSet(NpmProfileSetOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Query(NpmQueryOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Restart(NpmRestartOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Search(NpmSearchOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Start(NpmStartOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Stop(NpmStopOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamCreate(NpmTeamCreateOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamDestroy(NpmTeamDestroyOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamAdd(NpmTeamAddOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamRm(NpmTeamRmOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TeamLs(NpmTeamLsOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Test(NpmTestOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenList(NpmTokenListOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenRevoke(NpmTokenRevokeOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> TokenCreate(NpmTokenCreateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Uninstall(NpmUninstallOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Version(NpmVersionOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken cancellationToken = default);

    Task<CommandResult> Npx(NpxOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken cancellationToken = default);
}