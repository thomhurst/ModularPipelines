using ModularPipelines.Attributes;
using ModularPipelines.Distributed;

namespace ModularPipelines.UnitTests.Attributes;

public class CapabilityConditionsTests
{
    [GroupedOperatingSystem<OnLinux>]
    [GroupedOperatingSystem<OnWindows>]
    private sealed class GroupedAlternativeModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [FirstGroupedOperatingSystem<OnLinux>]
    [SecondGroupedOperatingSystem<OnWindows>]
    private sealed class SharedDeclaredGroupModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [GroupedOperatingSystem<OnLinux>]
    [GroupedOperatingSystem<OnWindows>]
    [RunIf<OnMacOS>]
    private sealed class ContradictoryGroupedAlternativeModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class GroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute,
        IPlanningConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(GroupedOperatingSystemAttribute<>);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class FirstGroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute,
        IPlanningConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class SecondGroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute,
        IPlanningConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    [RequiresCapability(Capability.Names.Docker)]
    [RunIfAny<OnLinux, OnMacOS>]
    private sealed class DeclaredAndConditionalModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [RequiresCapability(Capability.Names.Linux, Capability.Names.Windows)]
    private sealed class ConflictingDeclaredModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [RequiresCapability(Capability.Names.Linux)]
    [RunIfAny<OnWindows, OnMacOS>]
    private sealed class DeclaredConflictsWithConditionModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class WorkerOnlyCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    [RunIfAny<OnLinux, OnCI>]
    private sealed class MixedAlternativeModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class LinuxOnCiGroup : ConditionGroup, IPlanningRunCondition
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnCI()];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    private sealed class OnGpu : ICapabilityCondition
    {
        public Capability Capability => Capability.Gpu;

        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    [RunIfAny<OnLinux, OnMacOS>]
    [RunIf<OnWindows>]
    private sealed class ContradictoryAlternativeModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [RunIf<OnUnix>]
    [RunIf<OnWindows>]
    private sealed class ContradictoryGroupedConditionModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [Test]
    public async Task Direct_Operating_System_Uses_Its_Capability()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<OnLinux>());

        await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));
    }

    [Test]
    public async Task Alternative_Operating_System_Group_Matches_Either_Worker()
    {
        var requirement = CapabilityConditions.GetRoute(new RunIfAttribute<OnUnix>())!.Requirement;

        using (Assert.Multiple())
        {
            await Assert.That(requirement).IsEqualTo(CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS));
            await Assert.That(requirement.IsSatisfiedBy([Capability.Linux])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.MacOS])).IsTrue();
            await Assert.That(requirement.IsSatisfiedBy([Capability.Windows])).IsFalse();
        }
    }

    [Test]
    public async Task Alternative_Operating_System_Attributes_Match_Either_Worker()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAnyAttribute<OnLinux, OnMacOS>());

        using (Assert.Multiple())
        {
            await Assert.That(route?.Requirement)
                .IsEqualTo(CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS));
            await Assert.That(route?.IsConditional).IsEqualTo(false);
        }
    }

    [Test]
    public async Task Mixed_Alternative_Attribute_Has_Conditional_Route()
    {
        var attribute = new RunIfAnyAttribute<OnLinux, OnCI>();
        var route = CapabilityConditions.GetRoute(attribute);

        using (Assert.Multiple())
        {
            await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));
            await Assert.That(route?.IsConditional).IsEqualTo(true);
            await Assert.That(CapabilityConditions.GetLocalAlternatives(attribute)).IsEquivalentTo([typeof(OnCI)]);
        }
    }

    [Test]
    public async Task Non_Capability_Condition_Has_No_Route()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.GetRoute(new RunIfAttribute<OnCI>())).IsNull();
            await Assert.That(CapabilityConditions.GetRoute(new SkipIfAttribute<OnLinux>())).IsNull();
        }
    }

    [Test]
    public async Task Contradictory_Operating_System_Conditions_Are_Not_Routable()
    {
        var attribute = new RunIfAllAttribute<OnWindows, OnLinux>();

        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.IsRoutable(attribute)).IsFalse();
            await Assert.That(CapabilityConditions.HasRoutableRequirement([attribute])).IsFalse();
        }
    }

    [Test]
    public async Task Custom_Capability_Conditions_Combine_With_Operating_Systems()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAllAttribute<OnLinux, OnGpu>());

        await Assert.That(route?.Requirement)
            .IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux, Capability.Gpu));
    }

    [Test]
    public async Task Mixed_And_Conditions_Keep_Capability_Members()
    {
        using (Assert.Multiple())
        {
            // Every AND member must hold, so the GPU requirement stays; the worker evaluates OnCI.
            var route = CapabilityConditions.GetRoute(new RunIfAllAttribute<OnGpu, OnCI>());
            await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Gpu));
            await Assert.That(route?.IsConditional).IsEqualTo(false);

            var groupRoute = CapabilityConditions.GetRoute(new RunIfAttribute<LinuxOnCiGroup>());
            await Assert.That(groupRoute?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));

            // An OR alternative without a capability can hold anywhere, so it keeps the route conditional.
            var alternativeRoute = CapabilityConditions.GetRoute(new RunIfAnyAttribute<LinuxOnCiGroup, OnCI>());
            await Assert.That(alternativeRoute?.IsConditional).IsEqualTo(true);
        }
    }

    [Test]
    public async Task Module_Requirement_Combines_Attributes_And_Conditions()
    {
        var requirement = CapabilityConditions.GetModuleRequirement(typeof(DeclaredAndConditionalModule));

        await Assert.That(requirement).IsEqualTo(
            CapabilityRequirement.AllOf(Capability.Docker)
                .And(CapabilityRequirement.AnyOf(Capability.Linux, Capability.MacOS)));
    }

    [Test]
    public async Task Module_Requirement_Skips_Locally_Satisfied_Groups()
    {
        var requirement = CapabilityConditions.GetModuleRequirement(
            typeof(DeclaredAndConditionalModule),
            static _ => true);

        await Assert.That(requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Docker));
    }

    [Test]
    public async Task Module_Requirement_Includes_Conditional_Routes_Only_When_Required()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.GetModuleRequirement(typeof(MixedAlternativeModule)).IsEmpty)
                .IsTrue();
            await Assert.That(CapabilityConditions.GetModuleRequirement(
                    typeof(MixedAlternativeModule),
                    isConditionalRouteRequired: static _ => true))
                .IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));
        }
    }

    [Test]
    public async Task Mixed_Alternative_Local_Conditions_Report_Planning_Safety()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.CanRequireRoute(
                    new RunIfAnyAttribute<OnLinux, OnCI>()))
                .IsTrue();
            await Assert.That(CapabilityConditions.CanRequireRoute(
                    new RunIfAnyAttribute<OnLinux, WorkerOnlyCondition>()))
                .IsFalse();
        }
    }

    [Test]
    public async Task Declared_Capabilities_Participate_In_Contradiction_Checks()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.HasImpossibleCombination(typeof(ConflictingDeclaredModule)))
                .IsTrue();
            await Assert.That(CapabilityConditions.HasImpossibleCombination(
                    typeof(DeclaredConflictsWithConditionModule)))
                .IsTrue();
            await Assert.That(CapabilityConditions.HasImpossibleCombination(typeof(DeclaredAndConditionalModule)))
                .IsFalse();
        }
    }

    [Test]
    public async Task Alternative_Operating_System_Metadata_Participates_In_Contradiction_Checks()
    {
        await Assert.That(CapabilityConditions.HasImpossibleCombination(
                typeof(ContradictoryAlternativeModule)))
            .IsTrue();
    }

    [Test]
    public async Task Condition_Group_Metadata_Participates_In_Contradiction_Checks()
    {
        await Assert.That(CapabilityConditions.HasImpossibleCombination(
                typeof(ContradictoryGroupedConditionModule)))
            .IsTrue();
    }

    [Test]
    public async Task FreeBsd_Condition_Uses_FreeBsd_Capability()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<OnFreeBSD>());

        await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.FreeBSD));
    }

    [Test]
    public async Task Grouped_Operating_System_Metadata_Uses_Union_Semantics()
    {
        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.HasImpossibleCombination(
                    typeof(GroupedAlternativeModule)))
                .IsFalse();
            await Assert.That(CapabilityConditions.HasImpossibleCombination(
                    typeof(ContradictoryGroupedAlternativeModule)))
                .IsTrue();
        }
    }

    [Test]
    public async Task Metadata_Uses_Declared_Group_Across_Different_Attribute_Types()
    {
        await Assert.That(CapabilityConditions.HasImpossibleCombination(
                typeof(SharedDeclaredGroupModule)))
            .IsFalse();
    }
}
