
public class SouSam
{
    public static void Main()
    {
        string souhlasky = "bcčdďfghjklmnňpqrřsštťvwxzž";
        string samohlasky = "aáeéěiíoóuúůyý";

        string vstup = Console.ReadLine() ?? "";//check proti null
        int pocetSou = 0;
        int pocetSam = 0;
        string bezSou = "";
        string bezSam = "";

        foreach (char c in vstup)
        {
            if (souhlasky.Contains(c.ToString().ToLower()))
            {
                pocetSou++;
                bezSou += "X";
                bezSam += c;
            }
            else if (samohlasky.Contains(c.ToString().ToLower()))
            {
                pocetSam++;
                bezSam += "X";
                bezSou += c;
            }
            else
            {
                bezSam += c;
                bezSou += c;
            }
        }

        Console.WriteLine("Pocet souhlasek ve vete: {0}\nPocet samohlasek ve vete: {1}", pocetSou, pocetSam);
        Console.WriteLine($"Veta bez souhlasek: {bezSou}\nVeta bez samohlasek: {bezSam}");
    }
}