using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group")]
public class AzAccountManagementGroupTenantBackfill
{
    public AzAccountManagementGroupTenantBackfill(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Get(AzAccountManagementGroupTenantBackfillGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountManagementGroupTenantBackfillGetOptions(), token);
    }

    public async Task<CommandResult> Start(AzAccountManagementGroupTenantBackfillStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountManagementGroupTenantBackfillStartOptions(), token);
    }
}