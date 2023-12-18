using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update")]
public class AzIotDuUpdateInit
{
    public AzIotDuUpdateInit(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> V5(AzIotDuUpdateInitV5Options options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}