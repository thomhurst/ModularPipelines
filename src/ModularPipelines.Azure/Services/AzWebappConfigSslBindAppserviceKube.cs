using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl", "bind")]
public class AzWebappConfigSslBindAppserviceKube
{
    public AzWebappConfigSslBindAppserviceKube(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzWebappConfigSslBindAppserviceKubeExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}