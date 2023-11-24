using ModularPipelines.Models;
using ModularPipelines.WinGet.Options;

namespace ModularPipelines.WinGet;

public interface IWinGet
{
    Task<CommandResult> Configure(ConfigureOptions options, CancellationToken token = default);

    Task<CommandResult> Download(DownloadOptions options, CancellationToken token = default);

    Task<CommandResult> Export(ExportOptions options, CancellationToken token = default);

    Task<CommandResult> Features(FeaturesOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Hash(HashOptions options, CancellationToken token = default);

    Task<CommandResult> Help(HelpOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Import(ImportOptions options, CancellationToken token = default);

    Task<CommandResult> Info(InfoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Install(InstallOptions options, CancellationToken token = default);

    Task<CommandResult> List(ListOptions options, CancellationToken token = default);

    Task<CommandResult> Pin(PinOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Search(SearchOptions options, CancellationToken token = default);

    Task<CommandResult> Settings(SettingsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Show(ShowOptions options, CancellationToken token = default);

    Task<CommandResult> SourceAdd(SourceAddOptions options, CancellationToken token = default);

    Task<CommandResult> SourceList(SourceListOptions options, CancellationToken token = default);

    Task<CommandResult> SourceUpdate(SourceUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> SourceRemove(SourceRemoveOptions options, CancellationToken token = default);

    Task<CommandResult> SourceReset(SourceResetOptions options, CancellationToken token = default);

    Task<CommandResult> SourceExport(SourceExportOptions options, CancellationToken token = default);

    Task<CommandResult> Uninstall(UninstallOptions options, CancellationToken token = default);

    Task<CommandResult> Upgrade(UpgradeOptions options, CancellationToken token = default);

    Task<CommandResult> Validate(ValidateOptions options, CancellationToken token = default);
}