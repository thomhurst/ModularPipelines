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
    private sealed class GroupedOperatingSystemAttribute<TCondition>() : RunConditionAttribute(ConditionIntent.Run), IPlanningSafe
        where TCondition : IRunCondition, new()
    {
        public override Type? GroupKey => typeof(GroupedOperatingSystemAttribute<>);

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            new TCondition().EvaluateAsync(context, cancellationToken);
    }

    private sealed class FirstGroupedOperatingSystemAttribute<TCondition>() : RunConditionAttribute(ConditionIntent.Run), IPlanningSafe
        where TCondition : IRunCondition, new()
    {
        public override Type? GroupKey => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            new TCondition().EvaluateAsync(context, cancellationToken);
    }

    private sealed class SecondGroupedOperatingSystemAttribute<TCondition>() : RunConditionAttribute(ConditionIntent.Run), IPlanningSafe
        where TCondition : IRunCondition, new()
    {
        public override Type? GroupKey => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            new TCondition().EvaluateAsync(context, cancellationToken);
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
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [RunIfAny<OnLinux, OnCI>]
    private sealed class MixedAlternativeModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class LinuxAndCiGroup : ConditionGroup, IPlanningSafe
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnCI()];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    private sealed class LinuxOrWindowsGroup : ConditionGroup, IPlanningSafe
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnWindows()];

        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    private sealed class GpuOrCiGroup : ConditionGroup, IPlanningSafe
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnGpu(), new OnCI()];

        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    private sealed class LinuxOnCiGroup : ConditionGroup, IPlanningSafe
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnCI()];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    private sealed class OnGpu : ICapabilityCondition
    {
        public Capability Capability => Capability.Gpu;

        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(false);
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
            await Assert.That(route?.IsConditional).IsFalse();
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
            await Assert.That(route?.IsConditional).IsTrue();
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
        var attribute = new RunIfAttribute<OnWindows, OnLinux>();

        using (Assert.Multiple())
        {
            await Assert.That(CapabilityConditions.IsRoutable(attribute)).IsFalse();
            await Assert.That(CapabilityConditions.HasRoutableRequirement([attribute])).IsFalse();
        }
    }

    [Test]
    public async Task Custom_Capability_Conditions_Combine_With_Operating_Systems()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<OnLinux, OnGpu>());

        await Assert.That(route?.Requirement)
            .IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux, Capability.Gpu));
    }

    [Test]
    public async Task Mixed_And_Conditions_Keep_Capability_Members()
    {
        using (Assert.Multiple())
        {
            // Every AND member must hold, so the GPU requirement stays; the worker evaluates OnCI.
            var route = CapabilityConditions.GetRoute(new RunIfAttribute<OnGpu, OnCI>());
            await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Gpu));
            await Assert.That(route?.IsConditional).IsFalse();

            var groupRoute = CapabilityConditions.GetRoute(new RunIfAttribute<LinuxOnCiGroup>());
            await Assert.That(groupRoute?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));

            // An OR alternative without a capability can hold anywhere, so it keeps the route conditional.
            var alternativeRoute = CapabilityConditions.GetRoute(new RunIfAnyAttribute<LinuxOnCiGroup, OnCI>());
            await Assert.That(alternativeRoute?.IsConditional).IsTrue();
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
    public async Task Static_Module_Requirement_Needs_Only_What_Every_Outcome_Needs()
    {
        // OnCI could be true, so without evaluating it the module needs no capability.
        await Assert.That(CapabilityConditions.GetModuleRequirement(typeof(MixedAlternativeModule))!.IsEmpty)
            .IsTrue();
    }

    [Test]
    public async Task Formula_Uses_Known_Condition_Values()
    {
        var formula = ConditionFormula.ForAttribute(new RunIfAnyAttribute<OnLinux, OnCI>())!;
        var ci = formula.Atoms.Single();

        using (Assert.Multiple())
        {
            await Assert.That(ci.IsPlanning).IsTrue();
            await Assert.That(formula.Evaluate(_ => false).Requirement)
                .IsEqualTo(CapabilityRequirement.AllOf(Capability.Linux));
            await Assert.That(formula.Evaluate(_ => true).Kind).IsEqualTo(FormulaValueKind.True);
        }
    }

    [Test]
    public async Task Formula_Drops_Branches_That_Are_False_On_The_Master()
    {
        // (linux AND false) OR windows must route to Windows, not to Linux or Windows.
        var formula = ConditionFormula.ForAttribute(new RunIfAnyAttribute<LinuxAndCiGroup, OnWindows>())!;

        await Assert.That(formula.Evaluate(_ => false).Requirement)
            .IsEqualTo(CapabilityRequirement.AllOf(Capability.Windows));
    }

    [Test]
    public async Task Formula_Is_False_When_Master_Values_Contradict()
    {
        var formula = ConditionFormula.ForModule(
        [
            new RunIfAnyAttribute<OnLinux, OnCI>(),
            new RunIfAnyAttribute<OnWindows, OnCI>(),
        ])!;

        using (Assert.Multiple())
        {
            await Assert.That(formula.Evaluate(_ => false).Kind).IsEqualTo(FormulaValueKind.False);
            await Assert.That(formula.Evaluate(_ => true).Kind).IsEqualTo(FormulaValueKind.True);
        }
    }

    [Test]
    public async Task Worker_Only_Conditions_Are_Not_Evaluated_On_The_Master()
    {
        var formula = ConditionFormula.ForAttribute(new RunIfAnyAttribute<OnLinux, WorkerOnlyCondition>())!;

        await Assert.That(formula.Atoms.Single().IsPlanning).IsFalse();
    }

    [Test]
    public async Task Any_Logic_Condition_Group_Routes_To_Any_Capability()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<LinuxOrWindowsGroup>());

        await Assert.That(route?.Requirement)
            .IsEqualTo(CapabilityRequirement.AnyOf(Capability.Linux, Capability.Windows));
    }

    [Test]
    public async Task Mixed_Or_Condition_Group_Routes_Its_Capability_Branch()
    {
        var route = CapabilityConditions.GetRoute(new RunIfAttribute<GpuOrCiGroup>());

        using (Assert.Multiple())
        {
            await Assert.That(route?.Requirement).IsEqualTo(CapabilityRequirement.AllOf(Capability.Gpu));
            await Assert.That(route?.IsConditional).IsTrue();
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
