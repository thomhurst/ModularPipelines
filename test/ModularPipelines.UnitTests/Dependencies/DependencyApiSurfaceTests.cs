using ModularPipelines.Configuration;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Dependencies;

public class DependencyApiSurfaceTests
{
    [Test]
    public async Task Redundant_Dependency_APIs_Are_Not_Exposed()
    {
        var dependencyApiTypes = new[]
        {
            typeof(IDependencyDeclaration),
            typeof(DependencyDeclaration),
            typeof(ModuleConfigurationBuilder),
        };
        var lazyMethods = dependencyApiTypes
            .SelectMany(static type => type.GetMethods())
            .Where(static method => method.Name == "DependsOnLazy")
            .ToArray();
        var predicateOverloads = dependencyApiTypes
            .SelectMany(static type => type.GetMethods())
            .Where(static method => method.Name == "DependsOnIf")
            .Where(static method => method.GetParameters()
                .Any(static parameter => parameter.ParameterType == typeof(Func<bool>)))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(lazyMethods).IsEmpty();
            await Assert.That(predicateOverloads).IsEmpty();
            await Assert.That(Enum.GetNames<DependencyType>()).DoesNotContain("Lazy");
            await Assert.That(typeof(DeclaredDependency).GetMethod("Lazy")).IsNull();
        }
    }
}
