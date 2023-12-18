using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image")]
public class AzVmImageTerms
{
    public AzVmImageTerms(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Accept(AzVmImageTermsAcceptOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmImageTermsAcceptOptions(), token);
    }

    public async Task<CommandResult> Cancel(AzVmImageTermsCancelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmImageTermsCancelOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmImageTermsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmImageTermsShowOptions(), token);
    }
}