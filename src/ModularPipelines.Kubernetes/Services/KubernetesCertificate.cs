using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesCertificate(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> Approve(KubernetesCertificateApproveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deny(KubernetesCertificateDenyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}