﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace WC
{
    public class DadosGerais
    {
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
        public ConcurrentBag<DadosExtraidos> listaDados = new ConcurrentBag<DadosExtraidos>();
        public int quantidade_paginas { get; set; }
        public DadosExtraidos[] listaDadosArray { get; set; }
        public int quantidade_linhas_extraidas { get; set; }
        public string json { get; set; }





        public DateTime converterFormato(DateTime dataHora)
        {

            return DateTime.Parse(dataHora.ToString("yyyy-MM-dd HH:mm:ss"));

        }
    }

  
}
