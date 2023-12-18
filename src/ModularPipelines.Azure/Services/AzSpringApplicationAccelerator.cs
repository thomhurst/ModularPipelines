using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring")]
public class AzSpringApplicationAccelerator
{
    public AzSpringApplicationAccelerator(
        AzSpringApplicationAcceleratorCustomizedAccelerator customizedAccelerator,
        AzSpringApplicationAcceleratorPredefinedAccelerator predefinedAccelerator,
        ICommand internalCommand
    )
    {
        CustomizedAccelerator = customizedAccelerator;
        PredefinedAccelerator = predefinedAccelerator;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringApplicationAcceleratorCustomizedAccelerator CustomizedAccelerator { get; }

    public AzSpringApplicationAcceleratorPredefinedAccelerator PredefinedAccelerator { get; }

    public async Task<CommandResult> Create(AzSpringApplicationAcceleratorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringApplicationAcceleratorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringApplicationAcceleratorShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}