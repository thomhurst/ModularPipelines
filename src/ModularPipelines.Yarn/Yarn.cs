using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Yarn.Models;

namespace ModularPipelines.Yarn;

internal class Yarn : IYarn
{
    private readonly ICommand _command;

    public Yarn(ICommand command)
    {
        _command = command;
    }

    public async Task<CommandResult> Add(YarnAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnAddOptions(), token);
    }

    public async Task<CommandResult> Bin(YarnBinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnBinOptions(), token);
    }

    public async Task<CommandResult> CacheClean(YarnCacheCleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnCacheCleanOptions(), token);
    }

    public async Task<CommandResult> ConfigGet(YarnConfigGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Config(YarnConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnConfigOptions(), token);
    }

    public async Task<CommandResult> ConfigSet(YarnConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigUnset(YarnConfigUnsetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Constraints(YarnConstraintsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnConstraintsOptions(), token);
    }

    public async Task<CommandResult> ConstraintsQuery(YarnConstraintsQueryOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConstraintsSource(YarnConstraintsSourceOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnConstraintsSourceOptions(), token);
    }

    public async Task<CommandResult> Dedupe(YarnDedupeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnDedupeOptions(), token);
    }

    public async Task<CommandResult> Dlx(YarnDlxOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exec(YarnExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Explain(YarnExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnExplainOptions(), token);
    }

    public async Task<CommandResult> ExplainPeerRequirements(YarnExplainPeerRequirementsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnExplainPeerRequirementsOptions(), token);
    }

    public async Task<CommandResult> Info(YarnInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnInfoOptions(), token);
    }

    public async Task<CommandResult> Init(YarnInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnInitOptions(), token);
    }

    public async Task<CommandResult> Install(YarnInstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnInstallOptions(), token);
    }

    public async Task<CommandResult> Link(YarnLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnLinkOptions(), token);
    }

    public async Task<CommandResult> Node(YarnNodeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNodeOptions(), token);
    }

    public async Task<CommandResult> NpmAudit(YarnNpmAuditOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmAuditOptions(), token);
    }

    public async Task<CommandResult> NpmInfo(YarnNpmInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmInfoOptions(), token);
    }

    public async Task<CommandResult> NpmLogin(YarnNpmLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmLoginOptions(), token);
    }

    public async Task<CommandResult> NpmLogout(YarnNpmLogoutOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmLogoutOptions(), token);
    }

    public async Task<CommandResult> NpmPublish(YarnNpmPublishOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmPublishOptions(), token);
    }

    public async Task<CommandResult> NpmTagAdd(YarnNpmTagAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmTagList(YarnNpmTagListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmTagListOptions(), token);
    }

    public async Task<CommandResult> NpmTagRemove(YarnNpmTagRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NpmWhoami(YarnNpmWhoamiOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnNpmWhoamiOptions(), token);
    }

    public async Task<CommandResult> Pack(YarnPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnPackOptions(), token);
    }

    public async Task<CommandResult> PatchCommit(YarnPatchCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Patch(YarnPatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginCheck(YarnPluginCheckOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnPluginCheckOptions(), token);
    }

    public async Task<CommandResult> PluginImportFromSources(YarnPluginImportFromSourcesOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginImport(YarnPluginImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginList(YarnPluginListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnPluginListOptions(), token);
    }

    public async Task<CommandResult> PluginRemove(YarnPluginRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginRuntime(YarnPluginRuntimeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnPluginRuntimeOptions(), token);
    }

    public async Task<CommandResult> Rebuild(YarnRebuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnRebuildOptions(), token);
    }

    public async Task<CommandResult> Remove(YarnRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnRemoveOptions(), token);
    }

    public async Task<CommandResult> Run(YarnRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Search(YarnSearchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnSearchOptions(), token);
    }

    public async Task<CommandResult> SetResolution(YarnSetResolutionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetVersionFromSources(YarnSetVersionFromSourcesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnSetVersionFromSourcesOptions(), token);
    }

    public async Task<CommandResult> SetVersion(YarnSetVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stage(YarnStageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnStageOptions(), token);
    }

    public async Task<CommandResult> Unlink(YarnUnlinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnUnlinkOptions(), token);
    }

    public async Task<CommandResult> Unplug(YarnUnplugOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnUnplugOptions(), token);
    }

    public async Task<CommandResult> UpgradeInteractive(YarnUpgradeInteractiveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnUpgradeInteractiveOptions(), token);
    }

    public async Task<CommandResult> Up(YarnUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnUpOptions(), token);
    }

    public async Task<CommandResult> VersionApply(YarnVersionApplyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnVersionApplyOptions(), token);
    }

    public async Task<CommandResult> VersionCheck(YarnVersionCheckOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnVersionCheckOptions(), token);
    }

    public async Task<CommandResult> Version(YarnVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Why(YarnWhyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Workspace(YarnWorkspaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WorkspacesFocus(YarnWorkspacesFocusOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnWorkspacesFocusOptions(), token);
    }

    public async Task<CommandResult> WorkspacesForeach(YarnWorkspacesForeachOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WorkspacesList(YarnWorkspacesListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new YarnWorkspacesListOptions(), token);
    }
}