using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci")]
public class AzStackHciArcSetting
{
    public AzStackHciArcSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ConsentAndInstallDefaultExtension(AzStackHciArcSettingConsentAndInstallDefaultExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingConsentAndInstallDefaultExtensionOptions(), token);
    }

    public async Task<CommandResult> Create(AzStackHciArcSettingCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIdentity(AzStackHciArcSettingCreateIdentityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingCreateIdentityOptions(), token);
    }

    public async Task<CommandResult> Delete(AzStackHciArcSettingDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingDeleteOptions(), token);
    }

    public async Task<CommandResult> GeneratePassword(AzStackHciArcSettingGeneratePasswordOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingGeneratePasswordOptions(), token);
    }

    public async Task<CommandResult> InitializeDisableProces(AzStackHciArcSettingInitializeDisableProcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingInitializeDisableProcesOptions(), token);
    }

    public async Task<CommandResult> List(AzStackHciArcSettingListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStackHciArcSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciArcSettingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStackHciArcSettingWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciArcSettingWaitOptions(), token);
    }
}