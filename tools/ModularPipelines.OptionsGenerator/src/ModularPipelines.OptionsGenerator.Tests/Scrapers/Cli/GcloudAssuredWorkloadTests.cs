using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudAssuredWorkloadTests
{
    [Test]
    [Arguments("[[WORKLOAD_ID] --external-identifier=EXTERNAL_IDENTIFIER | --workload-id=FLAG_WORKLOAD_ID]")]
    [Arguments("[--workload-id=FLAG_WORKLOAD_ID | [WORKLOAD_ID] --external-identifier=EXTERNAL_IDENTIFIER]")]
    public async Task Synopsis_Preserves_Optional_Workload_Operand(string alternatives)
    {
        var usage = UsageSynopsisParser.Parse(
            $"Usage: gcloud assured v2 workloads create {alternatives} AFTER",
            ["gcloud", "assured", "v2", "workloads", "create"]);

        await Assert.That(usage.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["WorkloadId", "After"]);
        var workload = usage.PositionalArguments.Single(argument => argument.PropertyName == "WorkloadId");
        await Assert.That(workload.IsRequired).IsFalse();
        await Assert.That(workload.PositionIndex).IsEqualTo(0);
        var following = usage.PositionalArguments.Single(argument => argument.PropertyName == "After");
        await Assert.That(following.IsRequired).IsTrue();
        await Assert.That(following.Phase).IsNotEqualTo(workload.Phase);
        await Assert.That(following.PositionIndex).IsEqualTo(0);
        await Assert.That(usage.RequiredOptionSwitches).IsEmpty();
    }

    [Test]
    public async Task Captured_Create_Command_Preserves_Workload_Operand_And_Flags()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "586.0.0",
            "gcloud-assured-v2-workloads-create.txt"));
        var command = await new GcloudResourceArgumentTests.TestScraper().Parse(
            ["gcloud", "assured", "v2", "workloads", "create"], help);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments.Count).IsEqualTo(1);
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .Contains("--workload-id");
    }
}
