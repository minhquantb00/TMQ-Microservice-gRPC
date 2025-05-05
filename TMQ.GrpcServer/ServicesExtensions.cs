using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GrpcServer
{
    public static class ServicesExtensions
    {
        public static IServiceCollection ConfigCodeFirstGrpc(this IServiceCollection services)
        {
            services.AddCodeFirstGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.NoCompression;
                config.MaxReceiveMessageSize = int.MaxValue;
                config.MaxSendMessageSize = int.MaxValue;
                //config.Interceptors.Add<>();
            });
            services.TryAddSingleton(
                BinderConfiguration.Create(
                    binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));
            services.AddCodeFirstGrpcReflection();
            return services;
        }
    }
}
