using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage")]
public class AzAutomanageConfigurationProfile
{
    public AzAutomanageConfigurationProfile(
        AzAutomanageConfigurationProfileVersion version,
        ICommand internalCommand
    )
    {
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageConfigurationProfileVersion Version { get; }

    public async Task<CommandResult> Create(AzAutomanageConfigurationProfileCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzAutomanageConfigurationProfileDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAutomanageConfigurationProfileListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAutomanageConfigurationProfileShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzAutomanageConfigurationProfileUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileUpdateOptions(), cancellationToken: token);
    }
}