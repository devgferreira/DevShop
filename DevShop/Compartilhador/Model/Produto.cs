﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartilhador.Model
{
    public class Produto
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public decimal Valor { get; set; }

        public int Quantidade { get; set; }

        public bool Reservado { get; set; }
    }
}
