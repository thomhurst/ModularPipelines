using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts")]
public class GcloudArtifactsVpcscConfig
{
    public GcloudArtifactsVpcscConfig(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Allow(GcloudArtifactsVpcscConfigAllowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsVpcscConfigAllowOptions(), token);
    }

    public async Task<CommandResult> Deny(GcloudArtifactsVpcscConfigDenyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsVpcscConfigDenyOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudArtifactsVpcscConfigDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudArtifactsVpcscConfigDescribeOptions(), token);
    }
}