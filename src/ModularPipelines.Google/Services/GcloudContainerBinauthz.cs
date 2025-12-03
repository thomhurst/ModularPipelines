using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container")]
public class GcloudContainerBinauthz
{
    public GcloudContainerBinauthz(
        GcloudContainerBinauthzAttestations attestations,
        GcloudContainerBinauthzAttestors attestors,
        GcloudContainerBinauthzPolicy policy,
        ICommand internalCommand
    )
    {
        Attestations = attestations;
        Attestors = attestors;
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerBinauthzAttestations Attestations { get; }

    public GcloudContainerBinauthzAttestors Attestors { get; }

    public GcloudContainerBinauthzPolicy Policy { get; }

    public async Task<CommandResult> CreateSignaturePayload(GcloudContainerBinauthzCreateSignaturePayloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}