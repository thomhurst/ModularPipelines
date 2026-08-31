using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsEc2DescribeInstancesOptionsTests
{
    [Test]
    public async Task DescribeInstances_Groups_InstanceIds()
    {
        var arguments = BuildArguments(new AwsEc2DescribeInstancesOptions
        {
            InstanceIds = ["i-0123456789abcdef0", "i-0fedcba9876543210"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--instance-ids",
            "i-0123456789abcdef0",
            "i-0fedcba9876543210",
        ], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
