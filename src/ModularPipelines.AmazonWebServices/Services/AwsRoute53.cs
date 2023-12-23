using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsRoute53
{
    public AwsRoute53(
        AwsRoute53Wait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsRoute53Wait Wait { get; }

    public async Task<CommandResult> ActivateKeySigningKey(AwsRoute53ActivateKeySigningKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateVpcWithHostedZone(AwsRoute53AssociateVpcWithHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ChangeCidrCollection(AwsRoute53ChangeCidrCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ChangeResourceRecordSets(AwsRoute53ChangeResourceRecordSetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ChangeTagsForResource(AwsRoute53ChangeTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCidrCollection(AwsRoute53CreateCidrCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHealthCheck(AwsRoute53CreateHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHostedZone(AwsRoute53CreateHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateKeySigningKey(AwsRoute53CreateKeySigningKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateQueryLoggingConfig(AwsRoute53CreateQueryLoggingConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReusableDelegationSet(AwsRoute53CreateReusableDelegationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficPolicy(AwsRoute53CreateTrafficPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficPolicyInstance(AwsRoute53CreateTrafficPolicyInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTrafficPolicyVersion(AwsRoute53CreateTrafficPolicyVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcAssociationAuthorization(AwsRoute53CreateVpcAssociationAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeactivateKeySigningKey(AwsRoute53DeactivateKeySigningKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCidrCollection(AwsRoute53DeleteCidrCollectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHealthCheck(AwsRoute53DeleteHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteHostedZone(AwsRoute53DeleteHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeySigningKey(AwsRoute53DeleteKeySigningKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQueryLoggingConfig(AwsRoute53DeleteQueryLoggingConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReusableDelegationSet(AwsRoute53DeleteReusableDelegationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficPolicy(AwsRoute53DeleteTrafficPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTrafficPolicyInstance(AwsRoute53DeleteTrafficPolicyInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcAssociationAuthorization(AwsRoute53DeleteVpcAssociationAuthorizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableHostedZoneDnssec(AwsRoute53DisableHostedZoneDnssecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateVpcFromHostedZone(AwsRoute53DisassociateVpcFromHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableHostedZoneDnssec(AwsRoute53EnableHostedZoneDnssecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountLimit(AwsRoute53GetAccountLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetChange(AwsRoute53GetChangeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCheckerIpRanges(AwsRoute53GetCheckerIpRangesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53GetCheckerIpRangesOptions(), token);
    }

    public async Task<CommandResult> GetDnssec(AwsRoute53GetDnssecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGeoLocation(AwsRoute53GetGeoLocationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53GetGeoLocationOptions(), token);
    }

    public async Task<CommandResult> GetHealthCheck(AwsRoute53GetHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHealthCheckCount(AwsRoute53GetHealthCheckCountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53GetHealthCheckCountOptions(), token);
    }

    public async Task<CommandResult> GetHealthCheckLastFailureReason(AwsRoute53GetHealthCheckLastFailureReasonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHealthCheckStatus(AwsRoute53GetHealthCheckStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHostedZone(AwsRoute53GetHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetHostedZoneCount(AwsRoute53GetHostedZoneCountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53GetHostedZoneCountOptions(), token);
    }

    public async Task<CommandResult> GetHostedZoneLimit(AwsRoute53GetHostedZoneLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetQueryLoggingConfig(AwsRoute53GetQueryLoggingConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReusableDelegationSet(AwsRoute53GetReusableDelegationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetReusableDelegationSetLimit(AwsRoute53GetReusableDelegationSetLimitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrafficPolicy(AwsRoute53GetTrafficPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrafficPolicyInstance(AwsRoute53GetTrafficPolicyInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTrafficPolicyInstanceCount(AwsRoute53GetTrafficPolicyInstanceCountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53GetTrafficPolicyInstanceCountOptions(), token);
    }

    public async Task<CommandResult> ListCidrBlocks(AwsRoute53ListCidrBlocksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCidrCollections(AwsRoute53ListCidrCollectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListCidrCollectionsOptions(), token);
    }

    public async Task<CommandResult> ListCidrLocations(AwsRoute53ListCidrLocationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListGeoLocations(AwsRoute53ListGeoLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListGeoLocationsOptions(), token);
    }

    public async Task<CommandResult> ListHealthChecks(AwsRoute53ListHealthChecksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListHealthChecksOptions(), token);
    }

    public async Task<CommandResult> ListHostedZones(AwsRoute53ListHostedZonesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListHostedZonesOptions(), token);
    }

    public async Task<CommandResult> ListHostedZonesByName(AwsRoute53ListHostedZonesByNameOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListHostedZonesByNameOptions(), token);
    }

    public async Task<CommandResult> ListHostedZonesByVpc(AwsRoute53ListHostedZonesByVpcOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListQueryLoggingConfigs(AwsRoute53ListQueryLoggingConfigsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListQueryLoggingConfigsOptions(), token);
    }

    public async Task<CommandResult> ListResourceRecordSets(AwsRoute53ListResourceRecordSetsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListReusableDelegationSets(AwsRoute53ListReusableDelegationSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListReusableDelegationSetsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsRoute53ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResources(AwsRoute53ListTagsForResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrafficPolicies(AwsRoute53ListTrafficPoliciesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListTrafficPoliciesOptions(), token);
    }

    public async Task<CommandResult> ListTrafficPolicyInstances(AwsRoute53ListTrafficPolicyInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRoute53ListTrafficPolicyInstancesOptions(), token);
    }

    public async Task<CommandResult> ListTrafficPolicyInstancesByHostedZone(AwsRoute53ListTrafficPolicyInstancesByHostedZoneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrafficPolicyInstancesByPolicy(AwsRoute53ListTrafficPolicyInstancesByPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTrafficPolicyVersions(AwsRoute53ListTrafficPolicyVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVpcAssociationAuthorizations(AwsRoute53ListVpcAssociationAuthorizationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestDnsAnswer(AwsRoute53TestDnsAnswerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHealthCheck(AwsRoute53UpdateHealthCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateHostedZoneComment(AwsRoute53UpdateHostedZoneCommentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrafficPolicyComment(AwsRoute53UpdateTrafficPolicyCommentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTrafficPolicyInstance(AwsRoute53UpdateTrafficPolicyInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}