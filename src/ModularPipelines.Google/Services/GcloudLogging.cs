using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudLogging
{
    public GcloudLogging(
        GcloudLoggingBuckets buckets,
        GcloudLoggingLinks links,
        GcloudLoggingLocations locations,
        GcloudLoggingLogs logs,
        GcloudLoggingMetrics metrics,
        GcloudLoggingOperations operations,
        GcloudLoggingResourceDescriptors resourceDescriptors,
        GcloudLoggingSettings settings,
        GcloudLoggingSinks sinks,
        GcloudLoggingViews views,
        ICommand internalCommand
    )
    {
        Buckets = buckets;
        Links = links;
        Locations = locations;
        Logs = logs;
        Metrics = metrics;
        Operations = operations;
        ResourceDescriptors = resourceDescriptors;
        Settings = settings;
        Sinks = sinks;
        Views = views;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudLoggingBuckets Buckets { get; }

    public GcloudLoggingLinks Links { get; }

    public GcloudLoggingLocations Locations { get; }

    public GcloudLoggingLogs Logs { get; }

    public GcloudLoggingMetrics Metrics { get; }

    public GcloudLoggingOperations Operations { get; }

    public GcloudLoggingResourceDescriptors ResourceDescriptors { get; }

    public GcloudLoggingSettings Settings { get; }

    public GcloudLoggingSinks Sinks { get; }

    public GcloudLoggingViews Views { get; }

    public async Task<CommandResult> Copy(GcloudLoggingCopyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Read(GcloudLoggingReadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Write(GcloudLoggingWriteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}