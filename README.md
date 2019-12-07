# Blazor App
Experimenting with the Blazor WASM functionality

# Deployment

## Visual Studio
* Right click the server project and choose Publish
* If it asks you to sign in and doesn't return any values, make sure you have an Azure subscription
* Enter the details and click the publish button

## Powershell (Requires Azure CLI)
* Run the `build.ps1` script to generate the zip file to publish
* Find your Azure subscription id from the Portal (Under Subscriptions on the home screen)
* Run the `deploy.ps1` script with the subscription id and a name for the app
	* If you get a "The subscription of xxx' doesn't exist in cloud 'AzureCloud'" warning, run the `az login` command

## Github Actions
* Add secrets to Github via `Repository > Settings > Secrets`. https://github.com/Azure/login
* Conditional jobs: https://github.community/t5/GitHub-Actions/Github-Actions-CI-CD-Need-optional-jobs/td-p/29696
* https://help.github.com/en/actions/automating-your-workflow-with-github-actions/workflow-syntax-for-github-actions

### Deployment Errors
Github actions prepends $ErrorActionPreference = 'stop' to scripts which causes the scripts to stop on any warnings or errors
The az webapp deploy step always seems to throw a warning which then causes the step to fail. Following github issue was raised with a very similar issue.
https://github.com/Azure/azure-cli/issues/6818

```
az : WARNING: Getting scm site credentials for zip deployment
At D:\a\BlazorAppSpike\BlazorAppSpike\scripts\deploy.ps1:27 char:1
+ az webapp deployment source config-zip --resource-group $resourceGrou ...
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : NotSpecified: (WARNING: Gettin... zip deployment:String) [], RemoteException
    + FullyQualifiedErrorId : NativeCommandError
```

# Authentication
* Login by sending credentials to the UserController (`api/user/login`) via a HTTP request which will return a token if successful
* Uses the `UserManager` and `IdentityUser` that are built into the ASP.NET Core identity logic
* On the client side, the token is recieved and saved against the injected HttpClient and stored in local storage on the browser

# Notes
* Blazor doesn't support Standard 2.1 as of Nov 9. https://github.com/aspnet/AspNetCore/pull/16808

# Resources
* https://chrissainty.com/securing-your-blazor-apps-authentication-with-clientside-blazor-using-webapi-aspnet-core-identity/
* https://docs.microsoft.com/en-au/cli/azure/?view=azure-cli-latest
* https://medium.com/@st.mas29/microsoft-blazor-web-api-with-jwt-authentication-part-1-f33a44abab9d
* https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio



