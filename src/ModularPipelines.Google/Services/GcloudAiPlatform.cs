using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAiPlatform
{
    public GcloudAiPlatform(
        GcloudAiPlatformJobs jobs,
        GcloudAiPlatformLocal local,
        GcloudAiPlatformModels models,
        GcloudAiPlatformOperations operations,
        GcloudAiPlatformVersions versions,
        ICommand internalCommand
    )
    {
        Jobs = jobs;
        Local = local;
        Models = models;
        Operations = operations;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAiPlatformJobs Jobs { get; }

    public GcloudAiPlatformLocal Local { get; }

    public GcloudAiPlatformModels Models { get; }

    public GcloudAiPlatformOperations Operations { get; }

    public GcloudAiPlatformVersions Versions { get; }

    public async Task<CommandResult> Predict(GcloudAiPlatformPredictOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}