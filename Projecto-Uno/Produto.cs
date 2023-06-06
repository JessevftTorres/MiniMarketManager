using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectoUno
{
    internal class Produto
    {
        public int CodigoInterno { get; set; }
        public int CodigoBarras { get; set; }
        public string DescricaoProduto { get; set; }
        public double PrecoVenda { get; set; }
    }
}
