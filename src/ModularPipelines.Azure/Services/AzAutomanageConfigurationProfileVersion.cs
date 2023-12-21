using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile")]
public class AzAutomanageConfigurationProfileVersion
{
    public AzAutomanageConfigurationProfileVersion(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAutomanageConfigurationProfileVersionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomanageConfigurationProfileVersionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileVersionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAutomanageConfigurationProfileVersionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAutomanageConfigurationProfileVersionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileVersionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAutomanageConfigurationProfileVersionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageConfigurationProfileVersionUpdateOptions(), token);
    }
}