using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GoogleService
{
    public static class FirebaseMessagingExtension
    {
        public static void AddFirebaseClient(this IServiceCollection services)
        {
            services.AddSingleton<IFirebaseMessagingClient, FirebaseMessagingClient>();
        }
    }
}
