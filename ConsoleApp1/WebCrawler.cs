using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WC
{
    public class WebCrawler
    {
        CountdownEvent countdownEvent = new CountdownEvent(3);
        DadosGerais dadosGerais = new DadosGerais();
        
        private void extrairDados(string url, int pageInicio, int pageFim)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            
            var documentHTML = htmlWeb.Load(url + pageInicio.ToString());


            for (int page = pageInicio; page <= pageFim; page++) {
               
                var elements = documentHTML.DocumentNode.SelectNodes("//*[@id='content']/div/div/div[1]/div/table/tbody/tr[position()>1]");

                foreach (var element in elements)
                {
                    var dados = new DadosExtraidos()
                    {
                        IP_ADRESS = element.SelectSingleNode("td[2]").InnerText,
                        PORT = element.SelectSingleNode("td[3]").InnerText,
                        COUNTRY = element.SelectSingleNode("td[4]").InnerText,
                        PROTOCOL = element.SelectSingleNode("td[7]").InnerText,
                        
                    };
                    dadosGerais.listaDados.Add(dados);
                    dadosGerais.quantidade_linhas_extraidas++;
                }
                
                string pageString = page.ToString();
                string novaUrl = url + pageString;
                baixarPaginaHTML(novaUrl, page);
                documentHTML = htmlWeb.Load(url + pageString);
                dadosGerais.quantidade_paginas++;
            }
            countdownEvent.Signal();
        }
        public async void baixarPaginaHTML(string url, int numPage)
        {
            string fileName = "pagina" + numPage.ToString() + "_extraida.html";
            string folderName = "Paginas extraidas";
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), folderName);

            string filePath = Path.Combine(folderPath, fileName);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    HttpResponseMessage response = await httpClient.GetAsync(url);


                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }

                        Console.WriteLine($"A página HTML foi baixada e salva em {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"Falha ao baixar a página HTML. Código de status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao baixar a página HTML. Erro: {ex.Message}");
                }
            }
        }
        public void criarThreadDeExtracao()
        {   
            Thread t1 = new Thread(() => extrairDados("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/", 1, 9));
            Thread t2 = new Thread(() => extrairDados("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/", 10, 19));
            Thread t3 = new Thread(() => extrairDados("https://proxyservers.pro/proxy/list/order/updated/order_dir/desc/page/", 20, 27));

            t1.Start();
            t2.Start();
            t3.Start();

            countdownEvent.Wait();

           
            JsonResolver jsonResolver = new JsonResolver();
            jsonResolver.criarArquivoJson(dadosGerais);

         
        }

    }
}
