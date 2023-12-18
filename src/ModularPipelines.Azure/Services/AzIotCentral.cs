using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotCentral
{
    public AzIotCentral(
        AzIotCentralApiToken apiToken,
        AzIotCentralApp app,
        AzIotCentralDevice device,
        AzIotCentralDeviceGroup deviceGroup,
        AzIotCentralDeviceTemplate deviceTemplate,
        AzIotCentralDiagnostics diagnostics,
        AzIotCentralEnrollmentGroup enrollmentGroup,
        AzIotCentralExport export,
        AzIotCentralFileUploadConfig fileUploadConfig,
        AzIotCentralJob job,
        AzIotCentralOrganization organization,
        AzIotCentralRole role,
        AzIotCentralScheduledJob scheduledJob,
        AzIotCentralUser user,
        ICommand internalCommand
    )
    {
        ApiToken = apiToken;
        App = app;
        Device = device;
        DeviceGroup = deviceGroup;
        DeviceTemplate = deviceTemplate;
        Diagnostics = diagnostics;
        EnrollmentGroup = enrollmentGroup;
        Export = export;
        FileUploadConfig = fileUploadConfig;
        Job = job;
        Organization = organization;
        Role = role;
        ScheduledJob = scheduledJob;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotCentralApiToken ApiToken { get; }

    public AzIotCentralApp App { get; }

    public AzIotCentralDevice Device { get; }

    public AzIotCentralDeviceGroup DeviceGroup { get; }

    public AzIotCentralDeviceTemplate DeviceTemplate { get; }

    public AzIotCentralDiagnostics Diagnostics { get; }

    public AzIotCentralEnrollmentGroup EnrollmentGroup { get; }

    public AzIotCentralExport Export { get; }

    public AzIotCentralFileUploadConfig FileUploadConfig { get; }

    public AzIotCentralJob Job { get; }

    public AzIotCentralOrganization Organization { get; }

    public AzIotCentralRole Role { get; }

    public AzIotCentralScheduledJob ScheduledJob { get; }

    public AzIotCentralUser User { get; }

    public async Task<CommandResult> Query(AzIotCentralQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}