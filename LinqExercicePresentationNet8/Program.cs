using DataSources;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json.Linq;

Console.WriteLine("Hello, World!");

var allAlbums = ListAlbumsData.ListAlbums;
var allArtists = ListArtistsData.ListArtists;

/*

var request =
    from album in allAlbums
    let stringDisplay = $"Album {album.AlbumId} : {album.Title}"
    select stringDisplay;

foreach (var line in request)
{
    Console.WriteLine(line);
}

*/

/* Where

Console.WriteLine("Quelle est votre recherche ?");

var recherche = Console.ReadLine();

if (string.IsNullOrWhiteSpace(recherche)) {
            Console.WriteLine("Aucune recherche fournie");
            return;
        }

var whereRequest =
    (from album in allAlbums
     where album.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
     select album)
    .OrderBy(a => a.Title)
    .ThenByDescending(a => a.AlbumId)
    .Select(a => $"Album {a.AlbumId} : {a.Title}")
    .ToList();

whereRequest.ForEach(Console.WriteLine);

var groupes =
    from album in allAlbums
    where album.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
    orderby album.Title ascending, album.AlbumId descending
    group album by album.ArtistId into g
    select g;

foreach (var groupe in groupes)
{
    Console.WriteLine($"Artiste {groupe.Key} :");

    foreach (var album in groupe.OrderBy(a => a.Title))
    {
        Console.WriteLine($"Album {album.AlbumId} : {album.Title}");
    }
    Console.WriteLine();
}

var groupes =
    from album in allAlbums
    where album.Title.Contains(recherche, StringComparison.InvariantCultureIgnoreCase)
    join artist in allArtists on album.ArtistId equals artist.ArtistId
    orderby album.Title ascending, album.AlbumId descending
    select new
    {
        Artist = artist,
        Album = album
    }
    into resultat
    orderby resultat.Artist.Name
    group resultat by resultat.Artist into g
    select g;

foreach (var groupe in groupes)
{
    Console.WriteLine($"Artiste : {groupe.Key}");

    foreach (var item in groupe)
    {
        Console.WriteLine($"    {item.Album.Title}");
    }
    Console.WriteLine();
} 

var albumsForDisplay =
    from album in allAlbums
    let affichageAlbum = $"Album {album.AlbumId} : {album.Title}"
    orderby album.AlbumId
    select affichageAlbum;

int pageSize = 10;
int totalAlbums = albumsForDisplay.Count();
int totalPages = (int)Math.Ceiling(totalAlbums / (double)pageSize);

for (int page = 0; page < totalPages; page++)
{
    Console.WriteLine($"Page {page + 1}/{totalPages}");

    var pageAlbums = albumsForDisplay
        .Skip(page * pageSize)
        .Take(pageSize);

    foreach (var album in pageAlbums)
    {
        Console.WriteLine(album);
    }

    Console.WriteLine("\nAppuie sur Entrée pour continuer...");
    Console.ReadLine();
}

var path = Path.Combine(
    Directory.GetCurrentDirectory(),
    "..", "DataSources", "Text", "Albums.txt"
);

var lignes = File.ReadAllLines(path);

Console.Write("Quel est votre recherche ? ");
var recherche = Console.ReadLine();

var resultats = lignes
    .Where(ligne => ligne.Contains(recherche, StringComparison.InvariantCultureIgnoreCase))
    .ToList();

foreach (var ligne in resultats)
        {
            Console.WriteLine(ligne);
        } 

var allAlbumsXML =
    new XElement("Root",
        from album in allAlbums
        select new XElement("Album",
            new XElement("AlbumId", album.AlbumId),
            new XElement("Title", album.Title)
        )
    );

Console.WriteLine(allAlbumsXML); */

