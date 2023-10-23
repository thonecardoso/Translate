using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

var entrada = new[]
{
   //frases aqui 
   "Filtrar",
   "Todas",
   "Não Concluídas",
   "Aberta",
   "Aguardando Liberação",
   "Pendente",
   "Parcialmente Concluída",
   "Concluída",
   "Cancelada",
   "Todos",
   "Planejamento",
   "Ordem de serviço",
   "Limpar",
   "Abrir por código",
   "Nova Ordem de Serviço",
   "Nenhuma ordem de serviço para este planejamento.",
   "Previsto",
   "Realizado",
   "Ordem de serviço",
   "Planejamento",
   "Imprimir",
   "Excluir",
   "Cancelar",
   "Simulação",
   "Alterar",
   "Origem",
   "Status",
};

Processar(entrada);


void Processar(string[] entrada)
{
    //var str = "Informe a descrição"; this.FEITO = this.translate.instant("FEITO");
    //var res = Normalizar(str);
    Console.WriteLine(@"// tradução");
    foreach (var item in entrada)
    {
        Console.WriteLine($"{Normalizar(item)}: string = \'{item}\';");
    }
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();

    Console.WriteLine(@"this.configurarTraducao();");
    Console.WriteLine(@"configurarTraducao() {");
    foreach (var item in entrada)
    {
        var v = Normalizar(item);
        Console.WriteLine($"    this.{v} = this.translate.instant(\'{v}\');");
    }
    Console.WriteLine(@"}");

    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine("//portugues");
    //foreach (var item in entrada.Distinct().ToArray())
    //{
    //    Console.WriteLine($"\"{Normalizar(item)}\":\"{item}\",");
    //}


    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine();
    //Console.WriteLine("//espanhol");
    //UpdateJson(entrada.Distinct().ToArray());
    //foreach (var item in entrada.Distinct().ToArray())
    //{
    //    Console.WriteLine($"\"{Normalizar(item)}\":\"{Translate(item)}\",");
    //}

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    foreach (var item in entrada)
    {
        Console.WriteLine($@"translate>{Normalizar(item)}");
    }

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    foreach (var item in entrada)
    {
        Console.WriteLine($@"('{Normalizar(item)}' | translate)");
    }

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    foreach (var item in entrada)
    {
        Console.WriteLine($@"<span translate>{Normalizar(item)}</span>");
    }

    UpdateJson(entrada.Distinct().ToArray());
    Replace(entrada);
}


string Normalizar(string text)
{
    text = text.ToLower().Replace("ç", "c");

    string normalized = text.Normalize(NormalizationForm.FormD);

    StringBuilder builder = new StringBuilder();

    foreach (char c in normalized)
    {
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        {
            builder.Append(c);
        }
    }

    var res = builder.ToString();
    res = Regex.Replace(res, "[^a-zA-Z ]", "");
    res = string.Join("_", res.ToUpper().Split(' '));
    return res;
}

string Translate(string text)
{
    try
    {
        HttpClient httpClient = new HttpClient();
        string apiKey = "AIzaSyApZRjgwTnDvdCn3zI3IwMN8ZVg-QLgCGE";
        string url = $"https://www.googleapis.com/language/translate/v2?key={apiKey}&q={text}&source=pt-br&target=es";

        var result = httpClient.GetAsync(url).Result;

        var resultAsString = result.Content.ReadAsStringAsync().Result;

        JContainer resultAsObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JContainer>(resultAsString);

        return resultAsObject["data"]["translations"][0]["translatedText"].Value<string>();

    }
    catch (System.Exception)
    {
        return text;
    }
}


void Replace(string[] lista)
{
    // Especifique o arquivo de entrada e saída
    string inputFile = "C:\\Users\\thone\\source\\repos\\Linkfarm.Angular\\src\\app\\components\\ordem-servico\\ordem-servico\\ordem-servico.component.html";
    string outputFile = "C:\\Users\\thone\\source\\repos\\Linkfarm.Angular\\src\\app\\components\\ordem-servico\\ordem-servico\\ordem-servico.component.html";

    try
    {
        string text = File.ReadAllText(inputFile);

        foreach (var entry in lista)
        {
            // Use expressões regulares para encontrar todas as ocorrências da palavra no arquivo
            string pattern = @"\b" + Regex.Escape(entry) + @"\b";
            string replacement = $@"<span translate>{Normalizar(entry)}</span>";
            text = Regex.Replace(text, pattern, replacement);
        }

        // Escreva o texto modificado de volta para o arquivo de saída
        File.WriteAllText(outputFile, text);

        Console.WriteLine("Substituição concluída. Verifique o arquivo de saída.");
    }
    catch (IOException e)
    {
        Console.WriteLine("Ocorreu um erro ao processar o arquivo: " + e.Message);
    }
}

void UpdateJson(string[] lista)
{

    var es = "C:\\Users\\thone\\source\\repos\\Linkfarm.Angular\\src\\assets\\i18n\\es.json";

    var jsones = File.ReadAllText(es);
    var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsones);
    var dicpt = new Dictionary<string, string>();
    var dices = new Dictionary<string, string>();

    foreach ( var item in lista)
    {
        if (!dic.ContainsKey(Normalizar(item)))
        {
            dicpt[Normalizar(item)] = item;
            dices[Normalizar(item)] = Translate(item);
        }
    }


    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("//portugues");
    foreach ( var item in dicpt)
    {
        Console.WriteLine($"\"{item.Key}\":\"{item.Value}\",");
    }

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("//espanhol");
    foreach (var item in dices)
    {
        Console.WriteLine($"\"{item.Key}\":\"{item.Value}\",");
    }

}