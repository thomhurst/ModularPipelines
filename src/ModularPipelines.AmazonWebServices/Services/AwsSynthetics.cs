using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSynthetics
{
    public AwsSynthetics(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateResource(AwsSyntheticsAssociateResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCanary(AwsSyntheticsCreateCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGroup(AwsSyntheticsCreateGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCanary(AwsSyntheticsDeleteCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGroup(AwsSyntheticsDeleteGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCanaries(AwsSyntheticsDescribeCanariesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSyntheticsDescribeCanariesOptions(), token);
    }

    public async Task<CommandResult> DescribeCanariesLastRun(AwsSyntheticsDescribeCanariesLastRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSyntheticsDescribeCanariesLastRunOptions(), token);
    }

    public async Task<CommandResult> DescribeRuntimeVersions(AwsSyntheticsDescribeRuntimeVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSyntheticsDescribeRuntimeVersionsOptions(), token);
    }

    public async Task<CommandResult> DisassociateResource(AwsSyntheticsDisassociateResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCanary(AwsSyntheticsGetCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCanaryRuns(AwsSyntheticsGetCanaryRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGroup(AwsSyntheticsGetGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAssociatedGroups(AwsSyntheticsListAssociatedGroupsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroupResources(AwsSyntheticsListGroupResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGroups(AwsSyntheticsListGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSyntheticsListGroupsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSyntheticsListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartCanary(AwsSyntheticsStartCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopCanary(AwsSyntheticsStopCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSyntheticsTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSyntheticsUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCanary(AwsSyntheticsUpdateCanaryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}