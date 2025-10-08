// abastraction -> création de la classe animal mais pas possible de linstancier
abstract class Animal
{
    // encapsulation avec champs privé
    private string nom;

    // getter
    public string Nom => nom;

    protected Animal(string name)
    {
        nom = name;
    }

    // methode à surcharger -> polymorphisme
    public abstract void Crier();
}