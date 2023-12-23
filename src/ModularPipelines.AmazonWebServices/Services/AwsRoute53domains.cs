using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53domains
{
    public AwsRoute53domains(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptDomainTransferFromAnotherAwsAccount(AwsRoute53domainsAcceptDomainTransferFromAnotherAwsAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateDelegationSignerToDomain(AwsRoute53domainsAssociateDelegationSignerToDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelDomainTransferToAnotherAwsAccount(AwsRoute53domainsCancelDomainTransferToAnotherAwsAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckDomainAvailability(AwsRoute53domainsCheckDomainAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckDomainTransferability(AwsRoute53domainsCheckDomainTransferabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDomain(AwsRoute53domainsDeleteDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTagsForDomain(AwsRoute53domainsDeleteTagsForDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableDomainAutoRenew(AwsRoute53domainsDisableDomainAutoRenewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableDomainTransferLock(AwsRoute53domainsDisableDomainTransferLockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateDelegationSignerFromDomain(AwsRoute53domainsDisassociateDelegationSignerFromDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableDomainAutoRenew(AwsRoute53domainsEnableDomainAutoRenewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableDomainTransferLock(AwsRoute53domainsEnableDomainTransferLockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContactReachabilityStatus(AwsRoute53domainsGetContactReachabilityStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsGetContactReachabilityStatusOptions(), token);
    }

    public async Task<CommandResult> GetDomainDetail(AwsRoute53domainsGetDomainDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomainSuggestions(AwsRoute53domainsGetDomainSuggestionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOperationDetail(AwsRoute53domainsGetOperationDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDomains(AwsRoute53domainsListDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsListDomainsOptions(), token);
    }

    public async Task<CommandResult> ListOperations(AwsRoute53domainsListOperationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsListOperationsOptions(), token);
    }

    public async Task<CommandResult> ListPrices(AwsRoute53domainsListPricesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsListPricesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForDomain(AwsRoute53domainsListTagsForDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PushDomain(AwsRoute53domainsPushDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterDomain(AwsRoute53domainsRegisterDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectDomainTransferFromAnotherAwsAccount(AwsRoute53domainsRejectDomainTransferFromAnotherAwsAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RenewDomain(AwsRoute53domainsRenewDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResendContactReachabilityEmail(AwsRoute53domainsResendContactReachabilityEmailOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsResendContactReachabilityEmailOptions(), token);
    }

    public async Task<CommandResult> ResendOperationAuthorization(AwsRoute53domainsResendOperationAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrieveDomainAuthCode(AwsRoute53domainsRetrieveDomainAuthCodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TransferDomain(AwsRoute53domainsTransferDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TransferDomainToAnotherAwsAccount(AwsRoute53domainsTransferDomainToAnotherAwsAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDomainContact(AwsRoute53domainsUpdateDomainContactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDomainContactPrivacy(AwsRoute53domainsUpdateDomainContactPrivacyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDomainNameservers(AwsRoute53domainsUpdateDomainNameserversOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTagsForDomain(AwsRoute53domainsUpdateTagsForDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ViewBilling(AwsRoute53domainsViewBillingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53domainsViewBillingOptions(), token);
    }
}