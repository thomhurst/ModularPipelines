using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config")]
public class GcloudComputeOsConfigOsPolicyAssignmentReports
{
    public GcloudComputeOsConfigOsPolicyAssignmentReports(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudComputeOsConfigOsPolicyAssignmentReportsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeOsConfigOsPolicyAssignmentReportsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeOsConfigOsPolicyAssignmentReportsListOptions(), token);
    }
}