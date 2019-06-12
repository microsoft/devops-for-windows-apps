
# DevOps for Windows Desktop Apps

This repo contains a sample application to showcase best practices when doing DevOps to deploy/update Windows Desktop applications using MSIX. We used this content at //Build 2019 within the session [DevOps for applications running on Windows](https://mybuild.techcommunity.microsoft.com/sessions/77012)

Azure Dev Ops Pipelines are available at: https://dev.azure.com/devops-for-client-apps/MyWPFApp

### CI Build
[![Build Status](https://dev.azure.com/devops-for-client-apps/MyWPFApp/_apis/build/status/CI-build?branchName=master)](https://dev.azure.com/devops-for-client-apps/MyWPFApp/_build/latest?definitionId=1&branchName=master)

### CD - QA (WebApp) 
[![CD](https://img.shields.io/azure-devops/release/devops-for-client-apps/99e907d0-45c4-4065-9d18-a85a42d82d83/1/1.svg?style=flat-square)](https://mywpfapp.azurewebsites.net/CD/)

### CD - PROD (Blob)
[![PROD](https://img.shields.io/azure-devops/release/devops-for-client-apps/99e907d0-45c4-4065-9d18-a85a42d82d83/1/9.svg?style=flat-square)](https://mywpfapp.z5.web.core.windows.net/Prod)

### CD - PROD (Store)
[![Store](https://vsrm.dev.azure.com/devops-for-client-apps/_apis/public/Release/badge/99e907d0-45c4-4065-9d18-a85a42d82d83/1/10)](https://dev.azure.com/devops-for-client-apps/MyWPFApp/_releaseProgress?_a=release-pipeline-progress&releaseId=186)

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
