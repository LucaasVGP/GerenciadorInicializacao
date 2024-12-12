using GerenciadorInicializacao.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorInicializacao.Models
{
    public class Aplicativo
    {
        public string Nome { get; set; } = string.Empty;
        public string Caminho { get; set; } = string.Empty;
        public ETipoAbertura TipoAbertura { get; set; }
        public string? Argumento { get; set; }
        public string NomeProcesso { get; set; } = string.Empty;
    }
}