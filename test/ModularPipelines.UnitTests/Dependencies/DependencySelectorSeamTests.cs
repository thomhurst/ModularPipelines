using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class DependencySelectorSeamTests
{
    private abstract class BuildModuleBase : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class BuildModuleA : BuildModuleBase;

    private sealed class BuildModuleB : BuildModuleBase;

    private sealed class UnrelatedModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [DependsOnAllModulesInheritingFrom(typeof(BuildModuleBase))]
    private sealed class NonGenericSelectorModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [DependsOnAllModulesInheritingFrom<BuildModuleBase>]
    private sealed class GenericSelectorModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [DependsOnAllModulesInheritingFrom<BuildModuleBase>]
    private interface IAfterBuilds;

    private sealed class InterfaceSelectorModule : SimpleTestModule<bool>, IAfterBuilds
    {
        protected override bool Result => true;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    private sealed class DependsOnUnrelatedAttribute : DependsOnBaseAttribute
    {
        public override bool ShouldDependOn(Type candidateModule, IDependencyContext context) =>
            candidateModule == typeof(UnrelatedModule);
    }

    [DependsOnUnrelated]
    private interface IAfterUnrelated;

    private sealed class InterfacePredicateModule : SimpleTestModule<bool>, IAfterUnrelated
    {
        protected override bool Result => true;
    }

    private static readonly Type[] Available =
    [
        typeof(BuildModuleA),
        typeof(BuildModuleB),
        typeof(UnrelatedModule),
        typeof(NonGenericSelectorModule),
        typeof(GenericSelectorModule),
        typeof(InterfaceSelectorModule),
        typeof(InterfacePredicateModule),
    ];

    private static Task<Type[]> SelectAsync(Type moduleType, IDependencyContext? context) =>
        Task.FromResult(ModuleDependencyResolver
            .GetSelectorDependencies(moduleType, Available, context)
            .Select(dependency => dependency.DependencyType)
            .ToArray());

    [Test]
    public async Task InheritanceSelectors_Select_Derived_Modules_With_And_Without_Metadata()
    {
        foreach (var moduleType in new[]
                 {
                     typeof(NonGenericSelectorModule),
                     typeof(GenericSelectorModule),
                     typeof(InterfaceSelectorModule),
                 })
        {
            var withContext = await SelectAsync(moduleType, new MockDependencyContext());
            var withoutContext = await SelectAsync(moduleType, null);

            await Assert.That(withContext).IsEquivalentTo([typeof(BuildModuleA), typeof(BuildModuleB)]);
            await Assert.That(withoutContext).IsEquivalentTo([typeof(BuildModuleA), typeof(BuildModuleB)]);
        }
    }

    [Test]
    public async Task DependsOnBaseAttribute_Can_Be_Applied_To_An_Interface()
    {
        var selected = await SelectAsync(typeof(InterfacePredicateModule), new MockDependencyContext());

        using (Assert.Multiple())
        {
            await Assert.That(selected).IsEquivalentTo([typeof(UnrelatedModule)]);
            await Assert.That(GetUsage(typeof(DependsOnBaseAttribute)).ValidOn.HasFlag(AttributeTargets.Interface))
                .IsTrue();
            await Assert.That(GetUsage(typeof(DependsOnModulesWithTagAttribute)).ValidOn.HasFlag(AttributeTargets.Interface))
                .IsTrue();
            await Assert.That(GetUsage(typeof(DependsOnModulesInCategoryAttribute)).ValidOn.HasFlag(AttributeTargets.Interface))
                .IsTrue();
        }
    }

    [Test]
    public async Task Dependency_And_Secret_Attributes_Are_Sealed()
    {
        using (Assert.Multiple())
        {
            await Assert.That(typeof(DependsOnAttribute).IsSealed).IsTrue();
            await Assert.That(typeof(DependsOnAttribute<>).IsSealed).IsTrue();
            await Assert.That(typeof(DependsOnAttribute).IsAssignableFrom(typeof(DependsOnAttribute<UnrelatedModule>)))
                .IsFalse();
            await Assert.That(typeof(DependsOnAllModulesInheritingFromAttribute).IsSealed).IsTrue();
            await Assert.That(typeof(DependsOnAllModulesInheritingFromAttribute<>).IsSealed).IsTrue();
            await Assert.That(typeof(SecretValueAttribute).IsSealed).IsTrue();
        }
    }

    [Test]
    public async Task Built_In_Selectors_Are_Planning_Safe()
    {
        using (Assert.Multiple())
        {
            await Assert.That(typeof(IPlanningSafe).IsAssignableFrom(typeof(DependsOnAllModulesInheritingFromAttribute)))
                .IsTrue();
            await Assert.That(typeof(IPlanningSafe).IsAssignableFrom(typeof(DependsOnAllModulesInheritingFromAttribute<UnrelatedModule>)))
                .IsTrue();
            await Assert.That(typeof(DependsOnBaseAttribute).IsAssignableFrom(typeof(DependsOnAllModulesInheritingFromAttribute)))
                .IsTrue();
        }
    }

    [Test]
    public async Task DependsOn_Generic_And_NonGeneric_Declare_The_Same_Dependency()
    {
        var generic = new DependsOnAttribute<UnrelatedModule> { Optional = true };
        var nonGeneric = new DependsOnAttribute(typeof(UnrelatedModule)) { Optional = true };

        using (Assert.Multiple())
        {
            await Assert.That(generic.Type).IsEqualTo(nonGeneric.Type);
            await Assert.That(generic.Optional).IsEqualTo(nonGeneric.Optional);
            await Assert.That(() => new DependsOnAttribute(null!)).Throws<ArgumentNullException>();
        }
    }

    private static AttributeUsageAttribute GetUsage(Type attributeType) =>
        (AttributeUsageAttribute) Attribute.GetCustomAttribute(attributeType, typeof(AttributeUsageAttribute))!;
}
