Feature: Tables

	Operations on Tables

	The REST API provides operations to enumerate the tables in a storage account, create a new table, delete an existing table, and get or set stored access policies for a table.

	- http://msdn.microsoft.com/en-us/library/azure/dd179471.aspx

Background: 
	Given Tables has been initialized

Scenario: Query Tables
	When ListAsync() is called
	Then a list of tables for the current connection is returned

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

Scenario: Constructor(connectionString: null)
	Given connectionString is null
	When Tables(connectionString) is called
	Then ArgumentNullException is throw

Scenario: Constructor(connectionString: empty)
	Given connectionString is empty
	When Tables(connectionString) is called
	Then ArgumentException is thrown

Scenario: Constructor(connectionString: valid)
	Given connectionString is valid
	When Tables(connectionString) is called
	Then an instance is returned