using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociateOriginationIdentity(AwsPinpointSmsVoiceV2AssociateOriginationIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointSmsVoiceV2CreateConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEventDestination(AwsPinpointSmsVoiceV2CreateEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateOptOutList(AwsPinpointSmsVoiceV2CreateOptOutListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePool(AwsPinpointSmsVoiceV2CreatePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegistration(AwsPinpointSmsVoiceV2CreateRegistrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegistrationAssociation(AwsPinpointSmsVoiceV2CreateRegistrationAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateRegistrationAttachment(AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2CreateRegistrationAttachmentOptions(), token);
    }

    public async Task<CommandResult> CreateRegistrationVersion(AwsPinpointSmsVoiceV2CreateRegistrationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVerifiedDestinationNumber(AwsPinpointSmsVoiceV2CreateVerifiedDestinationNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointSmsVoiceV2DeleteConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDefaultMessageType(AwsPinpointSmsVoiceV2DeleteDefaultMessageTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDefaultSenderId(AwsPinpointSmsVoiceV2DeleteDefaultSenderIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEventDestination(AwsPinpointSmsVoiceV2DeleteEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteKeyword(AwsPinpointSmsVoiceV2DeleteKeywordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOptOutList(AwsPinpointSmsVoiceV2DeleteOptOutListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteOptedOutNumber(AwsPinpointSmsVoiceV2DeleteOptedOutNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePool(AwsPinpointSmsVoiceV2DeletePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistration(AwsPinpointSmsVoiceV2DeleteRegistrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistrationAttachment(AwsPinpointSmsVoiceV2DeleteRegistrationAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteRegistrationFieldValue(AwsPinpointSmsVoiceV2DeleteRegistrationFieldValueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTextMessageSpendLimitOverride(AwsPinpointSmsVoiceV2DeleteTextMessageSpendLimitOverrideOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DeleteTextMessageSpendLimitOverrideOptions(), token);
    }

    public async Task<CommandResult> DeleteVerifiedDestinationNumber(AwsPinpointSmsVoiceV2DeleteVerifiedDestinationNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVoiceMessageSpendLimitOverride(AwsPinpointSmsVoiceV2DeleteVoiceMessageSpendLimitOverrideOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DeleteVoiceMessageSpendLimitOverrideOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountAttributes(AwsPinpointSmsVoiceV2DescribeAccountAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeAccountAttributesOptions(), token);
    }

    public async Task<CommandResult> DescribeAccountLimits(AwsPinpointSmsVoiceV2DescribeAccountLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeAccountLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationSets(AwsPinpointSmsVoiceV2DescribeConfigurationSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeConfigurationSetsOptions(), token);
    }

    public async Task<CommandResult> DescribeKeywords(AwsPinpointSmsVoiceV2DescribeKeywordsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOptOutLists(AwsPinpointSmsVoiceV2DescribeOptOutListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeOptOutListsOptions(), token);
    }

    public async Task<CommandResult> DescribeOptedOutNumbers(AwsPinpointSmsVoiceV2DescribeOptedOutNumbersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePhoneNumbers(AwsPinpointSmsVoiceV2DescribePhoneNumbersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribePhoneNumbersOptions(), token);
    }

    public async Task<CommandResult> DescribePools(AwsPinpointSmsVoiceV2DescribePoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribePoolsOptions(), token);
    }

    public async Task<CommandResult> DescribeRegistrationAttachments(AwsPinpointSmsVoiceV2DescribeRegistrationAttachmentsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationAttachmentsOptions(), token);
    }

    public async Task<CommandResult> DescribeRegistrationFieldDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationFieldDefinitionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegistrationFieldValues(AwsPinpointSmsVoiceV2DescribeRegistrationFieldValuesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegistrationSectionDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationSectionDefinitionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegistrationTypeDefinitions(AwsPinpointSmsVoiceV2DescribeRegistrationTypeDefinitionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationTypeDefinitionsOptions(), token);
    }

    public async Task<CommandResult> DescribeRegistrationVersions(AwsPinpointSmsVoiceV2DescribeRegistrationVersionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRegistrations(AwsPinpointSmsVoiceV2DescribeRegistrationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeRegistrationsOptions(), token);
    }

    public async Task<CommandResult> DescribeSenderIds(AwsPinpointSmsVoiceV2DescribeSenderIdsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeSenderIdsOptions(), token);
    }

    public async Task<CommandResult> DescribeSpendLimits(AwsPinpointSmsVoiceV2DescribeSpendLimitsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeSpendLimitsOptions(), token);
    }

    public async Task<CommandResult> DescribeVerifiedDestinationNumbers(AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointSmsVoiceV2DescribeVerifiedDestinationNumbersOptions(), token);
    }

    public async Task<CommandResult> DisassociateOriginationIdentity(AwsPinpointSmsVoiceV2DisassociateOriginationIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DiscardRegistrationVersion(AwsPinpointSmsVoiceV2DiscardRegistrationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPoolOriginationIdentities(AwsPinpointSmsVoiceV2ListPoolOriginationIdentitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRegistrationAssociations(AwsPinpointSmsVoiceV2ListRegistrationAssociationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsPinpointSmsVoiceV2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutKeyword(AwsPinpointSmsVoiceV2PutKeywordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutOptedOutNumber(AwsPinpointSmsVoiceV2PutOptedOutNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRegistrationFieldValue(AwsPinpointSmsVoiceV2PutRegistrationFieldValueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleasePhoneNumber(AwsPinpointSmsVoiceV2ReleasePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReleaseSenderId(AwsPinpointSmsVoiceV2ReleaseSenderIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestPhoneNumber(AwsPinpointSmsVoiceV2RequestPhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestSenderId(AwsPinpointSmsVoiceV2RequestSenderIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendDestinationNumberVerificationCode(AwsPinpointSmsVoiceV2SendDestinationNumberVerificationCodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendTextMessage(AwsPinpointSmsVoiceV2SendTextMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendVoiceMessage(AwsPinpointSmsVoiceV2SendVoiceMessageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDefaultMessageType(AwsPinpointSmsVoiceV2SetDefaultMessageTypeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDefaultSenderId(AwsPinpointSmsVoiceV2SetDefaultSenderIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetTextMessageSpendLimitOverride(AwsPinpointSmsVoiceV2SetTextMessageSpendLimitOverrideOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetVoiceMessageSpendLimitOverride(AwsPinpointSmsVoiceV2SetVoiceMessageSpendLimitOverrideOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SubmitRegistrationVersion(AwsPinpointSmsVoiceV2SubmitRegistrationVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsPinpointSmsVoiceV2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsPinpointSmsVoiceV2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEventDestination(AwsPinpointSmsVoiceV2UpdateEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePhoneNumber(AwsPinpointSmsVoiceV2UpdatePhoneNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePool(AwsPinpointSmsVoiceV2UpdatePoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSenderId(AwsPinpointSmsVoiceV2UpdateSenderIdOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyDestinationNumber(AwsPinpointSmsVoiceV2VerifyDestinationNumberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}