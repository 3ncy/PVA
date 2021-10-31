public class Cenzura
{
    public static void Main()
    {

        List<string> denyList = new List<string>(new string[] { "linux", "unix", "arch" });

        string[] vstup = (Console.ReadLine() ?? "").Split(' '); //check proti CS8602 aby vstup nebyl null

        for (int i = 0; i < vstup.Length; i++)
        {
            string match = denyList.Find(v => vstup[i].ToLower().Contains(v));//tady ten warning je vyresen hned na dalsi radce, kde musim checkovat proti null anyways.
            if (match != null)
            {
                vstup[i] = vstup[i].Substring(0, 2) + "*" + vstup[i].Substring(3);
            }
        }

        System.Console.WriteLine(string.Join(' ', vstup));
    }
}