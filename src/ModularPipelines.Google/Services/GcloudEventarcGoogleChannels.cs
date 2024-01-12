using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc")]
public class GcloudEventarcGoogleChannels
{
    public GcloudEventarcGoogleChannels(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudEventarcGoogleChannelsDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEventarcGoogleChannelsDescribeOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudEventarcGoogleChannelsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEventarcGoogleChannelsUpdateOptions(), token);
    }
}