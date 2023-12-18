using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzManagedapp
{
    public AzManagedapp(
        AzManagedappDefinition definition,
        ICommand internalCommand
    )
    {
        Definition = definition;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzManagedappDefinition Definition { get; }

    public async Task<CommandResult> Create(AzManagedappCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzManagedappDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzManagedappDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzManagedappListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzManagedappListOptions(), token);
    }

    public async Task<CommandResult> Show(AzManagedappShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzManagedappShowOptions(), token);
    }
}