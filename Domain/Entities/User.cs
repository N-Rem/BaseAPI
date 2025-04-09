using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        //indica que es key
        [Key]
        //indica que el valor sera generado automaticamente por la base de datos cuando se inserte un nuevo registro.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        public UserType Type { get; set; }

        //si esta elimiado o no/ activo o inactivo
        public Status Status { get; set; } = Status.Active;


        // Almacena el código para recuperar la contraseña
        public string? PasswordResetCode { get; set; }

        // Almacenar la fecha y hora de caducidad del código, se usa para hacer validaciones.
        public DateTime? ResetCodeExpiration { get; set; }


    }
}
