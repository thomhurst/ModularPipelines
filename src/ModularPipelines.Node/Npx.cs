using ModularPipelines.Context;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

[ExcludeFromCodeCoverage]
internal class Npx : INpx
{
    private readonly ICommandContext _command;

    public Npx(ICommandContext command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> ExecuteAsync(NpxOptions npxOptions, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(npxOptions, null, cancellationToken);
    }
}