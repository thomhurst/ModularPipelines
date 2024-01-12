using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudPolicyIntelligence
{
    public GcloudPolicyIntelligence(
        GcloudPolicyIntelligenceTroubleshootPolicy troubleshootPolicy,
        ICommand internalCommand
    )
    {
        TroubleshootPolicy = troubleshootPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudPolicyIntelligenceTroubleshootPolicy TroubleshootPolicy { get; }

    public async Task<CommandResult> QueryActivity(GcloudPolicyIntelligenceQueryActivityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}