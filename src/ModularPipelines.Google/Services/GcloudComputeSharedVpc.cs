using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputeSharedVpc
{
    public GcloudComputeSharedVpc(
        GcloudComputeSharedVpcAssociatedProjects associatedProjects,
        GcloudComputeSharedVpcOrganizations organizations,
        ICommand internalCommand
    )
    {
        AssociatedProjects = associatedProjects;
        Organizations = organizations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeSharedVpcAssociatedProjects AssociatedProjects { get; }

    public GcloudComputeSharedVpcOrganizations Organizations { get; }

    public async Task<CommandResult> Disable(GcloudComputeSharedVpcDisableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(GcloudComputeSharedVpcEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHostProject(GcloudComputeSharedVpcGetHostProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociatedResources(GcloudComputeSharedVpcListAssociatedResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}