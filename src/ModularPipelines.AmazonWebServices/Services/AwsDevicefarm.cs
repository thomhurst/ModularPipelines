using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsDevicefarm
{
    public AwsDevicefarm(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateDevicePool(AwsDevicefarmCreateDevicePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateInstanceProfile(AwsDevicefarmCreateInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateNetworkProfile(AwsDevicefarmCreateNetworkProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProject(AwsDevicefarmCreateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRemoteAccessSession(AwsDevicefarmCreateRemoteAccessSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTestGridProject(AwsDevicefarmCreateTestGridProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTestGridUrl(AwsDevicefarmCreateTestGridUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateUpload(AwsDevicefarmCreateUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVpceConfiguration(AwsDevicefarmCreateVpceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDevicePool(AwsDevicefarmDeleteDevicePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInstanceProfile(AwsDevicefarmDeleteInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteNetworkProfile(AwsDevicefarmDeleteNetworkProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProject(AwsDevicefarmDeleteProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRemoteAccessSession(AwsDevicefarmDeleteRemoteAccessSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRun(AwsDevicefarmDeleteRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTestGridProject(AwsDevicefarmDeleteTestGridProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteUpload(AwsDevicefarmDeleteUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVpceConfiguration(AwsDevicefarmDeleteVpceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountSettings(AwsDevicefarmGetAccountSettingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmGetAccountSettingsOptions(), token);
    }

    public async Task<CommandResult> GetDevice(AwsDevicefarmGetDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDeviceInstance(AwsDevicefarmGetDeviceInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevicePool(AwsDevicefarmGetDevicePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDevicePoolCompatibility(AwsDevicefarmGetDevicePoolCompatibilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetInstanceProfile(AwsDevicefarmGetInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetJob(AwsDevicefarmGetJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetNetworkProfile(AwsDevicefarmGetNetworkProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetOfferingStatus(AwsDevicefarmGetOfferingStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmGetOfferingStatusOptions(), token);
    }

    public async Task<CommandResult> GetProject(AwsDevicefarmGetProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRemoteAccessSession(AwsDevicefarmGetRemoteAccessSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRun(AwsDevicefarmGetRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSuite(AwsDevicefarmGetSuiteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTest(AwsDevicefarmGetTestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTestGridProject(AwsDevicefarmGetTestGridProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTestGridSession(AwsDevicefarmGetTestGridSessionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmGetTestGridSessionOptions(), token);
    }

    public async Task<CommandResult> GetUpload(AwsDevicefarmGetUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVpceConfiguration(AwsDevicefarmGetVpceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InstallToRemoteAccessSession(AwsDevicefarmInstallToRemoteAccessSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListArtifacts(AwsDevicefarmListArtifactsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDeviceInstances(AwsDevicefarmListDeviceInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListDeviceInstancesOptions(), token);
    }

    public async Task<CommandResult> ListDevicePools(AwsDevicefarmListDevicePoolsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDevices(AwsDevicefarmListDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListDevicesOptions(), token);
    }

    public async Task<CommandResult> ListInstanceProfiles(AwsDevicefarmListInstanceProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListInstanceProfilesOptions(), token);
    }

    public async Task<CommandResult> ListJobs(AwsDevicefarmListJobsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListNetworkProfiles(AwsDevicefarmListNetworkProfilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOfferingPromotions(AwsDevicefarmListOfferingPromotionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListOfferingPromotionsOptions(), token);
    }

    public async Task<CommandResult> ListOfferingTransactions(AwsDevicefarmListOfferingTransactionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListOfferingTransactionsOptions(), token);
    }

    public async Task<CommandResult> ListOfferings(AwsDevicefarmListOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListOfferingsOptions(), token);
    }

    public async Task<CommandResult> ListProjects(AwsDevicefarmListProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListProjectsOptions(), token);
    }

    public async Task<CommandResult> ListRemoteAccessSessions(AwsDevicefarmListRemoteAccessSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRuns(AwsDevicefarmListRunsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSamples(AwsDevicefarmListSamplesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSuites(AwsDevicefarmListSuitesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsDevicefarmListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTestGridProjects(AwsDevicefarmListTestGridProjectsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListTestGridProjectsOptions(), token);
    }

    public async Task<CommandResult> ListTestGridSessionActions(AwsDevicefarmListTestGridSessionActionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTestGridSessionArtifacts(AwsDevicefarmListTestGridSessionArtifactsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTestGridSessions(AwsDevicefarmListTestGridSessionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTests(AwsDevicefarmListTestsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUniqueProblems(AwsDevicefarmListUniqueProblemsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUploads(AwsDevicefarmListUploadsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVpceConfigurations(AwsDevicefarmListVpceConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDevicefarmListVpceConfigurationsOptions(), token);
    }

    public async Task<CommandResult> PurchaseOffering(AwsDevicefarmPurchaseOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RenewOffering(AwsDevicefarmRenewOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScheduleRun(AwsDevicefarmScheduleRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopJob(AwsDevicefarmStopJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopRemoteAccessSession(AwsDevicefarmStopRemoteAccessSessionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopRun(AwsDevicefarmStopRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsDevicefarmTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsDevicefarmUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDeviceInstance(AwsDevicefarmUpdateDeviceInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDevicePool(AwsDevicefarmUpdateDevicePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInstanceProfile(AwsDevicefarmUpdateInstanceProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateNetworkProfile(AwsDevicefarmUpdateNetworkProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProject(AwsDevicefarmUpdateProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTestGridProject(AwsDevicefarmUpdateTestGridProjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateUpload(AwsDevicefarmUpdateUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVpceConfiguration(AwsDevicefarmUpdateVpceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}