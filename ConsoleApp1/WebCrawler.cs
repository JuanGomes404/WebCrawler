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
        ConcurrentBag<DadosExtraidos> listaDeDadosExtraidos = new ConcurrentBag<DadosExtraidos>();
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
                        PROTOCOL = element.SelectSingleNode("td[7]").InnerText
                    };
                    listaDeDadosExtraidos.Add(dados);
                }
                string pageString = page.ToString();
                documentHTML = htmlWeb.Load(url + pageString);
                
            }
            countdownEvent.Signal();
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

            var lista = listaDeDadosExtraidos.ToArray();

           for(int i = 0; i < lista.Length; i++)
            {
                Console.WriteLine("PORT: " + lista[i].PORT);
                Console.WriteLine("PROTOCOL: " + lista[i].PROTOCOL);
                Console.WriteLine("COUNTRY: " + lista[i].COUNTRY);
                Console.WriteLine("IP ADRESS: " + lista[i].IP_ADRESS);

                Console.WriteLine("\n\n\n");

            }

        }

    }
}
