using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "config")]
public class AzFunctionappConfigHostname
{
    public AzFunctionappConfigHostname(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzFunctionappConfigHostnameAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigHostnameAddOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzFunctionappConfigHostnameDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigHostnameDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetExternalIp(AzFunctionappConfigHostnameGetExternalIpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigHostnameGetExternalIpOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzFunctionappConfigHostnameListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}