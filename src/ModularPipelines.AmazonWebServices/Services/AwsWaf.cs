using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsWaf
{
    public AwsWaf(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateByteMatchSet(AwsWafCreateByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGeoMatchSet(AwsWafCreateGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIpSet(AwsWafCreateIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRateBasedRule(AwsWafCreateRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegexMatchSet(AwsWafCreateRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegexPatternSet(AwsWafCreateRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRule(AwsWafCreateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRuleGroup(AwsWafCreateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSizeConstraintSet(AwsWafCreateSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSqlInjectionMatchSet(AwsWafCreateSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWebAcl(AwsWafCreateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWebAclMigrationStack(AwsWafCreateWebAclMigrationStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateXssMatchSet(AwsWafCreateXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteByteMatchSet(AwsWafDeleteByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGeoMatchSet(AwsWafDeleteGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIpSet(AwsWafDeleteIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLoggingConfiguration(AwsWafDeleteLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePermissionPolicy(AwsWafDeletePermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRateBasedRule(AwsWafDeleteRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegexMatchSet(AwsWafDeleteRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegexPatternSet(AwsWafDeleteRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRule(AwsWafDeleteRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRuleGroup(AwsWafDeleteRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSizeConstraintSet(AwsWafDeleteSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSqlInjectionMatchSet(AwsWafDeleteSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWebAcl(AwsWafDeleteWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteXssMatchSet(AwsWafDeleteXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetByteMatchSet(AwsWafGetByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChangeToken(AwsWafGetChangeTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafGetChangeTokenOptions(), token);
    }

    public async Task<CommandResult> GetChangeTokenStatus(AwsWafGetChangeTokenStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGeoMatchSet(AwsWafGetGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIpSet(AwsWafGetIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggingConfiguration(AwsWafGetLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPermissionPolicy(AwsWafGetPermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRateBasedRule(AwsWafGetRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRateBasedRuleManagedKeys(AwsWafGetRateBasedRuleManagedKeysOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegexMatchSet(AwsWafGetRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegexPatternSet(AwsWafGetRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRule(AwsWafGetRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRuleGroup(AwsWafGetRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSampledRequests(AwsWafGetSampledRequestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSizeConstraintSet(AwsWafGetSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSqlInjectionMatchSet(AwsWafGetSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWebAcl(AwsWafGetWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetXssMatchSet(AwsWafGetXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListActivatedRulesInRuleGroup(AwsWafListActivatedRulesInRuleGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListActivatedRulesInRuleGroupOptions(), token);
    }

    public async Task<CommandResult> ListByteMatchSets(AwsWafListByteMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListByteMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListGeoMatchSets(AwsWafListGeoMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListGeoMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListIpSets(AwsWafListIpSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListIpSetsOptions(), token);
    }

    public async Task<CommandResult> ListLoggingConfigurations(AwsWafListLoggingConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListLoggingConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListRateBasedRules(AwsWafListRateBasedRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListRateBasedRulesOptions(), token);
    }

    public async Task<CommandResult> ListRegexMatchSets(AwsWafListRegexMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListRegexMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListRegexPatternSets(AwsWafListRegexPatternSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListRegexPatternSetsOptions(), token);
    }

    public async Task<CommandResult> ListRuleGroups(AwsWafListRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListRules(AwsWafListRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListRulesOptions(), token);
    }

    public async Task<CommandResult> ListSizeConstraintSets(AwsWafListSizeConstraintSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListSizeConstraintSetsOptions(), token);
    }

    public async Task<CommandResult> ListSqlInjectionMatchSets(AwsWafListSqlInjectionMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListSqlInjectionMatchSetsOptions(), token);
    }

    public async Task<CommandResult> ListSubscribedRuleGroups(AwsWafListSubscribedRuleGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListSubscribedRuleGroupsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsWafListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWebAcls(AwsWafListWebAclsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListWebAclsOptions(), token);
    }

    public async Task<CommandResult> ListXssMatchSets(AwsWafListXssMatchSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsWafListXssMatchSetsOptions(), token);
    }

    public async Task<CommandResult> PutLoggingConfiguration(AwsWafPutLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutPermissionPolicy(AwsWafPutPermissionPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsWafTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsWafUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateByteMatchSet(AwsWafUpdateByteMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGeoMatchSet(AwsWafUpdateGeoMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateIpSet(AwsWafUpdateIpSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRateBasedRule(AwsWafUpdateRateBasedRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegexMatchSet(AwsWafUpdateRegexMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRegexPatternSet(AwsWafUpdateRegexPatternSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRule(AwsWafUpdateRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRuleGroup(AwsWafUpdateRuleGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSizeConstraintSet(AwsWafUpdateSizeConstraintSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSqlInjectionMatchSet(AwsWafUpdateSqlInjectionMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWebAcl(AwsWafUpdateWebAclOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateXssMatchSet(AwsWafUpdateXssMatchSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}