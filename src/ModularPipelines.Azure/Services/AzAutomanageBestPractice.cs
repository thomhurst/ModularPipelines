using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("automanage")]
public class AzAutomanageBestPractice
{
    public AzAutomanageBestPractice(
        AzAutomanageBestPracticeVersion version,
        ICommand internalCommand
    )
    {
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomanageBestPracticeVersion Version { get; }

    public async Task<CommandResult> List(AzAutomanageBestPracticeListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomanageBestPracticeListOptions(), token);
    }

    public async Task<CommandResult> Show(AzAutomanageBestPracticeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}