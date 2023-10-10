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

    public async Task<CommandResult> Access(NpmAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmAccessOptions(), token);
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

    public async Task<CommandResult> Cache(NpmCacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCacheOptions(), token);
    }

    public async Task<CommandResult> CleanInstall(NpmCleanInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCleanInstallOptions(), token);
    }

    public async Task<CommandResult> Completion(NpmCompletionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmCompletionOptions(), token);
    }

    public async Task<CommandResult> Config(NpmConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmConfigOptions(), token);
    }

    public async Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDedupeOptions(), token);
    }

    public async Task<CommandResult> Deprecate(NpmDeprecateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDeprecateOptions(), token);
    }

    public async Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDiffOptions(), token);
    }

    public async Task<CommandResult> DistTag(NpmDistTagOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmDistTagOptions(), token);
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

    public async Task<CommandResult> Exec(NpmExecOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExecOptions(), token);
    }

    public async Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExplainOptions(), token);
    }

    public async Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmExploreOptions(), token);
    }

    public async Task<CommandResult> FindDupes(NpmFindDupesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFindDupesOptions(), token);
    }

    public async Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmFundOptions(), token);
    }

    public async Task<CommandResult> Help(NpmHelpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHelpOptions(), token);
    }

    public async Task<CommandResult> HelpSearch(NpmHelpSearchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHelpSearchOptions(), token);
    }

    public async Task<CommandResult> Hook(NpmHookOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmHookOptions(), token);
    }

    public async Task<CommandResult> Init(NpmInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInitOptions(), token);
    }

    public async Task<CommandResult> InstallCiTest(NpmInstallCiTestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallCiTestOptions(), token);
    }

    public async Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallOptions(), token);
    }

    public async Task<CommandResult> InstallTest(NpmInstallTestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmInstallTestOptions(), token);
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

    public async Task<CommandResult> Org(NpmOrgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOrgOptions(), token);
    }

    public async Task<CommandResult> Outdated(NpmOutdatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOutdatedOptions(), token);
    }

    public async Task<CommandResult> Owner(NpmOwnerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmOwnerOptions(), token);
    }

    public async Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPackOptions(), token);
    }

    public async Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPingOptions(), token);
    }

    public async Task<CommandResult> Pkg(NpmPkgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPkgOptions(), token);
    }

    public async Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPrefixOptions(), token);
    }

    public async Task<CommandResult> Profile(NpmProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmProfileOptions(), token);
    }

    public async Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPruneOptions(), token);
    }

    public async Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmPublishOptions(), token);
    }

    public async Task<CommandResult> Query(NpmQueryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmQueryOptions(), token);
    }

    public async Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRebuildOptions(), token);
    }

    public async Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRepoOptions(), token);
    }

    public async Task<CommandResult> Restart(NpmRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRestartOptions(), token);
    }

    public async Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRootOptions(), token);
    }

    public async Task<CommandResult> RunScript(NpmRunScriptOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmRunScriptOptions(), token);
    }

    public async Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSbomOptions(), token);
    }

    public async Task<CommandResult> Search(NpmSearchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmSearchOptions(), token);
    }

    public async Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default, CancellationToken token = default)
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

    public async Task<CommandResult> Start(NpmStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStartOptions(), token);
    }

    public async Task<CommandResult> Stop(NpmStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmStopOptions(), token);
    }

    public async Task<CommandResult> Team(NpmTeamOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTeamOptions(), token);
    }

    public async Task<CommandResult> Test(NpmTestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTestOptions(), token);
    }

    public async Task<CommandResult> Token(NpmTokenOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmTokenOptions(), cancellationToken);
    }

    public async Task<CommandResult> Uninstall(NpmUninstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmUninstallOptions(), token);
    }

    public async Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default, CancellationToken token = default)
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

    public async Task<CommandResult> Version(NpmVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new NpmVersionOptions(), token);
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
}
