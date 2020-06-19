using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Commons
{
    public interface IFormatString {
        String FormatUrl(String texto);
    }
    public class FormatString : IFormatString
    {

        public String FormatUrl(String texto)
        {
            var original = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýÿ ";
            // Cadena de caracteres ASCII que reemplazarán los originales.
            var ascii = "AAAAAAACEEEEIIIIDNOOOOOOUUUUYBaaaaaaaceeeeiiiionoooooouuuuyy-";
            var output = texto;
            for (int i = 0; i < original.Length; i++)
            {
                // Reemplazamos los caracteres especiales.

                output = output.Replace(original[i], ascii[i]);

            }

            return output;
        }
    }


}
