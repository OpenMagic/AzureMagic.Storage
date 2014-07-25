Feature: Tables

	Operations on Tables

	The REST API provides operations to enumerate the tables in a storage account, create a new table, delete an existing table, and get or set stored access policies for a table.

	- http://msdn.microsoft.com/en-us/library/azure/dd179471.aspx

Background: 
	Given a valid connection string

Scenario: Query Tables
	When ListAsync() is called
	Then the connection's list of tables is returned

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

