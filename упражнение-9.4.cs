using System;
using System.Collections.Generic;

class Song
{
    string name;
    string author;
    Song prev;

    public Song(string n, string a)
    {
        name = n;
        author = a;
        prev = null;
    }

    public Song(string n, string a, Song p)
    {
        name = n;
        author = a;
        prev = p;
    }

    public Song()
    {
        name = "";
        author = "";
        prev = null;
    }

    public void Name(string n)
    {
        name = n;
    }

    public void Author(string a)
    {
        author = a;
    }

    public void Prev(Song p)
    {
        prev = p;
    }

    public string Title()
    {
        return name + " - " + author;
    }

    public void Print()
    {
        Console.WriteLine(Title());
    }

    public override bool Equals(object d)
    {
        if (d == null || !(d is Song))
        {
            return false;
        }

        Song other = (Song)d;

        return name == other.name && author == other.author;
    }
}

class Program
{
    static void Main()
    {
        List<Song> songs = new List<Song>();

        Song song1 = new Song("Династия", "Villian");

        Song song2 = new Song("Prada party", "Kai angel", song1);

        Song song3 = new Song("Narcotic", "9mice", song2);

        Song song4 = new Song("Автозаправка", "Темный принц", song3);

        Song mySong = new Song();

        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song3);
        songs.Add(song4);

        foreach (Song song in songs)
        {
            song.Print();
        }

        if (songs[0].Equals(songs[1]))
        {
            Console.WriteLine("Песни одинаковые");
        }
        else
        {
            Console.WriteLine("Песни разные");
        }
    }
}
