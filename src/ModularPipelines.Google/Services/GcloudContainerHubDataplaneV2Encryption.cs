using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub")]
public class GcloudContainerHubDataplaneV2Encryption
{
    public GcloudContainerHubDataplaneV2Encryption(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerHubDataplaneV2EncryptionDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubDataplaneV2EncryptionDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerHubDataplaneV2EncryptionDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubDataplaneV2EncryptionDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubDataplaneV2EncryptionEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubDataplaneV2EncryptionEnableOptions(), token);
    }
}