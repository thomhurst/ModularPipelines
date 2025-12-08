using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("access-approval")]
public class GcloudAccessApprovalSettings
{
    public GcloudAccessApprovalSettings(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(GcloudAccessApprovalSettingsDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessApprovalSettingsDeleteOptions(), token);
    }

    public async Task<CommandResult> Get(GcloudAccessApprovalSettingsGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessApprovalSettingsGetOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudAccessApprovalSettingsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessApprovalSettingsUpdateOptions(), token);
    }
}