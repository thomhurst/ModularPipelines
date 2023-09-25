namespace ModularPipelines;

internal static class TestDetector
{
    public static readonly bool IsRunningFromNUnit =
        AppDomain.CurrentDomain.GetAssemblies().Any(
            a => a.FullName?.ToLowerInvariant().StartsWith("nunit.framework") == true);
}