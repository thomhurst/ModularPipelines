using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociatePhoneNumbersWithVoiceConnector(AwsChimeSdkVoiceAssociatePhoneNumbersWithVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociatePhoneNumbersWithVoiceConnectorGroup(AwsChimeSdkVoiceAssociatePhoneNumbersWithVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDeletePhoneNumber(AwsChimeSdkVoiceBatchDeletePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdatePhoneNumber(AwsChimeSdkVoiceBatchUpdatePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePhoneNumberOrder(AwsChimeSdkVoiceCreatePhoneNumberOrderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProxySession(AwsChimeSdkVoiceCreateProxySessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSipMediaApplication(AwsChimeSdkVoiceCreateSipMediaApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSipMediaApplicationCall(AwsChimeSdkVoiceCreateSipMediaApplicationCallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSipRule(AwsChimeSdkVoiceCreateSipRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVoiceConnector(AwsChimeSdkVoiceCreateVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVoiceConnectorGroup(AwsChimeSdkVoiceCreateVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVoiceProfile(AwsChimeSdkVoiceCreateVoiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVoiceProfileDomain(AwsChimeSdkVoiceCreateVoiceProfileDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePhoneNumber(AwsChimeSdkVoiceDeletePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProxySession(AwsChimeSdkVoiceDeleteProxySessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSipMediaApplication(AwsChimeSdkVoiceDeleteSipMediaApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSipRule(AwsChimeSdkVoiceDeleteSipRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnector(AwsChimeSdkVoiceDeleteVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoiceDeleteVoiceConnectorEmergencyCallingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorGroup(AwsChimeSdkVoiceDeleteVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorOrigination(AwsChimeSdkVoiceDeleteVoiceConnectorOriginationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorProxy(AwsChimeSdkVoiceDeleteVoiceConnectorProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorStreamingConfiguration(AwsChimeSdkVoiceDeleteVoiceConnectorStreamingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorTermination(AwsChimeSdkVoiceDeleteVoiceConnectorTerminationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceConnectorTerminationCredentials(AwsChimeSdkVoiceDeleteVoiceConnectorTerminationCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceProfile(AwsChimeSdkVoiceDeleteVoiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceProfileDomain(AwsChimeSdkVoiceDeleteVoiceProfileDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociatePhoneNumbersFromVoiceConnector(AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociatePhoneNumbersFromVoiceConnectorGroup(AwsChimeSdkVoiceDisassociatePhoneNumbersFromVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGlobalSettings(AwsChimeSdkVoiceGetGlobalSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceGetGlobalSettingsOptions(), token);
    }

    public async Task<CommandResult> GetPhoneNumber(AwsChimeSdkVoiceGetPhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPhoneNumberOrder(AwsChimeSdkVoiceGetPhoneNumberOrderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPhoneNumberSettings(AwsChimeSdkVoiceGetPhoneNumberSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceGetPhoneNumberSettingsOptions(), token);
    }

    public async Task<CommandResult> GetProxySession(AwsChimeSdkVoiceGetProxySessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSipMediaApplication(AwsChimeSdkVoiceGetSipMediaApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSipMediaApplicationAlexaSkillConfiguration(AwsChimeSdkVoiceGetSipMediaApplicationAlexaSkillConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSipMediaApplicationLoggingConfiguration(AwsChimeSdkVoiceGetSipMediaApplicationLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSipRule(AwsChimeSdkVoiceGetSipRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSpeakerSearchTask(AwsChimeSdkVoiceGetSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnector(AwsChimeSdkVoiceGetVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorEmergencyCallingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorGroup(AwsChimeSdkVoiceGetVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorLoggingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorOrigination(AwsChimeSdkVoiceGetVoiceConnectorOriginationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorProxy(AwsChimeSdkVoiceGetVoiceConnectorProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorStreamingConfiguration(AwsChimeSdkVoiceGetVoiceConnectorStreamingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorTermination(AwsChimeSdkVoiceGetVoiceConnectorTerminationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceConnectorTerminationHealth(AwsChimeSdkVoiceGetVoiceConnectorTerminationHealthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceProfile(AwsChimeSdkVoiceGetVoiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceProfileDomain(AwsChimeSdkVoiceGetVoiceProfileDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVoiceToneAnalysisTask(AwsChimeSdkVoiceGetVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAvailableVoiceConnectorRegions(AwsChimeSdkVoiceListAvailableVoiceConnectorRegionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListAvailableVoiceConnectorRegionsOptions(), token);
    }

    public async Task<CommandResult> ListPhoneNumberOrders(AwsChimeSdkVoiceListPhoneNumberOrdersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListPhoneNumberOrdersOptions(), token);
    }

    public async Task<CommandResult> ListPhoneNumbers(AwsChimeSdkVoiceListPhoneNumbersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListPhoneNumbersOptions(), token);
    }

    public async Task<CommandResult> ListProxySessions(AwsChimeSdkVoiceListProxySessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSipMediaApplications(AwsChimeSdkVoiceListSipMediaApplicationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListSipMediaApplicationsOptions(), token);
    }

    public async Task<CommandResult> ListSipRules(AwsChimeSdkVoiceListSipRulesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListSipRulesOptions(), token);
    }

    public async Task<CommandResult> ListSupportedPhoneNumberCountries(AwsChimeSdkVoiceListSupportedPhoneNumberCountriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsChimeSdkVoiceListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVoiceConnectorGroups(AwsChimeSdkVoiceListVoiceConnectorGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceConnectorGroupsOptions(), token);
    }

    public async Task<CommandResult> ListVoiceConnectorTerminationCredentials(AwsChimeSdkVoiceListVoiceConnectorTerminationCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVoiceConnectors(AwsChimeSdkVoiceListVoiceConnectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceConnectorsOptions(), token);
    }

    public async Task<CommandResult> ListVoiceProfileDomains(AwsChimeSdkVoiceListVoiceProfileDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceListVoiceProfileDomainsOptions(), token);
    }

    public async Task<CommandResult> ListVoiceProfiles(AwsChimeSdkVoiceListVoiceProfilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSipMediaApplicationAlexaSkillConfiguration(AwsChimeSdkVoicePutSipMediaApplicationAlexaSkillConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSipMediaApplicationLoggingConfiguration(AwsChimeSdkVoicePutSipMediaApplicationLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorEmergencyCallingConfiguration(AwsChimeSdkVoicePutVoiceConnectorEmergencyCallingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorLoggingConfiguration(AwsChimeSdkVoicePutVoiceConnectorLoggingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorOrigination(AwsChimeSdkVoicePutVoiceConnectorOriginationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorProxy(AwsChimeSdkVoicePutVoiceConnectorProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorStreamingConfiguration(AwsChimeSdkVoicePutVoiceConnectorStreamingConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorTermination(AwsChimeSdkVoicePutVoiceConnectorTerminationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutVoiceConnectorTerminationCredentials(AwsChimeSdkVoicePutVoiceConnectorTerminationCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestorePhoneNumber(AwsChimeSdkVoiceRestorePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchAvailablePhoneNumbers(AwsChimeSdkVoiceSearchAvailablePhoneNumbersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceSearchAvailablePhoneNumbersOptions(), token);
    }

    public async Task<CommandResult> StartSpeakerSearchTask(AwsChimeSdkVoiceStartSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartVoiceToneAnalysisTask(AwsChimeSdkVoiceStartVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopSpeakerSearchTask(AwsChimeSdkVoiceStopSpeakerSearchTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopVoiceToneAnalysisTask(AwsChimeSdkVoiceStopVoiceToneAnalysisTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsChimeSdkVoiceTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsChimeSdkVoiceUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateGlobalSettings(AwsChimeSdkVoiceUpdateGlobalSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsChimeSdkVoiceUpdateGlobalSettingsOptions(), token);
    }

    public async Task<CommandResult> UpdatePhoneNumber(AwsChimeSdkVoiceUpdatePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePhoneNumberSettings(AwsChimeSdkVoiceUpdatePhoneNumberSettingsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProxySession(AwsChimeSdkVoiceUpdateProxySessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSipMediaApplication(AwsChimeSdkVoiceUpdateSipMediaApplicationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSipMediaApplicationCall(AwsChimeSdkVoiceUpdateSipMediaApplicationCallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSipRule(AwsChimeSdkVoiceUpdateSipRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVoiceConnector(AwsChimeSdkVoiceUpdateVoiceConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVoiceConnectorGroup(AwsChimeSdkVoiceUpdateVoiceConnectorGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVoiceProfile(AwsChimeSdkVoiceUpdateVoiceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVoiceProfileDomain(AwsChimeSdkVoiceUpdateVoiceProfileDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateE911Address(AwsChimeSdkVoiceValidateE911AddressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}