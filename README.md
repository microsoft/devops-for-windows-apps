# DevOps for Windows Desktop Apps using GitHub Actions

### Create a CI/CD pipeline for a Wpf app built on Net Core

This repo contains a sample application to demonstrate how to create a CI/CD pipeline for a Wpf application built on Net Core and packaged with MSIX using GitHub Actions. 

With GitHub Actions, you can automate your software workflows.  You can build, test, and deploy your code within GitHub.  Documentation for GitHub Actions can be found here: https://github.com/features/actions


![](https://github.com/edwardskrod/devops-for-windows-apps/workflows/Wpf%20Continuous%20Integration/badge.svg)

![](https://github.com/edwardskrod/devops-for-windows-apps/workflows/Wpf%20Continuous%20Delivery/badge.svg)

## Workflows

Workflows are defined in YAML files in the .github/workflows folder.  In this project, we have two workflow definitions:
* ci.yml
* cd.yml

### ci.yml

Build, test, package, and save package artifacts.

On every `push` to the repo, [Install .NET Core](https://github.com/actions/setup-dotnet), add [MsBuild](https://github.com/topics/msbuild-action) to the PATH, and execute unit tests.

```yaml
    - name: Install .NET Core
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 3.1.100

    # Add  MsBuild to the PATH: https://github.com/topics/msbuild-action
    - name: Setup MSBuild.exe
      uses: warrenbuckley/Setup-MSBuild@v1

    # Test
    - name: Execute Unit Tests
      run: dotnet test MyWpfApp.Tests\MyWpfApp.Tests.csproj
```

Target multiple platforms by authoring the workflow to define a build matrix, a set of different configurations of the runner environment.   
```yaml
    strategy:
      matrix:
        targetplatform: [x86, x64]
```
See[Workflow syntax for GitHub Actions](https://help.github.com/en/actions/automating-your-workflow-with-github-actions/workflow-syntax-for-github-actions) for more information.

Build and package the Wpf Net Core application with MsBuild and then [upload the build artifacts](https://github.com/marketplace/actions/upload-artifact) to allow developers to deploy and test the app.

This CI pipeline uses the Package Identity Name defined in the Package.appxmanifest in the Windows Application Packaging project to identify the application as "MyWPFApp (Local)." By suffixing the application with "(Local)", we are able to install it side by side with other channels of the app.  Developers have the option to download the artifact to test the build or upload the artifact to a website or file share for app distribution.  

One incredibly powerful, yet simple, method of distribution is through the use of GitHub pages. To see the distribution website for the Local channel, please navigate to [edwardskrod/devops-for-windows-app-distribution-local.](https://github.com/edwardskrod/devops-for-windows-apps-distribution-local)

### cd.yml

Build, package, and create a GitHub release for multiple channels.

The continuous delivery workflow allows us to build, package and distribute our code for multiple channels such as 'Dev' and 'Prod.'   On every `push` to a [tag](https://git-scm.com/book/en/v2/Git-Basics-Tagging) matching the pattern `*`, [create a release](https://developer.github.com/v3/repos/releases/#create-a-release) and [upload a release asset](https://developer.github.com/v3/repos/releases/#upload-a-release-asset)  

```yaml
on: 
  push:
    tags:
      - '*'
```

To create a git tag, run the following commands on the branch you wish to release:
* git tag 1.0.0.0
* git push origin --tags

The above commands will add the tag "1.0.0.0" and then `push` the branch and tag to the repo. [Learn more.](https://git-scm.com/book/en/v2/Git-Basics-Tagging)

In this workflow, the GitHub agent builds the Wpf Net Core application and creates a MSIX package. However, prior to building the code, the application's Identity Name, Publisher, Application DisplayName, and other elements are changed according to which channel should be built. 

```yaml
    # Update the appxmanifest before build by setting the per-channel values set in the matrix.
    - name: Update manifest version
      run: |
        [xml]$manifest = get-content ".\MyWPFApp.Package\Package.appxmanifest"
        $manifest.Package.Identity.Name = "${{matrix.MsixPackageId}}"
        $manifest.Package.Identity.Publisher = "${{matrix.MsixPublisherId}}"
        $manifest.Package.Properties.DisplayName = "${{matrix.MsixPackageDisplayName}}"
        $manifest.Package.Applications.Application.VisualElements.DisplayName = "${{matrix.MsixPackageDisplayName}}"
        $manifest.save(".\MyWPFApp.Package\Package.appxmanifest")
```
 
 Channels and variables are defined in the build matrix.
```yaml
jobs:

  build:

    strategy:
      matrix:
        channel: [Channel_Dev, Channel_Prod]
        include:
          # includes the following variables for the matrix leg matching Channel_Dev
          - channel: Channel_Dev
            ChannelName: Dev
            Configuration: Debug
            DistributionUrl: https://edwardskrod.github.io/devops-for-windows-apps-distribution-dev
            MsixPackageId: MyWPFApp.DevOpsDemo.Dev
            MsixPublisherId: CN=EdwardSkrod
            MsixPackageDisplayName: MyWPFApp (Dev)

          # includes the following variables for the matrix leg matching Channel_Test
          - channel: Channel_Prod
            Configuration: Release
            ChannelName: Prod
            DistributionUrl: https://edwardskrod.github.io/devops-for-windows-apps-distribution-prod
            MsixPackageId: MyWPFApp.DevOpsDemo.Prod
            MsixPublisherId: CN=EdwardSkrod
            MsixPackageDisplayName: MyWPFApp (Prod)
```

The application is signed during the packaging step. Because best practices recommend against storing signing certificates in the Git repository, the .pfx file is encrypted using [Gpg4win](https://www.gpg4win.org/thanks-for-download.html) prior to adding it to the repo.  

In the workflow, we use [Chocolatey Package Manager](https://chocolatey.org/) to download gpg4win to the build agent, then use the shell to decrypt the .pfx, using the secret passphrase stored in the GitHub secrets to decrypt the file. 

```yaml
    # Install the Encryption tool, gpg4Win
    - name: Install gpg4Win
      run: choco install gpg4win

    # Decrypt the .pfx with gpg4win
    - name: Decrypt .pfx
      run: gpg --quiet --batch --yes --decrypt --passphrase=${{secrets.Pfx_gpg_secret_passphrase}} --output MyWpfApp.Package\EdwardSkrodDeveloper.pfx MyWPFApp.Package\EdwardSkrodDeveloper.pfx.gpg
```

To [add a secret to your workflow](https://help.github.com/en/actions/automating-your-workflow-with-github-actions/virtual-environments-for-github-hosted-runners#creating-and-using-secrets-encrypted-variables), navigate to Settings -> Secrets.

Once the certificate is decrypted, we sign the package during the packaging step and pass the signing certificate's password to MSBuild.

```yaml
    # Build the Windows Application Packaging project
    - name: Build MyWpfApp.Package 
      run: msbuild MyWpfApp.Package\MyWpfApp.Package.wapproj /p:Platform=$env:TargetPlatform /p:Configuration=$env:Configuration /p:UapAppxPackageBuildMode=$env:BuildMode /p:AppInstallerUri=$env:AppInstallerUri /p:PackageCertificatePassword=${{secrets.Pfx_Key}}
      env:
        AppInstallerUri: ${{matrix.DistributionUrl}}
        BuildMode: SideLoadOnly
        Configuration: ${{matrix.Configuration}}
        TargetPlatform: x86

```

Once the MSIX is created for each channel, the agent archives the AppPackages folder, then creates a Release with the specified git release tag.  The archive is uploaded to the release as an asset for storage or distribution.

Creating channels for the application is a powerful way to allow side-by-side installations of different releases.


# Contributions
This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the Microsoft Open Source Code of Conduct. For more information see the Code of Conduct FAQ or contact opencode@microsoft.com with any additional questions or comments.

## License
The scripts and documentation in this project are released under the [MIT License](LICENSE)
