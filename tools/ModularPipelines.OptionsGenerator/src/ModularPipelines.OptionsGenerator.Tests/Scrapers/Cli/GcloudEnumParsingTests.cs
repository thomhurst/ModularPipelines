using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudEnumParsingTests
{
    [Test]
    [Arguments("security-posture", "SECURITY_POSTURE", "disabled, standard, enterprise", "disabled,standard,enterprise")]
    [Arguments("binauthz-evaluation-mode", "BINAUTHZ_EVALUATION_MODE", "disabled, policy-bindings", "disabled,policy-bindings")]
    [Arguments("workload-vulnerability-scanning", "WORKLOAD_VULNERABILITY_SCANNING", "disabled, standard, enterprise", "disabled,standard,enterprise")]
    [Arguments("value", "VALUE", "alpha, beta and gamma", "alpha,beta,gamma")]
    [Arguments("value", "VALUE", "alpha, beta or gamma", "alpha,beta,gamma")]
    public async Task Standalone_Declarations_After_Command_Examples_Generate_Enums(
        string flag, string metavar, string choices, string expected)
    {
        var exampleValue = expected.Split(',')[^1];
        var help = $$"""
            SYNOPSIS
                gcloud container fleet create [--{{flag}}={{metavar}}]
            FLAGS
                 --{{flag}}={{metavar}}
                    Configure clusters to onboard the fleet:

                      $ gcloud container fleet create \
                          --{{flag}}={{exampleValue}}

                    {{metavar}} must be one
                    of:
                    {{choices}}.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("container fleet create", help)).Single();
        var option = command.Options.Single();

        await Assert.That(option.EnumDefinition).IsNotNull();
        await Assert.That(option.Description).DoesNotContain("\n");
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(expected.Split(','));
        var generated = (await new EnumGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        foreach (var value in expected.Split(','))
        {
            await Assert.That(generated).Contains($"[EnumValue(\"{value}\")]");
        }
    }

    [Test]
    [Arguments("If the scheme is INTERNAL,\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("If the scheme is INTERNAL, PROTOCOL\n        must be one of: TCP, UDP.")]
    [Arguments("If the scheme is INTERNAL:\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("If the scheme is INTERNAL,\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("For internal traffic:\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("When the scheme is INTERNAL\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("SCHEME must be one of: INTERNAL, EXTERNAL.")]
    public async Task Paragraphs_After_Examples_Do_Not_Promote_Conditional_Or_Unrelated_Lists(string declaration)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--protocol=PROTOCOL]
            FLAGS
                 --protocol=PROTOCOL
                    Select a protocol:

                      $ gcloud example update --protocol=TCP

                    {{declaration}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();

        await Assert.That(command.Options.Single().EnumDefinition).IsNull();
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("string?");
    }

    [Test]
    [Arguments("If the scheme is INTERNAL, the protocol must be one of: TCP, UDP, L3_DEFAULT.")]
    [Arguments("If the scheme is INTERNAL, IP_PROTOCOL must be one of: TCP, UDP, L3_DEFAULT.")]
    [Arguments("For internal traffic, IP_PROTOCOL must be one of: TCP, UDP, L3_DEFAULT.")]
    public async Task Unconditional_Metavariable_List_Takes_Precedence(string condition)
    {
        var help = $$"""
            SYNOPSIS
                gcloud compute forwarding-rules create [--ip-protocol=IP_PROTOCOL]
            FLAGS
                 --ip-protocol=IP_PROTOCOL
                    IP protocol that the rule will serve. {{condition}}
                    IP_PROTOCOL must be one of: AH, ESP, ICMP, SCTP, TCP, UDP, L3_DEFAULT.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("compute forwarding-rules create", help)).Single();
        var definition = command.Options.Single().EnumDefinition;

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["AH", "ESP", "ICMP", "SCTP", "TCP", "UDP", "L3_DEFAULT"]);
    }

    [Test]
    [Arguments("If the scheme is INTERNAL, the protocol must be one of: TCP, UDP, UNSPECIFIED. If the scheme is EXTERNAL, the protocol must be one of: HTTP, HTTPS, HTTP2, TCP, SSL.")]
    [Arguments("If the scheme is INTERNAL, PROTOCOL must be one of: TCP, UDP, UNSPECIFIED.")]
    [Arguments("The related SCHEME must be one of: INTERNAL, EXTERNAL.")]
    [Arguments("SCHEME must be one of: INTERNAL, EXTERNAL.")]
    [Arguments("The SCHEME must be one of: INTERNAL, EXTERNAL.")]
    [Arguments("If the scheme is INTERNAL, the PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("If the scheme is INTERNAL:\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("If the scheme is INTERNAL,\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("For internal traffic:\n\n        PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("When the scheme is INTERNAL\n\n        PROTOCOL must be one of: TCP, UDP.")]
    public async Task Conditional_Or_Unrelated_Lists_Leave_Option_As_String(string description)
    {
        var help = $$"""
            SYNOPSIS
                gcloud compute backend-services create [--protocol=PROTOCOL]
            FLAGS
                 --protocol=PROTOCOL
                    {{description}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("compute backend-services create", help)).Single();
        var option = command.Options.Single();

        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
    }

    [Test]
    [Arguments("VALUE must be one of: alpha, beta. VALUE must be one of: beta, gamma.", "alpha,beta,gamma")]
    [Arguments("Must be one of: alpha, beta.", "alpha,beta")]
    [Arguments("VALUE must be one of: alpha, beta. If another setting is enabled, VALUE must be one of: beta, gamma.", "alpha,beta")]
    public async Task Unconditional_Lists_Preserve_All_Declared_Values(string description, string expected)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                 --value=VALUE
                    {{description}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var definition = command.Options.Single().EnumDefinition;

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(expected.Split(','));
    }

    [Test]
    [Arguments("PROTOCOL", "Value must be one of: TCP, UDP.")]
    [Arguments("PROTOCOL", "The protocol must be one of: TCP, UDP.")]
    [Arguments("PROTOCOL", "The value must be one of: TCP, UDP.")]
    [Arguments("PROTOCOL", "Select a protocol. The PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("PROTOCOL ; default=TCP", "PROTOCOL must be one of: TCP, UDP.")]
    [Arguments("PROTOCOL", "PROTOCOL must be one of: A, B, C, D, E, F, G, H, I, J, K, L, M. Select a protocol; one of TCP, UDP.")]
    public async Task Enum_Lists_Support_Generic_Subjects_Whitespace_And_Inline_Fallback(string valueHint, string description)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--protocol=PROTOCOL]
            FLAGS
                 --protocol={{valueHint}}
                    {{description}}
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var definition = command.Options.Single().EnumDefinition;

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(["TCP", "UDP"]);
    }

    [Test]
    [Arguments("VALUE must be one of:", "IMMEDIATE, NEXT_AVAILABLE_WINDOW, or SPECIFIC-TIME", "IMMEDIATE,NEXT_AVAILABLE_WINDOW,SPECIFIC-TIME")]
    [Arguments("VALUE must be one of:", "alpha, beta, and gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, or beta", "alpha,beta")]
    [Arguments("VALUE must be one of:", "alpha, AND beta", "alpha,beta")]
    [Arguments("VALUE must be one of:", "alpha, beta, gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, or", "alpha,or")]
    [Arguments("VALUE must be one of:", "alpha, and", "alpha,and")]
    [Arguments("VALUE must be one of:", "alpha, orange, android", "alpha,orange,android")]
    [Arguments("VALUE must be one of:", "alpha and beta", "alpha,beta")]
    [Arguments("VALUE must be one of:", "alpha, beta and gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, beta or\n        gamma", "alpha,beta,gamma")]
    [Arguments("VALUE must be one of:", "alpha, and, or", "alpha,and,or")]
    [Arguments("VALUE must be one of:", "alpha or or", "alpha,or")]
    [Arguments("Select a value; one of", "alpha, beta, or gamma", "alpha,beta,gamma")]
    [Arguments("Select a value; one of", "alpha, and beta", "alpha,beta")]
    [Arguments("Select a value; one of", "alpha, beta", "alpha,beta")]
    [Arguments("Select a value; one of", "alpha, or", "alpha,or")]
    [Arguments("Select a value; one of", "alpha and beta", "alpha,beta")]
    [Arguments("Select a value; one of", "alpha, beta or gamma", "alpha,beta,gamma")]
    [Arguments("Select a value; one of", "alpha, and, or", "alpha,and,or")]
    [Arguments("Select a value; one of", "alpha and and", "alpha,and")]
    public async Task Enum_Lists_Preserve_Values_Without_Conjunction_Prose(string prefix, string choices, string expected)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                 --value=VALUE
                    {{prefix}} {{choices}}.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();
        var definition = command.Options.Single().EnumDefinition;

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(expected.Split(','));
    }

    [Test]
    [Arguments("VALUE must be one of:", "alpha, beta and 3gamma")]
    [Arguments("VALUE must be one of:", "alpha, beta, 3gamma")]
    [Arguments("Select a value; one of", "alpha, beta or 3gamma")]
    [Arguments("Select a value; one of", "alpha, beta, 3gamma")]
    [Arguments("VALUE must be one of:", "alpha, beta and a_choice_exceeding_the_length_limit")]
    [Arguments("Select a value; one of", "alpha, beta, a_choice_exceeding_the_length_limit")]
    public async Task Unsupported_Choice_List_Tails_Do_Not_Generate_Partial_Enums(string prefix, string choices)
    {
        var help = $$"""
            SYNOPSIS
                gcloud example update [--value=VALUE]
            FLAGS
                 --value=VALUE
                    {{prefix}} {{choices}}.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example update", help)).Single();

        await Assert.That(command.Options.Single().EnumDefinition).IsNull();
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Required_Reschedule_Type_Generates_Specific_Time_Enum_Value()
    {
        const string help = """
            NAME
                gcloud redis instances reschedule-maintenance - reschedule maintenance
            SYNOPSIS
                gcloud redis instances reschedule-maintenance INSTANCE --reschedule-type=RESCHEDULE_TYPE
            POSITIONAL ARGUMENTS
                 INSTANCE
                    The Redis instance to reschedule.
            REQUIRED FLAGS
                 --reschedule-type=RESCHEDULE_TYPE
                    RESCHEDULE_TYPE must be one of: IMMEDIATE, NEXT_AVAILABLE_WINDOW, or
                    SPECIFIC-TIME.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("redis instances reschedule-maintenance", help)).Single();
        var option = command.Options.Single();
        await Assert.That(option.IsRequired).IsTrue();
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["IMMEDIATE", "NEXT_AVAILABLE_WINDOW", "SPECIFIC-TIME"]);

        var generated = (await new EnumGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;

        await Assert.That(generated).Contains("[EnumValue(\"SPECIFIC-TIME\")]");
        await Assert.That(generated).DoesNotContain("[EnumValue(\"or\")]");
    }
}
