using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Model.DTOs.CustomValidations
{
    public class ConfigUrl : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var _url = value.ToString();

            _url = Regex.Replace(_url, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$+", "");

            return true;
        }
    }


    public class VerifyUrl
    {
        public string Name { get; set; }

    }

}
