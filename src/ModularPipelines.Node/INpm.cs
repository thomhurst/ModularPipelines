using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

public interface INpm
{
    Task<CommandResult> Access(NpmAccessOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Adduser(NpmAdduserOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Audit(NpmAuditOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Bugs(NpmBugsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Cache(NpmCacheOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CleanInstall(NpmCleanInstallOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Completion(NpmCompletionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Config(NpmConfigOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Dedupe(NpmDedupeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Deprecate(NpmDeprecateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Diff(NpmDiffOptions? options = default, CancellationToken token = default);

    Task<CommandResult> DistTag(NpmDistTagOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Docs(NpmDocsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Doctor(NpmDoctorOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Edit(NpmEditOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Exec(NpmExecOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Explain(NpmExplainOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Explore(NpmExploreOptions? options = default, CancellationToken token = default);

    Task<CommandResult> FindDupes(NpmFindDupesOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Fund(NpmFundOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Help(NpmHelpOptions? options = default, CancellationToken token = default);

    Task<CommandResult> HelpSearch(NpmHelpSearchOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Hook(NpmHookOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Init(NpmInitOptions? options = default, CancellationToken token = default);

    Task<CommandResult> InstallCiTest(NpmInstallCiTestOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Install(NpmInstallOptions? options = default, CancellationToken token = default);

    Task<CommandResult> InstallTest(NpmInstallTestOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Link(NpmLinkOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Login(NpmLoginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Logout(NpmLogoutOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Ls(NpmLsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Org(NpmOrgOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Outdated(NpmOutdatedOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Owner(NpmOwnerOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Pack(NpmPackOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Ping(NpmPingOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Pkg(NpmPkgOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Prefix(NpmPrefixOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Profile(NpmProfileOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Prune(NpmPruneOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Publish(NpmPublishOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Query(NpmQueryOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Rebuild(NpmRebuildOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Repo(NpmRepoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Restart(NpmRestartOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Root(NpmRootOptions? options = default, CancellationToken token = default);

    Task<CommandResult> RunScript(NpmRunScriptOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Sbom(NpmSbomOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Search(NpmSearchOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Shrinkwrap(NpmShrinkwrapOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Star(NpmStarOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Stars(NpmStarsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Start(NpmStartOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Stop(NpmStopOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Team(NpmTeamOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Test(NpmTestOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Token(NpmTokenOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Uninstall(NpmUninstallOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Unpublish(NpmUnpublishOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Unstar(NpmUnstarOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Update(NpmUpdateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Version(NpmVersionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> View(NpmViewOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Whoami(NpmWhoamiOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Npx(NpxOptions options, CancellationToken token = default);
}
