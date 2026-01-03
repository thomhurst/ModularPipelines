using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw")]
public class AzPaloAltoCloudngfwLocalRulestack
{
    public AzPaloAltoCloudngfwLocalRulestack(
        AzPaloAltoCloudngfwLocalRulestackCertificate certificate,
        AzPaloAltoCloudngfwLocalRulestackFqdnlist fqdnlist,
        AzPaloAltoCloudngfwLocalRulestackLocalRule localRule,
        AzPaloAltoCloudngfwLocalRulestackPrefixlist prefixlist,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        Fqdnlist = fqdnlist;
        LocalRule = localRule;
        Prefixlist = prefixlist;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPaloAltoCloudngfwLocalRulestackCertificate Certificate { get; }

    public AzPaloAltoCloudngfwLocalRulestackFqdnlist Fqdnlist { get; }

    public AzPaloAltoCloudngfwLocalRulestackLocalRule LocalRule { get; }

    public AzPaloAltoCloudngfwLocalRulestackPrefixlist Prefixlist { get; }

    public async Task<CommandResult> Commit(AzPaloAltoCloudngfwLocalRulestackCommitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackCommitOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListAdvancedSecurityObject(AzPaloAltoCloudngfwLocalRulestackListAdvancedSecurityObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListAppId(AzPaloAltoCloudngfwLocalRulestackListAppIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListAppIdOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListCountry(AzPaloAltoCloudngfwLocalRulestackListCountryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListCountryOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListFirewall(AzPaloAltoCloudngfwLocalRulestackListFirewallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListFirewallOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListPredefinedUrlCategory(AzPaloAltoCloudngfwLocalRulestackListPredefinedUrlCategoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListPredefinedUrlCategoryOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListSecurityService(AzPaloAltoCloudngfwLocalRulestackListSecurityServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Revert(AzPaloAltoCloudngfwLocalRulestackRevertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackRevertOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowChangeLog(AzPaloAltoCloudngfwLocalRulestackShowChangeLogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowChangeLogOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowSupportInfo(AzPaloAltoCloudngfwLocalRulestackShowSupportInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowSupportInfoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzPaloAltoCloudngfwLocalRulestackUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackWaitOptions(), cancellationToken: token);
    }
}