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
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListPackagesOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessListCollaborators(NpmAccessListCollaboratorsOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessListCollaboratorsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessGetStatus(NpmAccessGetStatusOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessGetStatusOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessSet(NpmAccessSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessGrant(NpmAccessGrantOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> AccessRevoke(NpmAccessRevokeOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAdduserOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAuditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmBugsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheAdd(NpmCacheAddOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheAddOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheClean(NpmCacheCleanOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheCleanOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheLs(NpmCacheLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> CacheVerify(NpmCacheVerifyOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheVerifyOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Ci(NpmCiOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCiOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Completion(NpmCompletionOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCompletionOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigSet(NpmConfigSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigSetRegistry(NpmConfigSetRegistryOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigGet(NpmConfigGetOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigDelete(NpmConfigDeleteOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigList(NpmConfigListOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigListOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigEdit(NpmConfigEditOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigEditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ConfigFix(NpmConfigFixOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigFixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDedupeOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Deprecate(NpmDeprecateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDiffOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDocsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDoctorOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmEditOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Exec(NpmExecOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> ExecC(NpmExecCOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExecCOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExplainOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExploreOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFundOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Help(NpmHelpOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookAdd(NpmHookAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookLs(NpmHookLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHookLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookRm(NpmHookRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> HookUpdate(NpmHookUpdateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Init(NpmInitOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLinkOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLoginOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLogoutOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgSet(NpmOrgSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgRm(NpmOrgRmOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OrgLs(NpmOrgLsOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Outdated(NpmOutdatedOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOutdatedOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerAdd(NpmOwnerAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerRm(NpmOwnerRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> OwnerLs(NpmOwnerLsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOwnerLsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPackOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPingOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgSet(NpmPkgSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgGet(NpmPkgGetOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgDelete(NpmPkgDeleteOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> PkgFix(NpmPkgFixOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgFixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPrefixOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileEnable2fa(NpmProfileEnable2faOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileEnable2faOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileDisable2fa(NpmProfileDisable2faOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileDisable2faOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileGet(NpmProfileGetOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileGetOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> ProfileSet(NpmProfileSetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPruneOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPublishOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Query(NpmQueryOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRebuildOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRepoOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Restart(NpmRestartOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRootOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Run(NpmRunOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSbomOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Search(NpmSearchOptions options,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmShrinkwrapOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStarsOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Start(NpmStartOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Stop(NpmStopOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamCreate(NpmTeamCreateOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamDestroy(NpmTeamDestroyOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamAdd(NpmTeamAddOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamRm(NpmTeamRmOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TeamLs(NpmTeamLsOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> Test(NpmTestOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenList(NpmTokenListOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenListOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenRevoke(NpmTokenRevokeOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> TokenCreate(NpmTokenCreateOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenCreateOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Uninstall(NpmUninstallOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUninstallOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnpublishOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUnstarOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUpdateOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Version(NpmVersionOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmViewOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmWhoamiOptions(), null, cancellationToken);
    }

    public virtual async Task<CommandResult> Npx(NpxOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> NpxC(NpxCOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpxCOptions(), null, cancellationToken);
    }
}