using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network-analytics")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkAnalyticsDataProductCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkAnalyticsDataProductDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkAnalyticsDataProductListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductListOptions(), token);
    }

    public async Task<CommandResult> ListRolesAssignment(AzNetworkAnalyticsDataProductListRolesAssignmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductListRolesAssignmentOptions(), token);
    }

    public async Task<CommandResult> RemoveUserRole(AzNetworkAnalyticsDataProductRemoveUserRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkAnalyticsDataProductShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkAnalyticsDataProductWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkAnalyticsDataProductWaitOptions(), token);
    }
}