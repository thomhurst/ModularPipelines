using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSelfHelp
{
    public AzSelfHelp(
        AzSelfHelpDiagnostic diagnostic,
        AzSelfHelpDiscoverySolution discoverySolution,
        AzSelfHelpSolution solution,
        AzSelfHelpTroubleshooter troubleshooter,
        ICommand internalCommand
    )
    {
        Diagnostic = diagnostic;
        DiscoverySolution = discoverySolution;
        Solution = solution;
        Troubleshooter = troubleshooter;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSelfHelpDiagnostic Diagnostic { get; }

    public AzSelfHelpDiscoverySolution DiscoverySolution { get; }

    public AzSelfHelpSolution Solution { get; }

    public AzSelfHelpTroubleshooter Troubleshooter { get; }

    public async Task<CommandResult> CheckNameAvailability(AzSelfHelpCheckNameAvailabilityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}