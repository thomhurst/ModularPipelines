using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config")]
public class GcloudComputeOsConfigOsPolicyAssignments
{
    public GcloudComputeOsConfigOsPolicyAssignments(
        GcloudComputeOsConfigOsPolicyAssignmentsOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeOsConfigOsPolicyAssignmentsOperations Operations { get; }

    public async Task<CommandResult> Create(GcloudComputeOsConfigOsPolicyAssignmentsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeOsConfigOsPolicyAssignmentsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeOsConfigOsPolicyAssignmentsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeOsConfigOsPolicyAssignmentsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeOsConfigOsPolicyAssignmentsListOptions(), token);
    }

    public async Task<CommandResult> ListRevisions(GcloudComputeOsConfigOsPolicyAssignmentsListRevisionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeOsConfigOsPolicyAssignmentsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}