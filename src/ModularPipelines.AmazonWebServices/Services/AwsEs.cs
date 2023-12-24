using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEs
{
    public AwsEs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptInboundCrossClusterSearchConnection(AwsEsAcceptInboundCrossClusterSearchConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddTags(AwsEsAddTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociatePackage(AwsEsAssociatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthorizeVpcEndpointAccess(AwsEsAuthorizeVpcEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelElasticsearchServiceSoftwareUpdate(AwsEsCancelElasticsearchServiceSoftwareUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateElasticsearchDomain(AwsEsCreateElasticsearchDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOutboundCrossClusterSearchConnection(AwsEsCreateOutboundCrossClusterSearchConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePackage(AwsEsCreatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpcEndpoint(AwsEsCreateVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteElasticsearchDomain(AwsEsDeleteElasticsearchDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteElasticsearchServiceRole(AwsEsDeleteElasticsearchServiceRoleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDeleteElasticsearchServiceRoleOptions(), token);
    }

    public async Task<CommandResult> DeleteInboundCrossClusterSearchConnection(AwsEsDeleteInboundCrossClusterSearchConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOutboundCrossClusterSearchConnection(AwsEsDeleteOutboundCrossClusterSearchConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePackage(AwsEsDeletePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpcEndpoint(AwsEsDeleteVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDomainAutoTunes(AwsEsDescribeDomainAutoTunesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeDomainChangeProgress(AwsEsDescribeDomainChangeProgressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeElasticsearchDomain(AwsEsDescribeElasticsearchDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeElasticsearchDomainConfig(AwsEsDescribeElasticsearchDomainConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeElasticsearchDomains(AwsEsDescribeElasticsearchDomainsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeElasticsearchInstanceTypeLimits(AwsEsDescribeElasticsearchInstanceTypeLimitsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInboundCrossClusterSearchConnections(AwsEsDescribeInboundCrossClusterSearchConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDescribeInboundCrossClusterSearchConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribeOutboundCrossClusterSearchConnections(AwsEsDescribeOutboundCrossClusterSearchConnectionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDescribeOutboundCrossClusterSearchConnectionsOptions(), token);
    }

    public async Task<CommandResult> DescribePackages(AwsEsDescribePackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDescribePackagesOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedElasticsearchInstanceOfferings(AwsEsDescribeReservedElasticsearchInstanceOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDescribeReservedElasticsearchInstanceOfferingsOptions(), token);
    }

    public async Task<CommandResult> DescribeReservedElasticsearchInstances(AwsEsDescribeReservedElasticsearchInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsDescribeReservedElasticsearchInstancesOptions(), token);
    }

    public async Task<CommandResult> DescribeVpcEndpoints(AwsEsDescribeVpcEndpointsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DissociatePackage(AwsEsDissociatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCompatibleElasticsearchVersions(AwsEsGetCompatibleElasticsearchVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsGetCompatibleElasticsearchVersionsOptions(), token);
    }

    public async Task<CommandResult> GetPackageVersionHistory(AwsEsGetPackageVersionHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUpgradeHistory(AwsEsGetUpgradeHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUpgradeStatus(AwsEsGetUpgradeStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDomainNames(AwsEsListDomainNamesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsListDomainNamesOptions(), token);
    }

    public async Task<CommandResult> ListDomainsForPackage(AwsEsListDomainsForPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListElasticsearchInstanceTypes(AwsEsListElasticsearchInstanceTypesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListElasticsearchVersions(AwsEsListElasticsearchVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsListElasticsearchVersionsOptions(), token);
    }

    public async Task<CommandResult> ListPackagesForDomain(AwsEsListPackagesForDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTags(AwsEsListTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVpcEndpointAccess(AwsEsListVpcEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVpcEndpoints(AwsEsListVpcEndpointsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEsListVpcEndpointsOptions(), token);
    }

    public async Task<CommandResult> ListVpcEndpointsForDomain(AwsEsListVpcEndpointsForDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseReservedElasticsearchInstanceOffering(AwsEsPurchaseReservedElasticsearchInstanceOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectInboundCrossClusterSearchConnection(AwsEsRejectInboundCrossClusterSearchConnectionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveTags(AwsEsRemoveTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RevokeVpcEndpointAccess(AwsEsRevokeVpcEndpointAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartElasticsearchServiceSoftwareUpdate(AwsEsStartElasticsearchServiceSoftwareUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateElasticsearchDomainConfig(AwsEsUpdateElasticsearchDomainConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePackage(AwsEsUpdatePackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVpcEndpoint(AwsEsUpdateVpcEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpgradeElasticsearchDomain(AwsEsUpgradeElasticsearchDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}