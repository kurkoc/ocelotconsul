using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ServiceDiscovery
{
    public static class ServiceDiscoveryExtensions
    {
		public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
			{
				var address = configuration["ConsulConfig:Host"];
				consulConfig.Address = new Uri(address);
			}));

			return services;
		}

		public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
		{
			var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
			var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

			if (!(app.Properties["server.Features"] is FeatureCollection features))
			{
				return app;
			}

			var addresses = features.Get<IServerAddressesFeature>();
			var address = addresses.Addresses.First();
			var serviceName = configuration["ConsulConfig:ServiceName"];
			var serviceId = configuration["ConsulConfig:ServiceId"];
			var uri = new Uri(address);

			var registration = new AgentServiceRegistration()
			{
				ID = serviceId,
				Name = serviceName,
				Address = $"{uri.Host}",
				Port = uri.Port
			};

			consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
			consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

			lifetime.ApplicationStopping.Register(() =>
			{
				consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
			});

			return app;
		}

	}
}
