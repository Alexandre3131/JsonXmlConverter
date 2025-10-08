// héritage
class Lion : Animal
{
    public Lion(string name) : base(name) { }

    public override void Crier() => Console.WriteLine("roar");
}