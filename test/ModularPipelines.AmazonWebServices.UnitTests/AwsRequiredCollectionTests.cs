using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsRequiredCollectionTests
{
    [Test]
    public async Task InstanceIds_Reject_A_Collection_Containing_Only_Null()
    {
        await Assert.That(() => new AwsEc2TerminateInstancesOptions([null!]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task InstanceIds_Render_The_NonNull_Values()
    {
        var arguments = BuildArguments(new AwsEc2TerminateInstancesOptions([null!, "i-example"]));

        await AssertArguments(arguments, ["--instance-ids", "i-example"]);
    }
}
