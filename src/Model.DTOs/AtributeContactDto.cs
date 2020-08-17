using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class AtributeContact
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(35)]
        public string Name { get; set; }
        [DisplayName("E-Mail")]
        [Required(ErrorMessage = "El E-Mail es requerido.")]
        [EmailAddress(ErrorMessage = "Error al escribir el E-Mail")]
        public string Email { get; set; }
        [DisplayName("Asunto")]
        [Required(ErrorMessage = "El Nombre es requerido."), MaxLength(200)]
        public string Subject { get; set; }
        [DisplayName("Mensaje")]
        [Required(ErrorMessage = "El Mensaje es requerido.")]
        public string Message { get; set; }
    }

}
