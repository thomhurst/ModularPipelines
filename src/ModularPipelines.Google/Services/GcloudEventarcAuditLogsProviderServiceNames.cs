using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "audit-logs-provider")]
public class GcloudEventarcAuditLogsProviderServiceNames
{
    public GcloudEventarcAuditLogsProviderServiceNames(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(GcloudEventarcAuditLogsProviderServiceNamesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudEventarcAuditLogsProviderServiceNamesListOptions(), token);
    }
}