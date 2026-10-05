using ModularPipelines.AmazonWebServices.Enums;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Global_Options_Render_Once_Before_Service_And_Command()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new AwsEc2DescribeInstancesOptions
        {
            CliReadTimeout = 0,
            NoCliPager = true,
            Output = AwsOutput.Json,
            Profile = "build",
            Region = "eu-west-1",
            InstanceIds = ["i-example"],
        };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "aws --cli-read-timeout 0 --no-cli-pager --output json --profile build --region eu-west-1 ec2 describe-instances --instance-ids i-example");
        await Assert.That(typeof(AwsEc2DescribeInstancesOptions).GetProperty(nameof(AwsOptions.Region))!.DeclaringType)
            .IsEqualTo(typeof(AwsOptions));
    }

    [Test]
    public async Task Global_Enums_And_Flags_Preserve_Cli_Spelling()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new AwsStsGetCallerIdentityOptions
        {
            CliBinaryFormat = AwsCliBinaryFormat.RawInBase64Out,
            Debug = true,
            NoCliAutoPrompt = true,
            Output = AwsOutput.YamlStream,
        };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "aws --cli-binary-format raw-in-base64-out --debug --no-cli-auto-prompt --output yaml-stream sts get-caller-identity");
    }
}
