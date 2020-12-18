# Rhodium24 Integration Quickstart

## How it works

- The NuGet package 'MetalHeaven.Integration.Shared' contains some basic classes & helpers to get you started.
- The host will load all assemblies inside the '.\\[Execution folder]\\Plugins' folder and sub directories inside the Plugins folder.
- Types that in inherit from type 'BaseIntegration' are being registered in DI on startup of the host.
- Register services needed for your integration with the 'RegisterServices' method inside your 'BaseIntegration' implementation class.
- All the integration assemblies are being used to register MediatR with. So you can use IMediator out of the box

## Help needed?

Please create an issue if you got a suggestion or if you need help.
