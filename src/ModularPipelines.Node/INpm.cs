using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
Task<CommandResult> NpmAccessListPackages(NpmAccessListPackagesOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmAccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmAccessGetStatus(NpmAccessGetStatusOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmAccessSet(NpmAccessSetOptions options, CancellationToken token = default);

Task<CommandResult> NpmAccessGrant(NpmAccessGrantOptions options, CancellationToken token = default);

Task<CommandResult> NpmAccessRevoke(NpmAccessRevokeOptions options, CancellationToken token = default);

Task<CommandResult> NpmAdduser(NpmAdduserOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmAudit(NpmAuditOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmBugs(NpmBugsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCacheAdd(NpmCacheAddOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCacheClean(NpmCacheCleanOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCacheLs(NpmCacheLsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCacheVerify(NpmCacheVerifyOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCi(NpmCiOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmCompletion(NpmCompletionOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmConfigSet(NpmConfigSetOptions options, CancellationToken token = default);

Task<CommandResult> NpmConfigGet(NpmConfigGetOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmConfigDelete(NpmConfigDeleteOptions options, CancellationToken token = default);

Task<CommandResult> NpmConfigList(NpmConfigListOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmConfigEdit(NpmConfigEditOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmConfigFix(NpmConfigFixOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmDedupe(NpmDedupeOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmDeprecate(NpmDeprecateOptions options, CancellationToken token = default);

Task<CommandResult> NpmDiff(NpmDiffOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmDocs(NpmDocsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmDoctor(NpmDoctorOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmEdit(NpmEditOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmExec(NpmExecOptions options, CancellationToken token = default);

Task<CommandResult> NpmExecC(NpmExecCOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmExplain(NpmExplainOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmExplore(NpmExploreOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmFund(NpmFundOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmHelp(NpmHelpOptions options, CancellationToken token = default);

Task<CommandResult> NpmHookAdd(NpmHookAddOptions options, CancellationToken token = default);

Task<CommandResult> NpmHookLs(NpmHookLsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmHookRm(NpmHookRmOptions options, CancellationToken token = default);

Task<CommandResult> NpmHookUpdate(NpmHookUpdateOptions options, CancellationToken token = default);

Task<CommandResult> NpmInit(NpmInitOptions options, CancellationToken token = default);

Task<CommandResult> NpmInstall(NpmInstallOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmLink(NpmLinkOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmLogin(NpmLoginOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmLogout(NpmLogoutOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmLs(NpmLsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmOrgSet(NpmOrgSetOptions options, CancellationToken token = default);

Task<CommandResult> NpmOrgRm(NpmOrgRmOptions options, CancellationToken token = default);

Task<CommandResult> NpmOrgLs(NpmOrgLsOptions options, CancellationToken token = default);

Task<CommandResult> NpmOutdated(NpmOutdatedOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmOwnerAdd(NpmOwnerAddOptions options, CancellationToken token = default);

Task<CommandResult> NpmOwnerRm(NpmOwnerRmOptions options, CancellationToken token = default);

Task<CommandResult> NpmOwnerLs(NpmOwnerLsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPack(NpmPackOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPing(NpmPingOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPkgSet(NpmPkgSetOptions options, CancellationToken token = default);

Task<CommandResult> NpmPkgGet(NpmPkgGetOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPkgDelete(NpmPkgDeleteOptions options, CancellationToken token = default);

Task<CommandResult> NpmPkgFix(NpmPkgFixOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPrefix(NpmPrefixOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmProfileEnable2fa(NpmProfileEnable2faOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmProfileDisable2fa(NpmProfileDisable2faOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmProfileGet(NpmProfileGetOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmProfileSet(NpmProfileSetOptions options, CancellationToken token = default);

Task<CommandResult> NpmPrune(NpmPruneOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmPublish(NpmPublishOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmQuery(NpmQueryOptions options, CancellationToken token = default);

Task<CommandResult> NpmRebuild(NpmRebuildOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmRepo(NpmRepoOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmRestart(NpmRestartOptions options, CancellationToken token = default);

Task<CommandResult> NpmRoot(NpmRootOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmSbom(NpmSbomOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmSearch(NpmSearchOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmShrinkwrap(NpmShrinkwrapOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmStar(NpmStarOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmStars(NpmStarsOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmStart(NpmStartOptions options, CancellationToken token = default);

Task<CommandResult> NpmStop(NpmStopOptions options, CancellationToken token = default);

Task<CommandResult> NpmTeamCreate(NpmTeamCreateOptions options, CancellationToken token = default);

Task<CommandResult> NpmTeamDestroy(NpmTeamDestroyOptions options, CancellationToken token = default);

Task<CommandResult> NpmTeamAdd(NpmTeamAddOptions options, CancellationToken token = default);

Task<CommandResult> NpmTeamRm(NpmTeamRmOptions options, CancellationToken token = default);

Task<CommandResult> NpmTeamLs(NpmTeamLsOptions options, CancellationToken token = default);

Task<CommandResult> NpmTest(NpmTestOptions options, CancellationToken token = default);

Task<CommandResult> NpmTokenList(NpmTokenListOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmTokenRevoke(NpmTokenRevokeOptions options, CancellationToken token = default);

Task<CommandResult> NpmTokenCreate(NpmTokenCreateOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmUninstall(NpmUninstallOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmUnpublish(NpmUnpublishOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmUnstar(NpmUnstarOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmUpdate(NpmUpdateOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmVersion(NpmVersionOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmView(NpmViewOptions? options = default, CancellationToken token = default);

Task<CommandResult> NpmWhoami(NpmWhoamiOptions? options = default, CancellationToken token = default);

Task<CommandResult> Npx(NpxOptions options, CancellationToken token = default);

Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken token = default);

}
