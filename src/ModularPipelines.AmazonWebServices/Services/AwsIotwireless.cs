using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIotwireless
{
    public AwsIotwireless(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateAwsAccountWithPartnerAccount(AwsIotwirelessAssociateAwsAccountWithPartnerAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateMulticastGroupWithFuotaTask(AwsIotwirelessAssociateMulticastGroupWithFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithFuotaTask(AwsIotwirelessAssociateWirelessDeviceWithFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithMulticastGroup(AwsIotwirelessAssociateWirelessDeviceWithMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithThing(AwsIotwirelessAssociateWirelessDeviceWithThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWirelessGatewayWithCertificate(AwsIotwirelessAssociateWirelessGatewayWithCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateWirelessGatewayWithThing(AwsIotwirelessAssociateWirelessGatewayWithThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelMulticastGroupSession(AwsIotwirelessCancelMulticastGroupSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDestination(AwsIotwirelessCreateDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeviceProfile(AwsIotwirelessCreateDeviceProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateDeviceProfileOptions(), token);
    }

    public async Task<CommandResult> CreateFuotaTask(AwsIotwirelessCreateFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMulticastGroup(AwsIotwirelessCreateMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkAnalyzerConfiguration(AwsIotwirelessCreateNetworkAnalyzerConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServiceProfile(AwsIotwirelessCreateServiceProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateServiceProfileOptions(), token);
    }

    public async Task<CommandResult> CreateWirelessDevice(AwsIotwirelessCreateWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWirelessGateway(AwsIotwirelessCreateWirelessGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWirelessGatewayTask(AwsIotwirelessCreateWirelessGatewayTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWirelessGatewayTaskDefinition(AwsIotwirelessCreateWirelessGatewayTaskDefinitionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateWirelessGatewayTaskDefinitionOptions(), token);
    }

    public async Task<CommandResult> DeleteDestination(AwsIotwirelessDeleteDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDeviceProfile(AwsIotwirelessDeleteDeviceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFuotaTask(AwsIotwirelessDeleteFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMulticastGroup(AwsIotwirelessDeleteMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkAnalyzerConfiguration(AwsIotwirelessDeleteNetworkAnalyzerConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQueuedMessages(AwsIotwirelessDeleteQueuedMessagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServiceProfile(AwsIotwirelessDeleteServiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWirelessDevice(AwsIotwirelessDeleteWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWirelessDeviceImportTask(AwsIotwirelessDeleteWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWirelessGateway(AwsIotwirelessDeleteWirelessGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWirelessGatewayTask(AwsIotwirelessDeleteWirelessGatewayTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWirelessGatewayTaskDefinition(AwsIotwirelessDeleteWirelessGatewayTaskDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterWirelessDevice(AwsIotwirelessDeregisterWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateAwsAccountFromPartnerAccount(AwsIotwirelessDisassociateAwsAccountFromPartnerAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateMulticastGroupFromFuotaTask(AwsIotwirelessDisassociateMulticastGroupFromFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromFuotaTask(AwsIotwirelessDisassociateWirelessDeviceFromFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromMulticastGroup(AwsIotwirelessDisassociateWirelessDeviceFromMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromThing(AwsIotwirelessDisassociateWirelessDeviceFromThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWirelessGatewayFromCertificate(AwsIotwirelessDisassociateWirelessGatewayFromCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateWirelessGatewayFromThing(AwsIotwirelessDisassociateWirelessGatewayFromThingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDestination(AwsIotwirelessGetDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeviceProfile(AwsIotwirelessGetDeviceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEventConfigurationByResourceTypes(AwsIotwirelessGetEventConfigurationByResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetEventConfigurationByResourceTypesOptions(), token);
    }

    public async Task<CommandResult> GetFuotaTask(AwsIotwirelessGetFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLogLevelsByResourceTypes(AwsIotwirelessGetLogLevelsByResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetLogLevelsByResourceTypesOptions(), token);
    }

    public async Task<CommandResult> GetMulticastGroup(AwsIotwirelessGetMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMulticastGroupSession(AwsIotwirelessGetMulticastGroupSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkAnalyzerConfiguration(AwsIotwirelessGetNetworkAnalyzerConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPartnerAccount(AwsIotwirelessGetPartnerAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPositionEstimate(AwsIotwirelessGetPositionEstimateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetPositionEstimateOptions(), token);
    }

    public async Task<CommandResult> GetResourceEventConfiguration(AwsIotwirelessGetResourceEventConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourceLogLevel(AwsIotwirelessGetResourceLogLevelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetResourcePosition(AwsIotwirelessGetResourcePositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceEndpoint(AwsIotwirelessGetServiceEndpointOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetServiceEndpointOptions(), token);
    }

    public async Task<CommandResult> GetServiceProfile(AwsIotwirelessGetServiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessDevice(AwsIotwirelessGetWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessDeviceImportTask(AwsIotwirelessGetWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessDeviceStatistics(AwsIotwirelessGetWirelessDeviceStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGateway(AwsIotwirelessGetWirelessGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGatewayCertificate(AwsIotwirelessGetWirelessGatewayCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGatewayFirmwareInformation(AwsIotwirelessGetWirelessGatewayFirmwareInformationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGatewayStatistics(AwsIotwirelessGetWirelessGatewayStatisticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGatewayTask(AwsIotwirelessGetWirelessGatewayTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetWirelessGatewayTaskDefinition(AwsIotwirelessGetWirelessGatewayTaskDefinitionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDestinations(AwsIotwirelessListDestinationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListDestinationsOptions(), token);
    }

    public async Task<CommandResult> ListDeviceProfiles(AwsIotwirelessListDeviceProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListDeviceProfilesOptions(), token);
    }

    public async Task<CommandResult> ListDevicesForWirelessDeviceImportTask(AwsIotwirelessListDevicesForWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEventConfigurations(AwsIotwirelessListEventConfigurationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFuotaTasks(AwsIotwirelessListFuotaTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListFuotaTasksOptions(), token);
    }

    public async Task<CommandResult> ListMulticastGroups(AwsIotwirelessListMulticastGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListMulticastGroupsOptions(), token);
    }

    public async Task<CommandResult> ListMulticastGroupsByFuotaTask(AwsIotwirelessListMulticastGroupsByFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNetworkAnalyzerConfigurations(AwsIotwirelessListNetworkAnalyzerConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListNetworkAnalyzerConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListPartnerAccounts(AwsIotwirelessListPartnerAccountsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListPartnerAccountsOptions(), token);
    }

    public async Task<CommandResult> ListQueuedMessages(AwsIotwirelessListQueuedMessagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServiceProfiles(AwsIotwirelessListServiceProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListServiceProfilesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotwirelessListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListWirelessDeviceImportTasks(AwsIotwirelessListWirelessDeviceImportTasksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessDeviceImportTasksOptions(), token);
    }

    public async Task<CommandResult> ListWirelessDevices(AwsIotwirelessListWirelessDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessDevicesOptions(), token);
    }

    public async Task<CommandResult> ListWirelessGatewayTaskDefinitions(AwsIotwirelessListWirelessGatewayTaskDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessGatewayTaskDefinitionsOptions(), token);
    }

    public async Task<CommandResult> ListWirelessGateways(AwsIotwirelessListWirelessGatewaysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessGatewaysOptions(), token);
    }

    public async Task<CommandResult> PutResourceLogLevel(AwsIotwirelessPutResourceLogLevelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResetAllResourceLogLevels(AwsIotwirelessResetAllResourceLogLevelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessResetAllResourceLogLevelsOptions(), token);
    }

    public async Task<CommandResult> ResetResourceLogLevel(AwsIotwirelessResetResourceLogLevelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendDataToMulticastGroup(AwsIotwirelessSendDataToMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendDataToWirelessDevice(AwsIotwirelessSendDataToWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBulkAssociateWirelessDeviceWithMulticastGroup(AwsIotwirelessStartBulkAssociateWirelessDeviceWithMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBulkDisassociateWirelessDeviceFromMulticastGroup(AwsIotwirelessStartBulkDisassociateWirelessDeviceFromMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartFuotaTask(AwsIotwirelessStartFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMulticastGroupSession(AwsIotwirelessStartMulticastGroupSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartSingleWirelessDeviceImportTask(AwsIotwirelessStartSingleWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartWirelessDeviceImportTask(AwsIotwirelessStartWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsIotwirelessTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestWirelessDevice(AwsIotwirelessTestWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIotwirelessUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDestination(AwsIotwirelessUpdateDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEventConfigurationByResourceTypes(AwsIotwirelessUpdateEventConfigurationByResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessUpdateEventConfigurationByResourceTypesOptions(), token);
    }

    public async Task<CommandResult> UpdateFuotaTask(AwsIotwirelessUpdateFuotaTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateLogLevelsByResourceTypes(AwsIotwirelessUpdateLogLevelsByResourceTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessUpdateLogLevelsByResourceTypesOptions(), token);
    }

    public async Task<CommandResult> UpdateMulticastGroup(AwsIotwirelessUpdateMulticastGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNetworkAnalyzerConfiguration(AwsIotwirelessUpdateNetworkAnalyzerConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePartnerAccount(AwsIotwirelessUpdatePartnerAccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourceEventConfiguration(AwsIotwirelessUpdateResourceEventConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateResourcePosition(AwsIotwirelessUpdateResourcePositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWirelessDevice(AwsIotwirelessUpdateWirelessDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWirelessDeviceImportTask(AwsIotwirelessUpdateWirelessDeviceImportTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateWirelessGateway(AwsIotwirelessUpdateWirelessGatewayOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}