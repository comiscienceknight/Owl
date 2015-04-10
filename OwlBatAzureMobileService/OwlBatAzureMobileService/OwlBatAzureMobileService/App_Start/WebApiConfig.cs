using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using OwlBatAzureMobileService.DataObjects;
using OwlBatAzureMobileService.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace OwlBatAzureMobileService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            //config.SetIsHosted(true);
        }
    }
}

