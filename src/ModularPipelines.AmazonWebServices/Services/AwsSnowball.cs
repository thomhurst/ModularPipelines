using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSnowball
{
    public AwsSnowball(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelCluster(AwsSnowballCancelClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelJob(AwsSnowballCancelJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateAddress(AwsSnowballCreateAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCluster(AwsSnowballCreateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateJob(AwsSnowballCreateJobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballCreateJobOptions(), token);
    }

    public async Task<CommandResult> CreateLongTermPricing(AwsSnowballCreateLongTermPricingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReturnShippingLabel(AwsSnowballCreateReturnShippingLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAddress(AwsSnowballDescribeAddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAddresses(AwsSnowballDescribeAddressesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballDescribeAddressesOptions(), token);
    }

    public async Task<CommandResult> DescribeCluster(AwsSnowballDescribeClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeJob(AwsSnowballDescribeJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReturnShippingLabel(AwsSnowballDescribeReturnShippingLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobManifest(AwsSnowballGetJobManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJobUnlockCode(AwsSnowballGetJobUnlockCodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSnowballUsage(AwsSnowballGetSnowballUsageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballGetSnowballUsageOptions(), token);
    }

    public async Task<CommandResult> GetSoftwareUpdates(AwsSnowballGetSoftwareUpdatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusterJobs(AwsSnowballListClusterJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListClusters(AwsSnowballListClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballListClustersOptions(), token);
    }

    public async Task<CommandResult> ListCompatibleImages(AwsSnowballListCompatibleImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballListCompatibleImagesOptions(), token);
    }

    public async Task<CommandResult> ListJobs(AwsSnowballListJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballListJobsOptions(), token);
    }

    public async Task<CommandResult> ListLongTermPricing(AwsSnowballListLongTermPricingOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballListLongTermPricingOptions(), token);
    }

    public async Task<CommandResult> ListPickupLocations(AwsSnowballListPickupLocationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSnowballListPickupLocationsOptions(), token);
    }

    public async Task<CommandResult> ListServiceVersions(AwsSnowballListServiceVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCluster(AwsSnowballUpdateClusterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateJob(AwsSnowballUpdateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateJobShipmentState(AwsSnowballUpdateJobShipmentStateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLongTermPricing(AwsSnowballUpdateLongTermPricingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}