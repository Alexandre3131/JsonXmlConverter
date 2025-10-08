using System;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


Console.WriteLine("--------------------------------");
Console.WriteLine("Entrez 'q' pour quitter.\n");

while (true)
{
    Console.Write("Chemin du fichier JSON : ");
    var path = Console.ReadLine();

    if (string.Equals(path, "q", StringComparison.OrdinalIgnoreCase))
        break;

    if (!File.Exists(path))
    {
        Console.WriteLine("Fichier introuvable.\n");
        continue;
    }

    if (Path.GetExtension(path).ToLower() != ".json")
    {
        Console.WriteLine("Le fichier doit être au format .json\n");
        continue;
    }

    string jsonContent = File.ReadAllText(path);
    JToken jsonData = JToken.Parse(jsonContent);

    Console.WriteLine("\n--- Prévisualisation du JSON ---");

    string[] lines = jsonData.ToString(Newtonsoft.Json.Formatting.Indented)
                            .Split('\n');

    foreach (var line in lines)
        Console.WriteLine(line);

    while (true)
    {
        try
        {
            Console.WriteLine("--------------------------------\n");

            Console.WriteLine("Que voulez-vous faire ?");
            Console.WriteLine("1 - Rechercher dans le JSON");
            Console.WriteLine("2 - Trier le JSON");
            Console.WriteLine("3 - Exporter en XML");
            Console.WriteLine("4 - Quitter vers un autre fichier\n");

            Console.Write("Votre choix : ");
            string? choix = Console.ReadLine();

            if (choix == "1")
            {
                jsonData = RechercherJson(jsonData);
                continue;
            }
            else if (choix == "2")
            {
                TrierJson(jsonData);
            }
            else if (choix == "3")
            {
                XElement xml = JsonToXml(jsonData, "Root");
                string outputPath = Path.ChangeExtension(path, ".xml");
                xml.Save(outputPath);

                Console.WriteLine("Conversion réussie !");
                continue;
            }
            else if (choix == "4")
            {
                Console.WriteLine("Retour au choix de fichier...\n");
                break;
            }
            else
            {
                Console.WriteLine("Choix invalide.\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}\n");
        }
    }
}

static JToken RechercherJson(JToken jsonData)
{
    Console.Write("\nEntrez un mot-clé à rechercher : ");
    string? motCle = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(motCle))
    {
        Console.WriteLine("Mot-clé vide, retour au menu.\n");
        return jsonData;
    }

    var valeurs = jsonData
        .SelectTokens("$..*")
        .OfType<Newtonsoft.Json.Linq.JValue>()
        .Where(v => v.Value<string>()?.Contains(motCle, StringComparison.OrdinalIgnoreCase) == true)
        .ToList();

    if (!valeurs.Any())
    {
        Console.WriteLine("Aucun résultat trouvé.\n");
        return jsonData;
    }

    var array = new JArray();

    foreach (var val in valeurs)
    {
        string keyName = val.Path.Split('.').Last();
        string safeKey = new string(keyName.Select(c =>
            char.IsLetterOrDigit(c) ? c : '_').ToArray());

        var obj = new JObject { [safeKey] = new JValue(val.Value) };
        array.Add(obj);
    }

    Console.WriteLine($"\n--- {valeurs.Count} résultat(s) trouvé(s) ---");

    string[] lines = array.ToString(Newtonsoft.Json.Formatting.Indented)
                            .Split('\n');

    foreach (var line in lines)
        Console.WriteLine(line);

    return array;
}

static XElement JsonToXml(JToken token, string name)
{   
    if (token is JObject obj)
    {
        return new XElement(name,
            obj.Properties().Select(p => JsonToXml(p.Value, p.Name))
        );
    }
    else if (token is JArray array)
    {
        return new XElement(name,
            array.Select(a => JsonToXml(a, "Item"))
        );
    }
    else if (token is JValue value)
    {
        return new XElement(name, value.Value);
    }
    else
    {
        return null!;
    }
}

static void TrierJson(Newtonsoft.Json.Linq.JToken jsonData)
{
    Console.Write("Ordre de tri (C croissant, D décroissant) : ");
    var ordre = Console.ReadLine();

    var elements = jsonData
        .SelectTokens("$..*")
        .OfType<Newtonsoft.Json.Linq.JValue>()
        .Select(v => $"{v.Path} : {v.Value}")
        .ToList();

    if (ordre == "D")
    {
        elements.Reverse();
    }
    else
    {
        elements = elements.OrderBy(e => e, StringComparer.OrdinalIgnoreCase).ToList();
    }

    Console.WriteLine("\n--- Résultats triés ---");
    foreach (var e in elements)
        Console.WriteLine(e);
}
