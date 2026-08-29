using ModularPipelines.Attributes;
using ModularPipelines;

namespace ModularPipelines.UnitTests.Attributes;

public class OperatingSystemConditionsTests
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
        IGroupedConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(GroupedOperatingSystemAttribute<>);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class FirstGroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class SecondGroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(SharedDeclaredGroupModule);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
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
    public async Task Direct_Operating_System_Uses_Existing_Capability()
    {
        var targets = OperatingSystemConditions.GetTargets(new RunIfAttribute<OnLinux>());

        await Assert.That(targets).IsEquivalentTo([OperatingSystemConditions.Linux]);
    }

    [Test]
    public async Task Alternative_Operating_System_Group_Matches_Either_Worker()
    {
        var target = OperatingSystemConditions
            .GetTargets(new RunIfAttribute<OnUnix>())
            .Single();

        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux))
                .Contains(target);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS))
                .Contains(target);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows))
                .DoesNotContain(target);
        }
    }

    [Test]
    public async Task Alternative_Operating_System_Attributes_Match_Either_Worker()
    {
        var target = OperatingSystemConditions
            .GetTargets(new RunIfAnyAttribute<OnLinux, OnMacOS>())
            .Single();

        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux))
                .Contains(target);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS))
                .Contains(target);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows))
                .DoesNotContain(target);
        }
    }

    [Test]
    public async Task Contradictory_Operating_System_Conditions_Have_No_Routable_Target()
    {
        var targets = OperatingSystemConditions.GetTargets(
            new RunIfAllAttribute<OnWindows, OnLinux>());

        await Assert.That(targets).IsEmpty();
    }

    [Test]
    public async Task Alternative_Operating_System_Metadata_Participates_In_Contradiction_Checks()
    {
        await Assert.That(OperatingSystemConditions.HasImpossibleCombination(
                typeof(ContradictoryAlternativeModule)))
            .IsTrue();
    }

    [Test]
    public async Task Condition_Group_Metadata_Participates_In_Contradiction_Checks()
    {
        await Assert.That(OperatingSystemConditions.HasImpossibleCombination(
                typeof(ContradictoryGroupedConditionModule)))
            .IsTrue();
    }

    [Test]
    public async Task FreeBsd_Condition_Uses_FreeBsd_Capability()
    {
        var targets = OperatingSystemConditions.GetTargets(new RunIfAttribute<OnFreeBSD>());

        using (Assert.Multiple())
        {
            await Assert.That(targets).IsEquivalentTo([OperatingSystemConditions.FreeBSD]);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.FreeBSD))
                .Contains(OperatingSystemConditions.FreeBSD);
        }
    }

    [Test]
    public async Task Grouped_Operating_System_Metadata_Uses_Union_Semantics()
    {
        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.HasImpossibleCombination(
                    typeof(GroupedAlternativeModule)))
                .IsFalse();
            await Assert.That(OperatingSystemConditions.HasImpossibleCombination(
                    typeof(ContradictoryGroupedAlternativeModule)))
                .IsTrue();
        }
    }

    [Test]
    public async Task Metadata_Uses_Declared_Group_Across_Different_Attribute_Types()
    {
        await Assert.That(OperatingSystemConditions.HasImpossibleCombination(
                typeof(SharedDeclaredGroupModule)))
            .IsFalse();
    }
}
