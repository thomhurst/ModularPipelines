using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAccount
{
    public AzAccount(
        AzAccountAlias alias,
        AzAccountLock @lock,
        AzAccountManagementGroup managementGroup,
        AzAccountSubscription subscription,
        AzAccountTenant tenant,
        ICommand internalCommand
    )
    {
        Alias = alias;
        Lock = @lock;
        ManagementGroup = managementGroup;
        Subscription = subscription;
        Tenant = tenant;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAccountAlias Alias { get; }

    public AzAccountLock Lock { get; }

    public AzAccountManagementGroup ManagementGroup { get; }

    public AzAccountSubscription Subscription { get; }

    public AzAccountTenant Tenant { get; }

    public async Task<CommandResult> AcceptOwnershipStatus(AzAccountAcceptOwnershipStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Clear(AzAccountClearOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountClearOptions(), token);
    }

    public async Task<CommandResult> Create(AzAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccessToken(AzAccountGetAccessTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountGetAccessTokenOptions(), token);
    }

    public async Task<CommandResult> List(AzAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountListOptions(), token);
    }

    public async Task<CommandResult> ListLocations(AzAccountListLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountListLocationsOptions(), token);
    }

    public async Task<CommandResult> Set(AzAccountSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountShowOptions(), token);
    }
}