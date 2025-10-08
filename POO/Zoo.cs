class Zoo
{
    public string Nom { get; }
    // composition
    private List<Animal> animaux = new();

    public Zoo(string nom)
    {
        Nom = nom;
    }

    public void AjouterAnimal(Animal animal) => animaux.Add(animal);

    public void LeZooCrie()
    {
        foreach (var a in animaux)
        {
            Console.Write($"{a.Nom} crie : ");
            a.Crier();
        }
    }
}