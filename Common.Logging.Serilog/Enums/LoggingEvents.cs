namespace Common.Logging.Serilog.Enums;

// Note: Please ensure these settings are updated in accordance with the evolving business logic of the application.
public enum BankServiceEvent
{
    TransactionProcessed = 1000,
    AccountCreated,
    AccountClosed,
    // Add other event types as needed
}

public enum FinancialAnalyticsServiceEvent
{
    DataAnalysisStarted = 2000,
    ReportGenerated,
    DataImported,
    // Add other event types as needed
}

public enum BffWebBankMqApiEvent
{
    MessageReceived = 3000,
    RequestForwarded,
    ResponseReceived,
    // Add other event types as needed
}