using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsServicecatalog
{
    public AwsServicecatalog(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AcceptPortfolioShare(AwsServicecatalogAcceptPortfolioShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateBudgetWithResource(AwsServicecatalogAssociateBudgetWithResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociatePrincipalWithPortfolio(AwsServicecatalogAssociatePrincipalWithPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateProductWithPortfolio(AwsServicecatalogAssociateProductWithPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateServiceActionWithProvisioningArtifact(AwsServicecatalogAssociateServiceActionWithProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AssociateTagOptionWithResource(AwsServicecatalogAssociateTagOptionWithResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchAssociateServiceActionWithProvisioningArtifact(AwsServicecatalogBatchAssociateServiceActionWithProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDisassociateServiceActionFromProvisioningArtifact(AwsServicecatalogBatchDisassociateServiceActionFromProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CopyProduct(AwsServicecatalogCopyProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConstraint(AwsServicecatalogCreateConstraintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePortfolio(AwsServicecatalogCreatePortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePortfolioShare(AwsServicecatalogCreatePortfolioShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProduct(AwsServicecatalogCreateProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisionedProductPlan(AwsServicecatalogCreateProvisionedProductPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateProvisioningArtifact(AwsServicecatalogCreateProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateServiceAction(AwsServicecatalogCreateServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTagOption(AwsServicecatalogCreateTagOptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConstraint(AwsServicecatalogDeleteConstraintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePortfolio(AwsServicecatalogDeletePortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePortfolioShare(AwsServicecatalogDeletePortfolioShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProduct(AwsServicecatalogDeleteProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProvisionedProductPlan(AwsServicecatalogDeleteProvisionedProductPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteProvisioningArtifact(AwsServicecatalogDeleteProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteServiceAction(AwsServicecatalogDeleteServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTagOption(AwsServicecatalogDeleteTagOptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConstraint(AwsServicecatalogDescribeConstraintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCopyProductStatus(AwsServicecatalogDescribeCopyProductStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePortfolio(AwsServicecatalogDescribePortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePortfolioShareStatus(AwsServicecatalogDescribePortfolioShareStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribePortfolioShares(AwsServicecatalogDescribePortfolioSharesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProduct(AwsServicecatalogDescribeProductOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProductOptions(), token);
    }

    public async Task<CommandResult> DescribeProductAsAdmin(AwsServicecatalogDescribeProductAsAdminOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProductAsAdminOptions(), token);
    }

    public async Task<CommandResult> DescribeProductView(AwsServicecatalogDescribeProductViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProvisionedProduct(AwsServicecatalogDescribeProvisionedProductOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisionedProductOptions(), token);
    }

    public async Task<CommandResult> DescribeProvisionedProductPlan(AwsServicecatalogDescribeProvisionedProductPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeProvisioningArtifact(AwsServicecatalogDescribeProvisioningArtifactOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisioningArtifactOptions(), token);
    }

    public async Task<CommandResult> DescribeProvisioningParameters(AwsServicecatalogDescribeProvisioningParametersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisioningParametersOptions(), token);
    }

    public async Task<CommandResult> DescribeRecord(AwsServicecatalogDescribeRecordOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServiceAction(AwsServicecatalogDescribeServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServiceActionExecutionParameters(AwsServicecatalogDescribeServiceActionExecutionParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTagOption(AwsServicecatalogDescribeTagOptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableAwsOrganizationsAccess(AwsServicecatalogDisableAwsOrganizationsAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDisableAwsOrganizationsAccessOptions(), token);
    }

    public async Task<CommandResult> DisassociateBudgetFromResource(AwsServicecatalogDisassociateBudgetFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociatePrincipalFromPortfolio(AwsServicecatalogDisassociatePrincipalFromPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateProductFromPortfolio(AwsServicecatalogDisassociateProductFromPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateServiceActionFromProvisioningArtifact(AwsServicecatalogDisassociateServiceActionFromProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateTagOptionFromResource(AwsServicecatalogDisassociateTagOptionFromResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableAwsOrganizationsAccess(AwsServicecatalogEnableAwsOrganizationsAccessOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogEnableAwsOrganizationsAccessOptions(), token);
    }

    public async Task<CommandResult> ExecuteProvisionedProductPlan(AwsServicecatalogExecuteProvisionedProductPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ExecuteProvisionedProductServiceAction(AwsServicecatalogExecuteProvisionedProductServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Generate(AwsServicecatalogGenerateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGenerateOptions(), token);
    }

    public async Task<CommandResult> GetAwsOrganizationsAccessStatus(AwsServicecatalogGetAwsOrganizationsAccessStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGetAwsOrganizationsAccessStatusOptions(), token);
    }

    public async Task<CommandResult> GetProvisionedProductOutputs(AwsServicecatalogGetProvisionedProductOutputsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGetProvisionedProductOutputsOptions(), token);
    }

    public async Task<CommandResult> ImportAsProvisionedProduct(AwsServicecatalogImportAsProvisionedProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAcceptedPortfolioShares(AwsServicecatalogListAcceptedPortfolioSharesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListAcceptedPortfolioSharesOptions(), token);
    }

    public async Task<CommandResult> ListBudgetsForResource(AwsServicecatalogListBudgetsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConstraintsForPortfolio(AwsServicecatalogListConstraintsForPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListLaunchPaths(AwsServicecatalogListLaunchPathsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOrganizationPortfolioAccess(AwsServicecatalogListOrganizationPortfolioAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPortfolioAccess(AwsServicecatalogListPortfolioAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPortfolios(AwsServicecatalogListPortfoliosOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListPortfoliosOptions(), token);
    }

    public async Task<CommandResult> ListPortfoliosForProduct(AwsServicecatalogListPortfoliosForProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPrincipalsForPortfolio(AwsServicecatalogListPrincipalsForPortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProvisionedProductPlans(AwsServicecatalogListProvisionedProductPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListProvisionedProductPlansOptions(), token);
    }

    public async Task<CommandResult> ListProvisioningArtifacts(AwsServicecatalogListProvisioningArtifactsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListProvisioningArtifactsForServiceAction(AwsServicecatalogListProvisioningArtifactsForServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecordHistory(AwsServicecatalogListRecordHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListRecordHistoryOptions(), token);
    }

    public async Task<CommandResult> ListResourcesForTagOption(AwsServicecatalogListResourcesForTagOptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServiceActions(AwsServicecatalogListServiceActionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListServiceActionsOptions(), token);
    }

    public async Task<CommandResult> ListServiceActionsForProvisioningArtifact(AwsServicecatalogListServiceActionsForProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListStackInstancesForProvisionedProduct(AwsServicecatalogListStackInstancesForProvisionedProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListTagOptions(AwsServicecatalogListTagOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListTagOptionsOptions(), token);
    }

    public async Task<CommandResult> NotifyProvisionProductEngineWorkflowResult(AwsServicecatalogNotifyProvisionProductEngineWorkflowResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotifyTerminateProvisionedProductEngineWorkflowResult(AwsServicecatalogNotifyTerminateProvisionedProductEngineWorkflowResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NotifyUpdateProvisionedProductEngineWorkflowResult(AwsServicecatalogNotifyUpdateProvisionedProductEngineWorkflowResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ProvisionProduct(AwsServicecatalogProvisionProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectPortfolioShare(AwsServicecatalogRejectPortfolioShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScanProvisionedProducts(AwsServicecatalogScanProvisionedProductsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogScanProvisionedProductsOptions(), token);
    }

    public async Task<CommandResult> SearchProducts(AwsServicecatalogSearchProductsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProductsOptions(), token);
    }

    public async Task<CommandResult> SearchProductsAsAdmin(AwsServicecatalogSearchProductsAsAdminOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProductsAsAdminOptions(), token);
    }

    public async Task<CommandResult> SearchProvisionedProducts(AwsServicecatalogSearchProvisionedProductsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProvisionedProductsOptions(), token);
    }

    public async Task<CommandResult> TerminateProvisionedProduct(AwsServicecatalogTerminateProvisionedProductOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogTerminateProvisionedProductOptions(), token);
    }

    public async Task<CommandResult> UpdateConstraint(AwsServicecatalogUpdateConstraintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePortfolio(AwsServicecatalogUpdatePortfolioOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePortfolioShare(AwsServicecatalogUpdatePortfolioShareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProduct(AwsServicecatalogUpdateProductOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProvisionedProduct(AwsServicecatalogUpdateProvisionedProductOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogUpdateProvisionedProductOptions(), token);
    }

    public async Task<CommandResult> UpdateProvisionedProductProperties(AwsServicecatalogUpdateProvisionedProductPropertiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateProvisioningArtifact(AwsServicecatalogUpdateProvisioningArtifactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateServiceAction(AwsServicecatalogUpdateServiceActionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTagOption(AwsServicecatalogUpdateTagOptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}