using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Chocolatey.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Chocolatey;

[ExcludeFromCodeCoverage]
internal class Choco : IChoco
{
    private readonly ICommand _command;

    public Choco(ICommand command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> ApiKey(ApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ApiKeyOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SetApiKey(SetApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SetApiKeyOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Cache(CacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new CacheOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> CacheRemove(CacheRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new CacheRemoveOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Config(ConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> ConfigGet(ConfigGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigGetOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> ConfigSet(ConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigSetOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> ConfigUnset(ConfigUnsetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigUnsetOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Convert(ConvertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Download(DownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Export(ExportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ExportOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Feature(FeatureOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> FeatureDisable(FeatureDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureDisableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> FeatureEnable(FeatureEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureEnableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Help(HelpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new HelpOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Info(InfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new InfoOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Optimize(OptimizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new OptimizeOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Install(InstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> List(ListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> New(NewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Outdated(OutdatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new OutdatedOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Pack(PackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Pin(PinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> PinAdd(PinAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinAddOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> PinRemove(PinRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinRemoveOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Push(PushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Find(FindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Search(SearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Apikey(ApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ApiKeyOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Setapikey(SetApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SetApiKeyOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Source(SourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourceAdd(SourceAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceAddOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourceRemove(SourceRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceRemoveOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourceDisable(SourceDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceDisableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourceEnable(SourceEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceEnableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Sources(SourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourcesAdd(SourcesAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesAddOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourcesRemove(SourcesRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesRemoveOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourcesDisable(SourcesDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesDisableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> SourcesEnable(SourcesEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesEnableOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Support(SupportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SupportOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Sync(SyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SyncOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Template(TemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TemplateOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> TemplateInfo(TemplateInfoOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TemplateInfoOptions(), null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Uninstall(UninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    public virtual async Task<CommandResult> Upgrade(UpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }
}