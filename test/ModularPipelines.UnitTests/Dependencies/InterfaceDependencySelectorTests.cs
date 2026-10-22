using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class InterfaceDependencySelectorTests
{
    private interface IPublishStep : IModule;

    private interface IDerivedPublishStep : IPublishStep;

    private interface IGenericStep<T> : IModule;

    private interface IIntStep : IGenericStep<int>;

    private interface IUnrelatedStep : IModule;

    private interface IMarker;

    private interface IGenericMarker<T>;

    private class PublishModule : TrueModule, IPublishStep;

    private sealed class DerivedInterfaceModule : TrueModule, IDerivedPublishStep;

    private sealed class InheritedImplementationModule : PublishModule;

    private sealed class IntStepModule : TrueModule, IGenericStep<int>;

    private sealed class StringStepModule : TrueModule, IGenericStep<string>;

    private sealed class InheritedGenericInterfaceModule : TrueModule, IIntStep;

    private sealed class UnrelatedModule : TrueModule, IUnrelatedStep;

    [DependsOnAllModulesInheritingFrom<IPublishStep>]
    private sealed class GenericInterfaceSelector : TrueModule, IPublishStep;

    [DependsOnAllModulesInheritingFrom(typeof(IPublishStep))]
    private sealed class NonGenericInterfaceSelector : TrueModule, IPublishStep;

    [DependsOnAllModulesInheritingFrom<IGenericStep<int>>]
    private sealed class GenericClosedInterfaceSelector : TrueModule, IGenericStep<int>;

    [DependsOnAllModulesInheritingFrom(typeof(IGenericStep<int>))]
    private sealed class NonGenericClosedInterfaceSelector : TrueModule, IGenericStep<int>;

    [DependsOnAllModulesInheritingFrom(typeof(IGenericStep<>))]
    private sealed class OpenInterfaceSelector : TrueModule, IGenericStep<int>;

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Interface_Selectors_Match_Direct_Inherited_And_Derived_Implementations(bool useGenericAttribute)
    {
        var selectorModule = useGenericAttribute ? typeof(GenericInterfaceSelector) : typeof(NonGenericInterfaceSelector);
        await VerifySelectorAsync(selectorModule,
            [typeof(PublishModule), typeof(DerivedInterfaceModule), typeof(InheritedImplementationModule)]);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Closed_Interface_Selectors_Only_Match_The_Requested_Construction(bool useGenericAttribute)
    {
        var selectorModule = useGenericAttribute ? typeof(GenericClosedInterfaceSelector) : typeof(NonGenericClosedInterfaceSelector);
        await VerifySelectorAsync(selectorModule, [typeof(IntStepModule), typeof(InheritedGenericInterfaceModule)]);
    }

    [Test]
    public async Task Open_Interface_Selector_Matches_All_Registered_Constructions()
    {
        await VerifySelectorAsync(typeof(OpenInterfaceSelector), [typeof(IntStepModule), typeof(StringStepModule), typeof(InheritedGenericInterfaceModule)]);
    }

    [Test]
    public async Task Non_Module_Selectors_Are_Rejected()
    {
        foreach (var selectorType in new[] { typeof(IMarker), typeof(IGenericMarker<>), typeof(string) })
        {
            await Assert.That(() => new DependsOnAllModulesInheritingFromAttribute(selectorType))
                .Throws<InvalidModuleTypeException>();
        }
    }

    private static async Task VerifySelectorAsync(Type selectorModule, Type[] expectedDependencies)
    {
        Type[] registeredModules =
        [
            typeof(PublishModule),
            typeof(DerivedInterfaceModule),
            typeof(InheritedImplementationModule),
            typeof(IntStepModule),
            typeof(StringStepModule),
            typeof(InheritedGenericInterfaceModule),
            typeof(UnrelatedModule),
            selectorModule,
        ];
        var selector = (DependsOnBaseAttribute) Attribute.GetCustomAttribute(selectorModule, typeof(DependsOnBaseAttribute))!;

        foreach (var candidate in registeredModules)
        {
            await Assert.That(selector.ShouldDependOn(candidate, new MockDependencyContext()))
                .IsEqualTo(candidate == selectorModule || expectedDependencies.Contains(candidate));
        }

        // Exercise both planning without metadata and runtime dependency resolution.
        foreach (IDependencyContext? context in new IDependencyContext?[] { null, new MockDependencyContext() })
        {
            var dependencies = ModuleDependencyResolver.GetSelectorDependencies(selectorModule, registeredModules, context)
                .ToArray();

            await Assert.That(dependencies.Select(dependency => dependency.DependencyType).ToArray())
                .IsEquivalentTo(expectedDependencies);
            await Assert.That(dependencies.All(dependency => !dependency.Optional)).IsTrue();
            await Assert.That(ModuleDependencyResolver.GetSelectorDependencies(selectorModule, [], context).ToArray())
                .IsEmpty();
        }
    }
}
