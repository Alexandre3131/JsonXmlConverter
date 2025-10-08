// héritage
class Chien : Animal
{
    public Chien(string name) : base(name) { }

    public override void Crier() => Console.WriteLine("waf");
}