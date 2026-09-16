using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsEnumBoundaryTests
{
    [Test]
    [Arguments("Policy")]
    [Arguments("DeliveryPolicy")]
    [Arguments("FifoThroughputScope")]
    public async Task Sns_Attribute_Names_Render_As_Required_Strings(string attributeName)
    {
        const string topicArn = "arn:aws:sns:us-east-1:123456789012:example";
        var arguments = BuildArguments(new AwsSnsSetTopicAttributesOptions(topicArn, attributeName));

        await AssertArguments(arguments, ["--topic-arn", topicArn, "--attribute-name", attributeName]);
    }

    [Test]
    public async Task Single_Vpn_Type_Renders_As_String()
    {
        var arguments = BuildArguments(new AwsEc2CreateVpnGatewayOptions("ipsec.1"));

        await AssertArguments(arguments, ["--type", "ipsec.1"]);
    }
}
