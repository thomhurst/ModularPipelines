using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudAi
{
    public GcloudAi(
        GcloudAiCustomJobs customJobs,
        GcloudAiEndpoints endpoints,
        GcloudAiHpTuningJobs hpTuningJobs,
        GcloudAiIndexEndpoints indexEndpoints,
        GcloudAiIndexes indexes,
        GcloudAiModelMonitoringJobs modelMonitoringJobs,
        GcloudAiModels models,
        GcloudAiOperations operations,
        GcloudAiTensorboards tensorboards
    )
    {
        CustomJobs = customJobs;
        Endpoints = endpoints;
        HpTuningJobs = hpTuningJobs;
        IndexEndpoints = indexEndpoints;
        Indexes = indexes;
        ModelMonitoringJobs = modelMonitoringJobs;
        Models = models;
        Operations = operations;
        Tensorboards = tensorboards;
    }

    public GcloudAiCustomJobs CustomJobs { get; }

    public GcloudAiEndpoints Endpoints { get; }

    public GcloudAiHpTuningJobs HpTuningJobs { get; }

    public GcloudAiIndexEndpoints IndexEndpoints { get; }

    public GcloudAiIndexes Indexes { get; }

    public GcloudAiModelMonitoringJobs ModelMonitoringJobs { get; }

    public GcloudAiModels Models { get; }

    public GcloudAiOperations Operations { get; }

    public GcloudAiTensorboards Tensorboards { get; }
}