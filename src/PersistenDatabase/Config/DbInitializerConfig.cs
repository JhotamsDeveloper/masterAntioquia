using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persisten.Database.Config
{
    public static class DbInitializerConfig
    {
        public static void Initialize(ApplicationDbContext context)
        {
            /*Asegura que la base de datos para el contexto existe. Si existe, no se toman medidas.
            Si no existe, se crea la base de datos y todo su esquema. Si la base de datos existe, 
            no se hace ningún esfuerzo para garantizar que sea compatible con el modelo para este 
            contexto.*/

            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.infrastructure.databasefacade.ensurecreated?view=efcore-3.1

            context.Database.EnsureCreated();

            // Si existe categoria.
            if (context.Categorys.Any())
            {
                return;   // DB ha sido sembrada
            }


            var _category = new Category[]
            {
                new Category{Name="Hotel",Icono="Hotel",Stated=true},
                new Category{Name="Restaurante",Icono="Restaurante",Stated=true},
                new Category{Name="Tienda",Icono="Tienda",Stated=true},
                new Category{Name="Blog",Icono="Blog",Stated=true}
            };

            context.Categorys.AddRange(_category);
            context.SaveChanges();
        }
    }
}

/*NOTA: Si vas utilizar DbInitializerConfig debes de tambien configurar el Program.cs
 * para mas informacio https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.1&tabs=visual-studio.*/
