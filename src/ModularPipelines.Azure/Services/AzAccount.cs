using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Clear(AzAccountClearOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountClearOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> GetAccessToken(AzAccountGetAccessTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountGetAccessTokenOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListLocations(AzAccountListLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountListLocationsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Set(AzAccountSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAccountShowOptions(), cancellationToken: token);
    }
}