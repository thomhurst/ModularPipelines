using ModularPipelines.Models;
using ModularPipelines.Yarn.Models;

namespace ModularPipelines.Yarn;

public interface IYarn
{
    Task<CommandResult> Add(YarnAddOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Bin(YarnBinOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheClean(YarnCacheCleanOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ConfigGet(YarnConfigGetOptions options, CancellationToken token = default);
    Task<CommandResult> Config(YarnConfigOptions? options = default, CancellationToken token = default);
    Task<CommandResult> ConfigSet(YarnConfigSetOptions options, CancellationToken token = default);
    Task<CommandResult> ConfigUnset(YarnConfigUnsetOptions options, CancellationToken token = default);

    Task<CommandResult> Constraints(YarnConstraintsOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ConstraintsQuery(YarnConstraintsQueryOptions options,
        CancellationToken token = default);

    Task<CommandResult> ConstraintsSource(YarnConstraintsSourceOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Dedupe(YarnDedupeOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Dlx(YarnDlxOptions options, CancellationToken token = default);
    Task<CommandResult> Exec(YarnExecOptions options, CancellationToken token = default);
    Task<CommandResult> Explain(YarnExplainOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ExplainPeerRequirements(YarnExplainPeerRequirementsOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Info(YarnInfoOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Init(YarnInitOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Install(YarnInstallOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Link(YarnLinkOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Node(YarnNodeOptions? options = default, CancellationToken token = default);
    Task<CommandResult> NpmAudit(YarnNpmAuditOptions? options = default, CancellationToken token = default);
    Task<CommandResult> NpmInfo(YarnNpmInfoOptions? options = default, CancellationToken token = default);
    Task<CommandResult> NpmLogin(YarnNpmLoginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> NpmLogout(YarnNpmLogoutOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> NpmPublish(YarnNpmPublishOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> NpmTagAdd(YarnNpmTagAddOptions options, CancellationToken token = default);

    Task<CommandResult> NpmTagList(YarnNpmTagListOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> NpmTagRemove(YarnNpmTagRemoveOptions options, CancellationToken token = default);

    Task<CommandResult> NpmWhoami(YarnNpmWhoamiOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Pack(YarnPackOptions? options = default, CancellationToken token = default);
    Task<CommandResult> PatchCommit(YarnPatchCommitOptions options, CancellationToken token = default);
    Task<CommandResult> Patch(YarnPatchOptions options, CancellationToken token = default);

    Task<CommandResult> PluginCheck(YarnPluginCheckOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> PluginImportFromSources(YarnPluginImportFromSourcesOptions options,
        CancellationToken token = default);

    Task<CommandResult> PluginImport(YarnPluginImportOptions options, CancellationToken token = default);

    Task<CommandResult> PluginList(YarnPluginListOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> PluginRemove(YarnPluginRemoveOptions options, CancellationToken token = default);

    Task<CommandResult> PluginRuntime(YarnPluginRuntimeOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Rebuild(YarnRebuildOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Remove(YarnRemoveOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Run(YarnRunOptions options, CancellationToken token = default);
    Task<CommandResult> Search(YarnSearchOptions? options = default, CancellationToken token = default);
    Task<CommandResult> SetResolution(YarnSetResolutionOptions options, CancellationToken token = default);

    Task<CommandResult> SetVersionFromSources(YarnSetVersionFromSourcesOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> SetVersion(YarnSetVersionOptions options, CancellationToken token = default);
    Task<CommandResult> Stage(YarnStageOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Unlink(YarnUnlinkOptions? options = default, CancellationToken token = default);
    Task<CommandResult> Unplug(YarnUnplugOptions? options = default, CancellationToken token = default);

    Task<CommandResult> UpgradeInteractive(YarnUpgradeInteractiveOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Up(YarnUpOptions? options = default, CancellationToken token = default);

    Task<CommandResult> VersionApply(YarnVersionApplyOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> VersionCheck(YarnVersionCheckOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Version(YarnVersionOptions options, CancellationToken token = default);
    Task<CommandResult> Why(YarnWhyOptions options, CancellationToken token = default);
    Task<CommandResult> Workspace(YarnWorkspaceOptions options, CancellationToken token = default);

    Task<CommandResult> WorkspacesFocus(YarnWorkspacesFocusOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> WorkspacesForeach(YarnWorkspacesForeachOptions options,
        CancellationToken token = default);

    Task<CommandResult> WorkspacesList(YarnWorkspacesListOptions? options = default,
        CancellationToken token = default);
}