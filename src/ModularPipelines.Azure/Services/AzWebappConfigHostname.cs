using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config")]
public class AzWebappConfigHostname
{
    public AzWebappConfigHostname(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzWebappConfigHostnameAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigHostnameAddOptions(), token);
    }

    public async Task<CommandResult> Delete(AzWebappConfigHostnameDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigHostnameDeleteOptions(), token);
    }

    public async Task<CommandResult> GetExternalIp(AzWebappConfigHostnameGetExternalIpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConfigHostnameGetExternalIpOptions(), token);
    }

    public async Task<CommandResult> List(AzWebappConfigHostnameListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}