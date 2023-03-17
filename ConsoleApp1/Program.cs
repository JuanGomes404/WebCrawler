using WC;

Console.WriteLine("Iniciando Web Crawler....");

WebCrawler wc = new WebCrawler();

DadosGerais dadosExtraidos = wc.extrairDados();


WebCrawlerDAO wcDAO = new WebCrawlerDAO();

wcDAO.inserirDados(dadosExtraidos);

Console.WriteLine("INFORMAÇÕES:\n\n\n");

Console.WriteLine("O arquivo JSON com os dados extraídos foi salvo em " + @"C:\path.json");
Console.WriteLine("As páginas HTML extraídas estão localizadas na área de trabalho de seu computador na pasta 'Paginas extraidas'");