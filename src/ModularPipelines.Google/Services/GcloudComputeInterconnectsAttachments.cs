using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects")]
public class GcloudComputeInterconnectsAttachments
{
    public GcloudComputeInterconnectsAttachments(
        GcloudComputeInterconnectsAttachmentsDedicated dedicated,
        GcloudComputeInterconnectsAttachmentsPartner partner,
        ICommand internalCommand
    )
    {
        Dedicated = dedicated;
        Partner = partner;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeInterconnectsAttachmentsDedicated Dedicated { get; }

    public GcloudComputeInterconnectsAttachmentsPartner Partner { get; }

    public async Task<CommandResult> Delete(GcloudComputeInterconnectsAttachmentsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeInterconnectsAttachmentsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeInterconnectsAttachmentsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeInterconnectsAttachmentsListOptions(), token);
    }
}