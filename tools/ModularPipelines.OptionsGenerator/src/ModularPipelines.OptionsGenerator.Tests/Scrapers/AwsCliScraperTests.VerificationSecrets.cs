using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class AwsCliScraperTests
{
    [Test]
    [Arguments("The one-time passcode that the recipient submitted for validation.", true)]
    [Arguments("The one-time password that the recipient submitted for validation.", true)]
    [Arguments("An authentication code emitted by the device. The format for this parameter is a string of six digits.", true)]
    [Arguments("A subsequent authentication code emitted by the device. The format for this parameter is a string of six digits.", true)]
    [Arguments("The activation code for this email contact. An email contact has a maximum of five activation attempts. Activation codes expire after 12 hours.", true)]
    [Arguments("The validation code received from the procurement portal in response to a previous SendProcurementPortalValidation request.", true)]
    [Arguments("The verification code that your user pool sent to the added or changed attribute, for example the user's email address.", true)]
    [Arguments("The confirmation code that your user pool sent in response to the SignUp request.", true)]
    [Arguments("The verification code.", true)]
    [Arguments("The activation code status returned by the service.", false)]
    [Arguments("The validation code format accepted by the portal.", false)]
    [Arguments("The number of confirmation codes sent to the user.", false)]
    [Arguments("The status code returned after validating a verification code.", false)]
    [Arguments("The authentication code status returned by the device.", false)]
    [Arguments("The number of authentication codes emitted by the device.", false)]
    [Arguments("The status code returned after validating a one-time passcode.", false)]
    [Arguments("The number of attempts to validate a one-time passcode.", false)]
    public async Task Verification_Code_Secrets_Follow_Value_Description(string description, bool expectedSecret)
    {
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "endusermessaging", "validate-notify-code-verification"], $$"""
            OPTIONS
                   --code (string)
                    {{description}}
            """))!;
        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceManualOverridesAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var files = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(tool.Commands.Single().Options.Single().IsSecret).IsEqualTo(expectedSecret);
        await Assert.That(files.Single().Content.Contains("[SecretValue]", StringComparison.Ordinal)).IsEqualTo(expectedSecret);
    }
}
