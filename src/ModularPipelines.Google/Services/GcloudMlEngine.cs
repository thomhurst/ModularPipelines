using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudMlEngine
{
    public GcloudMlEngine(
        GcloudMlEngineJobs jobs,
        GcloudMlEngineLocal local,
        GcloudMlEngineModels models,
        GcloudMlEngineOperations operations,
        GcloudMlEngineVersions versions,
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

    public GcloudMlEngineJobs Jobs { get; }

    public GcloudMlEngineLocal Local { get; }

    public GcloudMlEngineModels Models { get; }

    public GcloudMlEngineOperations Operations { get; }

    public GcloudMlEngineVersions Versions { get; }

    public async Task<CommandResult> Predict(GcloudMlEnginePredictOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}