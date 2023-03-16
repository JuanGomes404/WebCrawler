using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WC
{
    public class JsonResolver
    {
        
        public async void criarArquivoJson(DadosGerais data)
        {
            DadosExtraidos[] listaDados = data.listaDados.ToArray();

            data.listaDadosArray = listaDados;
            string json = JsonSerializer.Serialize(data);
            
            File.WriteAllText(@"C:\path.json", json);
        }

    }
}
