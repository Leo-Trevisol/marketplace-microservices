using Ftec.ProjetoWeb.Produtos.Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao.DTO {
    public class MediaDTO {

        public MediaDTO() {

        }

        public string NomeArquivo { get; set; }
        public string NomeUnico { get; set; }
        public string CaminhoArquivo { get; set; }
        public TipoArquivo TipoArquivo { get; set; }
        public string Extensao { get; set; }
        public DateTime DataUpload { get; set; }
    }
}
