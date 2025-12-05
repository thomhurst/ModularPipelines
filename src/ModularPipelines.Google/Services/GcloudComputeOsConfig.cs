using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute")]
public class GcloudComputeOsConfig
{
    public GcloudComputeOsConfig(
        GcloudComputeOsConfigInventories inventories,
        GcloudComputeOsConfigOsPolicyAssignmentReports osPolicyAssignmentReports,
        GcloudComputeOsConfigOsPolicyAssignments osPolicyAssignments,
        GcloudComputeOsConfigPatchDeployments patchDeployments,
        GcloudComputeOsConfigPatchJobs patchJobs,
        GcloudComputeOsConfigVulnerabilityReports vulnerabilityReports,
        ICommand internalCommand
    )
    {
        Inventories = inventories;
        OsPolicyAssignmentReports = osPolicyAssignmentReports;
        OsPolicyAssignments = osPolicyAssignments;
        PatchDeployments = patchDeployments;
        PatchJobs = patchJobs;
        VulnerabilityReports = vulnerabilityReports;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeOsConfigInventories Inventories { get; }

    public GcloudComputeOsConfigOsPolicyAssignmentReports OsPolicyAssignmentReports { get; }

    public GcloudComputeOsConfigOsPolicyAssignments OsPolicyAssignments { get; }

    public GcloudComputeOsConfigPatchDeployments PatchDeployments { get; }

    public GcloudComputeOsConfigPatchJobs PatchJobs { get; }

    public GcloudComputeOsConfigVulnerabilityReports VulnerabilityReports { get; }

    public async Task<CommandResult> Troubleshoot(GcloudComputeOsConfigTroubleshootOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}