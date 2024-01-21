using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Services;

[ExcludeFromCodeCoverage]
public class DotNetAdd
{
    public DotNetAdd(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Package(DotNetAddPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reference(DotNetAddReferenceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
