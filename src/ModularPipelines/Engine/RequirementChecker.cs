using System.Collections.Concurrent;
using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;

namespace ModularPipelines.Engine;

internal class RequirementChecker : IRequirementChecker
{
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker(IEnumerable<IPipelineRequirement> requirements, IPipelineContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
        _requirements = requirements.ToList();
    }

    public async Task CheckRequirementsAsync(CancellationToken cancellationToken)
    {
        var failures = new ConcurrentBag<RequirementFailure>();

        var groupedRequirements = _requirements
            .Select(static (requirement, index) => (Requirement: requirement, Index: index))
            .GroupBy(static entry => entry.Requirement.Order)
            .OrderBy(static group => group.Key);

        // Every requirement is evaluated, so all failures, including requirements that throw,
        // are reported together.
        foreach (var pipelineRequirements in groupedRequirements)
        {
            await pipelineRequirements.ToAsyncProcessorBuilder()
                .ForEachAsync(async entry =>
                {
                    var requirement = entry.Requirement;
                    try
                    {
                        var requirementDecision = await requirement
                            .EvaluateAsync(_moduleContextProvider.GetModuleContext(), cancellationToken)
                            .ConfigureAwait(false);

                        if (!requirementDecision.IsSatisfied)
                        {
                            failures.Add(new RequirementFailure(
                                entry.Index,
                                requirementDecision.Reason ?? requirement.GetType().Name,
                                Exception: null));
                        }
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        failures.Add(new RequirementFailure(
                            entry.Index,
                            $"{requirement.GetType().Name} threw {exception.GetType().Name}: {exception.Message}",
                            exception));
                    }
                })
                .ProcessInParallel();
        }

        if (!failures.IsEmpty)
        {
            var orderedFailures = failures.OrderBy(static failure => failure.Index).ToArray();
            throw new RequirementNotMetException(
                $"Requirements failed:{Environment.NewLine}"
                + string.Join(Environment.NewLine, orderedFailures.Select(static failure => failure.Reason)),
                orderedFailures
                    .Select(static failure => failure.Exception)
                    .OfType<Exception>());
        }
    }

    private readonly record struct RequirementFailure(int Index, string Reason, Exception? Exception);
}
