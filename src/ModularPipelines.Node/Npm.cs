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

    public virtual async Task<CommandResult> AccessListPackages(NpmAccessListPackagesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListPackagesOptions(), null, token);
    }

    public virtual async Task<CommandResult> AccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListCollaboratorsOptions(), null, token);
    }

    public virtual async Task<CommandResult> AccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessGetStatusOptions(), null, token);
    }

    public virtual async Task<CommandResult> AccessSet(NpmAccessSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> AccessGrant(NpmAccessGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> AccessRevoke(NpmAccessRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAdduserOptions(), null, token);
    }

    public virtual async Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAuditOptions(), null, token);
    }

    public virtual async Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmBugsOptions(), null, token);
    }

    public virtual async Task<CommandResult> CacheAdd(NpmCacheAddOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheAddOptions(), null, token);
    }

    public virtual async Task<CommandResult> CacheClean(NpmCacheCleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheCleanOptions(), null, token);
    }

    public virtual async Task<CommandResult> CacheLs(NpmCacheLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheLsOptions(), null, token);
    }

    public virtual async Task<CommandResult> CacheVerify(NpmCacheVerifyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheVerifyOptions(), null, token);
    }

    public virtual async Task<CommandResult> Ci(NpmCiOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCiOptions(), null, token);
    }

    public virtual async Task<CommandResult> Completion(NpmCompletionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCompletionOptions(), null, token);
    }

    public virtual async Task<CommandResult> ConfigSet(NpmConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> ConfigSetRegistry(NpmConfigSetRegistryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> ConfigGet(NpmConfigGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigGetOptions(), null, token);
    }

    public virtual async Task<CommandResult> ConfigDelete(NpmConfigDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> ConfigList(NpmConfigListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigListOptions(), null, token);
    }

    public virtual async Task<CommandResult> ConfigEdit(NpmConfigEditOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigEditOptions(), null, token);
    }

    public virtual async Task<CommandResult> ConfigFix(NpmConfigFixOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigFixOptions(), null, token);
    }

    public virtual async Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDedupeOptions(), null, token);
    }

    public virtual async Task<CommandResult> Deprecate(NpmDeprecateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDiffOptions(), null, token);
    }

    public virtual async Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDocsOptions(), null, token);
    }

    public virtual async Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDoctorOptions(), null, token);
    }

    public virtual async Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmEditOptions(), null, token);
    }

    public virtual async Task<CommandResult> Exec(NpmExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> ExecC(NpmExecCOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExecCOptions(), null, token);
    }

    public virtual async Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExplainOptions(), null, token);
    }

    public virtual async Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExploreOptions(), null, token);
    }

    public virtual async Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFundOptions(), null, token);
    }

    public virtual async Task<CommandResult> Help(NpmHelpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> HookAdd(NpmHookAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> HookLs(NpmHookLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHookLsOptions(), null, token);
    }

    public virtual async Task<CommandResult> HookRm(NpmHookRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> HookUpdate(NpmHookUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Init(NpmInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallOptions(), null, token);
    }

    public virtual async Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLinkOptions(), null, token);
    }

    public virtual async Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLoginOptions(), null, token);
    }

    public virtual async Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLogoutOptions(), null, token);
    }

    public virtual async Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLsOptions(), null, token);
    }

    public virtual async Task<CommandResult> OrgSet(NpmOrgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> OrgRm(NpmOrgRmOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> OrgLs(NpmOrgLsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Outdated(NpmOutdatedOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOutdatedOptions(), null, token);
    }

    public virtual async Task<CommandResult> OwnerAdd(NpmOwnerAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> OwnerRm(NpmOwnerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> OwnerLs(NpmOwnerLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOwnerLsOptions(), null, token);
    }

    public virtual async Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPackOptions(), null, token);
    }

    public virtual async Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPingOptions(), null, token);
    }

    public virtual async Task<CommandResult> PkgSet(NpmPkgSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> PkgGet(NpmPkgGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgGetOptions(), null, token);
    }

    public virtual async Task<CommandResult> PkgDelete(NpmPkgDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> PkgFix(NpmPkgFixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgFixOptions(), null, token);
    }

    public virtual async Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPrefixOptions(), null, token);
    }

    public virtual async Task<CommandResult> ProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileEnable2faOptions(), null, token);
    }

    public virtual async Task<CommandResult> ProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileDisable2faOptions(), null, token);
    }

    public virtual async Task<CommandResult> ProfileGet(NpmProfileGetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileGetOptions(), null, token);
    }

    public virtual async Task<CommandResult> ProfileSet(NpmProfileSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPruneOptions(), null, token);
    }

    public virtual async Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPublishOptions(), null, token);
    }

    public virtual async Task<CommandResult> Query(NpmQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRebuildOptions(), null, token);
    }

    public virtual async Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRepoOptions(), null, token);
    }

    public virtual async Task<CommandResult> Restart(NpmRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRootOptions(), null, token);
    }

    public virtual async Task<CommandResult> Run(NpmRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSbomOptions(), null, token);
    }

    public virtual async Task<CommandResult> Search(NpmSearchOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmShrinkwrapOptions(), null, token);
    }

    public virtual async Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarOptions(), null, token);
    }

    public virtual async Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarsOptions(), null, token);
    }

    public virtual async Task<CommandResult> Start(NpmStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Stop(NpmStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TeamCreate(NpmTeamCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TeamDestroy(NpmTeamDestroyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TeamAdd(NpmTeamAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TeamRm(NpmTeamRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TeamLs(NpmTeamLsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> Test(NpmTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TokenList(NpmTokenListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenListOptions(), null, token);
    }

    public virtual async Task<CommandResult> TokenRevoke(NpmTokenRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> TokenCreate(NpmTokenCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenCreateOptions(), null, token);
    }

    public virtual async Task<CommandResult> Uninstall(NpmUninstallOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUninstallOptions(), null, token);
    }

    public virtual async Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnpublishOptions(), null, token);
    }

    public virtual async Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnstarOptions(), null, token);
    }

    public virtual async Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUpdateOptions(), null, token);
    }

    public virtual async Task<CommandResult> Version(NpmVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmViewOptions(), null, token);
    }

    public virtual async Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmWhoamiOptions(), null, token);
    }

    public virtual async Task<CommandResult> Npx(NpxOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    public virtual async Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpxCOptions(), null, token);
    }
}