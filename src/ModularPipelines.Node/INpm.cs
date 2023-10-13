using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
    Task<CommandResult> AccessListPackages(NpmAccessListPackagesOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> AccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> AccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> AccessSet(NpmAccessSetOptions options, CancellationToken token = default);

    Task<CommandResult> AccessGrant(NpmAccessGrantOptions options, CancellationToken token = default);

    Task<CommandResult> AccessRevoke(NpmAccessRevokeOptions options, CancellationToken token = default);

    Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheAdd(NpmCacheAddOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheClean(NpmCacheCleanOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheLs(NpmCacheLsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheVerify(NpmCacheVerifyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Ci(NpmCiOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Completion(NpmCompletionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigSet(NpmConfigSetOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigGet(NpmConfigGetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigDelete(NpmConfigDeleteOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigList(NpmConfigListOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigEdit(NpmConfigEditOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigFix(NpmConfigFixOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Deprecate(NpmDeprecateOptions options, CancellationToken token = default);

    Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Exec(NpmExecOptions options, CancellationToken token = default);

    Task<CommandResult> ExecC(NpmExecCOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Help(NpmHelpOptions options, CancellationToken token = default);

    Task<CommandResult> HookAdd(NpmHookAddOptions options, CancellationToken token = default);

    Task<CommandResult> HookLs(NpmHookLsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> HookRm(NpmHookRmOptions options, CancellationToken token = default);

    Task<CommandResult> HookUpdate(NpmHookUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> Init(NpmInitOptions options, CancellationToken token = default);

    Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> OrgSet(NpmOrgSetOptions options, CancellationToken token = default);

    Task<CommandResult> OrgRm(NpmOrgRmOptions options, CancellationToken token = default);

    Task<CommandResult> OrgLs(NpmOrgLsOptions options, CancellationToken token = default);

    Task<CommandResult> Outdated(NpmOutdatedOptions? options = default, CancellationToken token = default);

    Task<CommandResult> OwnerAdd(NpmOwnerAddOptions options, CancellationToken token = default);

    Task<CommandResult> OwnerRm(NpmOwnerRmOptions options, CancellationToken token = default);

    Task<CommandResult> OwnerLs(NpmOwnerLsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken token = default);

    Task<CommandResult> PkgSet(NpmPkgSetOptions options, CancellationToken token = default);

    Task<CommandResult> PkgGet(NpmPkgGetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> PkgDelete(NpmPkgDeleteOptions options, CancellationToken token = default);

    Task<CommandResult> PkgFix(NpmPkgFixOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ProfileGet(NpmProfileGetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ProfileSet(NpmProfileSetOptions options, CancellationToken token = default);

    Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Query(NpmQueryOptions options, CancellationToken token = default);

    Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Restart(NpmRestartOptions options, CancellationToken token = default);

    Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Search(NpmSearchOptions options, CancellationToken token = default);

    Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Start(NpmStartOptions options, CancellationToken token = default);

    Task<CommandResult> Stop(NpmStopOptions options, CancellationToken token = default);

    Task<CommandResult> TeamCreate(NpmTeamCreateOptions options, CancellationToken token = default);

    Task<CommandResult> TeamDestroy(NpmTeamDestroyOptions options, CancellationToken token = default);

    Task<CommandResult> TeamAdd(NpmTeamAddOptions options, CancellationToken token = default);

    Task<CommandResult> TeamRm(NpmTeamRmOptions options, CancellationToken token = default);

    Task<CommandResult> TeamLs(NpmTeamLsOptions options, CancellationToken token = default);

    Task<CommandResult> Test(NpmTestOptions options, CancellationToken token = default);

    Task<CommandResult> TokenList(NpmTokenListOptions? options = default, CancellationToken token = default);

    Task<CommandResult> TokenRevoke(NpmTokenRevokeOptions options, CancellationToken token = default);

    Task<CommandResult> TokenCreate(NpmTokenCreateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Uninstall(NpmUninstallOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Version(NpmVersionOptions options, CancellationToken token = default);

    Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Npx(NpxOptions options, CancellationToken token = default);

    Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken token = default);
}