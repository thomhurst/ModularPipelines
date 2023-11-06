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

    public async Task<CommandResult> AccessListPackages(NpmAccessListPackagesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListPackagesOptions(), token);
    }

    public async Task<CommandResult> AccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListCollaboratorsOptions(), token);
    }

    public async Task<CommandResult> AccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessGetStatusOptions(), token);
    }

    public async Task<CommandResult> AccessSet(NpmAccessSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AccessGrant(NpmAccessGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AccessRevoke(NpmAccessRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAdduserOptions(), token);
    }

    public async Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAuditOptions(), token);
    }

    public async Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmBugsOptions(), token);
    }

    public async Task<CommandResult> CacheAdd(NpmCacheAddOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheAddOptions(), token);
    }

    public async Task<CommandResult> CacheClean(NpmCacheCleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheCleanOptions(), token);
    }

    public async Task<CommandResult> CacheLs(NpmCacheLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheLsOptions(), token);
    }

    public async Task<CommandResult> CacheVerify(NpmCacheVerifyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheVerifyOptions(), token);
    }

    public async Task<CommandResult> Ci(NpmCiOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCiOptions(), token);
    }

    public async Task<CommandResult> Completion(NpmCompletionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCompletionOptions(), token);
    }

    public async Task<CommandResult> ConfigSet(NpmConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigSetRegistry(NpmConfigSetRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigGet(NpmConfigGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigGetOptions(), token);
    }

    public async Task<CommandResult> ConfigDelete(NpmConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigList(NpmConfigListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigListOptions(), token);
    }

    public async Task<CommandResult> ConfigEdit(NpmConfigEditOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigEditOptions(), token);
    }

    public async Task<CommandResult> ConfigFix(NpmConfigFixOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigFixOptions(), token);
    }

    public async Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDedupeOptions(), token);
    }

    public async Task<CommandResult> Deprecate(NpmDeprecateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDiffOptions(), token);
    }

    public async Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDocsOptions(), token);
    }

    public async Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDoctorOptions(), token);
    }

    public async Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmEditOptions(), token);
    }

    public async Task<CommandResult> Exec(NpmExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecC(NpmExecCOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExecCOptions(), token);
    }

    public async Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExplainOptions(), token);
    }

    public async Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExploreOptions(), token);
    }

    public async Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFundOptions(), token);
    }

    public async Task<CommandResult> Help(NpmHelpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> HookAdd(NpmHookAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> HookLs(NpmHookLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHookLsOptions(), token);
    }

    public async Task<CommandResult> HookRm(NpmHookRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> HookUpdate(NpmHookUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Init(NpmInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallOptions(), token);
    }

    public async Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLinkOptions(), token);
    }

    public async Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLoginOptions(), token);
    }

    public async Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLogoutOptions(), token);
    }

    public async Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLsOptions(), token);
    }

    public async Task<CommandResult> OrgSet(NpmOrgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OrgRm(NpmOrgRmOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OrgLs(NpmOrgLsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Outdated(NpmOutdatedOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOutdatedOptions(), token);
    }

    public async Task<CommandResult> OwnerAdd(NpmOwnerAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OwnerRm(NpmOwnerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> OwnerLs(NpmOwnerLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOwnerLsOptions(), token);
    }

    public async Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPackOptions(), token);
    }

    public async Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPingOptions(), token);
    }

    public async Task<CommandResult> PkgSet(NpmPkgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PkgGet(NpmPkgGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgGetOptions(), token);
    }

    public async Task<CommandResult> PkgDelete(NpmPkgDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PkgFix(NpmPkgFixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgFixOptions(), token);
    }

    public async Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPrefixOptions(), token);
    }

    public async Task<CommandResult> ProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileEnable2faOptions(), token);
    }

    public async Task<CommandResult> ProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileDisable2faOptions(), token);
    }

    public async Task<CommandResult> ProfileGet(NpmProfileGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileGetOptions(), token);
    }

    public async Task<CommandResult> ProfileSet(NpmProfileSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPruneOptions(), token);
    }

    public async Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPublishOptions(), token);
    }

    public async Task<CommandResult> Query(NpmQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRebuildOptions(), token);
    }

    public async Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRepoOptions(), token);
    }

    public async Task<CommandResult> Restart(NpmRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRootOptions(), token);
    }

    public async Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSbomOptions(), token);
    }

    public async Task<CommandResult> Search(NpmSearchOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmShrinkwrapOptions(), token);
    }

    public async Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarOptions(), token);
    }

    public async Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarsOptions(), token);
    }

    public async Task<CommandResult> Start(NpmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(NpmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TeamCreate(NpmTeamCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TeamDestroy(NpmTeamDestroyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TeamAdd(NpmTeamAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TeamRm(NpmTeamRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TeamLs(NpmTeamLsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Test(NpmTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TokenList(NpmTokenListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenListOptions(), token);
    }

    public async Task<CommandResult> TokenRevoke(NpmTokenRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TokenCreate(NpmTokenCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenCreateOptions(), token);
    }

    public async Task<CommandResult> Uninstall(NpmUninstallOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUninstallOptions(), token);
    }

    public async Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnpublishOptions(), token);
    }

    public async Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnstarOptions(), token);
    }

    public async Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUpdateOptions(), token);
    }

    public async Task<CommandResult> Version(NpmVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmViewOptions(), token);
    }

    public async Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken token = default)
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