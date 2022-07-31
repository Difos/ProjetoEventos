using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "O campo {0} é necessário"),
        // MinLength(3, ErrorMessage="O {0} deve ter no minimo 3 caracteres"),
        // MaxLength(49, ErrorMessage="O {0} deve ter no máximo 50 caracteres"),
        StringLength(50,MinimumLength=3, ErrorMessage="Intervalo permitido de 3 a 50 caractres")] 
        public string Tema { get; set; }
        [Required(ErrorMessage="O campo {0} é necessário"),
        Range(1,120000,ErrorMessage="Não pode ser menor que 1 e maior que 120 mil")
        ]
        public int QtdPessoas { get; set; }
        [RegularExpression(@".*(gif|jpg?g|bmp|png)$)",ErrorMessage="Nao é uma imagem válida (gif,jpg, bmp ou png")]
        public string ImagemURL { get; set; }
        [Required(ErrorMessage = "O campo {0} é necessário"),
        Phone(ErrorMessage="O {0} esta com numero invalido")
        ]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "O campo {0} é necessário"),
        Display(Name ="e-mail"),
        EmailAddress(ErrorMessage = "O campo {0} precisa ser um email valido")]
        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteEventoDto> PalestrantesEventos { get; set; }
    }
}