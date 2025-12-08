using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("compute")]
public class GcloudComputeProjectInfo
{
    public GcloudComputeProjectInfo(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddMetadata(GcloudComputeProjectInfoAddMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeProjectInfoAddMetadataOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudComputeProjectInfoDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeProjectInfoDescribeOptions(), token);
    }

    public async Task<CommandResult> RemoveMetadata(GcloudComputeProjectInfoRemoveMetadataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeProjectInfoRemoveMetadataOptions(), token);
    }

    public async Task<CommandResult> SetUsageBucket(GcloudComputeProjectInfoSetUsageBucketOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeProjectInfoUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeProjectInfoUpdateOptions(), token);
    }
}