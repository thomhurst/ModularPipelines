using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class OptionTypeEnhancerTests
{
    [Test]
    [Arguments(false, false, 1)]
    [Arguments(true, false, 4)]
    [Arguments(true, true, 2)]
    public async Task CreateDefault_Uses_Supplied_Executor_For_Help_Detection(bool timedOut, bool recovers, int expectedAttempts)
    {
        var executor = new HelpExecutor(timedOut, recovers);
        var enhancer = OptionTypeEnhancer.CreateDefault(executor, NullLoggerFactory.Instance);
        var tool = new CliToolDefinition
        {
            ToolName = "kubectl",
            NamespacePrefix = "Kubectl",
            TargetNamespace = "ModularPipelines.Kubernetes",
            OutputDirectory = "unused",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "kubectl example",
                    CommandParts = ["example"],
                    ClassName = "KubectlExampleOptions",
                    ParentClassName = "KubectlOptions",
                    ToolNamespacePrefix = "Kubectl",
                    Options = [new CliOptionDefinition { SwitchName = "--style", PropertyName = "Style", CSharpType = "string?" }],
                },
            ],
        };

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        await Assert.That(executor.Calls).IsEquivalentTo(Enumerable.Repeat(("kubectl", "example --help"), expectedAttempts));
        if (timedOut && !recovers)
        {
            await Assert.That(option.EnumDefinition).IsNull();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
        }
        else
        {
            await Assert.That(option.EnumDefinition).IsNotNull();
            await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["compact", "expanded"]);
        }
    }

    private sealed class HelpExecutor(bool timedOut, bool recovers) : ICliCommandExecutor
    {
        public List<(string Command, string Arguments)> Calls { get; } = [];

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            Calls.Add((command, arguments));
            var attemptTimedOut = timedOut && (!recovers || Calls.Count == 1);
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = attemptTimedOut ? string.Empty : "  --style string   One of: compact|expanded",
                StandardError = string.Empty,
                ExitCode = attemptTimedOut ? -1 : 0,
                TimedOut = attemptTimedOut,
            });
        }
    }

    [Test]
    public async Task EnhanceAsync_Preserves_Repeatability_For_Detected_Enums()
    {
        var detector = new HeuristicTypeDetector(NullLogger<HeuristicTypeDetector>.Instance);
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var option = new CliOptionDefinition
        {
            SwitchName = "--sort",
            PropertyName = "Sort",
            CSharpType = "IEnumerable<string>?",
            Description = "May be repeated or comma-separated. Possible values: ascending, descending.",
            AcceptsMultipleValues = true,
        };
        var command = new CliCommandDefinition
        {
            FullCommand = "pulumi stack ls",
            CommandParts = ["stack", "ls"],
            ClassName = "PulumiStackLsOptions",
            ParentClassName = "PulumiOptions",
            ToolNamespacePrefix = "Pulumi",
            Options = [option],
        };
        var tool = new CliToolDefinition
        {
            ToolName = "pulumi",
            NamespacePrefix = "Pulumi",
            TargetNamespace = "ModularPipelines.Pulumi",
            OutputDirectory = "src/ModularPipelines.Pulumi",
            Commands = [command],
        };

        var enhanced = await enhancer.EnhanceAsync(tool);
        var enhancedOption = enhanced.Commands.Single().Options.Single();

        await Assert.That(enhancedOption.AcceptsMultipleValues).IsTrue();
        await Assert.That(enhancedOption.EnumDefinition).IsNotNull();
        await Assert.That(enhancedOption.CSharpType)
            .IsEqualTo($"IEnumerable<{enhancedOption.EnumDefinition!.EnumName}>?");
    }

    [Test]
    public async Task EnhanceAsync_Builds_The_Same_Enum_Regardless_Of_Detected_Value_Order()
    {
        // Case variants retain distinct CLI values and deterministic member names.
        var first = await EnhanceWithDetectedEnum(["PUBLIC", "public", "internal"]);
        var second = await EnhanceWithDetectedEnum(["internal", "public", "PUBLIC"]);

        using (Assert.Multiple())
        {
            await Assert.That(first.Values.Select(value => value.CliValue))
                .IsEquivalentTo(["internal", "public", "PUBLIC"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
            await Assert.That(second.Values)
                .IsEquivalentTo(first.Values, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        }
    }

    [Test]
    [Arguments(3)]
    [Arguments(21)]
    public async Task EnhanceAsync_Replaces_Existing_Enum_Without_Losing_Values_Or_Documentation(int valueCount)
    {
        string[] values = valueCount == 3 ? ["public", "PUBLIC", "internal"]
            : [.. Enumerable.Range(1, valueCount).Select(index => $"mode{index}")];
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Enum,
            Confidence = 100,
            Source = "ManualOverride",
            EnumValues = values,
        };
        var pipeline = new OptionTypeDetectorPipeline([new FixedDetector(result)], NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var originalEnum = new CliEnumDefinition
        {
            EnumName = "DockerBuildVisibility",
            Values =
            [
                new() { CliValue = "public", MemberName = "Public", Description = "Public visibility." },
                new() { CliValue = "internal", MemberName = "Internal", Description = "Internal visibility." },
            ],
        };
        var original = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--visibility",
            PropertyName = "Visibility",
            CSharpType = "DockerBuildVisibility?",
            Description = "Select visibility.",
            EnumDefinition = originalEnum,
        });
        original = original with { Commands = [original.Commands.Single() with { Enums = [originalEnum] }] };

        var enhanced = await enhancer.EnhanceAsync(original);
        var option = enhanced.Commands.Single().Options.Single();

        if (valueCount == 3)
        {
            await Assert.That(enhanced.AllEnums.Single().Values.Select(value => value.CliValue)).IsEquivalentTo(values);
            await Assert.That(option.EnumDefinition!.Values.Single(value => value.CliValue == "public").Description)
                .IsEqualTo("Public visibility.");
        }
        else
        {
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.EnumDefinition).IsNull();
            await Assert.That(enhanced.AllEnums).IsEmpty();
            await Assert.That(option.Description).IsEqualTo($"Select visibility. [possible values: {string.Join(", ", values)}]");
            var enhancedAgain = await enhancer.EnhanceAsync(enhanced);
            await Assert.That(enhancedAgain.Commands.Single().Options.Single().Description).IsEqualTo(option.Description);
        }
    }

    private static async Task<CliEnumDefinition> EnhanceWithDetectedEnum(string[] enumValues)
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Enum,
            Confidence = 100,
            Source = "ManualOverride",
            EnumValues = enumValues,
        };
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(result)],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--visibility",
            PropertyName = "Visibility",
            CSharpType = "string?",
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        return enhanced.Commands.Single().Options.Single().EnumDefinition!;
    }

    [Test]
    public async Task EnhanceAsync_Applies_Key_Filtered_Secret_Metadata()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.KeyValue,
            Confidence = 100,
            Source = "ManualOverride",
            SecretValueKeys = ["token", "password"],
        };
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(result)],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--build-arg",
            PropertyName = "BuildArg",
            CSharpType = "string[]?",
            AcceptsMultipleValues = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
            await Assert.That(option.IsKeyValue).IsTrue();
            await Assert.That(option.IsSecret).IsTrue();
            await Assert.That(option.SecretValueKeys).IsEquivalentTo(["token", "password"]);
        }
    }

    [Test]
    public async Task EnhanceAsync_Applies_Metadata_Only_Secret_Override()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Unknown,
            Confidence = 100,
            Source = "ManualOverride",
            IsSecret = false,
        };
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(result)],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--password",
            PropertyName = "Password",
            CSharpType = "string?",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
    }

    [Test]
    public async Task Explicit_Secret_Override_Takes_Precedence_Over_Metadata_Inference()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(new OptionTypeDetectionResult
            {
                Type = CliOptionType.Unknown,
                Confidence = 100,
                Source = "ManualOverride",
                IsSecret = true,
            })],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--token-size",
            PropertyName = "TokenSize",
            CSharpType = "int?",
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Documented_Enum_Choices_Require_An_Explicit_Secret_Override(bool explicitlySecret)
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [new FixedDetector(new OptionTypeDetectionResult
            {
                Type = CliOptionType.Unknown,
                Confidence = 100,
                Source = "ManualOverride",
                IsSecret = explicitlySecret ? true : null,
            })],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--http-tokens",
            PropertyName = "HttpTokens",
            CSharpType = "HttpTokens?",
            IsSecret = true,
            EnumDefinition = new CliEnumDefinition
            {
                EnumName = "HttpTokens",
                Values =
                [
                    new CliEnumValue { MemberName = "Required", CliValue = "required" },
                    new CliEnumValue { MemberName = "Optional", CliValue = "optional" },
                ],
            },
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsEqualTo(explicitlySecret);
    }

    [Test]
    public async Task Docker_Override_Seeds_Build_Argument_Secret_Keys()
    {
        var detector = new ManualOverrideDetector(
            NullLogger<ManualOverrideDetector>.Instance,
            Path.Combine(AppContext.BaseDirectory, "TypeOverrides"));
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--build-arg",
            PropertyName = "BuildArg",
            CSharpType = "string[]?",
            AcceptsMultipleValues = true,
        });

        var enhanced = await enhancer.EnhanceManualOverridesAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsKeyValue).IsTrue();
            await Assert.That(option.IsSecret).IsTrue();
            await Assert.That(option.SecretValueKeys).Contains("token");
            await Assert.That(option.SecretValueKeys).Contains("private_key");
        }
    }

    [Test]
    public async Task Kubectl_Apply_Validate_Override_Preserves_Value()
    {
        var detector = new ManualOverrideDetector(
            NullLogger<ManualOverrideDetector>.Instance,
            Path.Combine(AppContext.BaseDirectory, "TypeOverrides"));
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--validate",
            PropertyName = "Validate",
            CSharpType = "bool?",
            IsFlag = true,
        }, toolName: "kubectl", commandName: "apply");

        var enhanced = await enhancer.EnhanceManualOverridesAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.IsFlag).IsFalse();
        }
    }

    [Test]
    [Arguments("--identity-token", "IdentityToken", "Token or path to a file containing the token.")]
    [Arguments("--oidc-client-secret-file", "OidcClientSecretFile", "Path to the OIDC client secret file.")]
    public async Task Cosign_Override_Preserves_Path_Capable_Secrets(
        string switchName,
        string propertyName,
        string description)
    {
        var detector = new ManualOverrideDetector(
            NullLogger<ManualOverrideDetector>.Instance,
            Path.Combine(AppContext.BaseDirectory, "TypeOverrides"));
        var pipeline = new OptionTypeDetectorPipeline(
            [detector],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = switchName,
            PropertyName = propertyName,
            CSharpType = "string?",
            Description = description,
            IsSecret = true,
        }, toolName: "cosign");

        var enhanced = await enhancer.EnhanceAsync(tool);

        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsTrue();
    }

    [Test]
    public async Task EnhanceAsync_Removes_Inferred_Secret_From_Path_Option()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--private-key-location",
            PropertyName = "PrivateKeyLocation",
            CSharpType = "string?",
            Description = "Path to the private key file.",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
    }

    [Test]
    public async Task EnhanceAsync_Warns_When_Secret_Looking_Option_Is_Boolean()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var logger = new RecordingLogger<OptionTypeEnhancer>();
        var enhancer = new OptionTypeEnhancer(pipeline, logger);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--show-password",
            PropertyName = "ShowPassword",
            CSharpType = "bool?",
            IsFlag = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);

        using (Assert.Multiple())
        {
            await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsFalse();
            await Assert.That(logger.Messages).Contains(message =>
                message.Contains("was detected as boolean", StringComparison.Ordinal));
        }
    }

    [Test]
    public async Task EnhanceAsync_Removes_Inferred_Secret_From_Boolean_Value_Option()
    {
        var pipeline = new OptionTypeDetectorPipeline(
            [],
            NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--xml-raw-token",
            PropertyName = "XmlRawToken",
            CSharpType = "bool?",
            Description = "Enables using RawToken instead of Token.",
            IsSecret = true,
        });

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.IsSecret).IsFalse();
            await Assert.That(option.SecretValueKeys).IsEmpty();
        }
    }

    [Test]
    [Arguments(false, "TokenAuthUser", "The tokenAuthUser id of the authToken resource.")]
    [Arguments(true, "TokenAuthUser", "The tokenAuthUser id of the authToken resource.")]
    [Arguments(false, "ApiKeyConfigHttpElementLocation", "The location of the API key. The default value is QUERY.")]
    [Arguments(true, "ApiKeyConfigHttpElementLocation", "The location of the API key. The default value is QUERY.")]
    [Arguments(false, "CredentialSourceType", "Format of the credential source (JSON or text).")]
    [Arguments(true, "CredentialSourceType", "Format of the credential source (JSON or text).")]
    [Arguments(false, "PrivateKeySecretVersion", "Secret containing the private key of the GitHub App.")]
    [Arguments(true, "PrivateKeySecretVersion", "Secret containing the private key of the GitHub App.")]
    public async Task Credential_Metadata_Clears_Inferred_Secrets_But_Respects_Overrides(
        bool explicitSecret, string propertyName, string description)
    {
        var pipeline = new OptionTypeDetectorPipeline(
            explicitSecret ? [new FixedDetector(new OptionTypeDetectionResult
            {
                Type = CliOptionType.Unknown, Confidence = 100, Source = "ManualOverride", IsSecret = true,
            })] : [], NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--metadata",
            PropertyName = propertyName,
            CSharpType = "string?",
            Description = description,
            IsSecret = true,
        });
        var enhanced = await enhancer.EnhanceAsync(tool);
        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsEqualTo(explicitSecret);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Secret_References_Respect_Explicit_Masking_Overrides(bool explicitSecret)
    {
        var pipeline = new OptionTypeDetectorPipeline(
            explicitSecret ? [new FixedDetector(new OptionTypeDetectionResult
            {
                Type = CliOptionType.Unknown, Confidence = 100, Source = "ManualOverride", IsSecret = true,
            })] : [], NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--set-secrets",
            PropertyName = "SetSecrets",
            CSharpType = "string?",
            IsSecret = true,
            IsResourceReference = true,
        });
        var enhanced = await enhancer.EnhanceAsync(tool);
        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsEqualTo(explicitSecret);
    }

    [Test]
    [Arguments("ServiceAccountKeyFile", "The base64 encoded content of the service account key file.", true)]
    [Arguments("ServiceAccountKeyFile", "Specify the base64 encoded content of the service account key file.", true)]
    [Arguments("ServiceAccountKeyFile", "Provide the contents of the service account key file.", true)]
    [Arguments("ServiceAccountKeyFile", "Use the base64-encoded content of the service-account key file.", true)]
    [Arguments("ServiceAccountKeyFile", "Path to the service account key file.", false)]
    [Arguments("ConfigFile", "The base64 encoded content of the configuration file.", false)]
    [Arguments("SslKeyFile", "Client Key - The base64 encoded content of a .pem or .crt file containing the client private key (for 2-way SSL).", true)]
    [Arguments("SslKeyFile", "Client Key: Specify the base64-encoded contents of a .pem file containing the client private key.", true)]
    [Arguments("SslKeyFile", "The base64 encoded private key of the PostgreSQL server.", true)]
    [Arguments("KeyStoreFile", "The base64 encoded content of the KeyStore file.", true)]
    [Arguments("WalletFile", "The wallet contents Oracle Goldengate uses to make connections to a database. This attribute is expected to be base64 encoded.", true)]
    [Arguments("SslKeyFile", "Client Key - Path to the file containing the client private key.", false)]
    [Arguments("SslKeyFile", "The file containing the base64 encoded private key.", false)]
    [Arguments("KeyStoreFile", "Path to the KeyStore file.", false)]
    [Arguments("WalletFile", "The wallet file to read.", false)]
    [Arguments("TrustStoreFile", "The base64 encoded content of the TrustStore file.", false)]
    [Arguments("SslCertFile", "Client Certificate - The base64 encoded content of a .pem file containing the client public key.", false)]
    [Arguments("Credential", "ID of the oauth client credential or fully qualified identifier for the oauth client credential.", false)]
    [Arguments("Credential", "ID of the oauth-client credential or fully qualified identifier for the oauth-client credential.", false)]
    [Arguments("Credential", "The oauth client credential id of the oauth client resource.", false)]
    [Arguments("Credential", "The credential value for the oauth client resource.", true)]
    [Arguments("CredentialSourceType", "Format of the credential source (JSON or text).", false)]
    [Arguments("SubjectTokenType", "The type of token being used for authorization.", false)]
    [Arguments("CredentialMode", "Credential mode to create the catalog with.", false)]
    [Arguments("TokenFormat", "The format of the token.", false)]
    [Arguments("TokenEncoding", "The encoding used for tokens.", false)]
    [Arguments("TokenAlgorithm", "The signature algorithm used for tokens.", false)]
    [Arguments("OauthTokenScope", "The scope to be used when generating an OAuth2 access token.", false)]
    [Arguments("OidcTokenAudience", "The audience to be used when generating an OpenID Connect token.", false)]
    [Arguments("HttpOauthTokenScopeOverride", "The scope to be used when generating an OAuth2 access token.", false)]
    [Arguments("HttpOidcTokenAudienceOverride", "The audience to be used when generating an OpenID Connect token.", false)]
    [Arguments("ProxySecretNamespace", "Namespace of the Kubernetes secret containing the proxy configuration.", false)]
    [Arguments("ProxySecretArn", "ARN of the AWS Secrets Manager secret.", false)]
    [Arguments("PrivateKeySecretVersion", "Secret containing the private key of the GitHub App.", false)]
    [Arguments("SecuritySettingsAwsV4AccessKeyVersion", "The optional version identifier for the AWS access key.", false)]
    [Arguments("SecretManagerRotationInterval", "Set the rotation period for secrets.", false)]
    [Arguments("PasswordPolicyPasswordChangeInterval", "Minimum interval after which the password can be changed.", false)]
    [Arguments("PasswordPolicyComplexity", "The complexity of the password.", false)]
    [Arguments("TargetCostPerMillionInputTokens", "The target cost per million input tokens to filter profiles by, unit is 1 USD.", false)]
    [Arguments("TargetCostPerMillionOutputTokens", "The target cost per million output tokens to filter profiles by, unit is 1 USD.", false)]
    [Arguments("ThreeLeggedOauthTokenUrl", "The token endpoint for requesting tokens on behalf of an end user.", false)]
    [Arguments("CustomOauthConfigTokenUri", "The OAuth2 token request URL.", false)]
    [Arguments("CredentialSourceUrl", "The URL to obtain the credential from.", false)]
    [Arguments("KerberosRootPrincipalPasswordUri", "Google Cloud Storage URI of a KMS encrypted file containing the root principal password.", false)]
    [Arguments("ActiveDirectorySecretManagerKey", "The secret manager key storing administrator credentials.", false)]
    [Arguments("Secret", "The resource name of the secret version.", false)]
    [Arguments("CredentialSourceHeaders", "Headers to use when querying the credential-source-url.", true)]
    [Arguments("CredentialType", "The credential value to send.", true)]
    [Arguments("TokenFormat", "The token contents to send.", true)]
    [Arguments("SecretVersion", "The secret value to send.", true)]
    [Arguments("TokenUrl", "The token value to send as a URL.", true)]
    [Arguments("TokenOverride", "The token value to send.", true)]
    [Arguments("Token", "The token used to query the target cost per million tokens.", true)]
    public async Task Secret_Inference_Uses_Option_Local_Prose(string propertyName, string localDescription, bool secret)
    {
        var pipeline = new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance);
        var enhancer = new OptionTypeEnhancer(pipeline, NullLogger<OptionTypeEnhancer>.Instance);
        var tool = CreateTool(new CliOptionDefinition
        {
            SwitchName = "--file",
            PropertyName = propertyName,
            CSharpType = "string?",
            Description = "Parent group: path to a credential file. The token content. " + localDescription,
            ValueShapeDescription = localDescription,
        });
        var enhanced = await enhancer.EnhanceAsync(tool);
        await Assert.That(enhanced.Commands.Single().Options.Single().IsSecret).IsEqualTo(secret);
        var generated = await new OptionsClassGenerator().GenerateAsync(enhanced);
        await Assert.That(generated.Single().Content.Contains("[SecretValue]", StringComparison.Ordinal)).IsEqualTo(secret);
    }

    private static CliToolDefinition CreateTool(
        CliOptionDefinition option,
        string toolName = "docker",
        string commandName = "build")
    {
        return new CliToolDefinition
        {
            ToolName = toolName,
            NamespacePrefix = "Docker",
            TargetNamespace = "ModularPipelines.Docker",
            OutputDirectory = "src/ModularPipelines.Docker",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = $"{toolName} {commandName}",
                    CommandParts = [commandName],
                    ClassName = "DockerBuildOptions",
                    ParentClassName = "DockerOptions",
                    ToolNamespacePrefix = "Docker",
                    Options = [option],
                }
            ],
        };
    }

    private sealed class FixedDetector(OptionTypeDetectionResult result) : IOptionTypeDetector
    {
        public int Priority => 0;

        public string Name => result.Source;

        public bool CanHandle(string toolName) => true;

        public Task<OptionTypeDetectionResult> DetectTypeAsync(
            OptionDetectionContext context,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(result);
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning)
            {
                Messages.Add(formatter(state, exception));
            }
        }
    }
}
