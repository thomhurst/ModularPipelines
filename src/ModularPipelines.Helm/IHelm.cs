using ModularPipelines.Helm.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Helm;

public interface IHelm
{
    Task<CommandResult> CompletionBash(HelmCompletionBashOptions options, CancellationToken token = default);

    Task<CommandResult> CompletionFish(HelmCompletionFishOptions options, CancellationToken token = default);

    Task<CommandResult> CompletionPowershell(HelmCompletionPowershellOptions options, CancellationToken token = default);

    Task<CommandResult> CompletionZsh(HelmCompletionZshOptions options, CancellationToken token = default);

    Task<CommandResult> Create(HelmCreateOptions options, CancellationToken token = default);

    Task<CommandResult> DependencyBuild(HelmDependencyBuildOptions options, CancellationToken token = default);

    Task<CommandResult> DependencyList(HelmDependencyListOptions options, CancellationToken token = default);

    Task<CommandResult> DependencyUpdate(HelmDependencyUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> GetAll(HelmGetAllOptions options, CancellationToken token = default);

    Task<CommandResult> GetHooks(HelmGetHooksOptions options, CancellationToken token = default);

    Task<CommandResult> GetManifest(HelmGetManifestOptions options, CancellationToken token = default);

    Task<CommandResult> GetNotes(HelmGetNotesOptions options, CancellationToken token = default);

    Task<CommandResult> GetValues(HelmGetValuesOptions options, CancellationToken token = default);

    Task<CommandResult> History(HelmHistoryOptions options, CancellationToken token = default);

    Task<CommandResult> Install(HelmInstallOptions options, CancellationToken token = default);

    Task<CommandResult> Lint(HelmLintOptions options, CancellationToken token = default);

    Task<CommandResult> List(HelmListOptions options, CancellationToken token = default);

    Task<CommandResult> Package(HelmPackageOptions options, CancellationToken token = default);

    Task<CommandResult> PluginInstall(HelmPluginInstallOptions options, CancellationToken token = default);

    Task<CommandResult> Pull(HelmPullOptions options, CancellationToken token = default);

    Task<CommandResult> Push(HelmPushOptions options, CancellationToken token = default);

    Task<CommandResult> RegistryLogin(HelmRegistryLoginOptions options, CancellationToken token = default);

    Task<CommandResult> RepoAdd(HelmRepoAddOptions options, CancellationToken token = default);

    Task<CommandResult> RepoIndex(HelmRepoIndexOptions options, CancellationToken token = default);

    Task<CommandResult> RepoList(HelmRepoListOptions options, CancellationToken token = default);

    Task<CommandResult> RepoUpdate(HelmRepoUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> Rollback(HelmRollbackOptions options, CancellationToken token = default);

    Task<CommandResult> SearchHub(HelmSearchHubOptions options, CancellationToken token = default);

    Task<CommandResult> SearchRepo(HelmSearchRepoOptions options, CancellationToken token = default);

    Task<CommandResult> ShowAll(HelmShowAllOptions options, CancellationToken token = default);

    Task<CommandResult> ShowChart(HelmShowChartOptions options, CancellationToken token = default);

    Task<CommandResult> ShowCrds(HelmShowCrdsOptions options, CancellationToken token = default);

    Task<CommandResult> ShowReadme(HelmShowReadmeOptions options, CancellationToken token = default);

    Task<CommandResult> ShowValues(HelmShowValuesOptions options, CancellationToken token = default);

    Task<CommandResult> Status(HelmStatusOptions options, CancellationToken token = default);

    Task<CommandResult> Template(HelmTemplateOptions options, CancellationToken token = default);

    Task<CommandResult> Test(HelmTestOptions options, CancellationToken token = default);

    Task<CommandResult> Uninstall(HelmUninstallOptions options, CancellationToken token = default);

    Task<CommandResult> Upgrade(HelmUpgradeOptions options, CancellationToken token = default);

    Task<CommandResult> Verify(HelmVerifyOptions options, CancellationToken token = default);

    Task<CommandResult> Version(HelmVersionOptions options, CancellationToken token = default);
}