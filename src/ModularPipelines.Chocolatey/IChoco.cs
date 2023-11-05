using ModularPipelines.Chocolatey.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Chocolatey;

public interface IChoco
{
    Task<CommandResult> ApiKey(ApiKeyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SetApiKey(SetApiKeyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Cache(CacheOptions? options = default, CancellationToken token = default);

    Task<CommandResult> CacheRemove(CacheRemoveOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Config(ConfigOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigGet(ConfigGetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigSet(ConfigSetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ConfigUnset(ConfigUnsetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Convert(ConvertOptions options, CancellationToken token = default);

    Task<CommandResult> Download(DownloadOptions options, CancellationToken token = default);

    Task<CommandResult> Export(ExportOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Feature(FeatureOptions? options = default, CancellationToken token = default);

    Task<CommandResult> FeatureDisable(FeatureDisableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> FeatureEnable(FeatureEnableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Help(HelpOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Info(InfoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Optimize(OptimizeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Install(InstallOptions options, CancellationToken token = default);

    Task<CommandResult> List(ListOptions options, CancellationToken token = default);

    Task<CommandResult> New(NewOptions options, CancellationToken token = default);

    Task<CommandResult> Outdated(OutdatedOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Pack(PackOptions options, CancellationToken token = default);

    Task<CommandResult> Pin(PinOptions? options = default, CancellationToken token = default);

    Task<CommandResult> PinAdd(PinAddOptions? options = default, CancellationToken token = default);

    Task<CommandResult> PinRemove(PinRemoveOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Push(PushOptions options, CancellationToken token = default);

    Task<CommandResult> Find(FindOptions options, CancellationToken token = default);

    Task<CommandResult> Search(SearchOptions options, CancellationToken token = default);

    Task<CommandResult> Source(SourceOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourceAdd(SourceAddOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourceRemove(SourceRemoveOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourceDisable(SourceDisableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourceEnable(SourceEnableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Sources(SourcesOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourcesAdd(SourcesAddOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourcesRemove(SourcesRemoveOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourcesDisable(SourcesDisableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> SourcesEnable(SourcesEnableOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Support(SupportOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Sync(SyncOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Template(TemplateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> TemplateInfo(TemplateInfoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Uninstall(UninstallOptions options, CancellationToken token = default);

    Task<CommandResult> Upgrade(UpgradeOptions options, CancellationToken token = default);
}