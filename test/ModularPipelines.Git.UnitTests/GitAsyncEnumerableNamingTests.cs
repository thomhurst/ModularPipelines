using System.Reflection;

namespace ModularPipelines.Git.UnitTests;

public class GitAsyncEnumerableNamingTests
{
    [Test]
    public async Task PublicMethodsReturningAsyncEnumerables_UseAsyncSuffix()
    {
        var violations = typeof(IGit).Assembly
            .GetExportedTypes()
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(method => !method.IsSpecialName)
            .Where(method => ReturnsAsyncEnumerable(method.ReturnType))
            .Where(method => !method.Name.EndsWith("Async", StringComparison.Ordinal))
            .Select(method => $"{method.DeclaringType?.FullName}.{method.Name}")
            .Distinct()
            .Order()
            .ToArray();

        await Assert.That(violations).IsEmpty();
    }

    [Test]
    public async Task GitInformation_Exposes_CommitsAsync()
    {
        var methods = typeof(IGitInformation).GetMethods().Select(method => method.Name).ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(methods).Contains("CommitsAsync");
            await Assert.That(methods).DoesNotContain("Commits");
        }
    }

    // Covers concrete types and derived interfaces as well as IAsyncEnumerable<T> itself.
    private static bool ReturnsAsyncEnumerable(Type returnType) =>
        IsAsyncEnumerable(returnType) || returnType.GetInterfaces().Any(IsAsyncEnumerable);

    private static bool IsAsyncEnumerable(Type type) =>
        type.IsGenericType
        && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>);
}
