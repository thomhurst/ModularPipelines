using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support")]
public class AzSupportServices
{
    public AzSupportServices(
        AzSupportServicesProblemClassifications problemClassifications,
        ICommand internalCommand
    )
    {
        ProblemClassifications = problemClassifications;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSupportServicesProblemClassifications ProblemClassifications { get; }

    public async Task<CommandResult> List(AzSupportServicesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSupportServicesListOptions(), token);
    }

    public async Task<CommandResult> Show(AzSupportServicesShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}