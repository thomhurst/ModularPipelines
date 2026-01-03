using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-analytics")]
public class AzNetworkAnalyticsDataProduct
{
    public AzNetworkAnalyticsDataProduct(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddUserRole(AzNetworkAnalyticsDataProductAddUserRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzNetworkAnalyticsDataProductCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkAnalyticsDataProductDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkAnalyticsDataProductListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListRolesAssignment(AzNetworkAnalyticsDataProductListRolesAssignmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductListRolesAssignmentOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RemoveUserRole(AzNetworkAnalyticsDataProductRemoveUserRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkAnalyticsDataProductShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkAnalyticsDataProductWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductWaitOptions(), cancellationToken: token);
    }
}