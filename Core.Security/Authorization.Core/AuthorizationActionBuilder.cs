using Core.Utility.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Authorization.Core
{
    public class AuthorizationActionBuilder
    {
        /// <summary>
        /// The <see cref="IServiceCollection"/> services are attached to.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AuthorizationActionBuilder"/>.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <exception cref="ArgumentNullException">The argument <paramref name="services"/> is null.</exception>
        public AuthorizationActionBuilder(IServiceCollection services)
        {
            Guard.ArgumentNotNull(services, nameof(services));
            this.Services = services;
        }
    }
}
