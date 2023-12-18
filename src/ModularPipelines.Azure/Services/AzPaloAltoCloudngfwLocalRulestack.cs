using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackCommitOptions(), token);
    }

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListOptions(), token);
    }

    public async Task<CommandResult> ListAdvancedSecurityObject(AzPaloAltoCloudngfwLocalRulestackListAdvancedSecurityObjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAppId(AzPaloAltoCloudngfwLocalRulestackListAppIdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListAppIdOptions(), token);
    }

    public async Task<CommandResult> ListCountry(AzPaloAltoCloudngfwLocalRulestackListCountryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListCountryOptions(), token);
    }

    public async Task<CommandResult> ListFirewall(AzPaloAltoCloudngfwLocalRulestackListFirewallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListFirewallOptions(), token);
    }

    public async Task<CommandResult> ListPredefinedUrlCategory(AzPaloAltoCloudngfwLocalRulestackListPredefinedUrlCategoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackListPredefinedUrlCategoryOptions(), token);
    }

    public async Task<CommandResult> ListSecurityService(AzPaloAltoCloudngfwLocalRulestackListSecurityServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Revert(AzPaloAltoCloudngfwLocalRulestackRevertOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackRevertOptions(), token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowOptions(), token);
    }

    public async Task<CommandResult> ShowChangeLog(AzPaloAltoCloudngfwLocalRulestackShowChangeLogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowChangeLogOptions(), token);
    }

    public async Task<CommandResult> ShowSupportInfo(AzPaloAltoCloudngfwLocalRulestackShowSupportInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackShowSupportInfoOptions(), token);
    }

    public async Task<CommandResult> Update(AzPaloAltoCloudngfwLocalRulestackUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackWaitOptions(), token);
    }
}