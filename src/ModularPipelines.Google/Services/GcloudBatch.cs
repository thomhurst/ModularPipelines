using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBatch
{
    public GcloudBatch(
        GcloudBatchJobs jobs,
        GcloudBatchTasks tasks
    )
    {
        Jobs = jobs;
        Tasks = tasks;
    }

    public GcloudBatchJobs Jobs { get; }

    public GcloudBatchTasks Tasks { get; }
}