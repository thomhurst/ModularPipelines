using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("artifacts")]
public class GcloudArtifactsSettings
{
    public GcloudArtifactsSettings(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudArtifactsSettingsDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsSettingsDescribeOptions(), token);
    }

    public async Task<CommandResult> DisableUpgradeRedirection(GcloudArtifactsSettingsDisableUpgradeRedirectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsSettingsDisableUpgradeRedirectionOptions(), token);
    }

    public async Task<CommandResult> EnableUpgradeRedirection(GcloudArtifactsSettingsEnableUpgradeRedirectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsSettingsEnableUpgradeRedirectionOptions(), token);
    }
}