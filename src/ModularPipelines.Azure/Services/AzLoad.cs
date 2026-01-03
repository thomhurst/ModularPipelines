using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzLoad
{
    public AzLoad(
        AzLoadTest test,
        AzLoadTestRun testRun,
        ICommand internalCommand
    )
    {
        Test = test;
        TestRun = testRun;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLoadTest Test { get; }

    public AzLoadTestRun TestRun { get; }

    public async Task<CommandResult> Create(AzLoadCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzLoadDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLoadDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzLoadListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLoadListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzLoadShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLoadShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzLoadUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLoadUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzLoadWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLoadWaitOptions(), cancellationToken: token);
    }
}