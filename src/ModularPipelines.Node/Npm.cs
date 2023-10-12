using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
internal class Npm : INpm
{
    private readonly ICommand _command;

    public Npm(ICommand command)
    {
        _command = command;
    }

    public async Task<CommandResult> NpmAccessListPackages(NpmAccessListPackagesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListPackagesOptions(), token);
    }

    public async Task<CommandResult> NpmAccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListCollaboratorsOptions(), token);
    }

    public async Task<CommandResult> NpmAccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessGetStatusOptions(), token);
    }

    public async Task<CommandResult> NpmAccessSet(NpmAccessSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmAccessGrant(NpmAccessGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmAccessRevoke(NpmAccessRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmAdduser(NpmAdduserOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAdduserOptions(), token);
    }

    public async Task<CommandResult> NpmAudit(NpmAuditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAuditOptions(), token);
    }

    public async Task<CommandResult> NpmBugs(NpmBugsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmBugsOptions(), token);
    }

    public async Task<CommandResult> NpmCacheAdd(NpmCacheAddOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheAddOptions(), token);
    }

    public async Task<CommandResult> NpmCacheClean(NpmCacheCleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheCleanOptions(), token);
    }

    public async Task<CommandResult> NpmCacheLs(NpmCacheLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheLsOptions(), token);
    }

    public async Task<CommandResult> NpmCacheVerify(NpmCacheVerifyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheVerifyOptions(), token);
    }

    public async Task<CommandResult> NpmCi(NpmCiOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCiOptions(), token);
    }

    public async Task<CommandResult> NpmCompletion(NpmCompletionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCompletionOptions(), token);
    }

    public async Task<CommandResult> NpmConfigSet(NpmConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmConfigGet(NpmConfigGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigGetOptions(), token);
    }

    public async Task<CommandResult> NpmConfigDelete(NpmConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmConfigList(NpmConfigListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigListOptions(), token);
    }

    public async Task<CommandResult> NpmConfigEdit(NpmConfigEditOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigEditOptions(), token);
    }

    public async Task<CommandResult> NpmConfigFix(NpmConfigFixOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigFixOptions(), token);
    }

    public async Task<CommandResult> NpmDedupe(NpmDedupeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDedupeOptions(), token);
    }

    public async Task<CommandResult> NpmDeprecate(NpmDeprecateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmDiff(NpmDiffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDiffOptions(), token);
    }

    public async Task<CommandResult> NpmDocs(NpmDocsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDocsOptions(), token);
    }

    public async Task<CommandResult> NpmDoctor(NpmDoctorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDoctorOptions(), token);
    }

    public async Task<CommandResult> NpmEdit(NpmEditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmEditOptions(), token);
    }

    public async Task<CommandResult> NpmExec(NpmExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmExecC(NpmExecCOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExecCOptions(), token);
    }

    public async Task<CommandResult> NpmExplain(NpmExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExplainOptions(), token);
    }

    public async Task<CommandResult> NpmExplore(NpmExploreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExploreOptions(), token);
    }

    public async Task<CommandResult> NpmFund(NpmFundOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFundOptions(), token);
    }

    public async Task<CommandResult> NpmHelp(NpmHelpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmHookAdd(NpmHookAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmHookLs(NpmHookLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHookLsOptions(), token);
    }

    public async Task<CommandResult> NpmHookRm(NpmHookRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmHookUpdate(NpmHookUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmInit(NpmInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmInstall(NpmInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallOptions(), token);
    }

    public async Task<CommandResult> NpmLink(NpmLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLinkOptions(), token);
    }

    public async Task<CommandResult> NpmLogin(NpmLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLoginOptions(), token);
    }

    public async Task<CommandResult> NpmLogout(NpmLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLogoutOptions(), token);
    }

    public async Task<CommandResult> NpmLs(NpmLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLsOptions(), token);
    }

    public async Task<CommandResult> NpmOrgSet(NpmOrgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmOrgRm(NpmOrgRmOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmOrgLs(NpmOrgLsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmOutdated(NpmOutdatedOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOutdatedOptions(), token);
    }

    public async Task<CommandResult> NpmOwnerAdd(NpmOwnerAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmOwnerRm(NpmOwnerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmOwnerLs(NpmOwnerLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOwnerLsOptions(), token);
    }

    public async Task<CommandResult> NpmPack(NpmPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPackOptions(), token);
    }

    public async Task<CommandResult> NpmPing(NpmPingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPingOptions(), token);
    }

    public async Task<CommandResult> NpmPkgSet(NpmPkgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmPkgGet(NpmPkgGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgGetOptions(), token);
    }

    public async Task<CommandResult> NpmPkgDelete(NpmPkgDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmPkgFix(NpmPkgFixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgFixOptions(), token);
    }

    public async Task<CommandResult> NpmPrefix(NpmPrefixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPrefixOptions(), token);
    }

    public async Task<CommandResult> NpmProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileEnable2faOptions(), token);
    }

    public async Task<CommandResult> NpmProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileDisable2faOptions(), token);
    }

    public async Task<CommandResult> NpmProfileGet(NpmProfileGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileGetOptions(), token);
    }

    public async Task<CommandResult> NpmProfileSet(NpmProfileSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmPrune(NpmPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPruneOptions(), token);
    }

    public async Task<CommandResult> NpmPublish(NpmPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPublishOptions(), token);
    }

    public async Task<CommandResult> NpmQuery(NpmQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmRebuild(NpmRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRebuildOptions(), token);
    }

    public async Task<CommandResult> NpmRepo(NpmRepoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRepoOptions(), token);
    }

    public async Task<CommandResult> NpmRestart(NpmRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmRoot(NpmRootOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRootOptions(), token);
    }

    public async Task<CommandResult> NpmSbom(NpmSbomOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSbomOptions(), token);
    }

    public async Task<CommandResult> NpmSearch(NpmSearchOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmShrinkwrap(NpmShrinkwrapOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmShrinkwrapOptions(), token);
    }

    public async Task<CommandResult> NpmStar(NpmStarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarOptions(), token);
    }

    public async Task<CommandResult> NpmStars(NpmStarsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarsOptions(), token);
    }

    public async Task<CommandResult> NpmStart(NpmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmStop(NpmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTeamCreate(NpmTeamCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTeamDestroy(NpmTeamDestroyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTeamAdd(NpmTeamAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTeamRm(NpmTeamRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTeamLs(NpmTeamLsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTest(NpmTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTokenList(NpmTokenListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenListOptions(), token);
    }

    public async Task<CommandResult> NpmTokenRevoke(NpmTokenRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTokenCreate(NpmTokenCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenCreateOptions(), token);
    }

    public async Task<CommandResult> NpmUninstall(NpmUninstallOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUninstallOptions(), token);
    }

    public async Task<CommandResult> NpmUnpublish(NpmUnpublishOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnpublishOptions(), token);
    }

    public async Task<CommandResult> NpmUnstar(NpmUnstarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnstarOptions(), token);
    }

    public async Task<CommandResult> NpmUpdate(NpmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUpdateOptions(), token);
    }

    public async Task<CommandResult> NpmVersion(NpmVersionOptions? options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmView(NpmViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmViewOptions(), token);
    }

    public async Task<CommandResult> NpmWhoami(NpmWhoamiOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmWhoamiOptions(), token);
    }

    public async Task<CommandResult> Npx(NpxOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpxCOptions(), token);
    }
}