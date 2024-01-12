using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets")]
public class GcloudSecretsLocations
{
    public GcloudSecretsLocations(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudSecretsLocationsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudSecretsLocationsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudSecretsLocationsListOptions(), token);
    }
}