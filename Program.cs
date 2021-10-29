List<string> denyList = new List<string>(new string[] { "linux", "unix", "arch" });

string[] vstup = Console.ReadLine().Split(' ');

for(int i = 0; i < vstup.Length; i++){
    string match = denyList.Find(v => vstup[i].ToLower().Contains(v));
    if(match != null){
        vstup[i] = vstup[i].Substring(0, 2) + "*" + vstup[i].Substring(3);
    }
}

System.Console.WriteLine(string.Join(' ', vstup));