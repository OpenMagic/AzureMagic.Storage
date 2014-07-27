Feature: StorageAccount
	The StorageAccount is required to communicate with Azure Storage REST API.

Background:
	Given accountName is devstoreaccount1
	And accountKey is Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==
	And tablesUri is http://127.0.0.1:10002/devstoreaccount1/

Scenario: Constructor(accountName: is not null, accountKey: is not null)
	Given accountName is dummyAccountName
	Given accountKey is dummyAccountKey
	When Constructor(accountName, accountKey) is called
	Then a StorageAccount instance is returned
	And Name should be accountName
	And Key should be accountKey
	And TablesUri should be https://dummyaccountname.table.core.windows.net/
	
Scenario: Constructor(accountName: is null, accountKey: is not null)
	Given accountName is null
	When Constructor(accountName, accountKey) is called
	Then ArgumentNullException is thrown for accountName

Scenario: Constructor(accountName: is empty, accountKey: is not null)
	Given accountName is empty
	When Constructor(accountName, accountKey) is called
	Then ArgumentException is thrown for accountName

Scenario: Constructor(accountName: is not null, accountKey: is null)
	Given accountName is null
	When Constructor(accountName, accountKey) is called
	Then ArgumentNullException is thrown for accountName

Scenario: Constructor(accountName: is not null, accountKey: is empty)
	Given accountKey is empty
	When Constructor(accountName, accountKey) is called
	Then ArgumentException is thrown for accountKey

Scenario: Constructor(accountName: is not null, accountKey: is not null, tablesUri: is not null)
	When Constructor(accountName, accountKey, tablesUri) is called
	Then a StorageAccount instance is returned

Scenario: Constructor(accountName: is null, accountKey: is not null, tablesUri: is not null)
	Given accountName is null
	When Constructor(accountName, accountKey, tablesUri) is called
	Then ArgumentNullException is thrown for accountName

Scenario: Constructor(accountName: is not null, accountKey: is null, tablesUri: is not null)
	Given accountKey is null
	When Constructor(accountName, accountKey, tablesUri) is called
	Then ArgumentNullException is thrown for accountKey

Scenario: Constructor(accountName: is not null, accountKey: is not null, tablesUri: is null)
	Given tablesUri is null
	When Constructor(accountName, accountKey, tablesUri) is called
	Then ArgumentNullException is thrown for tablesUri

Scenario: Constructor(accountName: is empty, accountKey: is not null, tablesUri: is not null)
	Given accountName is empty
	When Constructor(accountName, accountKey, tablesUri) is called
	Then ArgumentException is thrown for accountName

Scenario: Constructor(accountName: is not null, accountKey: is empty, tablesUri: is not null)
	Given accountKey is empty
	When Constructor(accountName, accountKey, tablesUri) is called
	Then ArgumentException is thrown for accountKey

Scenario: CreateTablesUri(accountName: is null)
	Given accountName is null
	When CreateTablesUri(accountName) is called
	Then ArgumentNullException is thrown for accountName

Scenario: CreateTablesUri(accountName: is empty)
	Given accountName is empty
	When CreateTablesUri(accountName) is called
	Then ArgumentException is thrown for accountName

Scenario: CreateTablesUri(accountName: dummy)
	Given accountName is dummy
	When CreateTablesUri(accountName) is called
	Then Uri https://dummy.table.core.windows.net/ should be returned

Scenario: ForStorageEmulator()
	When ForStorageEmulator() is called
	Then a StorageAccount instance is returned
	And Name should be devstoreaccount1
	And Key should be Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==
	And TablesUri should be http://127.0.0.1:10002/devstoreaccount1