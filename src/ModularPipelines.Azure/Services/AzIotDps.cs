using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotDps
{
    public AzIotDps(
        AzIotDpsCertificate certificate,
        AzIotDpsConnectionString connectionString,
        AzIotDpsEnrollment enrollment,
        AzIotDpsEnrollmentGroup enrollmentGroup,
        AzIotDpsLinkedHub linkedHub,
        AzIotDpsPolicy policy,
        AzIotDpsRegistration registration,
        ICommand internalCommand
    )
    {
        Certificate = certificate;
        ConnectionString = connectionString;
        Enrollment = enrollment;
        EnrollmentGroup = enrollmentGroup;
        LinkedHub = linkedHub;
        Policy = policy;
        Registration = registration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDpsCertificate Certificate { get; }

    public AzIotDpsConnectionString ConnectionString { get; }

    public AzIotDpsEnrollment Enrollment { get; }

    public AzIotDpsEnrollmentGroup EnrollmentGroup { get; }

    public AzIotDpsLinkedHub LinkedHub { get; }

    public AzIotDpsPolicy Policy { get; }

    public AzIotDpsRegistration Registration { get; }

    public async Task<CommandResult> ComputeDeviceKey(AzIotDpsComputeDeviceKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzIotDpsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotDpsDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzIotDpsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsListOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotDpsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzIotDpsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsUpdateOptions(), token);
    }
}

