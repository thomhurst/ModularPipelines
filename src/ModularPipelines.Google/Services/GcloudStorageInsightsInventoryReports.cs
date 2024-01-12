using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights")]
public class GcloudStorageInsightsInventoryReports
{
    public GcloudStorageInsightsInventoryReports(
        GcloudStorageInsightsInventoryReportsDetails details,
        ICommand internalCommand
    )
    {
        Details = details;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudStorageInsightsInventoryReportsDetails Details { get; }

    public async Task<CommandResult> Create(GcloudStorageInsightsInventoryReportsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudStorageInsightsInventoryReportsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudStorageInsightsInventoryReportsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudStorageInsightsInventoryReportsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudStorageInsightsInventoryReportsListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudStorageInsightsInventoryReportsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}