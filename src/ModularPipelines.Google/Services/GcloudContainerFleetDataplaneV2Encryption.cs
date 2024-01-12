using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetDataplaneV2Encryption
{
    public GcloudContainerFleetDataplaneV2Encryption(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Describe(GcloudContainerFleetDataplaneV2EncryptionDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetDataplaneV2EncryptionDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetDataplaneV2EncryptionDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetDataplaneV2EncryptionDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetDataplaneV2EncryptionEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetDataplaneV2EncryptionEnableOptions(), token);
    }
}