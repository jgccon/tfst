﻿namespace TFST.SharedKernel.Configuration;

public class FeatureFlags
{
    public bool MigrateAtStartup { get; set; } = false;
    public string MessagingProvider { get; set; } = "RabbitMQ";
    public string SecretsProvider { get; set; } = "AzureKeyVault";
    public bool LogUnhandledEvents { get; set; } = false;
}
