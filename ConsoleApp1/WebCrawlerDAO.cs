using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC
{
    public class WebCrawlerDAO
    {
       private string connectionString;
        public void inserirDados(DadosGerais dados)
        {
            connectionString = "server=sql10.freesqldatabase.com;database=sql10606399;uid=sql10606399;pwd=faq9GW56VV;";
            using (var connection = new MySqlConnection(connectionString))
            {
                string sqlQuery = "INSERT INTO DadosExtraidos (data_inicio_exec, data_termino_exec, qnt_paginas, qnt_linhas_extraidas, arquivo_json) " +
                                   "VALUES (@dataInicioExec, @dataTerminoExec, @qntPaginas, @qntLinhasExtraidas, @arquivoJson)";
                connection.Open();

                using (var command = new MySqlCommand(sqlQuery, connection))
                {

                    command.Parameters.AddWithValue("@dataInicioExec", dados.converterFormato(dados.dataInicio));
                    command.Parameters.AddWithValue("@dataTerminoExec", dados.converterFormato(dados.dataFim));
                    command.Parameters.AddWithValue("@qntPaginas", dados.quantidade_paginas);
                    command.Parameters.AddWithValue("@qntLinhasExtraidas", dados.quantidade_linhas_extraidas);
                    command.Parameters.AddWithValue("@arquivoJson", dados.json);

                    
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine("Rows Affected: " + rowsAffected);
                }
                connection.Close();

                
            }
        }
    } 
}
