using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("image")]
public class AzImageBuilder
{
    public AzImageBuilder(
        AzImageBuilderCustomizer customizer,
        AzImageBuilderIdentity identity,
        AzImageBuilderOptimizer optimizer,
        AzImageBuilderOutput output,
        AzImageBuilderTrigger trigger,
        AzImageBuilderValidator validator,
        ICommand internalCommand
    )
    {
        Customizer = customizer;
        Identity = identity;
        Optimizer = optimizer;
        Output = output;
        Trigger = trigger;
        Validator = validator;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzImageBuilderCustomizer Customizer { get; }

    public AzImageBuilderIdentity Identity { get; }

    public AzImageBuilderOptimizer Optimizer { get; }

    public AzImageBuilderOutput Output { get; }

    public AzImageBuilderTrigger Trigger { get; }

    public AzImageBuilderValidator Validator { get; }

    public async Task<CommandResult> Cancel(AzImageBuilderCancelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderCancelOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzImageBuilderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzImageBuilderDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzImageBuilderListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Run(AzImageBuilderRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderRunOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzImageBuilderShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowRuns(AzImageBuilderShowRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderShowRunsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzImageBuilderUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzImageBuilderWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderWaitOptions(), cancellationToken: token);
    }
}