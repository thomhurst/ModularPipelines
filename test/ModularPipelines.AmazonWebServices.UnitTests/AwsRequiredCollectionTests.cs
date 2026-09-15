using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsRequiredCollectionTests
{
    [Test]
    [Arguments("")]
    [Arguments(" \t\r\n")]
    public async Task Alternate_Input_Factories_Reject_Blank_Json(string input)
    {
        await Assert.That(() => AwsEc2TerminateInstancesOptions.FromCliInputJson(input))
            .Throws<ArgumentException>();
        await Assert.That(() => AwsEc2CreateKeyPairOptions.FromCliInputJson(input))
            .Throws<ArgumentException>();
        await Assert.That(() => AwsAutoscalingSetInstanceProtectionOptions.FromCliInputJson(input))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Required_Strings_Reject_Null()
    {
        await Assert.That(() => new AwsEc2CreateKeyPairOptions(null!))
            .Throws<ArgumentNullException>();
        await Assert.That(() => new AwsAutoscalingSetInstanceProtectionOptions(["i-example"], null!, true))
            .Throws<ArgumentNullException>();
    }

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
