using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption")]
public class AzConsumptionBudget
{
    public AzConsumptionBudget(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConsumptionBudgetCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateWithRg(AzConsumptionBudgetCreateWithRgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetCreateWithRgOptions(), token);
    }

    public async Task<CommandResult> Delete(AzConsumptionBudgetDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteWithRg(AzConsumptionBudgetDeleteWithRgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetDeleteWithRgOptions(), token);
    }

    public async Task<CommandResult> List(AzConsumptionBudgetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConsumptionBudgetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowWithRg(AzConsumptionBudgetShowWithRgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetShowWithRgOptions(), token);
    }

    public async Task<CommandResult> Update(AzConsumptionBudgetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateWithRg(AzConsumptionBudgetUpdateWithRgOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConsumptionBudgetUpdateWithRgOptions(), token);
    }
}