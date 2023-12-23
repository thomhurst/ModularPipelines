using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsWafRegional
{
    public AwsWafRegional(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateWebAcl(AwsWafRegionalAssociateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateByteMatchSet(AwsWafRegionalCreateByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGeoMatchSet(AwsWafRegionalCreateGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIpSet(AwsWafRegionalCreateIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRateBasedRule(AwsWafRegionalCreateRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegexMatchSet(AwsWafRegionalCreateRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegexPatternSet(AwsWafRegionalCreateRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRule(AwsWafRegionalCreateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRuleGroup(AwsWafRegionalCreateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSizeConstraintSet(AwsWafRegionalCreateSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSqlInjectionMatchSet(AwsWafRegionalCreateSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWebAcl(AwsWafRegionalCreateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWebAclMigrationStack(AwsWafRegionalCreateWebAclMigrationStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateXssMatchSet(AwsWafRegionalCreateXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteByteMatchSet(AwsWafRegionalDeleteByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGeoMatchSet(AwsWafRegionalDeleteGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpSet(AwsWafRegionalDeleteIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoggingConfiguration(AwsWafRegionalDeleteLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePermissionPolicy(AwsWafRegionalDeletePermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRateBasedRule(AwsWafRegionalDeleteRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegexMatchSet(AwsWafRegionalDeleteRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegexPatternSet(AwsWafRegionalDeleteRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRule(AwsWafRegionalDeleteRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRuleGroup(AwsWafRegionalDeleteRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSizeConstraintSet(AwsWafRegionalDeleteSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSqlInjectionMatchSet(AwsWafRegionalDeleteSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWebAcl(AwsWafRegionalDeleteWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteXssMatchSet(AwsWafRegionalDeleteXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWebAcl(AwsWafRegionalDisassociateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetByteMatchSet(AwsWafRegionalGetByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChangeToken(AwsWafRegionalGetChangeTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalGetChangeTokenOptions(), token);
    }

    public async Task<CommandResult> GetChangeTokenStatus(AwsWafRegionalGetChangeTokenStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGeoMatchSet(AwsWafRegionalGetGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpSet(AwsWafRegionalGetIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggingConfiguration(AwsWafRegionalGetLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPermissionPolicy(AwsWafRegionalGetPermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRateBasedRule(AwsWafRegionalGetRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRateBasedRuleManagedKeys(AwsWafRegionalGetRateBasedRuleManagedKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegexMatchSet(AwsWafRegionalGetRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegexPatternSet(AwsWafRegionalGetRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRule(AwsWafRegionalGetRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRuleGroup(AwsWafRegionalGetRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSampledRequests(AwsWafRegionalGetSampledRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSizeConstraintSet(AwsWafRegionalGetSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSqlInjectionMatchSet(AwsWafRegionalGetSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWebAcl(AwsWafRegionalGetWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWebAclForResource(AwsWafRegionalGetWebAclForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetXssMatchSet(AwsWafRegionalGetXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListActivatedRulesInRuleGroup(AwsWafRegionalListActivatedRulesInRuleGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListActivatedRulesInRuleGroupOptions(), token);
    }

    public async Task<CommandResult> ListByteMatchSets(AwsWafRegionalListByteMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListByteMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListGeoMatchSets(AwsWafRegionalListGeoMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListGeoMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListIpSets(AwsWafRegionalListIpSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListIpSetsOptions(), token);
    }

    public async Task<CommandResult> ListLoggingConfigurations(AwsWafRegionalListLoggingConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListLoggingConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListRateBasedRules(AwsWafRegionalListRateBasedRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListRateBasedRulesOptions(), token);
    }

    public async Task<CommandResult> ListRegexMatchSets(AwsWafRegionalListRegexMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListRegexMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListRegexPatternSets(AwsWafRegionalListRegexPatternSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListRegexPatternSetsOptions(), token);
    }

    public async Task<CommandResult> ListResourcesForWebAcl(AwsWafRegionalListResourcesForWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRuleGroups(AwsWafRegionalListRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListRules(AwsWafRegionalListRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListRulesOptions(), token);
    }

    public async Task<CommandResult> ListSizeConstraintSets(AwsWafRegionalListSizeConstraintSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListSizeConstraintSetsOptions(), token);
    }

    public async Task<CommandResult> ListSqlInjectionMatchSets(AwsWafRegionalListSqlInjectionMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListSqlInjectionMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListSubscribedRuleGroups(AwsWafRegionalListSubscribedRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListSubscribedRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsWafRegionalListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWebAcls(AwsWafRegionalListWebAclsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListWebAclsOptions(), token);
    }

    public async Task<CommandResult> ListXssMatchSets(AwsWafRegionalListXssMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafRegionalListXssMatchSetsOptions(), token);
    }

    public async Task<CommandResult> PutLoggingConfiguration(AwsWafRegionalPutLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutPermissionPolicy(AwsWafRegionalPutPermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsWafRegionalTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsWafRegionalUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateByteMatchSet(AwsWafRegionalUpdateByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGeoMatchSet(AwsWafRegionalUpdateGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIpSet(AwsWafRegionalUpdateIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRateBasedRule(AwsWafRegionalUpdateRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegexMatchSet(AwsWafRegionalUpdateRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegexPatternSet(AwsWafRegionalUpdateRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRule(AwsWafRegionalUpdateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRuleGroup(AwsWafRegionalUpdateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSizeConstraintSet(AwsWafRegionalUpdateSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSqlInjectionMatchSet(AwsWafRegionalUpdateSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWebAcl(AwsWafRegionalUpdateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateXssMatchSet(AwsWafRegionalUpdateXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}