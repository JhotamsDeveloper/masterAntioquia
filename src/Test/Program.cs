using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Persisten.Database;
using Service;
using Service.Commons;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configuracion del proyecto
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=JHOTAN\\JHOTAMS;Database=dbMasterAntioquia;Trusted_Connection=True;MultipleActiveResultSets=true");
            var Contex = new ApplicationDbContext(optionsBuilder.Options);

            //var _productService = new ProductService(Contex, null, null);
            //using (Contex)
            //{
            //    var product = _productService.GetAllTest();
            //}



            //TesConnection(Contex);
        }

        //Método para validar si la conexión es correcta
        static void TesConnection(ApplicationDbContext context)
        {
            var isConnected = false;

            try
            {
                //Esta linea falla si la base de datos no esta creada
                isConnected = context.Database.GetService <IRelationalDatabaseCreator>().Exists() ;
            }
            catch (Exception)
            {

                throw;
            }

            if (isConnected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connection successful");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection unsuccessful");
            }

            Console.Read();
        }

    }
}
