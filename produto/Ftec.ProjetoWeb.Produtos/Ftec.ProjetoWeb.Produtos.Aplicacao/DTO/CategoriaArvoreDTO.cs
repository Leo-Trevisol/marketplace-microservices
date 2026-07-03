using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao.DTO
{
    public class CategoriaArvoreDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<CategoriaArvoreDTO> Filhos { get; set; } = new();
    }
}
