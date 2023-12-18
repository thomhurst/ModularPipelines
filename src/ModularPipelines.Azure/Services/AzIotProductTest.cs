using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "product")]
public class AzIotProductTest
{
    public AzIotProductTest(
        AzIotProductTestCase @case,
        AzIotProductTestRun run,
        AzIotProductTestTask task,
        ICommand internalCommand
    )
    {
        Case = @case;
        Run = run;
        Task = task;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotProductTestCase Case { get; }

    public AzIotProductTestRun Run { get; }

    public AzIotProductTestTask Task { get; }

    public async Task<CommandResult> Create(AzIotProductTestCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Search(AzIotProductTestSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotProductTestShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotProductTestUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}