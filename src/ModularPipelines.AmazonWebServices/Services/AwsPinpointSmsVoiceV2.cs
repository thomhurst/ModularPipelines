using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPinpointSmsVoiceV2
{
    public AwsPinpointSmsVoiceV2(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateOriginationIdentity(AwsPinpointSmsVoiceV2AssociateOriginationIdentityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointSmsVoiceV2CreateConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEventDestination(AwsPinpointSmsVoiceV2CreateEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateOptOutList(AwsPinpointSmsVoiceV2CreateOptOutListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePool(AwsPinpointSmsVoiceV2CreatePoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRegistration(AwsPinpointSmsVoiceV2CreateRegistrationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRegistrationAssociation(AwsPinpointSmsVoiceV2CreateRegistrationAssociationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRegistrationAttachment(AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateRegistrationVersion(AwsPinpointSmsVoiceV2CreateRegistrationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateVerifiedDestinationNumber(AwsPinpointSmsVoiceV2CreateVerifiedDestinationNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointSmsVoiceV2DeleteConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDefaultMessageType(AwsPinpointSmsVoiceV2DeleteDefaultMessageTypeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDefaultSenderId(AwsPinpointSmsVoiceV2DeleteDefaultSenderIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEventDestination(AwsPinpointSmsVoiceV2DeleteEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteKeyword(AwsPinpointSmsVoiceV2DeleteKeywordOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteOptOutList(AwsPinpointSmsVoiceV2DeleteOptOutListOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteOptedOutNumber(AwsPinpointSmsVoiceV2DeleteOptedOutNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePool(AwsPinpointSmsVoiceV2DeletePoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRegistration(AwsPinpointSmsVoiceV2DeleteRegistrationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRegistrationAttachment(AwsPinpointSmsVoiceV2DeleteRegistrationAttachmentOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteRegistrationFieldValue(AwsPinpointSmsVoiceV2DeleteRegistrationFieldValueOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTextMessageSpendLimitOverride(AwsPinpointSmsVoiceV2DeleteTextMessageSpendLimitOverrideOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DeleteTextMessageSpendLimitOverrideOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVerifiedDestinationNumber(AwsPinpointSmsVoiceV2DeleteVerifiedDestinationNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteVoiceMessageSpendLimitOverride(AwsPinpointSmsVoiceV2DeleteVoiceMessageSpendLimitOverrideOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DeleteVoiceMessageSpendLimitOverrideOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsPinpointSmsVoiceV2DescribeAccountAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeAccountAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsPinpointSmsVoiceV2DescribeAccountLimitsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeAccountLimitsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConfigurationSets(AwsPinpointSmsVoiceV2DescribeConfigurationSetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeConfigurationSetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeKeywords(AwsPinpointSmsVoiceV2DescribeKeywordsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOptOutLists(AwsPinpointSmsVoiceV2DescribeOptOutListsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeOptOutListsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeOptedOutNumbers(AwsPinpointSmsVoiceV2DescribeOptedOutNumbersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePhoneNumbers(AwsPinpointSmsVoiceV2DescribePhoneNumbersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribePhoneNumbersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePools(AwsPinpointSmsVoiceV2DescribePoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribePoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationAttachments(AwsPinpointSmsVoiceV2DescribeRegistrationAttachmentsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationAttachmentsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationFieldDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationFieldDefinitionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationFieldValues(AwsPinpointSmsVoiceV2DescribeRegistrationFieldValuesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationSectionDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationSectionDefinitionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationTypeDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationTypeDefinitionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationTypeDefinitionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrationVersions(AwsPinpointSmsVoiceV2DescribeRegistrationVersionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRegistrations(AwsPinpointSmsVoiceV2DescribeRegistrationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSenderIds(AwsPinpointSmsVoiceV2DescribeSenderIdsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeSenderIdsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeSpendLimits(AwsPinpointSmsVoiceV2DescribeSpendLimitsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeSpendLimitsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeVerifiedDestinationNumbers(AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateOriginationIdentity(AwsPinpointSmsVoiceV2DisassociateOriginationIdentityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DiscardRegistrationVersion(AwsPinpointSmsVoiceV2DiscardRegistrationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPoolOriginationIdentities(AwsPinpointSmsVoiceV2ListPoolOriginationIdentitiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRegistrationAssociations(AwsPinpointSmsVoiceV2ListRegistrationAssociationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsPinpointSmsVoiceV2ListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutKeyword(AwsPinpointSmsVoiceV2PutKeywordOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutOptedOutNumber(AwsPinpointSmsVoiceV2PutOptedOutNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutRegistrationFieldValue(AwsPinpointSmsVoiceV2PutRegistrationFieldValueOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReleasePhoneNumber(AwsPinpointSmsVoiceV2ReleasePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReleaseSenderId(AwsPinpointSmsVoiceV2ReleaseSenderIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestPhoneNumber(AwsPinpointSmsVoiceV2RequestPhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestSenderId(AwsPinpointSmsVoiceV2RequestSenderIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendDestinationNumberVerificationCode(AwsPinpointSmsVoiceV2SendDestinationNumberVerificationCodeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendTextMessage(AwsPinpointSmsVoiceV2SendTextMessageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendVoiceMessage(AwsPinpointSmsVoiceV2SendVoiceMessageOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SetDefaultMessageType(AwsPinpointSmsVoiceV2SetDefaultMessageTypeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SetDefaultSenderId(AwsPinpointSmsVoiceV2SetDefaultSenderIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SetTextMessageSpendLimitOverride(AwsPinpointSmsVoiceV2SetTextMessageSpendLimitOverrideOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SetVoiceMessageSpendLimitOverride(AwsPinpointSmsVoiceV2SetVoiceMessageSpendLimitOverrideOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SubmitRegistrationVersion(AwsPinpointSmsVoiceV2SubmitRegistrationVersionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsPinpointSmsVoiceV2TagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsPinpointSmsVoiceV2UntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateEventDestination(AwsPinpointSmsVoiceV2UpdateEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePhoneNumber(AwsPinpointSmsVoiceV2UpdatePhoneNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePool(AwsPinpointSmsVoiceV2UpdatePoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSenderId(AwsPinpointSmsVoiceV2UpdateSenderIdOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> VerifyDestinationNumber(AwsPinpointSmsVoiceV2VerifyDestinationNumberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}