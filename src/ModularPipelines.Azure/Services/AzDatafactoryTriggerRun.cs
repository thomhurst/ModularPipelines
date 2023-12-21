using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
public class AzDatafactoryTriggerRun
{
    public AzDatafactoryTriggerRun(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzDatafactoryTriggerRunCancelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerRunCancelOptions(), token);
    }

    public async Task<CommandResult> QueryByFactory(AzDatafactoryTriggerRunQueryByFactoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rerun(AzDatafactoryTriggerRunRerunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryTriggerRunRerunOptions(), token);
    }
}