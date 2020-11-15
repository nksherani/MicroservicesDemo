using Common;
using Common.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoggingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        CustomConfiguration customConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            customConfiguration = new CustomConfiguration();
            Configuration.GetSection("CustomConfiguration").Bind(customConfiguration);
            services.Configure<CustomConfiguration>(Configuration);

            ConfluentConsumer consumer = new ConfluentConsumer();
            Task.Run(()=> consumer.SubscribeAsync("test-topic", ProcessMessage));
        }
        public void ProcessMessage(string msg)
        {
            var logModel = JsonConvert.DeserializeObject<LogModel>(msg);
            logModel.Data = logModel.Data == null ? "null" : $"'{logModel.Data}'";
            Console.WriteLine($"Started processing {msg}");
            string query = "";
            using (SqlConnection sqlConnection = new SqlConnection(customConfiguration.DefaultConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    query = "INSERT INTO [dbo].[Log] ([Level],[Message],[Data],[Timestamp],[CreatedBy])  VALUES " +
                          $"( {logModel.Level}, '{logModel.Message }', {logModel.Data},'{logModel.Timestamp}',{logModel.CreatedBy})";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    int rows = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {

                }
            }
                
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
