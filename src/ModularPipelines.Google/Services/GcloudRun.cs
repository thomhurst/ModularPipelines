using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudRun
{
    public GcloudRun(
        GcloudRunDomainMappings domainMappings,
        GcloudRunJobs jobs,
        GcloudRunRegions regions,
        GcloudRunRevisions revisions,
        GcloudRunServices services,
        ICommand internalCommand
    )
    {
        DomainMappings = domainMappings;
        Jobs = jobs;
        Regions = regions;
        Revisions = revisions;
        Services = services;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudRunDomainMappings DomainMappings { get; }

    public GcloudRunJobs Jobs { get; }

    public GcloudRunRegions Regions { get; }

    public GcloudRunRevisions Revisions { get; }

    public GcloudRunServices Services { get; }

    public async Task<CommandResult> Deploy(GcloudRunDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}