using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal sealed class ModulePlanningSkipEvaluator(
    IServiceProvider serviceProvider,
    IModuleConditionHandler moduleConditionHandler,
    IMediator mediator,
    ISafeModuleEstimatedTimeProvider estimatedTimeProvider)
{
    public async Task<SkipDecision?> EvaluateAsync(
        IModule module,
        CancellationToken cancellationToken)
    {
        var (shouldIgnore, attributeDecision) = await moduleConditionHandler
            .ShouldIgnoreForPlanning(module, cancellationToken)
            .ConfigureAwait(false);
        if (shouldIgnore)
        {
            return attributeDecision ?? SkipDecision.Skip("Module was ignored");
        }

        var planningSkipCondition = module.Configuration.PlanningSkipCondition;
        if (planningSkipCondition is null)
        {
            return null;
        }

        return await EvaluateConditionAsync(module, planningSkipCondition, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<SkipDecision?> EvaluateGraphSafeAsync(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken)
    {
        var conditionResult = await moduleConditionHandler
            .ShouldIgnoreForGraphPlanning(module, metadataRegistry, cancellationToken)
            .ConfigureAwait(false);
        if (conditionResult.ShouldIgnore)
        {
            return conditionResult.SkipDecision ?? SkipDecision.Skip("Module was ignored");
        }

        var planningSkipCondition = module.Configuration.SynchronousPlanningSkipCondition;
        if (planningSkipCondition is null)
        {
            return null;
        }

        var skipDecision = await EvaluateConditionAsync(module, planningSkipCondition, cancellationToken)
            .ConfigureAwait(false);
        return conditionResult.IsResolved || skipDecision?.ShouldSkip == true
            ? skipDecision
            : null;
    }

    private async Task<SkipDecision?> EvaluateConditionAsync(
        IModule module,
        Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>> planningSkipCondition,
        CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var scopedServices = scope.ServiceProvider;
        var executionContext = ExecutionContextFactory.Create(module, module.GetType());
        try
        {
            var moduleContext = new ModuleContext(
                scopedServices.GetRequiredService<IPipelineContext>(),
                module,
                executionContext,
                scopedServices.GetRequiredService<IInternalModuleLoggerProvider>()
                    .GetLogger(module.GetType()),
                mediator,
                estimatedTimeProvider,
                moduleResultAccessAllowed: false);
            try
            {
                using var planningResultAccess = PlanningModuleResultAccess.Enter();
                return await planningSkipCondition(moduleContext, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (PlanningModuleResultUnavailableException)
            {
                return null;
            }
        }
        finally
        {
            executionContext.ModuleCancellationTokenSource.Dispose();
        }
    }
}
