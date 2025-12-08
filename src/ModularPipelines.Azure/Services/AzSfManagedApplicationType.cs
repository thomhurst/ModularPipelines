using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf")]
public class AzSfManagedApplicationType
{
    public AzSfManagedApplicationType(
        AzSfManagedApplicationTypeVersion version,
        ICommand internalCommand
    )
    {
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSfManagedApplicationTypeVersion Version { get; }

    public async Task<CommandResult> Create(AzSfManagedApplicationTypeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSfManagedApplicationTypeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSfManagedApplicationTypeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSfManagedApplicationTypeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSfManagedApplicationTypeUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}