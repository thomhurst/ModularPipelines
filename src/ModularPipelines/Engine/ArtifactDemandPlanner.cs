namespace ModularPipelines.Engine;

internal static class ArtifactDemandPlanner
{
    public static async Task<HashSet<Type>> ResolveAsync(
        Func<IReadOnlySet<Type>, Task<HashSet<Type>>> getNextState)
    {
        var forcedTypes = new HashSet<Type>();
        while (true)
        {
            var attempt = await TryResolveAsync(forcedTypes, getNextState).ConfigureAwait(false);
            if (attempt.Converged)
            {
                return await MinimizeForcedTypesAsync(
                        forcedTypes,
                        attempt.State,
                        getNextState)
                    .ConfigureAwait(false);
            }

            var cycleBreaker = attempt.CycleStates
                .SelectMany(static state => state)
                .Where(type => !forcedTypes.Contains(type))
                .OrderBy(type => type.AssemblyQualifiedName, StringComparer.Ordinal)
                .FirstOrDefault();
            if (cycleBreaker is null)
            {
                return attempt.State;
            }

            forcedTypes.Add(cycleBreaker);
        }
    }

    private static async Task<HashSet<Type>> MinimizeForcedTypesAsync(
        HashSet<Type> forcedTypes,
        HashSet<Type> resolvedState,
        Func<IReadOnlySet<Type>, Task<HashSet<Type>>> getNextState)
    {
        var changed = true;
        while (changed)
        {
            changed = false;
            foreach (var forcedType in forcedTypes
                         .OrderBy(type => type.AssemblyQualifiedName, StringComparer.Ordinal)
                         .ToArray())
            {
                var trialForcedTypes = new HashSet<Type>(forcedTypes);
                trialForcedTypes.Remove(forcedType);
                var attempt = await TryResolveAsync(trialForcedTypes, getNextState)
                    .ConfigureAwait(false);
                if (!attempt.Converged)
                {
                    continue;
                }

                forcedTypes = trialForcedTypes;
                resolvedState = attempt.State;
                changed = true;
                break;
            }
        }

        return resolvedState;
    }

    private static async Task<ResolutionAttempt> TryResolveAsync(
        IReadOnlySet<Type> forcedTypes,
        Func<IReadOnlySet<Type>, Task<HashSet<Type>>> getNextState)
    {
        var currentState = new HashSet<Type>(forcedTypes);
        var visitedStates = new List<HashSet<Type>>();
        while (true)
        {
            var cycleStart = visitedStates.FindIndex(state => state.SetEquals(currentState));
            if (cycleStart >= 0)
            {
                return new ResolutionAttempt(
                    Converged: false,
                    currentState,
                    visitedStates.Skip(cycleStart).ToArray());
            }

            visitedStates.Add(new HashSet<Type>(currentState));
            var nextState = await getNextState(currentState).ConfigureAwait(false);
            nextState.UnionWith(forcedTypes);
            if (currentState.SetEquals(nextState))
            {
                return new ResolutionAttempt(Converged: true, currentState, []);
            }

            currentState = nextState;
        }
    }

    private sealed record ResolutionAttempt(
        bool Converged,
        HashSet<Type> State,
        IReadOnlyList<HashSet<Type>> CycleStates);
}
