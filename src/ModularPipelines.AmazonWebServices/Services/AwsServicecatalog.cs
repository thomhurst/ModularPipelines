using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> AcceptPortfolioShare(AwsServicecatalogAcceptPortfolioShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateBudgetWithResource(AwsServicecatalogAssociateBudgetWithResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociatePrincipalWithPortfolio(AwsServicecatalogAssociatePrincipalWithPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateProductWithPortfolio(AwsServicecatalogAssociateProductWithPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateServiceActionWithProvisioningArtifact(AwsServicecatalogAssociateServiceActionWithProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> AssociateTagOptionWithResource(AwsServicecatalogAssociateTagOptionWithResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchAssociateServiceActionWithProvisioningArtifact(AwsServicecatalogBatchAssociateServiceActionWithProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> BatchDisassociateServiceActionFromProvisioningArtifact(AwsServicecatalogBatchDisassociateServiceActionFromProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CopyProduct(AwsServicecatalogCopyProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConstraint(AwsServicecatalogCreateConstraintOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePortfolio(AwsServicecatalogCreatePortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreatePortfolioShare(AwsServicecatalogCreatePortfolioShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProduct(AwsServicecatalogCreateProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProvisionedProductPlan(AwsServicecatalogCreateProvisionedProductPlanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateProvisioningArtifact(AwsServicecatalogCreateProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateServiceAction(AwsServicecatalogCreateServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateTagOption(AwsServicecatalogCreateTagOptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConstraint(AwsServicecatalogDeleteConstraintOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePortfolio(AwsServicecatalogDeletePortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeletePortfolioShare(AwsServicecatalogDeletePortfolioShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProduct(AwsServicecatalogDeleteProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProvisionedProductPlan(AwsServicecatalogDeleteProvisionedProductPlanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteProvisioningArtifact(AwsServicecatalogDeleteProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteServiceAction(AwsServicecatalogDeleteServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteTagOption(AwsServicecatalogDeleteTagOptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeConstraint(AwsServicecatalogDescribeConstraintOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeCopyProductStatus(AwsServicecatalogDescribeCopyProductStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePortfolio(AwsServicecatalogDescribePortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePortfolioShareStatus(AwsServicecatalogDescribePortfolioShareStatusOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribePortfolioShares(AwsServicecatalogDescribePortfolioSharesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProduct(AwsServicecatalogDescribeProductOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProductOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProductAsAdmin(AwsServicecatalogDescribeProductAsAdminOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProductAsAdminOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProductView(AwsServicecatalogDescribeProductViewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProvisionedProduct(AwsServicecatalogDescribeProvisionedProductOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisionedProductOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProvisionedProductPlan(AwsServicecatalogDescribeProvisionedProductPlanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProvisioningArtifact(AwsServicecatalogDescribeProvisioningArtifactOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisioningArtifactOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeProvisioningParameters(AwsServicecatalogDescribeProvisioningParametersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDescribeProvisioningParametersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeRecord(AwsServicecatalogDescribeRecordOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeServiceAction(AwsServicecatalogDescribeServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeServiceActionExecutionParameters(AwsServicecatalogDescribeServiceActionExecutionParametersOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeTagOption(AwsServicecatalogDescribeTagOptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableAwsOrganizationsAccess(AwsServicecatalogDisableAwsOrganizationsAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogDisableAwsOrganizationsAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateBudgetFromResource(AwsServicecatalogDisassociateBudgetFromResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociatePrincipalFromPortfolio(AwsServicecatalogDisassociatePrincipalFromPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateProductFromPortfolio(AwsServicecatalogDisassociateProductFromPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateServiceActionFromProvisioningArtifact(AwsServicecatalogDisassociateServiceActionFromProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateTagOptionFromResource(AwsServicecatalogDisassociateTagOptionFromResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableAwsOrganizationsAccess(AwsServicecatalogEnableAwsOrganizationsAccessOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogEnableAwsOrganizationsAccessOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteProvisionedProductPlan(AwsServicecatalogExecuteProvisionedProductPlanOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ExecuteProvisionedProductServiceAction(AwsServicecatalogExecuteProvisionedProductServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> Generate(AwsServicecatalogGenerateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGenerateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAwsOrganizationsAccessStatus(AwsServicecatalogGetAwsOrganizationsAccessStatusOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGetAwsOrganizationsAccessStatusOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetProvisionedProductOutputs(AwsServicecatalogGetProvisionedProductOutputsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogGetProvisionedProductOutputsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ImportAsProvisionedProduct(AwsServicecatalogImportAsProvisionedProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAcceptedPortfolioShares(AwsServicecatalogListAcceptedPortfolioSharesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListAcceptedPortfolioSharesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListBudgetsForResource(AwsServicecatalogListBudgetsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListConstraintsForPortfolio(AwsServicecatalogListConstraintsForPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLaunchPaths(AwsServicecatalogListLaunchPathsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationPortfolioAccess(AwsServicecatalogListOrganizationPortfolioAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPortfolioAccess(AwsServicecatalogListPortfolioAccessOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPortfolios(AwsServicecatalogListPortfoliosOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListPortfoliosOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPortfoliosForProduct(AwsServicecatalogListPortfoliosForProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListPrincipalsForPortfolio(AwsServicecatalogListPrincipalsForPortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProvisionedProductPlans(AwsServicecatalogListProvisionedProductPlansOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListProvisionedProductPlansOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProvisioningArtifacts(AwsServicecatalogListProvisioningArtifactsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListProvisioningArtifactsForServiceAction(AwsServicecatalogListProvisioningArtifactsForServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecordHistory(AwsServicecatalogListRecordHistoryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListRecordHistoryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListResourcesForTagOption(AwsServicecatalogListResourcesForTagOptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServiceActions(AwsServicecatalogListServiceActionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListServiceActionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServiceActionsForProvisioningArtifact(AwsServicecatalogListServiceActionsForProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListStackInstancesForProvisionedProduct(AwsServicecatalogListStackInstancesForProvisionedProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagOptions(AwsServicecatalogListTagOptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogListTagOptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NotifyProvisionProductEngineWorkflowResult(AwsServicecatalogNotifyProvisionProductEngineWorkflowResultOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NotifyTerminateProvisionedProductEngineWorkflowResult(AwsServicecatalogNotifyTerminateProvisionedProductEngineWorkflowResultOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> NotifyUpdateProvisionedProductEngineWorkflowResult(AwsServicecatalogNotifyUpdateProvisionedProductEngineWorkflowResultOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ProvisionProduct(AwsServicecatalogProvisionProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RejectPortfolioShare(AwsServicecatalogRejectPortfolioShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ScanProvisionedProducts(AwsServicecatalogScanProvisionedProductsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogScanProvisionedProductsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchProducts(AwsServicecatalogSearchProductsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProductsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchProductsAsAdmin(AwsServicecatalogSearchProductsAsAdminOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProductsAsAdminOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SearchProvisionedProducts(AwsServicecatalogSearchProvisionedProductsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogSearchProvisionedProductsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TerminateProvisionedProduct(AwsServicecatalogTerminateProvisionedProductOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogTerminateProvisionedProductOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConstraint(AwsServicecatalogUpdateConstraintOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePortfolio(AwsServicecatalogUpdatePortfolioOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdatePortfolioShare(AwsServicecatalogUpdatePortfolioShareOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProduct(AwsServicecatalogUpdateProductOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProvisionedProduct(AwsServicecatalogUpdateProvisionedProductOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServicecatalogUpdateProvisionedProductOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProvisionedProductProperties(AwsServicecatalogUpdateProvisionedProductPropertiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateProvisioningArtifact(AwsServicecatalogUpdateProvisioningArtifactOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateServiceAction(AwsServicecatalogUpdateServiceActionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateTagOption(AwsServicecatalogUpdateTagOptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}