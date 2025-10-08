Console.WriteLine("Quel est votre calcul ?");

var calcul = Console.ReadLine();

if (string.IsNullOrWhiteSpace(calcul)) {
            Console.WriteLine("Aucun calcul fourni");
            return;
        }

char operateur = ' ';

if (calcul.Contains('+')) operateur = '+';
else if (calcul.Contains('-')) operateur = '-';
else if (calcul.Contains('*')) operateur = '*';
else if (calcul.Contains('/')) operateur = '/';

if (operateur == ' ')
        {
            Console.WriteLine("Aucun opérateur");
            return;
        }

string[] parties = calcul.Split(operateur);

double a = double.Parse(parties[0]);
double b = double.Parse(parties[1]);
double resultat = 0;

    switch (operateur) {
            case '+': resultat = a + b; break;
            case '-': resultat = a - b; break;
            case '*': resultat = a * b; break;
            case '/':
                if (b == 0)
                {
                    Console.WriteLine("Division par zéro");
                    return;
                }
                resultat = a / b; 
                break;
        }

Console.WriteLine($"Résultat : {resultat}");