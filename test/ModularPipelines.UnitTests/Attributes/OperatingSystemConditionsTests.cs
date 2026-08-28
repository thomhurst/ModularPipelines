using ModularPipelines.Attributes;
using ModularPipelines;

namespace ModularPipelines.UnitTests.Attributes;

public class OperatingSystemConditionsTests
{
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
    public async Task Contradictory_Operating_System_Conditions_Have_No_Routable_Target()
    {
        var targets = OperatingSystemConditions.GetTargets(
            new RunIfAllAttribute<OnWindows, OnLinux>());

        await Assert.That(targets).IsEmpty();
    }
}
