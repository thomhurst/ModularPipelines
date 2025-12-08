using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("assured")]
public class GcloudAssuredWorkloads
{
    public GcloudAssuredWorkloads(
        GcloudAssuredWorkloadsViolations violations,
        ICommand internalCommand
    )
    {
        Violations = violations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAssuredWorkloadsViolations Violations { get; }

    public async Task<CommandResult> Create(GcloudAssuredWorkloadsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudAssuredWorkloadsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAssuredWorkloadsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableResourceMonitoring(GcloudAssuredWorkloadsEnableResourceMonitoringOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAssuredWorkloadsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudAssuredWorkloadsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}