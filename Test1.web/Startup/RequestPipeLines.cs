using System.Runtime.CompilerServices;

namespace Test.Web.Startup
{
    public class RequestPipeLines
    {
        public static WebApplication App(WebApplicationBuilder builder)
        {

            var app=builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Departments}/{action=Index}/{id?}");
            });
            return app;
        }
    }
}
