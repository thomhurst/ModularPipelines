using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSecurity
{
    public AzSecurity(
        AzSecurityAdaptive_network_hardenings adaptive_network_hardenings,
        AzSecurityAdaptiveApplicationControls adaptiveApplicationControls,
        AzSecurityAlert alert,
        AzSecurityAlertsSuppressionRule alertsSuppressionRule,
        AzSecurityAllowed_connections allowed_connections,
        AzSecurityAssessment assessment,
        AzSecurityAssessmentMetadata assessmentMetadata,
        AzSecurityAtp atp,
        AzSecurityAutoProvisioningSetting autoProvisioningSetting,
        AzSecurityAutomation automation,
        AzSecurityAutomationActionEventHub automationActionEventHub,
        AzSecurityAutomationActionLogicApp automationActionLogicApp,
        AzSecurityAutomationActionWorkspace automationActionWorkspace,
        AzSecurityAutomationRule automationRule,
        AzSecurityAutomationRuleSet automationRuleSet,
        AzSecurityAutomationScope automationScope,
        AzSecurityAutomationSource automationSource,
        AzSecurityContact contact,
        AzSecurityDiscoveredSecuritySolution discoveredSecuritySolution,
        AzSecurityExternalSecuritySolution externalSecuritySolution,
        AzSecurityIotAlerts iotAlerts,
        AzSecurityIotAnalytics iotAnalytics,
        AzSecurityIotRecommendations iotRecommendations,
        AzSecurityIotSolution iotSolution,
        AzSecurityJitPolicy jitPolicy,
        AzSecurityLocation location,
        AzSecurityPricing pricing,
        AzSecurityRegulatoryComplianceAssessments regulatoryComplianceAssessments,
        AzSecurityRegulatoryComplianceControls regulatoryComplianceControls,
        AzSecurityRegulatoryComplianceStandards regulatoryComplianceStandards,
        AzSecuritySecureScoreControlDefinitions secureScoreControlDefinitions,
        AzSecuritySecureScoreControls secureScoreControls,
        AzSecuritySecureScores secureScores,
        AzSecuritySecuritySolutions securitySolutions,
        AzSecuritySecuritySolutionsReferenceData securitySolutionsReferenceData,
        AzSecuritySetting setting,
        AzSecuritySubAssessment subAssessment,
        AzSecurityTask task,
        AzSecurityTopology topology,
        AzSecurityVa va,
        AzSecurityWorkspaceSetting workspaceSetting
    )
    {
        Adaptive_network_hardenings = adaptive_network_hardenings;
        AdaptiveApplicationControls = adaptiveApplicationControls;
        Alert = alert;
        AlertsSuppressionRule = alertsSuppressionRule;
        Allowed_connections = allowed_connections;
        Assessment = assessment;
        AssessmentMetadata = assessmentMetadata;
        Atp = atp;
        AutoProvisioningSetting = autoProvisioningSetting;
        Automation = automation;
        AutomationActionEventHub = automationActionEventHub;
        AutomationActionLogicApp = automationActionLogicApp;
        AutomationActionWorkspace = automationActionWorkspace;
        AutomationRule = automationRule;
        AutomationRuleSet = automationRuleSet;
        AutomationScope = automationScope;
        AutomationSource = automationSource;
        Contact = contact;
        DiscoveredSecuritySolution = discoveredSecuritySolution;
        ExternalSecuritySolution = externalSecuritySolution;
        IotAlerts = iotAlerts;
        IotAnalytics = iotAnalytics;
        IotRecommendations = iotRecommendations;
        IotSolution = iotSolution;
        JitPolicy = jitPolicy;
        Location = location;
        Pricing = pricing;
        RegulatoryComplianceAssessments = regulatoryComplianceAssessments;
        RegulatoryComplianceControls = regulatoryComplianceControls;
        RegulatoryComplianceStandards = regulatoryComplianceStandards;
        SecureScoreControlDefinitions = secureScoreControlDefinitions;
        SecureScoreControls = secureScoreControls;
        SecureScores = secureScores;
        SecuritySolutions = securitySolutions;
        SecuritySolutionsReferenceData = securitySolutionsReferenceData;
        Setting = setting;
        SubAssessment = subAssessment;
        Task = task;
        Topology = topology;
        Va = va;
        WorkspaceSetting = workspaceSetting;
    }

    public AzSecurityAdaptive_network_hardenings Adaptive_network_hardenings { get; }

    public AzSecurityAdaptiveApplicationControls AdaptiveApplicationControls { get; }

    public AzSecurityAlert Alert { get; }

    public AzSecurityAlertsSuppressionRule AlertsSuppressionRule { get; }

    public AzSecurityAllowed_connections Allowed_connections { get; }

    public AzSecurityAssessment Assessment { get; }

    public AzSecurityAssessmentMetadata AssessmentMetadata { get; }

    public AzSecurityAtp Atp { get; }

    public AzSecurityAutoProvisioningSetting AutoProvisioningSetting { get; }

    public AzSecurityAutomation Automation { get; }

    public AzSecurityAutomationActionEventHub AutomationActionEventHub { get; }

    public AzSecurityAutomationActionLogicApp AutomationActionLogicApp { get; }

    public AzSecurityAutomationActionWorkspace AutomationActionWorkspace { get; }

    public AzSecurityAutomationRule AutomationRule { get; }

    public AzSecurityAutomationRuleSet AutomationRuleSet { get; }

    public AzSecurityAutomationScope AutomationScope { get; }

    public AzSecurityAutomationSource AutomationSource { get; }

    public AzSecurityContact Contact { get; }

    public AzSecurityDiscoveredSecuritySolution DiscoveredSecuritySolution { get; }

    public AzSecurityExternalSecuritySolution ExternalSecuritySolution { get; }

    public AzSecurityIotAlerts IotAlerts { get; }

    public AzSecurityIotAnalytics IotAnalytics { get; }

    public AzSecurityIotRecommendations IotRecommendations { get; }

    public AzSecurityIotSolution IotSolution { get; }

    public AzSecurityJitPolicy JitPolicy { get; }

    public AzSecurityLocation Location { get; }

    public AzSecurityPricing Pricing { get; }

    public AzSecurityRegulatoryComplianceAssessments RegulatoryComplianceAssessments { get; }

    public AzSecurityRegulatoryComplianceControls RegulatoryComplianceControls { get; }

    public AzSecurityRegulatoryComplianceStandards RegulatoryComplianceStandards { get; }

    public AzSecuritySecureScoreControlDefinitions SecureScoreControlDefinitions { get; }

    public AzSecuritySecureScoreControls SecureScoreControls { get; }

    public AzSecuritySecureScores SecureScores { get; }

    public AzSecuritySecuritySolutions SecuritySolutions { get; }

    public AzSecuritySecuritySolutionsReferenceData SecuritySolutionsReferenceData { get; }

    public AzSecuritySetting Setting { get; }

    public AzSecuritySubAssessment SubAssessment { get; }

    public AzSecurityTask Task { get; }

    public AzSecurityTopology Topology { get; }

    public AzSecurityVa Va { get; }

    public AzSecurityWorkspaceSetting WorkspaceSetting { get; }
}