using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Node;

internal class Npx : INpx
{
    private readonly ICommand _command;

    public Npx(ICommand command)
    {
        _command = command;
    }
    
    public async Task<CommandResult> ExecuteAsync(NpxOptions npxOptions, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(npxOptions, cancellationToken);
    }
}