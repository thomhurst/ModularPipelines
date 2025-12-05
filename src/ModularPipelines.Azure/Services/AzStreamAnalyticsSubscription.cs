using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("stream-analytics")]
public class AzStreamAnalyticsSubscription
{
    public AzStreamAnalyticsSubscription(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Inspect(AzStreamAnalyticsSubscriptionInspectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsSubscriptionInspectOptions(), token);
    }
}