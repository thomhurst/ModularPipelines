using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image")]
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

    public async Task<CommandResult> Cancel(AzImageBuilderCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzImageBuilderCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzImageBuilderDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzImageBuilderListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderListOptions(), token);
    }

    public async Task<CommandResult> Run(AzImageBuilderRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderRunOptions(), token);
    }

    public async Task<CommandResult> Show(AzImageBuilderShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderShowOptions(), token);
    }

    public async Task<CommandResult> ShowRuns(AzImageBuilderShowRunsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderShowRunsOptions(), token);
    }

    public async Task<CommandResult> Update(AzImageBuilderUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzImageBuilderWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzImageBuilderWaitOptions(), token);
    }
}

