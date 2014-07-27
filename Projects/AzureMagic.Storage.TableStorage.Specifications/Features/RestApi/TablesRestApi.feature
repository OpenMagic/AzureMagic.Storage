Feature: TablesRestApi

	Operations on Tables

	The REST API provides operations to enumerate the tables in a storage account, create a new table, delete an existing table, and get or set stored access policies for a table.

	- http://msdn.microsoft.com/en-us/library/azure/dd179471.aspx

Background: 
	Given TablesRestApi has been initialized

Scenario: Query Tables
	When ListAsync() is called	
	Then a successful HttpResponseMessage is returned
	And the HttpResponseMessage.Content is a JSON list of tables for the current connection

Scenario: Create Table
	Given todo: write scenario

Scenario: Delete Table
	Given todo: write scenario

@ignore @todo
Scenario: Get Table ACL
	Given todo: write scenario

@ignore @todo
Scenario: Set Table ACL
	Given todo: write scenario

Scenario: Constructor(account: null)
	Given storageAccount is null
	When Tables(account) is called
	Then ArgumentNullException is thrown for account

Scenario: Constructor(account: is not null)
	Given storageAccount is not null
	When Tables(account) is called
	Then an instance is returned