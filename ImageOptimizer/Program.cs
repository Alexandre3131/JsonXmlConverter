using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // repertoire ou se trouve les images
        string inputDir = "images";

        // repertoires ou on mettra les images avec differentes resolutions
        string resizeSeqDir = "resizedSequential";
        string resizeParDir = "resizedParallel";

        // création des repertoires
        Directory.CreateDirectory(resizeSeqDir);
        Directory.CreateDirectory(resizeParDir);

        // on recupere le nombre d'image
        var files = Directory.GetFiles(inputDir, "*.jpg");
        Console.WriteLine($"Images détectées : {files.Length}");

        // calcul vitesse d'exec en séquentiel
        var sw = Stopwatch.StartNew();
        ResizeImagesSequential(inputDir, resizeSeqDir);
        sw.Stop();

        Console.WriteLine($"Resize images séquentiel : {sw.ElapsedMilliseconds} ms");

        // calcul vitesse d'exec en Parallel
        sw.Restart();
        ResizeImagesParallel(inputDir, resizeParDir);
        sw.Stop();
        Console.WriteLine($"Resize img parallèle : {sw.ElapsedMilliseconds} ms");
    }

    static void ResizeImagesSequential(string inputDir, string outputDir)
    {
        // differentes resolutions
        var sizes = new[] { 1080, 720, 440 };
        var files = Directory.GetFiles(inputDir, "*.jpg");

        foreach (var file in files)
        {
            // on charge limage en mémoire
            using var img = Image.Load(file);
            string name = Path.GetFileNameWithoutExtension(file);

            // on genere une version de l'image pour chaque resolution
            foreach (var size in sizes)
            {
                string outputPath = Path.Combine(outputDir, $"{name}_{size}px.jpg");
                SaveResized(img, size, outputPath);
            }
        }
    }

    static void ResizeImagesParallel(string inputDir, string outputDir)
    {
        // differentes resolutions
        var sizes = new[] { 1080, 720, 440 };
        var files = Directory.GetFiles(inputDir, "*.jpg");

        // on répartit le travail sur plusieurs threads
        Parallel.ForEach(files, file =>
        {
            // on charge limage en mémoire
            using var img = Image.Load(file);
            string name = Path.GetFileNameWithoutExtension(file);

            // on genere une version de l'image pour chaque resolution
            foreach (var size in sizes)
            {
                string outputPath = Path.Combine(outputDir, $"{name}_{size}px.jpg");
                SaveResized(img, size, outputPath);
            }
        });
    }
    
    static void SaveResized(Image src, int maxSize, string outputPath)
    {
        // on clone l'image avec la bonne resolution
        using var clone = src.Clone(ctx =>
        {
            ctx.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxSize, maxSize),
            });
        });

        var encoder = new JpegEncoder { Quality = 85 };
        // sauvegarde du fichier
        clone.Save(outputPath, encoder);
    }
}