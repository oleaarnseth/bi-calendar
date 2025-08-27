namespace BICalendar.Configuration;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty; // Swagger UI as start page
            });
        }

        return app;
    }
}
