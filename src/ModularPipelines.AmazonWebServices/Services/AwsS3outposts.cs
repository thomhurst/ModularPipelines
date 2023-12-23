using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsS3outposts
{
    public AwsS3outposts(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateEndpoint(AwsS3outpostsCreateEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEndpoint(AwsS3outpostsDeleteEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEndpoints(AwsS3outpostsListEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsS3outpostsListEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListOutpostsWithS3(AwsS3outpostsListOutpostsWithS3Options? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsS3outpostsListOutpostsWithS3Options(), token);
    }

    public async Task<CommandResult> ListSharedEndpoints(AwsS3outpostsListSharedEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}