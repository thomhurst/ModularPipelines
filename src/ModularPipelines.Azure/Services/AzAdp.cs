using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAdp
{
    public AzAdp(
        AzAdpAccount account,
        AzAdpWorkspace workspace,
        ICommand internalCommand
    )
    {
        Account = account;
        Workspace = workspace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAdpAccount Account { get; }

    public AzAdpWorkspace Workspace { get; }

    public async Task<CommandResult> CheckNameAvailability(AzAdpCheckNameAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}