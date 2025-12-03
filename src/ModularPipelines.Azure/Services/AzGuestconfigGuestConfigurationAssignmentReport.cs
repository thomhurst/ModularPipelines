using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("guestconfig")]
public class AzGuestconfigGuestConfigurationAssignmentReport
{
    public AzGuestconfigGuestConfigurationAssignmentReport(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzGuestconfigGuestConfigurationAssignmentReportListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGuestconfigGuestConfigurationAssignmentReportShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationAssignmentReportShowOptions(), token);
    }
}