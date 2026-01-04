using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateEndpoint(AwsS3outpostsCreateEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEndpoint(AwsS3outpostsDeleteEndpointOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEndpoints(AwsS3outpostsListEndpointsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsS3outpostsListEndpointsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOutpostsWithS3(AwsS3outpostsListOutpostsWithS3Options? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsS3outpostsListOutpostsWithS3Options(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSharedEndpoints(AwsS3outpostsListSharedEndpointsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}