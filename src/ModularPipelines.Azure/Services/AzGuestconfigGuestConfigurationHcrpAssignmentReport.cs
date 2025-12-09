using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("guestconfig")]
public class AzGuestconfigGuestConfigurationHcrpAssignmentReport
{
    public AzGuestconfigGuestConfigurationHcrpAssignmentReport(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzGuestconfigGuestConfigurationHcrpAssignmentReportListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzGuestconfigGuestConfigurationHcrpAssignmentReportShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzGuestconfigGuestConfigurationHcrpAssignmentReportShowOptions(), token);
    }
}