using System.Reflection;
using ModularPipelines.Configuration;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Dependencies;

public class DependencyApiSurfaceTests
{
    [Test]
    public async Task Only_Configuration_And_Attribute_Dependency_APIs_Are_Exposed()
    {
        var assembly = typeof(Module<>).Assembly;
        var builderMethods = typeof(ModuleConfigurationBuilder).GetMethods();
        var lazyMethods = builderMethods
            .Where(static method => method.Name == "DependsOnLazy")
            .ToArray();
        var predicateOverloads = builderMethods
            .Where(static method => method.Name == "DependsOnIf")
            .Where(static method => method.GetParameters()
                .Any(static parameter => parameter.ParameterType == typeof(Func<bool>)))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(assembly.GetType("ModularPipelines.Modules.IDependencyDeclaration"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Modules.DependencyDeclaration"))
                .IsNull();
            await Assert.That(typeof(Module<>).GetMethod(
                    "DeclareDependencies",
                    BindingFlags.Instance | BindingFlags.NonPublic))
                .IsNull();
            await Assert.That(lazyMethods).IsEmpty();
            await Assert.That(predicateOverloads).IsEmpty();
            await Assert.That(Enum.GetNames<DependencyType>()).DoesNotContain("Lazy");
            await Assert.That(Enum.GetNames<DependencyType>()).DoesNotContain("Conditional");
            await Assert.That(typeof(DeclaredDependency).GetMethod("Lazy")).IsNull();
            await Assert.That(typeof(DeclaredDependency).GetMethod("Conditional")).IsNull();
            await Assert.That(typeof(DependencyType).IsPublic).IsFalse();
            await Assert.That(typeof(DeclaredDependency).IsPublic).IsFalse();
        }
    }
}
