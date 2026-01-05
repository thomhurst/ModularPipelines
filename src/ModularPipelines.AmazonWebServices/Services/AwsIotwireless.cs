using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AssociateAwsAccountWithPartnerAccount(AwsIotwirelessAssociateAwsAccountWithPartnerAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateMulticastGroupWithFuotaTask(AwsIotwirelessAssociateMulticastGroupWithFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithFuotaTask(AwsIotwirelessAssociateWirelessDeviceWithFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithMulticastGroup(AwsIotwirelessAssociateWirelessDeviceWithMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateWirelessDeviceWithThing(AwsIotwirelessAssociateWirelessDeviceWithThingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateWirelessGatewayWithCertificate(AwsIotwirelessAssociateWirelessGatewayWithCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateWirelessGatewayWithThing(AwsIotwirelessAssociateWirelessGatewayWithThingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CancelMulticastGroupSession(AwsIotwirelessCancelMulticastGroupSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDestination(AwsIotwirelessCreateDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDeviceProfile(AwsIotwirelessCreateDeviceProfileOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateDeviceProfileOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateFuotaTask(AwsIotwirelessCreateFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateMulticastGroup(AwsIotwirelessCreateMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateNetworkAnalyzerConfiguration(AwsIotwirelessCreateNetworkAnalyzerConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateServiceProfile(AwsIotwirelessCreateServiceProfileOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateServiceProfileOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateWirelessDevice(AwsIotwirelessCreateWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateWirelessGateway(AwsIotwirelessCreateWirelessGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateWirelessGatewayTask(AwsIotwirelessCreateWirelessGatewayTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateWirelessGatewayTaskDefinition(AwsIotwirelessCreateWirelessGatewayTaskDefinitionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessCreateWirelessGatewayTaskDefinitionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDestination(AwsIotwirelessDeleteDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDeviceProfile(AwsIotwirelessDeleteDeviceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteFuotaTask(AwsIotwirelessDeleteFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteMulticastGroup(AwsIotwirelessDeleteMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteNetworkAnalyzerConfiguration(AwsIotwirelessDeleteNetworkAnalyzerConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteQueuedMessages(AwsIotwirelessDeleteQueuedMessagesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteServiceProfile(AwsIotwirelessDeleteServiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteWirelessDevice(AwsIotwirelessDeleteWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteWirelessDeviceImportTask(AwsIotwirelessDeleteWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteWirelessGateway(AwsIotwirelessDeleteWirelessGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteWirelessGatewayTask(AwsIotwirelessDeleteWirelessGatewayTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteWirelessGatewayTaskDefinition(AwsIotwirelessDeleteWirelessGatewayTaskDefinitionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterWirelessDevice(AwsIotwirelessDeregisterWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateAwsAccountFromPartnerAccount(AwsIotwirelessDisassociateAwsAccountFromPartnerAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateMulticastGroupFromFuotaTask(AwsIotwirelessDisassociateMulticastGroupFromFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromFuotaTask(AwsIotwirelessDisassociateWirelessDeviceFromFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromMulticastGroup(AwsIotwirelessDisassociateWirelessDeviceFromMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateWirelessDeviceFromThing(AwsIotwirelessDisassociateWirelessDeviceFromThingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateWirelessGatewayFromCertificate(AwsIotwirelessDisassociateWirelessGatewayFromCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateWirelessGatewayFromThing(AwsIotwirelessDisassociateWirelessGatewayFromThingOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDestination(AwsIotwirelessGetDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDeviceProfile(AwsIotwirelessGetDeviceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEventConfigurationByResourceTypes(AwsIotwirelessGetEventConfigurationByResourceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetEventConfigurationByResourceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetFuotaTask(AwsIotwirelessGetFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetLogLevelsByResourceTypes(AwsIotwirelessGetLogLevelsByResourceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetLogLevelsByResourceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMulticastGroup(AwsIotwirelessGetMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetMulticastGroupSession(AwsIotwirelessGetMulticastGroupSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetNetworkAnalyzerConfiguration(AwsIotwirelessGetNetworkAnalyzerConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPartnerAccount(AwsIotwirelessGetPartnerAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPositionEstimate(AwsIotwirelessGetPositionEstimateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetPositionEstimateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceEventConfiguration(AwsIotwirelessGetResourceEventConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourceLogLevel(AwsIotwirelessGetResourceLogLevelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResourcePosition(AwsIotwirelessGetResourcePositionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServiceEndpoint(AwsIotwirelessGetServiceEndpointOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessGetServiceEndpointOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServiceProfile(AwsIotwirelessGetServiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessDevice(AwsIotwirelessGetWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessDeviceImportTask(AwsIotwirelessGetWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessDeviceStatistics(AwsIotwirelessGetWirelessDeviceStatisticsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGateway(AwsIotwirelessGetWirelessGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGatewayCertificate(AwsIotwirelessGetWirelessGatewayCertificateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGatewayFirmwareInformation(AwsIotwirelessGetWirelessGatewayFirmwareInformationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGatewayStatistics(AwsIotwirelessGetWirelessGatewayStatisticsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGatewayTask(AwsIotwirelessGetWirelessGatewayTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetWirelessGatewayTaskDefinition(AwsIotwirelessGetWirelessGatewayTaskDefinitionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDestinations(AwsIotwirelessListDestinationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListDestinationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDeviceProfiles(AwsIotwirelessListDeviceProfilesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListDeviceProfilesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDevicesForWirelessDeviceImportTask(AwsIotwirelessListDevicesForWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEventConfigurations(AwsIotwirelessListEventConfigurationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListFuotaTasks(AwsIotwirelessListFuotaTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListFuotaTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMulticastGroups(AwsIotwirelessListMulticastGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListMulticastGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListMulticastGroupsByFuotaTask(AwsIotwirelessListMulticastGroupsByFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListNetworkAnalyzerConfigurations(AwsIotwirelessListNetworkAnalyzerConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListNetworkAnalyzerConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPartnerAccounts(AwsIotwirelessListPartnerAccountsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListPartnerAccountsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListQueuedMessages(AwsIotwirelessListQueuedMessagesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServiceProfiles(AwsIotwirelessListServiceProfilesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListServiceProfilesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotwirelessListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListWirelessDeviceImportTasks(AwsIotwirelessListWirelessDeviceImportTasksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessDeviceImportTasksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListWirelessDevices(AwsIotwirelessListWirelessDevicesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessDevicesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListWirelessGatewayTaskDefinitions(AwsIotwirelessListWirelessGatewayTaskDefinitionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessGatewayTaskDefinitionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListWirelessGateways(AwsIotwirelessListWirelessGatewaysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessListWirelessGatewaysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutResourceLogLevel(AwsIotwirelessPutResourceLogLevelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetAllResourceLogLevels(AwsIotwirelessResetAllResourceLogLevelsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessResetAllResourceLogLevelsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ResetResourceLogLevel(AwsIotwirelessResetResourceLogLevelOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendDataToMulticastGroup(AwsIotwirelessSendDataToMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendDataToWirelessDevice(AwsIotwirelessSendDataToWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartBulkAssociateWirelessDeviceWithMulticastGroup(AwsIotwirelessStartBulkAssociateWirelessDeviceWithMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartBulkDisassociateWirelessDeviceFromMulticastGroup(AwsIotwirelessStartBulkDisassociateWirelessDeviceFromMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartFuotaTask(AwsIotwirelessStartFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartMulticastGroupSession(AwsIotwirelessStartMulticastGroupSessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartSingleWirelessDeviceImportTask(AwsIotwirelessStartSingleWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartWirelessDeviceImportTask(AwsIotwirelessStartWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsIotwirelessTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TestWirelessDevice(AwsIotwirelessTestWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsIotwirelessUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDestination(AwsIotwirelessUpdateDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEventConfigurationByResourceTypes(AwsIotwirelessUpdateEventConfigurationByResourceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessUpdateEventConfigurationByResourceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateFuotaTask(AwsIotwirelessUpdateFuotaTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateLogLevelsByResourceTypes(AwsIotwirelessUpdateLogLevelsByResourceTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotwirelessUpdateLogLevelsByResourceTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateMulticastGroup(AwsIotwirelessUpdateMulticastGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateNetworkAnalyzerConfiguration(AwsIotwirelessUpdateNetworkAnalyzerConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePartnerAccount(AwsIotwirelessUpdatePartnerAccountOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResourceEventConfiguration(AwsIotwirelessUpdateResourceEventConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateResourcePosition(AwsIotwirelessUpdateResourcePositionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateWirelessDevice(AwsIotwirelessUpdateWirelessDeviceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateWirelessDeviceImportTask(AwsIotwirelessUpdateWirelessDeviceImportTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateWirelessGateway(AwsIotwirelessUpdateWirelessGatewayOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}