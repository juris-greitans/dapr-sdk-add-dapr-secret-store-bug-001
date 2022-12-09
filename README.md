A .NET Core 7 app demo for [issue](https://github.com/dapr/dotnet-sdk/issues/993) when `Dapr.Extensions.Configuration.AddDaprSecretStore()` fails if Dapr sidecar is started separately and waits for the app to start listening on its port.

Steps to repeat:
1. `cd` to `src/app` directory
2. Launch Dapr sidecar (probably from separate terminal):
```
C:\src\dapr-bug-001\src\app>dapr run --app-id "app" --app-port "5200" --components-path "components"
WARNING: no application command found.
Starting Dapr with id app. HTTP Port: 59532. gRPC Port: 59533
time="2022-12-10T00:02:52.7376062+02:00" level=info msg="starting Dapr Runtime -- version 1.9.4 -- commit 576ac0348e56e25b906d9e7a5c817db451cc5b34" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7381516+02:00" level=info msg="log level set to: info" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7381516+02:00" level=info msg="metrics server started on :59534/" app_id=app instance=DESKTOP-018LENC scope=dapr.metrics type=log ver=1.9.4
time="2022-12-10T00:02:52.7392169+02:00" level=info msg="standalone mode configured" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7398239+02:00" level=info msg="app id: app" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7398239+02:00" level=info msg="mTLS is disabled. Skipping certificate request and tls validation" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7398239+02:00" level=info msg="Dapr trace sampler initialized: DaprTraceSampler(P=1.000000)" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7550095+02:00" level=info msg="local service entry announced: app -> 192.168.1.111:59535" app_id=app instance=DESKTOP-018LENC scope=dapr.contrib type=log ver=1.9.4
time="2022-12-10T00:02:52.7555293+02:00" level=info msg="Initialized name resolution to mdns" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7555293+02:00" level=info msg="loading components" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7565806+02:00" level=info msg="waiting for all outstanding components to be processed" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7571807+02:00" level=info msg="component loaded. name: secrets, type: secretstores.local.file/v1" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7571807+02:00" level=info msg="all outstanding components processed" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7571807+02:00" level=info msg="gRPC proxy enabled" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7576867+02:00" level=info msg="enabled gRPC tracing middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.api type=log ver=1.9.4
time="2022-12-10T00:02:52.7582346+02:00" level=info msg="enabled gRPC metrics middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.api type=log ver=1.9.4
time="2022-12-10T00:02:52.7583168+02:00" level=info msg="API gRPC server is running on port 59533" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7583168+02:00" level=info msg="enabled metrics http middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.http type=log ver=1.9.4
time="2022-12-10T00:02:52.7588259+02:00" level=info msg="enabled tracing http middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.http type=log ver=1.9.4
time="2022-12-10T00:02:52.7588259+02:00" level=info msg="http server is running on port 59532" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7593662+02:00" level=info msg="The request body size parameter is: 4" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7593662+02:00" level=info msg="enabled gRPC tracing middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.internal type=log ver=1.9.4
time="2022-12-10T00:02:52.7598947+02:00" level=info msg="enabled gRPC metrics middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.internal type=log ver=1.9.4
time="2022-12-10T00:02:52.7598947+02:00" level=info msg="internal gRPC server is running on port 59535" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-10T00:02:52.7604495+02:00" level=info msg="application protocol: http. waiting on port 5200.  This will block until the app is listening on that port." app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
You're up and running! Dapr logs will appear here.
```
3. Launch the app. It will crash with `TaskCancelledException`:
```
C:\src\dapr-bug-001\src\app>dotnet run
Building...
Unhandled exception. System.Threading.Tasks.TaskCanceledException: A task was canceled.
   at Dapr.Client.DaprClientGrpc.WaitForSidecarAsync(CancellationToken cancellationToken)
   at Dapr.Extensions.Configuration.DaprSecretStore.DaprSecretStoreConfigurationProvider.LoadAsync()
   at Dapr.Extensions.Configuration.DaprSecretStore.DaprSecretStoreConfigurationProvider.Load()
   at Microsoft.Extensions.Configuration.ConfigurationManager.AddSource(IConfigurationSource source)
   at Microsoft.Extensions.Configuration.ConfigurationManager.Microsoft.Extensions.Configuration.IConfigurationBuilder.Add(IConfigurationSource source)
   at Dapr.Extensions.Configuration.DaprSecretStoreConfigurationExtensions.AddDaprSecretStore(IConfigurationBuilder configurationBuilder, String store, DaprClient client, TimeSpan sidecarWaitTimeout, IReadOnlyDictionary`2 metadata)
   at Program.<Main>$(String[] args) in C:\src\dapr-bug-001\src\app\Program.cs:line 7
```

If you run the app via `dapr`, it starts without problems:
```
C:\src\dapr-bug-001\src\app>dapr run --app-id "app" --app-port "5200" --components-path "components" -- dotnet run
Starting Dapr with id app. HTTP Port: 59327. gRPC Port: 59328
time="2022-12-09T23:30:39.5239467+02:00" level=info msg="starting Dapr Runtime -- version 1.9.4 -- commit 576ac0348e56e25b906d9e7a5c817db451cc5b34" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.524465+02:00" level=info msg="log level set to: info" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.524465+02:00" level=info msg="metrics server started on :59329/" app_id=app instance=DESKTOP-018LENC scope=dapr.metrics type=log ver=1.9.4
time="2022-12-09T23:30:39.5255746+02:00" level=info msg="standalone mode configured" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5260795+02:00" level=info msg="app id: app" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5260795+02:00" level=info msg="mTLS is disabled. Skipping certificate request and tls validation" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5260795+02:00" level=info msg="Dapr trace sampler initialized: DaprTraceSampler(P=1.000000)" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5422883+02:00" level=info msg="local service entry announced: app -> 192.168.1.111:59330" app_id=app instance=DESKTOP-018LENC scope=dapr.contrib type=log ver=1.9.4
time="2022-12-09T23:30:39.5427981+02:00" level=info msg="Initialized name resolution to mdns" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5427981+02:00" level=info msg="loading components" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5439142+02:00" level=info msg="waiting for all outstanding components to be processed" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5439142+02:00" level=info msg="component loaded. name: secrets, type: secretstores.local.file/v1" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5444192+02:00" level=info msg="all outstanding components processed" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5444862+02:00" level=info msg="gRPC proxy enabled" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5444862+02:00" level=info msg="enabled gRPC tracing middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.api type=log ver=1.9.4
time="2022-12-09T23:30:39.544989+02:00" level=info msg="enabled gRPC metrics middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.api type=log ver=1.9.4
time="2022-12-09T23:30:39.544989+02:00" level=info msg="API gRPC server is running on port 59328" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.544989+02:00" level=info msg="enabled metrics http middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.http type=log ver=1.9.4
time="2022-12-09T23:30:39.5455408+02:00" level=info msg="enabled tracing http middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.http type=log ver=1.9.4
time="2022-12-09T23:30:39.5455408+02:00" level=info msg="http server is running on port 59327" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5455408+02:00" level=info msg="The request body size parameter is: 4" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5461126+02:00" level=info msg="enabled gRPC tracing middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.internal type=log ver=1.9.4
time="2022-12-09T23:30:39.5466156+02:00" level=info msg="enabled gRPC metrics middleware" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.grpc.internal type=log ver=1.9.4
time="2022-12-09T23:30:39.5466156+02:00" level=info msg="internal gRPC server is running on port 59330" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:39.5466156+02:00" level=info msg="application protocol: http. waiting on port 5200.  This will block until the app is listening on that port." app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
== APP == Building...
Updating metadata for app command: dotnet run
You're up and running! Both Dapr and your app logs will appear here.

== APP == info: Microsoft.Hosting.Lifetime[14]
== APP ==       Now listening on: http://localhost:5200
== APP == info: Microsoft.Hosting.Lifetime[0]
== APP ==       Application started. Press Ctrl+C to shut down.
== APP == info: Microsoft.Hosting.Lifetime[0]
== APP ==       Hosting environment: Development
== APP == info: Microsoft.Hosting.Lifetime[0]
== APP ==       Content root path: C:\src\dapr-bug-001\src\app
time="2022-12-09T23:30:41.8087352+02:00" level=info msg="application discovered on port 5200" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:41.87075+02:00" level=info msg="application configuration loaded" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:41.8789273+02:00" level=info msg="actors: state store is not configured - this is okay for clients but services with hosted actors will fail to initialize!" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:41.8794915+02:00" level=info msg="actor runtime started. actor idle timeout: 1h0m0s. actor scan interval: 30s" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.actor type=log ver=1.9.4
time="2022-12-09T23:30:41.8794915+02:00" level=info msg="dapr initialized. Status: Running. Init Elapsed 2353ms" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime type=log ver=1.9.4
time="2022-12-09T23:30:41.8893655+02:00" level=info msg="placement tables updated, version: 0" app_id=app instance=DESKTOP-018LENC scope=dapr.runtime.actor.internal.placement type=log ver=1.9.4
```


