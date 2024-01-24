namespace DotNetExcptionHandling.Configurations
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddGloblaErrorHandling(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
