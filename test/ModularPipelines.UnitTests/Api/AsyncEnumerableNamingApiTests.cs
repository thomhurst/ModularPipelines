using System.Reflection;

namespace ModularPipelines.UnitTests.Api;

public class AsyncEnumerableNamingApiTests
{
    [Test]
    public async Task PublicMethodsReturningAsyncEnumerables_UseAsyncSuffix()
    {
        var violations = typeof(IModuleContext).Assembly
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

    // Covers concrete types and derived interfaces as well as IAsyncEnumerable<T> itself.
    private static bool ReturnsAsyncEnumerable(Type returnType) =>
        IsAsyncEnumerable(returnType) || returnType.GetInterfaces().Any(IsAsyncEnumerable);

    private static bool IsAsyncEnumerable(Type type) =>
        type.IsGenericType
        && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>);
}
