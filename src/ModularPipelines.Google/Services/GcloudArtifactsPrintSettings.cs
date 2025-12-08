using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("artifacts")]
public class GcloudArtifactsPrintSettings
{
    public GcloudArtifactsPrintSettings(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Gradle(GcloudArtifactsPrintSettingsGradleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsPrintSettingsGradleOptions(), token);
    }

    public async Task<CommandResult> Mvn(GcloudArtifactsPrintSettingsMvnOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsPrintSettingsMvnOptions(), token);
    }

    public async Task<CommandResult> Npm(GcloudArtifactsPrintSettingsNpmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsPrintSettingsNpmOptions(), token);
    }

    public async Task<CommandResult> Python(GcloudArtifactsPrintSettingsPythonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsPrintSettingsPythonOptions(), token);
    }
}