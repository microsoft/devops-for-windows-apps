# DevOps for Windows Desktop Apps using GitHub Actions

## Create a CI/CD pipeline for a Wpf app built on Net Core

This repo contains a sample application to demonstrate how to create a CI/CD pipeline for a Wpf application built on Net Core and packaged with MSIX using GitHub Actions. 

With GitHub Actions, you can automate your software workflows.  You can build, test, and deploy your code within GitHub.  Documentation for GitHub Actions can be found here: https://github.com/features/actions

Workflows are defined in YAML files in the .github/workflows folder.  In this project, we have two workflow definitions:
* ci.yml
* cd.yml

## ci.yml

The continuous integration workflow gets triggered anytime a developer pushes code to the repo.  The GitHub build agent calls a GitHub action to add the MSBuild.exe to the PATH, then executes unit tests by calling "dotnet test MyWpfApp.Tests.csproj".  In order to prevent a known error, the workflow cleans the solution then builds the Wpf Net Core application with MsBuild.  From there, the agent creates an MSIX app package and uploads it as a [build artifact](https://github.com/marketplace/actions/upload-artifact), along with an .appinstaller file, allowing developers to deploy and test the app.

With GitHub Actions, we are able to target multiple platforms by setting the build matrix for x86 and x64, for example.  See the article [Workflow syntax for GitHub Actions](https://help.github.com/en/actions/automating-your-workflow-with-github-actions/workflow-syntax-for-github-actions) for more information.

The CI pipeline uses the Package Identity Name that is defined in the Package.appxmanifest in the Windows Application Packaging project, currently set to MyWPFApp (Local).  Developers have the option to download the artifact to test the build or upload the artifact to a website or file share for app distribution.  One incredibly powerful, yet simple, method of distribution is through the use of GitHub pages.  

To see distribution websites for the Local channel, please navigate to the following:
* [edwardskrod/devops-for-windows-app-distribution-local](https://github.com/edwardskrod/devops-for-windows-apps-distribution-local)

## cd.yml

The continuous delivery workflow allows us to build, package and distribute our code for multiple channels such as 'Production' and 'Development.'   The workflow gets triggered anytime a developer pushes code to the repo that has a git [tag](https://git-scm.com/book/en/v2/Git-Basics-Tagging).   To add a release tag to a commit, run the following commands:
* git tag 1.0.0.0
* git push --tags

The first step adds the tag "1.0.0.0" while the second step pushes the branch and tag to the repo.

Similar to the way the continuous integration workflow works, in this workflow, the GitHub build agent builds the Wpf Net Core application and creates a MSIX package. However, prior to building the code, the Package.appxmanifest has the Identity Name, Publisher, DisplayName, and other elements changed according to which channel should be built. The channels are defined in the build matrix.  Once the MSIX is created for each channel, the agent archives the AppPackages folder, then creates a Release with the specified git release tag.  The archive is uploaded to the release as an asset for storage or distribution.

To see distribution websites for the Dev and Prod channels, please navigate to the following:
* [edwardskrod/devops-for-windows-app-distribution-dev](https://github.com/edwardskrod/devops-for-windows-apps-distribution-dev)
* [edwardskrod/devops-for-windows-app-distribution-prod](https://github.com/edwardskrod/devops-for-windows-apps-distribution-prod)

Creating channels for the application is a powerful way to allow side-by-side installations of different releases.


## CI / CD
### branch/master push

![](https://github.com/edwardskrod/devops-for-windows-apps/workflows/Wpf%20Continuous%20Integration/badge.svg)

![](https://github.com/edwardskrod/devops-for-windows-apps/workflows/Wpf%20Continuous%20Delivery/badge.svg)

# Contributions
This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the Microsoft Open Source Code of Conduct. For more information see the Code of Conduct FAQ or contact opencode@microsoft.com with any additional questions or comments.
