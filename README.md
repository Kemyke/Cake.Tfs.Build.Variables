# Cake.Tfs.Build.Variables

## Summary

Retrieving TFS variable from arguments or environment variables. 

## Usage

```
#addin "nuget:?package=Cake.Tfs.Build.Variables"


var testDbName = EvaluateTfsBuildVariable("TestDb.Name");
var testDbConnectionString = EvaluateTfsBuildVariable("TestDb.ConnectionString");
```

## Details
This namespace contain types used for evaluating TFS build variables by name. By default TFS expose its variables as environment variables. 
First they convert the name of the variable to be compatible with environment variables names. (eg. TestDb.Name -> TESTDB_NAME)

TFS doesn't resolve chained variables. This library recursively resolve chained variables and substitute all variables found in the value of other variables. 
For example:

TFS variables:
```
TestDb.ConnectionString = "data source=(localDb)\mssqllocaldb;initial catalog=$(TestDb.Name);integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
TestDb.Name 			= "TestCatalog"
```

But the TFS converts this to environment variables like these:
```
TESTDB_CONNECTIONSTRING = "data source=(localDb)\mssqllocaldb;initial catalog=$(TestDb.Name);integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
TESTDB_NAME 			= "TestCatalog"
```

As you can see TFS doesn't evaluate the environment variables like you expect. This library does the substitution and when you call `EvaluateTfsBuildVariable("TestDb.ConnectionString"`
you will receive `"data source=(localDb)\mssqllocaldb;initial catalog=TestCatalog;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"`


Before retreiving environment variable this library try to retrieve an Cake Argument with the variable name. This is useful because you can override values by passing 
it to the Cake script. And it is also useful bacuse of the secret variables. TFS doesn't expose secret variables as environment variables. You must pass 
secret variables as argument to the script from the TFS build step. If no argument and no environment variable found then a default value is returned 
if it is provided. If not an ArgumentException is thrown.