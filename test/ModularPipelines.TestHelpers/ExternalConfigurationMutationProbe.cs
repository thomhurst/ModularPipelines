namespace ModularPipelines.TestHelpers;

public sealed class ExternalConfigurationMutationProbe
{
    private static int _constructorCalls;

    public ExternalConfigurationMutationProbe() =>
        Interlocked.Increment(ref _constructorCalls);

    public static int ConstructorCalls => Volatile.Read(ref _constructorCalls);

    public static void Reset() => Volatile.Write(ref _constructorCalls, 0);
}
