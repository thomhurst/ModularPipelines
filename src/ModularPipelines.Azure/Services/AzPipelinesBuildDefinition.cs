using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "build")]
public class AzPipelinesBuildDefinition
{
    public AzPipelinesBuildDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzPipelinesBuildDefinitionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesBuildDefinitionListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPipelinesBuildDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPipelinesBuildDefinitionShowOptions(), token);
    }
}