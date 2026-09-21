using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Captured_Provider_Requirements_Are_Conditional_At_Runtime()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("developer-connect connections create");
        var tool = CreateGcloudValidationTool(command);
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var enums = (await new EnumGenerator().GenerateAsync(tool)).Select(file => file.Content).ToArray();
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var (properties, valid) in new (string[], bool)[]
            {
                ([], true),
                (["SecureSourceManagerInstanceConfig"], true),
                (["BitbucketCloudConfigWorkspace"], false),
                (["BitbucketCloudConfigWorkspace", "BitbucketCloudConfigAuthorizerCredentialUserTokenSecretVersion",
                    "BitbucketCloudConfigReadAuthorizerCredentialUserTokenSecretVersion", "BitbucketCloudConfigWebhookSecretVersion"], true),
                (["SecureSourceManagerInstanceConfig", "BitbucketCloudConfigWorkspace"], false),
                (["BitbucketDataCenterConfigHostUri", "BitbucketDataCenterConfigAuthorizerCredentialUserTokenSecretVersion",
                    "BitbucketDataCenterConfigReadAuthorizerCredentialUserTokenSecretVersion", "BitbucketDataCenterConfigWebhookSecretVersion"], true),
                (["BitbucketDataCenterConfigHostUri"], false),
                (["BitbucketDataCenterConfigHostUri", "BitbucketDataCenterConfigReadAuthorizerCredentialUserTokenSecretVersion",
                    "BitbucketDataCenterConfigWebhookSecretVersion"], false),
                (["GithubConfigApp", "GithubConfigAuthorizerCredentialOauthTokenSecretVersion"], true),
                (["GithubConfigApp"], true),
                (["GithubConfigAuthorizerCredentialOauthTokenSecretVersion"], false),
                (["GithubEnterpriseConfigHostUri"], true),
                (["GithubEnterpriseConfigHostUri", "GithubEnterpriseConfigPrivateKeySecretVersion", "GithubEnterpriseConfigWebhookSecretVersion"], true),
                (["GithubEnterpriseConfigPrivateKeySecretVersion"], false),
                (["GitlabConfigAuthorizerCredentialUserTokenSecretVersion", "GitlabConfigReadAuthorizerCredentialUserTokenSecretVersion",
                    "GitlabConfigWebhookSecretVersion"], true),
                (["GitlabConfigWebhookSecretVersion"], false),
                (["GitlabEnterpriseConfigHostUri", "GitlabEnterpriseConfigAuthorizerCredentialUserTokenSecretVersion",
                    "GitlabEnterpriseConfigReadAuthorizerCredentialUserTokenSecretVersion", "GitlabEnterpriseConfigWebhookSecretVersion"], true),
                (["GitlabEnterpriseConfigHostUri"], false),
                (["GitlabEnterpriseConfigHostUri", "GitlabEnterpriseConfigReadAuthorizerCredentialUserTokenSecretVersion",
                    "GitlabEnterpriseConfigWebhookSecretVersion"], false),
                (["HttpConfigHostUri"], true),
                (["HttpConfigServiceDirectory"], false),
                (["HttpConfigSslCaCertificate"], false),
                (["HttpConfigHostUri", "HttpConfigServiceDirectory", "HttpConfigSslCaCertificate"], true),
                (["HttpConfigHostUri", "HttpConfigBearerTokenAuthenticationSecretVersion"], true),
                (["HttpConfigHostUri", "HttpConfigServiceDirectory", "HttpConfigBearerTokenAuthenticationSecretVersion"], true),
                (["HttpConfigHostUri", "HttpConfigBasicAuthenticationUsername"], true),
                (["HttpConfigHostUri", "HttpConfigBasicAuthenticationPasswordSecretVersion"], false),
                (["HttpConfigHostUri", "HttpConfigBasicAuthenticationUsername", "HttpConfigBearerTokenAuthenticationSecretVersion"], false),
                (["CryptoKeyConfigReference"], true),
                (["KeyRing"], false),
            })
            {
                var instance = Activator.CreateInstance(type, ["connection"])!;
                foreach (var property in properties)
                {
                    type.GetProperty(property)!.SetValue(instance, "resource");
                }
                var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(valid)
                    .Because(string.Join(", ", properties) + ": " + string.Join("; ", errors.Select(error => error.ErrorMessage)));
            }
        }, enums);
    }
}
