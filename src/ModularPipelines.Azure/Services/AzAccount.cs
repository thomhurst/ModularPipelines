using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzAccount
{
    public AzAccount(
        AzAccountAlias alias,
        AzAccountLock @Lock,
        AzAccountManagementGroup managementGroup,
        AzAccountSubscription subscription,
        AzAccountTenant tenant,
        ICommand internalCommand
    )
    {
        Alias = alias;
        Lock = @Lock;
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

    public async Task<CommandResult> Clear(AzAccountClearOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccessToken(AzAccountGetAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAccountListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLocations(AzAccountListLocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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

