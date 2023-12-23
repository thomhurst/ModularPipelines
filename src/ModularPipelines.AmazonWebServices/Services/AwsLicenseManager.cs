using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsLicenseManager
{
    public AwsLicenseManager(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptGrant(AwsLicenseManagerAcceptGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckInLicense(AwsLicenseManagerCheckInLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckoutBorrowLicense(AwsLicenseManagerCheckoutBorrowLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckoutLicense(AwsLicenseManagerCheckoutLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGrant(AwsLicenseManagerCreateGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateGrantVersion(AwsLicenseManagerCreateGrantVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLicense(AwsLicenseManagerCreateLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLicenseConfiguration(AwsLicenseManagerCreateLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLicenseConversionTaskForResource(AwsLicenseManagerCreateLicenseConversionTaskForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLicenseManagerReportGenerator(AwsLicenseManagerCreateLicenseManagerReportGeneratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateLicenseVersion(AwsLicenseManagerCreateLicenseVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateToken(AwsLicenseManagerCreateTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteGrant(AwsLicenseManagerDeleteGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLicense(AwsLicenseManagerDeleteLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLicenseConfiguration(AwsLicenseManagerDeleteLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteLicenseManagerReportGenerator(AwsLicenseManagerDeleteLicenseManagerReportGeneratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteToken(AwsLicenseManagerDeleteTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExtendLicenseConsumption(AwsLicenseManagerExtendLicenseConsumptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccessToken(AwsLicenseManagerGetAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGrant(AwsLicenseManagerGetGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLicense(AwsLicenseManagerGetLicenseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLicenseConfiguration(AwsLicenseManagerGetLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLicenseConversionTask(AwsLicenseManagerGetLicenseConversionTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLicenseManagerReportGenerator(AwsLicenseManagerGetLicenseManagerReportGeneratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLicenseUsage(AwsLicenseManagerGetLicenseUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceSettings(AwsLicenseManagerGetServiceSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerGetServiceSettingsOptions(), token);
    }

    public async Task<CommandResult> ListAssociationsForLicenseConfiguration(AwsLicenseManagerListAssociationsForLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDistributedGrants(AwsLicenseManagerListDistributedGrantsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListDistributedGrantsOptions(), token);
    }

    public async Task<CommandResult> ListFailuresForLicenseConfigurationOperations(AwsLicenseManagerListFailuresForLicenseConfigurationOperationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLicenseConfigurations(AwsLicenseManagerListLicenseConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListLicenseConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListLicenseConversionTasks(AwsLicenseManagerListLicenseConversionTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListLicenseConversionTasksOptions(), token);
    }

    public async Task<CommandResult> ListLicenseManagerReportGenerators(AwsLicenseManagerListLicenseManagerReportGeneratorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListLicenseManagerReportGeneratorsOptions(), token);
    }

    public async Task<CommandResult> ListLicenseSpecificationsForResource(AwsLicenseManagerListLicenseSpecificationsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLicenseVersions(AwsLicenseManagerListLicenseVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLicenses(AwsLicenseManagerListLicensesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListLicensesOptions(), token);
    }

    public async Task<CommandResult> ListReceivedGrants(AwsLicenseManagerListReceivedGrantsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListReceivedGrantsOptions(), token);
    }

    public async Task<CommandResult> ListReceivedGrantsForOrganization(AwsLicenseManagerListReceivedGrantsForOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListReceivedLicenses(AwsLicenseManagerListReceivedLicensesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListReceivedLicensesOptions(), token);
    }

    public async Task<CommandResult> ListReceivedLicensesForOrganization(AwsLicenseManagerListReceivedLicensesForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListReceivedLicensesForOrganizationOptions(), token);
    }

    public async Task<CommandResult> ListResourceInventory(AwsLicenseManagerListResourceInventoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListResourceInventoryOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsLicenseManagerListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTokens(AwsLicenseManagerListTokensOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerListTokensOptions(), token);
    }

    public async Task<CommandResult> ListUsageForLicenseConfiguration(AwsLicenseManagerListUsageForLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectGrant(AwsLicenseManagerRejectGrantOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsLicenseManagerTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsLicenseManagerUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLicenseConfiguration(AwsLicenseManagerUpdateLicenseConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLicenseManagerReportGenerator(AwsLicenseManagerUpdateLicenseManagerReportGeneratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLicenseSpecificationsForResource(AwsLicenseManagerUpdateLicenseSpecificationsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceSettings(AwsLicenseManagerUpdateServiceSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsLicenseManagerUpdateServiceSettingsOptions(), token);
    }
}