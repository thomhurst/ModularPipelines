using System.ComponentModel.DataAnnotations;
using ModularPipelines.AmazonWebServices.Enums;
using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsRequiredCollectionTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Network_Acl_Operations_Render_Both_Directions(bool egress)
    {
        AwsOptions[] operations =
        [
            new AwsEc2CreateNetworkAclEntryOptions("acl-example", 100, "6", AwsEc2CreateNetworkAclEntryRuleAction.Allow, egress),
            new AwsEc2DeleteNetworkAclEntryOptions("acl-example", 100, egress),
            new AwsEc2ReplaceNetworkAclEntryOptions("acl-example", 100, "6", AwsEc2ReplaceNetworkAclEntryRuleAction.Allow, egress),
        ];
        foreach (var options in operations)
        {
            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(options, new ValidationContext(options), errors, true)).IsTrue();
            var arguments = BuildArguments(options);
            await Assert.That(arguments.Contains(egress ? "--egress" : "--ingress")).IsTrue();
            await Assert.That(arguments.Contains(egress ? "--ingress" : "--egress")).IsFalse();
        }
    }

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
