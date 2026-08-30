using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task SharedParser_Recovers_Nested_Alternatives_And_Documentation()
    {
        const string section = """
              --mode=MODE
                 Select the operating mode.

                 Shorthand Example:
                   --example-only=literal

              At most one of these can be specified:
                --token=TOKEN
                   Authenticate with a token.

                Or at least one of these can be specified:
                  --profile=PROFILE
                     Select a saved profile.
                  --[no-]interactive
                     Enable or disable prompts.
            """;

        var group = TestArgumentGroupScraper.ParseGroups(section);
        var arguments = group.FlattenArguments().ToArray();

        await Assert.That(string.Join(" ", arguments.Select(argument => argument.SwitchName)))
            .IsEqualTo("--mode --token --profile --interactive");
        await Assert.That(group.Groups.Single().Kind)
            .IsEqualTo(CliArgumentGroupKind.AtMostOne);
        await Assert.That(group.Groups.Single().Groups.Single().Kind)
            .IsEqualTo(CliArgumentGroupKind.Alternative | CliArgumentGroupKind.AtLeastOne);
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--profile").Documentation)
            .Contains("At most one of these can be specified")
            .And.Contains("Or at least one of these can be specified")
            .And.Contains("Select a saved profile");
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--interactive").IsNegatable)
            .IsTrue();
    }

    [Test]
    public async Task SharedValidation_Rejects_Grouped_Declaration_Not_Emitted_As_Option()
    {
        const string helpText = """
            Fake help.

            FLAGS
              --parent=PARENT
                 Parent value.

              At most one of these can be specified:
                --nested=NESTED
                   Nested value.
            """;
        var scraper = new OmittingArgumentScraper(helpText);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands).IsEmpty();
    }

    [Test]
    public async Task SharedParser_Preserves_Undocumented_Sibling_And_Final_Declarations()
    {
        const string section = """
              --quiet
              --verbosity=VERBOSITY
                 Override the default verbosity.
              --recursive
            """;

        var arguments = TestArgumentGroupScraper.ParseGroups(section)
            .FlattenArguments()
            .ToArray();

        await Assert.That(arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--quiet", "--verbosity", "--recursive"]);
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--quiet").Description)
            .IsNull();
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--recursive").Description)
            .IsNull();
    }

    [Test]
    public async Task Gcloud_Emits_Long_Options_With_Short_Aliases()
    {
        const string helpText = """
            NAME
                gcloud compute instances list - list instances

            SYNOPSIS
                gcloud compute instances list

            FLAGS
                 --zone=ZONE, -z ZONE
                    Zone to list.
                 --tag=TAG, -t TAG

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "list"],
            helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--zone", "--tag"]);
    }

    [Test]
    public async Task Gcloud_Does_Not_Inherit_Previous_Option_Documentation_Into_Sibling_Groups()
    {
        const string helpText = """
            NAME
                gcloud functions deploy - deploy a function

            SYNOPSIS
                gcloud functions deploy

            FLAGS
                 --security-level=SECURITY_LEVEL; default="secure-always"
                    SECURITY_LEVEL must be one of: secure-always, secure-optional.

                 At most one of these can be specified:

                   --clear-max-instances
                      Clear the maximum instances setting.

                   --max-instances=MAX_INSTANCES
                      Set the maximum number of instances.

                 At most one of these can be specified:

                   --clear-min-instances
                      Clear the minimum instances setting.

                   --min-instances=MIN_INSTANCES
                      Set the minimum number of instances.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "functions", "deploy"],
            helpText);
        var maxInstances = command!.Options.Single(option => option.SwitchName == "--max-instances");
        var minInstances = command.Options.Single(option => option.SwitchName == "--min-instances");

        await Assert.That(maxInstances.Description!).DoesNotContain("--security-level");
        await Assert.That(maxInstances.EnumDefinition).IsNull();
        await Assert.That(minInstances.Description!).DoesNotContain("--security-level");
        await Assert.That(minInstances.EnumDefinition).IsNull();
    }

    [Test]
    public async Task Gcloud_Emits_Both_Forms_Of_Negatable_Flags()
    {
        const string helpText = """
            NAME
                gcloud run deploy - deploy a service

            SYNOPSIS
                gcloud run deploy

            FLAGS
                 --[no-]allow-unauthenticated
                    Use --allow-unauthenticated to enable and
                    --no-allow-unauthenticated to disable.

                 --launch-browser
                    Enabled by default, use --no-launch-browser to disable.

                 --use
                    This does not control --no-use-orchestrator.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "run", "deploy"],
            helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo([
                "--allow-unauthenticated",
                "--no-allow-unauthenticated",
                "--launch-browser",
                "--no-launch-browser",
                "--use",
            ]);
        await Assert.That(command.Options.Select(option => option.PropertyName))
            .IsEquivalentTo([
                "AllowUnauthenticated",
                "NoAllowUnauthenticated",
                "LaunchBrowser",
                "NoLaunchBrowser",
                "Use",
            ]);
    }

    [Test]
    public async Task Gcloud_Description_Negation_Of_Value_Option_Is_A_Flag()
    {
        const string helpText = """
            NAME
                gcloud compute disks update - update a disk

            SYNOPSIS
                gcloud compute disks update

            FLAGS
                 --boot-disk-size=SIZE
                    Set the boot disk size, or use --no-boot-disk-size to unset it.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "disks", "update"],
            helpText);
        var positive = command!.Options.Single(option => option.SwitchName == "--boot-disk-size");
        var negative = command.Options.Single(option => option.SwitchName == "--no-boot-disk-size");

        using (Assert.Multiple())
        {
            await Assert.That(positive.IsFlag).IsFalse();
            await Assert.That(negative.PropertyName).IsEqualTo("NoBootDiskSize");
            await Assert.That(negative.CSharpType).IsEqualTo("bool?");
            await Assert.That(negative.IsFlag).IsTrue();
            await Assert.That(negative.ValueSeparator).IsEqualTo(" ");
            await Assert.That(negative.IsNumeric).IsFalse();
        }
    }

    [Test]
    public async Task Gcloud_Numeric_Hints_Require_Whole_Tokens()
    {
        const string helpText = """
            NAME
                gcloud functions upgrade - upgrade a function

            SYNOPSIS
                gcloud functions upgrade

            FLAGS
                 --trigger-service-account=TRIGGER_SERVICE_ACCOUNT
                    IAM service-account email address.
                 --retry-count=RETRY_COUNT
                    Number of retries.
                 --disk-size=DISK_SIZE
                    Disk size.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "functions", "upgrade"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--trigger-service-account").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--retry-count").CSharpType)
                .IsEqualTo("int?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--disk-size").CSharpType)
                .IsEqualTo("int?");
        }
    }

    [Test]
    public async Task Gcloud_File_Hints_Take_Precedence_Over_Numeric_Tokens()
    {
        const string helpText = """
            NAME
                gcloud storage insights dataset-configs update - update a dataset config

            SYNOPSIS
                gcloud storage insights dataset-configs update

            FLAGS
                 --source-folders=FOLDER_NUMBERS,...
                    List of source folder IDs.
                 --source-folders-file=FOLDER_NUMBERS_FILE
                    CSV formatted file containing source folder IDs, one per line.
                 --source-projects=PROJECT_NUMBERS,...
                    List of source project numbers.
                 --source-projects-file=PROJECT_NUMBERS_FILE
                    CSV formatted file containing source project numbers, one per line.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "storage", "insights", "dataset-configs", "update"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--source-folders").CSharpType)
                .IsEqualTo("IEnumerable<int>?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-folders-file").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-projects").CSharpType)
                .IsEqualTo("IEnumerable<int>?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-projects-file").CSharpType)
                .IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gcloud_Structured_And_Categorical_Values_Remain_Textual()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --actions=ACTIONS
                    Shorthand Example: --actions=actionId=string. File Example: --actions=path_to_file.yaml.
                 --build-service-account=BUILD_SERVICE_ACCOUNT
                    Must be of the format projects/${PROJECT_ID}/serviceAccounts/${ACCOUNT_EMAIL_ADDRESS}.
                 --instance-size=INSTANCE_SIZE
                    INSTANCE_SIZE must be one of: extra-small Extra small instance size, maps to 0.1. small Small instance size, maps to 0.5.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);

        using (Assert.Multiple())
        {
            foreach (var option in command!.Options)
            {
                await Assert.That(option.CSharpType).IsEqualTo("string?");
            }
        }
    }

    [Test]
    public async Task Gcloud_Models_Declared_Value_Lists_As_Collections()
    {
        const string helpText = """
            NAME
                gcloud storage diagnose - diagnose storage performance

            SYNOPSIS
                gcloud storage diagnose

            FLAGS
                 --object-sizes=SIZES
                    List of object sizes to use for the tests. Sizes should be provided for each object specified using --object-count flag.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "storage", "diagnose"],
            helpText);
        var objectSizes = command!.Options.Single(option => option.SwitchName == "--object-sizes");

        await Assert.That(objectSizes.AcceptsMultipleValues).IsTrue();
        await Assert.That(objectSizes.CSharpType).IsEqualTo("IEnumerable<int>?");
    }

    [Test]
    public async Task Gcloud_Keeps_Identifiers_And_Composite_Values_Textual()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --billing-account=BILLING_ACCOUNT
                    Billing account of the resource.
                 --oauth-service-account-email=OAUTH_SERVICE_ACCOUNT_EMAIL
                    IAM service-account email address.
                 --lint-response-summary=COUNT
                    Shorthand Example: --lint-response-summary=count=int,severity=string. JSON Example: --lint-response-summary='[{"count": int}]'. File Example: --lint-response-summary=path_to_file.json.
                 --autoprovisioning-standard-rollout-policy=[BATCH_NODE_COUNT=...,...]
                    Standard rollout policy options for blue-green upgrade.
                 --max-accelerator=type=TYPE,count=COUNT
                    Sets the accelerator type and count.
                 --composite-application-parameters-service-account-map=SERVICE_ACCOUNT_MAP
                    Shorthand Example: --composite-application-parameters-service-account-map=string=string. JSON Example: --composite-application-parameters-service-account-map='{"string":"string"}'.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--billing-account").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--oauth-service-account-email").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--lint-response-summary").CSharpType)
                .IsEqualTo("string?");
            var rolloutPolicy = command.Options.Single(option =>
                option.SwitchName == "--autoprovisioning-standard-rollout-policy");
            await Assert.That(rolloutPolicy.CSharpType).IsEqualTo("string?");
            await Assert.That(rolloutPolicy.AcceptsMultipleValues).IsFalse();
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--max-accelerator").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--composite-application-parameters-service-account-map").CSharpType)
                .IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gcloud_Models_Repeatable_Options_As_Collections()
    {
        const string helpText = """
            NAME
                gcloud asset search-all-resources - search all resources

            SYNOPSIS
                gcloud asset search-all-resources

            FLAGS
                 --order-by=FIELD
                    This flag can be repeated to provide a list of fields.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "asset", "search-all-resources"],
            helpText);
        var orderBy = command!.Options.Single(option => option.SwitchName == "--order-by");

        await Assert.That(orderBy.AcceptsMultipleValues).IsTrue();
        await Assert.That(orderBy.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Gcloud_Uses_Whole_Option_Block_For_Repeatability()
    {
        const string helpText = """
            NAME
                gcloud compute instances create - create an instance

            SYNOPSIS
                gcloud compute instances create

            FLAGS
                 --ipv6-public-ptr-domain=DOMAIN
                    Domain for the public PTR record.
                 This flag can be repeated to configure multiple domains.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "create"],
            helpText);
        var option = command!.Options.Single(candidate =>
            candidate.SwitchName == "--ipv6-public-ptr-domain");

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Gcloud_Repeatable_Key_Value_Option_Keeps_Key_Value_Type()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --metadata=KEY=VALUE
                    This flag can be repeated to add metadata entries.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);
        var metadata = command!.Options.Single(option => option.SwitchName == "--metadata");

        await Assert.That(metadata.AcceptsMultipleValues).IsTrue();
        await Assert.That(metadata.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
    }

    [Test]
    public async Task Gcloud_External_Ipv6_Prefix_Length_Is_Known_Scalar()
    {
        var scraper = CreateGcloudScraper();

        await Assert.That(scraper.IsKnownScalar(
                ["compute", "instances", "create"],
                "--external-ipv6-prefix-length"))
            .IsTrue();
    }

    [Test]
    public async Task Gcloud_Prioritizes_Enum_Types_Over_Key_Value_Hints()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --labels=KEY=VALUE,...
                    Value must be one of: alpha, beta.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);
        var labels = command!.Options.Single(option => option.SwitchName == "--labels");

        await Assert.That(labels.CSharpType).IsEqualTo("GcloudLabels?");
        await Assert.That(labels.EnumDefinition).IsNotNull();
    }

    [Test]
    public async Task Gcloud_Does_Not_Mask_Secret_Named_File_Path_Options()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example

            SYNOPSIS
                gcloud example create

            FLAGS
                 --credential=CREDENTIAL
                    Path to the credential file.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "create"],
            helpText);
        var credential = command!.Options.Single(option => option.SwitchName == "--credential");

        await Assert.That(credential.IsSecret).IsFalse();
    }

    [Test]
    public async Task GcloudAgentIdentityUpdate_Emits_Nested_Scope_Credential_And_Negatable_Flags()
    {
        const string helpText = """
            NAME
                gcloud agent-identity auth-providers update - update an auth provider

            SYNOPSIS
                gcloud agent-identity auth-providers update

            FLAGS
                 --request-id=REQUEST_ID
                    An optional request ID.

                 Update allowed_scopes.

                 At most one of these can be specified:
                   --allowed-scopes=[ALLOWED_SCOPES,...]
                      Set allowed_scopes to a new value.

                   Or at least one of these can be specified:
                     --add-allowed-scopes=[ADD_ALLOWED_SCOPES,...]
                        Add values to allowed_scopes.

                     At most one of these can be specified:
                       --clear-allowed-scopes
                          Clear allowed_scopes.
                       --remove-allowed-scopes=[REMOVE_ALLOWED_SCOPES,...]
                          Remove values from allowed_scopes.

                 AuthProvider type specific parameters.

                   --clear-auth-provider-type-params
                      Reset type parameters.

                   At most one of these can be specified:
                     --api-key=API_KEY
                        The API key for this auth provider.
                     --three-legged-oauth-client-secret=THREE_LEGGED_OAUTH_CLIENT_SECRET
                        The OAuth client secret.
                     --[no-]three-legged-oauth-enable-pkce
                        Enable or disable PKCE.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;
        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "agent-identity", "auth-providers", "update"],
            helpText);
        var switches = command!.Options.Select(option => option.SwitchName).ToArray();

        await Assert.That(switches).Contains("--allowed-scopes");
        await Assert.That(switches).Contains("--add-allowed-scopes");
        await Assert.That(switches).Contains("--clear-allowed-scopes");
        await Assert.That(switches).Contains("--remove-allowed-scopes");
        await Assert.That(switches).Contains("--api-key");
        await Assert.That(switches).Contains("--three-legged-oauth-client-secret");
        await Assert.That(switches).Contains("--three-legged-oauth-enable-pkce");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--request-id").Description!)
            .DoesNotContain("--allowed-scopes");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--three-legged-oauth-client-secret").IsSecret)
            .IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--three-legged-oauth-enable-pkce").IsFlag)
            .IsTrue();
    }

    [Test]
    public async Task GcloudDeveloperConnectCreate_Emits_Nested_Resource_And_Secret_Flags()
    {
        const string helpText = """
            NAME
                gcloud developer-connect connections create - create a connection resource

            SYNOPSIS
                gcloud developer-connect connections create CONNECTION

            FLAGS
                 --validate-only
                    If set, validate the request without posting it.

                 The crypto key configuration. The arguments in this resource group
                 can specify its attributes.

                   --crypto-key-config-reference=CRYPTO_KEY_CONFIG_REFERENCE
                      ID of the cryptoKey.
                   --key-ring=KEY_RING
                      The keyRing ID.

                 Arguments for the connection config.

                 At most one of these can be specified:
                   Configuration for Bitbucket Cloud.

                     --bitbucket-cloud-config-authorizer-credential-user-token-secret-version=BITBUCKET_SECRET_VERSION
                        SecretManager version containing the user token.
                     --bitbucket-cloud-config-workspace=BITBUCKET_WORKSPACE
                        The workspace ID.

                   Configuration for an HTTP provider.

                     Arguments for the authentication.

                     At most one of these can be specified:
                       --http-config-bearer-token-authentication-secret-version=HTTP_BEARER_SECRET_VERSION
                          SecretManager version containing the bearer token.
                       --http-config-basic-authentication-password-secret-version=HTTP_PASSWORD_SECRET_VERSION
                          SecretManager version containing the password.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;
        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "developer-connect", "connections", "create"],
            helpText);
        var switches = command!.Options.Select(option => option.SwitchName).ToArray();

        await Assert.That(switches).Contains("--crypto-key-config-reference");
        await Assert.That(switches).Contains("--key-ring");
        await Assert.That(switches)
            .Contains("--bitbucket-cloud-config-authorizer-credential-user-token-secret-version");
        await Assert.That(switches)
            .Contains("--http-config-bearer-token-authentication-secret-version");
        await Assert.That(switches)
            .Contains("--http-config-basic-authentication-password-secret-version");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--validate-only").Description!)
            .DoesNotContain("--crypto-key-config-reference");
        await Assert.That(command.Options
                .Single(option => option.SwitchName == "--http-config-basic-authentication-password-secret-version")
                .IsSecret)
            .IsTrue();
        await Assert.That(command.ArgumentGroups.Single().Groups)
            .Contains(group => group.Kind.HasFlag(CliArgumentGroupKind.Resource));
    }

    private static TestGcloudScraper CreateGcloudScraper() =>
        new(new EmptyExecutor());

    private sealed class TestGcloudScraper(ICliCommandExecutor executor)
        : GcloudCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<GcloudCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);

        public bool IsKnownScalar(IReadOnlyList<string> commandParts, string switchName) =>
            ShouldTreatOptionAsScalar(commandParts, switchName);
    }

    private sealed class TestArgumentGroupScraper : CliScraperBase
    {
        private TestArgumentGroupScraper()
            : base(
                new EmptyExecutor(),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestArgumentGroupScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        public static CliArgumentGroup ParseGroups(string section) =>
            ParseArgumentGroups(section, ParseArgument);

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(null);

        private static CliArgumentDefinition? ParseArgument(string line)
        {
            var match = ArgumentPattern().Match(line);
            if (!match.Success)
            {
                return null;
            }

            var negatable = match.Groups["negatable"].Success;
            return new CliArgumentDefinition
            {
                SwitchName = negatable
                    ? $"--{match.Groups["negatableName"].Value}"
                    : match.Groups["long"].Value,
                ValueHint = match.Groups["value"].Value,
                IsNegatable = negatable,
                Indentation = match.Groups["indent"].Value.Length,
            };
        }
    }

    private sealed class OmittingArgumentScraper(string helpText) : TestArgumentGroupScraperBase(helpText)
    {
        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string commandHelpText,
            CancellationToken cancellationToken)
        {
            var group = ParseArgumentGroups(
                commandHelpText[(commandHelpText.IndexOf("FLAGS", StringComparison.Ordinal) + "FLAGS".Length)..],
                ParseNeutralArgument);
            var parent = group.FlattenArguments().Single(argument => argument.SwitchName == "--parent");

            return Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = parent.SwitchName,
                        PropertyName = "Parent",
                        CSharpType = "string?",
                        Description = parent.Documentation,
                    },
                ],
                ArgumentGroups = [group],
            });
        }
    }

    private abstract class TestArgumentGroupScraperBase(string helpText) : CliScraperBase(
        new SingleHelpExecutor(helpText),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<TestArgumentGroupScraperBase>.Instance)
    {
        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string commandHelpText) => [];

        protected static CliArgumentDefinition? ParseNeutralArgument(string line)
        {
            var match = ArgumentPattern().Match(line);
            return !match.Success
                ? null
                : new CliArgumentDefinition
                {
                    SwitchName = match.Groups["long"].Value,
                    ValueHint = match.Groups["value"].Value,
                    Indentation = match.Groups["indent"].Value.Length,
                };
        }
    }

    private sealed class SingleHelpExecutor(string helpText) : EmptyExecutor
    {
        public override Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
    }

    private class EmptyExecutor : ICliCommandExecutor
    {
        public virtual Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new InvalidOperationException("Execution was not expected.");

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    [GeneratedRegex(
        @"^(?<indent>\s+)(?:(?<negatable>--\[no-\])(?<negatableName>[\w-]+)|(?<long>--[\w-]+))(?:=(?<value>\S+))?$")]
    private static partial Regex ArgumentPattern();
}
