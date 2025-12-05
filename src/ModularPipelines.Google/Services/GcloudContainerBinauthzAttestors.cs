using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz")]
public class GcloudContainerBinauthzAttestors
{
    public GcloudContainerBinauthzAttestors(
        GcloudContainerBinauthzAttestorsPublicKeys publicKeys,
        ICommand internalCommand
    )
    {
        PublicKeys = publicKeys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerBinauthzAttestorsPublicKeys PublicKeys { get; }

    public async Task<CommandResult> AddIamPolicyBinding(GcloudContainerBinauthzAttestorsAddIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudContainerBinauthzAttestorsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudContainerBinauthzAttestorsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerBinauthzAttestorsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIamPolicy(GcloudContainerBinauthzAttestorsGetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudContainerBinauthzAttestorsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerBinauthzAttestorsListOptions(), token);
    }

    public async Task<CommandResult> RemoveIamPolicyBinding(GcloudContainerBinauthzAttestorsRemoveIamPolicyBindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIamPolicy(GcloudContainerBinauthzAttestorsSetIamPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudContainerBinauthzAttestorsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}