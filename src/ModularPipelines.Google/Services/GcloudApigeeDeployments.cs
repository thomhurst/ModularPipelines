using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("apigee")]
public class GcloudApigeeDeployments
{
    public GcloudApigeeDeployments(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudApigeeDeploymentsDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudApigeeDeploymentsDescribeOptions(), token);
    }

    public async Task<CommandResult> List(GcloudApigeeDeploymentsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudApigeeDeploymentsListOptions(), token);
    }
}