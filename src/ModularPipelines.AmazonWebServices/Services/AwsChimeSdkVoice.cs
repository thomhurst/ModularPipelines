using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsChimeSdkVoice
{
    public AwsChimeSdkVoice(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociatePhoneNumbersWithVoiceConnector(AwsChimeSdkVoiceAssociatePhoneNumbersWithVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociatePhoneNumbersWithVoiceConnectorGroup(AwsChimeSdkVoiceAssociatePhoneNumbersWithVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchDeletePhoneNumber(AwsChimeSdkVoiceBatchDeletePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchUpdatePhoneNumber(AwsChimeSdkVoiceBatchUpdatePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePhoneNumberOrder(AwsChimeSdkVoiceCreatePhoneNumberOrderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProxySession(AwsChimeSdkVoiceCreateProxySessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSipMediaApplication(AwsChimeSdkVoiceCreateSipMediaApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSipMediaApplicationCall(AwsChimeSdkVoiceCreateSipMediaApplicationCallOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSipRule(AwsChimeSdkVoiceCreateSipRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVoiceConnector(AwsChimeSdkVoiceCreateVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVoiceConnectorGroup(AwsChimeSdkVoiceCreateVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVoiceProfile(AwsChimeSdkVoiceCreateVoiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVoiceProfileDomain(AwsChimeSdkVoiceCreateVoiceProfileDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePhoneNumber(AwsChimeSdkVoiceDeletePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProxySession(AwsChimeSdkVoiceDeleteProxySessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSipMediaApplication(AwsChimeSdkVoiceDeleteSipMediaApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSipRule(AwsChimeSdkVoiceDeleteSipRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnector(AwsChimeSdkVoiceDeleteVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoiceDeleteVoiceConnectorEmergencyCallingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorGroup(AwsChimeSdkVoiceDeleteVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorOrigination(AwsChimeSdkVoiceDeleteVoiceConnectorOriginationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorProxy(AwsChimeSdkVoiceDeleteVoiceConnectorProxyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorStreamingConfiguration(AwsChimeSdkVoiceDeleteVoiceConnectorStreamingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorTermination(AwsChimeSdkVoiceDeleteVoiceConnectorTerminationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceConnectorTerminationCredentials(AwsChimeSdkVoiceDeleteVoiceConnectorTerminationCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceProfile(AwsChimeSdkVoiceDeleteVoiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceProfileDomain(AwsChimeSdkVoiceDeleteVoiceProfileDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociatePhoneNumbersFromVoiceConnector(AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociatePhoneNumbersFromVoiceConnectorGroup(AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetGlobalSettings(AwsChimeSdkVoiceGetGlobalSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceGetGlobalSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPhoneNumber(AwsChimeSdkVoiceGetPhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPhoneNumberOrder(AwsChimeSdkVoiceGetPhoneNumberOrderOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetPhoneNumberSettings(AwsChimeSdkVoiceGetPhoneNumberSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceGetPhoneNumberSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetProxySession(AwsChimeSdkVoiceGetProxySessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSipMediaApplication(AwsChimeSdkVoiceGetSipMediaApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSipMediaApplicationAlexaSkillConfiguration(AwsChimeSdkVoiceGetSipMediaApplicationAlexaSkillConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSipMediaApplicationLoggingConfiguration(AwsChimeSdkVoiceGetSipMediaApplicationLoggingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSipRule(AwsChimeSdkVoiceGetSipRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSpeakerSearchTask(AwsChimeSdkVoiceGetSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnector(AwsChimeSdkVoiceGetVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorEmergencyCallingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorGroup(AwsChimeSdkVoiceGetVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorLoggingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorLoggingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorOrigination(AwsChimeSdkVoiceGetVoiceConnectorOriginationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorProxy(AwsChimeSdkVoiceGetVoiceConnectorProxyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorStreamingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorStreamingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorTermination(AwsChimeSdkVoiceGetVoiceConnectorTerminationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceConnectorTerminationHealth(AwsChimeSdkVoiceGetVoiceConnectorTerminationHealthOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceProfile(AwsChimeSdkVoiceGetVoiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceProfileDomain(AwsChimeSdkVoiceGetVoiceProfileDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetVoiceToneAnalysisTask(AwsChimeSdkVoiceGetVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAvailableVoiceConnectorRegions(AwsChimeSdkVoiceListAvailableVoiceConnectorRegionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListAvailableVoiceConnectorRegionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPhoneNumberOrders(AwsChimeSdkVoiceListPhoneNumberOrdersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListPhoneNumberOrdersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPhoneNumbers(AwsChimeSdkVoiceListPhoneNumbersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListPhoneNumbersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProxySessions(AwsChimeSdkVoiceListProxySessionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSipMediaApplications(AwsChimeSdkVoiceListSipMediaApplicationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListSipMediaApplicationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSipRules(AwsChimeSdkVoiceListSipRulesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListSipRulesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSupportedPhoneNumberCountries(AwsChimeSdkVoiceListSupportedPhoneNumberCountriesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkVoiceListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVoiceConnectorGroups(AwsChimeSdkVoiceListVoiceConnectorGroupsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceConnectorGroupsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVoiceConnectorTerminationCredentials(AwsChimeSdkVoiceListVoiceConnectorTerminationCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVoiceConnectors(AwsChimeSdkVoiceListVoiceConnectorsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceConnectorsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVoiceProfileDomains(AwsChimeSdkVoiceListVoiceProfileDomainsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceProfileDomainsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListVoiceProfiles(AwsChimeSdkVoiceListVoiceProfilesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutSipMediaApplicationAlexaSkillConfiguration(AwsChimeSdkVoicePutSipMediaApplicationAlexaSkillConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutSipMediaApplicationLoggingConfiguration(AwsChimeSdkVoicePutSipMediaApplicationLoggingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoicePutVoiceConnectorEmergencyCallingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorLoggingConfiguration(AwsChimeSdkVoicePutVoiceConnectorLoggingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorOrigination(AwsChimeSdkVoicePutVoiceConnectorOriginationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorProxy(AwsChimeSdkVoicePutVoiceConnectorProxyOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorStreamingConfiguration(AwsChimeSdkVoicePutVoiceConnectorStreamingConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorTermination(AwsChimeSdkVoicePutVoiceConnectorTerminationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutVoiceConnectorTerminationCredentials(AwsChimeSdkVoicePutVoiceConnectorTerminationCredentialsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RestorePhoneNumber(AwsChimeSdkVoiceRestorePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchAvailablePhoneNumbers(AwsChimeSdkVoiceSearchAvailablePhoneNumbersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceSearchAvailablePhoneNumbersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartSpeakerSearchTask(AwsChimeSdkVoiceStartSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartVoiceToneAnalysisTask(AwsChimeSdkVoiceStartVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopSpeakerSearchTask(AwsChimeSdkVoiceStopSpeakerSearchTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StopVoiceToneAnalysisTask(AwsChimeSdkVoiceStopVoiceToneAnalysisTaskOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkVoiceTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkVoiceUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateGlobalSettings(AwsChimeSdkVoiceUpdateGlobalSettingsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceUpdateGlobalSettingsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePhoneNumber(AwsChimeSdkVoiceUpdatePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePhoneNumberSettings(AwsChimeSdkVoiceUpdatePhoneNumberSettingsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProxySession(AwsChimeSdkVoiceUpdateProxySessionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSipMediaApplication(AwsChimeSdkVoiceUpdateSipMediaApplicationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSipMediaApplicationCall(AwsChimeSdkVoiceUpdateSipMediaApplicationCallOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSipRule(AwsChimeSdkVoiceUpdateSipRuleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVoiceConnector(AwsChimeSdkVoiceUpdateVoiceConnectorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVoiceConnectorGroup(AwsChimeSdkVoiceUpdateVoiceConnectorGroupOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVoiceProfile(AwsChimeSdkVoiceUpdateVoiceProfileOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateVoiceProfileDomain(AwsChimeSdkVoiceUpdateVoiceProfileDomainOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ValidateE911Address(AwsChimeSdkVoiceValidateE911AddressOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}