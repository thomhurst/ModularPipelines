using System.Reflection;

namespace ModularPipelines.UnitTests.Api;

public class AsyncEnumerableNamingApiTests
{
    [Test]
    public async Task PublicMethodsReturningAsyncEnumerables_UseAsyncSuffix()
    {
        var violations = typeof(IModuleContext).Assembly
            .GetExportedTypes()

            // The Mediator source generator emits its own public Mediator.Mediator.CreateStream API.
            .Where(type => type.Namespace?.StartsWith("ModularPipelines", StringComparison.Ordinal) == true)
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

    private static bool ReturnsAsyncEnumerable(Type returnType) =>
        returnType.IsGenericType
        && returnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>);
}
