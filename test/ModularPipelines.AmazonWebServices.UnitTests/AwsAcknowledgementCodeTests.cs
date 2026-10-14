using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.AmazonWebServices.Enums;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.AmazonWebServices.UnitTests;

public class AwsAcknowledgementCodeTests
{
    [Test]
    public async Task Accept_Code_Is_Masked_In_Logs_And_Preserved_In_Command_Arguments()
    {
        const string code = "123456";
        const string pageId = "arn:aws:ssm-contacts:us-east-1:111122223333:page/example";
        var options = new AwsSsmContactsAcceptPageOptions(pageId, AwsSsmContactsAcceptPageAcceptType.Read, code)
        {
            AcceptCodeValidation = AwsSsmContactsAcceptPageAcceptCodeValidation.Enforce,
        };
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<TrueModule>().BuildAsync();
        var obfuscator = pipeline.Services.GetRequiredService<ISecretObfuscator>();

        var output = obfuscator.Obfuscate($"--page-id {pageId} --accept-code {code} --accept-code-validation ENFORCE", options);

        await Assert.That(output).IsEqualTo($"--page-id {pageId} --accept-code {new SecretMaskingOptions().MaskValue} --accept-code-validation ENFORCE");
        await AssertArguments(BuildArguments(options),
            ["--page-id", pageId, "--accept-type", "READ", "--accept-code", code, "--accept-code-validation", "ENFORCE"]);
    }

    [Test]
    [Arguments(nameof(AwsSsmContactsAcceptPageOptions.AcceptCode), true)]
    [Arguments(nameof(AwsSsmContactsAcceptPageOptions.AcceptType), false)]
    [Arguments(nameof(AwsSsmContactsAcceptPageOptions.AcceptCodeValidation), false)]
    public async Task Only_Acknowledgement_Value_Has_Secret_Metadata(string propertyName, bool secret)
    {
        var property = typeof(AwsSsmContactsAcceptPageOptions).GetProperty(propertyName)!;

        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>() is not null).IsEqualTo(secret);
    }
}
