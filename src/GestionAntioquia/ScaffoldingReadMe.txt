Se agregó soporte para ASP.NET Core Identity a su proyecto
- El código para agregar Identity a su proyecto se generó en Áreas / Identity.

La configuración de los servicios relacionados con la identidad se puede encontrar 
en el archivo Áreas / Identity / IdentityHostingStartup.cs.

La IU generada requiere soporte para archivos estáticos. Para agregar archivos estáticos a su aplicación:
1. Llame a app.UseStaticFiles () desde su método Configure

Para usar ASP.NET Core Identity también necesita habilitar la autenticación. Para autenticar tu aplicación:
1. Llame a app.UseAuthentication () desde su método Configure (después de los archivos estáticos)

La IU generada requiere MVC. Para agregar MVC a su aplicación:
1. Llame a services.AddMvc () desde su método ConfigureServices
2. Llame a app.UseRouting () en la parte superior de su método Configure y UseEndpoints () después de la autenticación:
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
    });

Las aplicaciones que usan ASP.NET Core Identity también deben usar HTTPS. Para habilitar HTTPS, consulte https://go.microsoft.com/fwlink/?linkid=848054.

