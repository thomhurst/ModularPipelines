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
        return await _command.ExecuteCommandLineTool(options ?? new ApiKeyOptions(), token);
    }

    public virtual async Task<CommandResult> SetApiKey(SetApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SetApiKeyOptions(), token);
    }

    public virtual async Task<CommandResult> Cache(CacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new CacheOptions(), token);
    }

    public virtual async Task<CommandResult> CacheRemove(CacheRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new CacheRemoveOptions(), token);
    }

    public virtual async Task<CommandResult> Config(ConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigOptions(), token);
    }

    public virtual async Task<CommandResult> ConfigGet(ConfigGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigGetOptions(), token);
    }

    public virtual async Task<CommandResult> ConfigSet(ConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigSetOptions(), token);
    }

    public virtual async Task<CommandResult> ConfigUnset(ConfigUnsetOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ConfigUnsetOptions(), token);
    }

    public virtual async Task<CommandResult> Convert(ConvertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Download(DownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Export(ExportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ExportOptions(), token);
    }

    public virtual async Task<CommandResult> Feature(FeatureOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureOptions(), token);
    }

    public virtual async Task<CommandResult> FeatureDisable(FeatureDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureDisableOptions(), token);
    }

    public virtual async Task<CommandResult> FeatureEnable(FeatureEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new FeatureEnableOptions(), token);
    }

    public virtual async Task<CommandResult> Help(HelpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new HelpOptions(), token);
    }

    public virtual async Task<CommandResult> Info(InfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new InfoOptions(), token);
    }

    public virtual async Task<CommandResult> Optimize(OptimizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new OptimizeOptions(), token);
    }

    public virtual async Task<CommandResult> Install(InstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> List(ListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> New(NewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Outdated(OutdatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new OutdatedOptions(), token);
    }

    public virtual async Task<CommandResult> Pack(PackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Pin(PinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinOptions(), token);
    }

    public virtual async Task<CommandResult> PinAdd(PinAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinAddOptions(), token);
    }

    public virtual async Task<CommandResult> PinRemove(PinRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new PinRemoveOptions(), token);
    }

    public virtual async Task<CommandResult> Push(PushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Find(FindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Search(SearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Apikey(ApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ApiKeyOptions(), token);
    }

    public virtual async Task<CommandResult> Setapikey(SetApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SetApiKeyOptions(), token);
    }

    public virtual async Task<CommandResult> Source(SourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceOptions(), token);
    }

    public virtual async Task<CommandResult> SourceAdd(SourceAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceAddOptions(), token);
    }

    public virtual async Task<CommandResult> SourceRemove(SourceRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceRemoveOptions(), token);
    }

    public virtual async Task<CommandResult> SourceDisable(SourceDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceDisableOptions(), token);
    }

    public virtual async Task<CommandResult> SourceEnable(SourceEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourceEnableOptions(), token);
    }

    public virtual async Task<CommandResult> Sources(SourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesOptions(), token);
    }

    public virtual async Task<CommandResult> SourcesAdd(SourcesAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesAddOptions(), token);
    }

    public virtual async Task<CommandResult> SourcesRemove(SourcesRemoveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesRemoveOptions(), token);
    }

    public virtual async Task<CommandResult> SourcesDisable(SourcesDisableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesDisableOptions(), token);
    }

    public virtual async Task<CommandResult> SourcesEnable(SourcesEnableOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SourcesEnableOptions(), token);
    }

    public virtual async Task<CommandResult> Support(SupportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SupportOptions(), token);
    }

    public virtual async Task<CommandResult> Sync(SyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new SyncOptions(), token);
    }

    public virtual async Task<CommandResult> Template(TemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TemplateOptions(), token);
    }

    public virtual async Task<CommandResult> TemplateInfo(TemplateInfoOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TemplateInfoOptions(), token);
    }

    public virtual async Task<CommandResult> Uninstall(UninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Upgrade(UpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}