using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Services;

[ExcludeFromCodeCoverage]
public class DockerTrust
{
    public DockerTrust(
        DockerTrustSigner trustSigner,
        DockerTrustKey trustKey,
        ICommand internalCommand
    )
    {
        TrustSigner = trustSigner;
        TrustKey = trustKey;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public DockerTrustSigner TrustSigner { get; }

    public DockerTrustKey TrustKey { get; }

    public virtual async Task<CommandResult> Inspect(DockerTrustInspectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustInspectOptions(), token);
    }

    public virtual async Task<CommandResult> Key(DockerTrustKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustKeyOptions(), token);
    }

    public virtual async Task<CommandResult> Revoke(DockerTrustRevokeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Sign(DockerTrustSignOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustSignOptions(), token);
    }

    public virtual async Task<CommandResult> Signer(DockerTrustSignerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustSignerOptions(), token);
    }
}
