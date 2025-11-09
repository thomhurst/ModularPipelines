using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IBash
{
    Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default);
}
