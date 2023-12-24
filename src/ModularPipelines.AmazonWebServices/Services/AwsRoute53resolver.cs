using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53resolver
{
    public AwsRoute53resolver(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateFirewallRuleGroup(AwsRoute53resolverAssociateFirewallRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateResolverEndpointIpAddress(AwsRoute53resolverAssociateResolverEndpointIpAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateResolverQueryLogConfig(AwsRoute53resolverAssociateResolverQueryLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateResolverRule(AwsRoute53resolverAssociateResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFirewallDomainList(AwsRoute53resolverCreateFirewallDomainListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFirewallRule(AwsRoute53resolverCreateFirewallRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFirewallRuleGroup(AwsRoute53resolverCreateFirewallRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOutpostResolver(AwsRoute53resolverCreateOutpostResolverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResolverEndpoint(AwsRoute53resolverCreateResolverEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResolverQueryLogConfig(AwsRoute53resolverCreateResolverQueryLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateResolverRule(AwsRoute53resolverCreateResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFirewallDomainList(AwsRoute53resolverDeleteFirewallDomainListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFirewallRule(AwsRoute53resolverDeleteFirewallRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFirewallRuleGroup(AwsRoute53resolverDeleteFirewallRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOutpostResolver(AwsRoute53resolverDeleteOutpostResolverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResolverEndpoint(AwsRoute53resolverDeleteResolverEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResolverQueryLogConfig(AwsRoute53resolverDeleteResolverQueryLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteResolverRule(AwsRoute53resolverDeleteResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateFirewallRuleGroup(AwsRoute53resolverDisassociateFirewallRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateResolverEndpointIpAddress(AwsRoute53resolverDisassociateResolverEndpointIpAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateResolverQueryLogConfig(AwsRoute53resolverDisassociateResolverQueryLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateResolverRule(AwsRoute53resolverDisassociateResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFirewallConfig(AwsRoute53resolverGetFirewallConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFirewallDomainList(AwsRoute53resolverGetFirewallDomainListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFirewallRuleGroup(AwsRoute53resolverGetFirewallRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFirewallRuleGroupAssociation(AwsRoute53resolverGetFirewallRuleGroupAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetFirewallRuleGroupPolicy(AwsRoute53resolverGetFirewallRuleGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOutpostResolver(AwsRoute53resolverGetOutpostResolverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverConfig(AwsRoute53resolverGetResolverConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverDnssecConfig(AwsRoute53resolverGetResolverDnssecConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverEndpoint(AwsRoute53resolverGetResolverEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverQueryLogConfig(AwsRoute53resolverGetResolverQueryLogConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverQueryLogConfigAssociation(AwsRoute53resolverGetResolverQueryLogConfigAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverQueryLogConfigPolicy(AwsRoute53resolverGetResolverQueryLogConfigPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverRule(AwsRoute53resolverGetResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverRuleAssociation(AwsRoute53resolverGetResolverRuleAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResolverRulePolicy(AwsRoute53resolverGetResolverRulePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportFirewallDomains(AwsRoute53resolverImportFirewallDomainsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFirewallConfigs(AwsRoute53resolverListFirewallConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListFirewallConfigsOptions(), token);
    }

    public async Task<CommandResult> ListFirewallDomainLists(AwsRoute53resolverListFirewallDomainListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListFirewallDomainListsOptions(), token);
    }

    public async Task<CommandResult> ListFirewallDomains(AwsRoute53resolverListFirewallDomainsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFirewallRuleGroupAssociations(AwsRoute53resolverListFirewallRuleGroupAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListFirewallRuleGroupAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListFirewallRuleGroups(AwsRoute53resolverListFirewallRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListFirewallRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListFirewallRules(AwsRoute53resolverListFirewallRulesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOutpostResolvers(AwsRoute53resolverListOutpostResolversOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListOutpostResolversOptions(), token);
    }

    public async Task<CommandResult> ListResolverConfigs(AwsRoute53resolverListResolverConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverConfigsOptions(), token);
    }

    public async Task<CommandResult> ListResolverDnssecConfigs(AwsRoute53resolverListResolverDnssecConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverDnssecConfigsOptions(), token);
    }

    public async Task<CommandResult> ListResolverEndpointIpAddresses(AwsRoute53resolverListResolverEndpointIpAddressesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListResolverEndpoints(AwsRoute53resolverListResolverEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListResolverQueryLogConfigAssociations(AwsRoute53resolverListResolverQueryLogConfigAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverQueryLogConfigAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListResolverQueryLogConfigs(AwsRoute53resolverListResolverQueryLogConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverQueryLogConfigsOptions(), token);
    }

    public async Task<CommandResult> ListResolverRuleAssociations(AwsRoute53resolverListResolverRuleAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverRuleAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListResolverRules(AwsRoute53resolverListResolverRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53resolverListResolverRulesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRoute53resolverListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutFirewallRuleGroupPolicy(AwsRoute53resolverPutFirewallRuleGroupPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResolverQueryLogConfigPolicy(AwsRoute53resolverPutResolverQueryLogConfigPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutResolverRulePolicy(AwsRoute53resolverPutResolverRulePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsRoute53resolverTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsRoute53resolverUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallConfig(AwsRoute53resolverUpdateFirewallConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallDomains(AwsRoute53resolverUpdateFirewallDomainsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallRule(AwsRoute53resolverUpdateFirewallRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFirewallRuleGroupAssociation(AwsRoute53resolverUpdateFirewallRuleGroupAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateOutpostResolver(AwsRoute53resolverUpdateOutpostResolverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResolverConfig(AwsRoute53resolverUpdateResolverConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResolverDnssecConfig(AwsRoute53resolverUpdateResolverDnssecConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResolverEndpoint(AwsRoute53resolverUpdateResolverEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResolverRule(AwsRoute53resolverUpdateResolverRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}