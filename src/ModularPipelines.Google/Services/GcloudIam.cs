using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudIam
{
    public GcloudIam(
        GcloudIamPolicies policies,
        GcloudIamRoles roles,
        GcloudIamServiceAccounts serviceAccounts,
        GcloudIamSimulator simulator,
        GcloudIamWorkforcePools workforcePools,
        GcloudIamWorkloadIdentityPools workloadIdentityPools,
        ICommand internalCommand
    )
    {
        Policies = policies;
        Roles = roles;
        ServiceAccounts = serviceAccounts;
        Simulator = simulator;
        WorkforcePools = workforcePools;
        WorkloadIdentityPools = workloadIdentityPools;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudIamPolicies Policies { get; }

    public GcloudIamRoles Roles { get; }

    public GcloudIamServiceAccounts ServiceAccounts { get; }

    public GcloudIamSimulator Simulator { get; }

    public GcloudIamWorkforcePools WorkforcePools { get; }

    public GcloudIamWorkloadIdentityPools WorkloadIdentityPools { get; }

    public async Task<CommandResult> ListGrantableRoles(GcloudIamListGrantableRolesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTestablePermissions(GcloudIamListTestablePermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}