using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Services;

[ExcludeFromCodeCoverage]
public class DotNetNugetEnable
{
    public DotNetNugetEnable(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Source(DotNetNugetEnableSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
