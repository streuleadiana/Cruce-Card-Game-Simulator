using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

public enum Culoare
{
    Rosu,
    Duba,
    Verde,
    Ghinda
}

public enum Valoare
{
    Noua=0,
    Doi=2,
    Trei=3,
    Patru=4,
    Zece=10,
    As=11
}

public class Carte
{
    public Valoare valoare;
    public Culoare culoare;

    public Carte(Valoare valoare, Culoare culoare)
    {
        this.valoare = valoare;
        this.culoare = culoare;
    }
    
    public int getPuncte()
    {
        return (int)valoare;
    }
    
    public override string ToString()
    {
        return $"{valoare} de {culoare}";
    }
}

public class Jucator
{
    public string nume;
    public List<Carte> mana;

    public Jucator(string nume)
    {
        this.nume = nume;
        this.mana = new List<Carte>();
    }
    
    public void joaca(Carte carte)
    {
        //de facut
    }
}

public class Joc
{
    public List<Carte> pachet;
    public List<Jucator> jucatori;

    public Culoare tromf;
    public int puncte_max=0;
    public int index_jucator = -1;
    
    public int ScorEchipa1 = 0; // Nord-Sud
    public int ScorEchipa2 = 0; // Est-Vest
    
    public List<Carte> CartiCastigateEchipa1 = new List<Carte>(); 
    public List<Carte> CartiCastigateEchipa2 = new List<Carte>();
    
    public int AnunturiEchipa1 = 0;
    public int AnunturiEchipa2 = 0;
    
    public string NumeEchipa1 => $"{jucatori[0].nume}-{jucatori[2].nume}";
    public string NumeEchipa2 => $"{jucatori[1].nume}-{jucatori[3].nume}";
    public Joc(List<string> numeJucatori)
    {
        pachet=new List<Carte>();
        jucatori=new List<Jucator>();
        
        for (int i = 0; i < 4; i++)
        {
            string nume = (numeJucatori.Count > i) ? numeJucatori[i] : $"Jucator {i + 1}";
            jucatori.Add(new Jucator(nume));
        }
    }

    public void GenereazaPachet()
    {
        pachet.Clear();
        foreach (Culoare culoare in Enum.GetValues(typeof(Culoare)))
        {
            foreach (Valoare valoare in Enum.GetValues(typeof(Valoare)))
            {
                pachet.Add(new Carte(valoare, culoare));
            }
        }
    }

    public void AmestecaPachet()
    {
        Random rng = new Random();
        int n=pachet.Count;
        while (n > 1)
        {
            n--;
            int k=rng.Next(n+1);
            Carte  temp = pachet[k];
            pachet[k] = pachet[n];
            pachet[n] = temp;
        }
    }

    public void ImpartireCarti()
    {
        int index = 0;
        foreach (var j in jucatori)
        {
            j.mana.Clear();
            for (int i = 0; i < 6; i++)
            {
                if (index < pachet.Count)
                {
                    j.mana.Add(pachet[index]);
                    index++;
                }
            }
        }
    }
    public void Start()
    {
        GenereazaPachet();
        AmestecaPachet();
        ImpartireCarti();
    }
    public void Licitatie()
    {
        int puncte = 0;
        index_jucator = 0;
        for (int i = 0; i < 4; i++)
        {
            Console.Clear();
            Console.WriteLine($"SCOR GLOBAL | {NumeEchipa1}: {ScorEchipa1}  vs  {NumeEchipa2}: {ScorEchipa2}");
            Console.WriteLine("-LICITATIE-");
            Console.WriteLine($"Licitatie actuala: {jucatori[index_jucator].nume} ({puncte} puncte).");
            Jucator temp = jucatori[i];
            Console.WriteLine($"\nEste randul lui {temp.nume}");
            Console.WriteLine("Cartile tale sunt:");
            foreach(var c in temp.mana) 
                Console.WriteLine(c + ", ");
            Console.WriteLine($"Liciteaza mai mult de {puncte} puncte sau scrie 'pas'.");
            string input = Console.ReadLine();
            if (input == "pas")
            {
                Console.WriteLine($"Jucatorul {temp.nume} a spus pas.");
            }
            else
            {
                if (int.TryParse(input, out int oferta))
                {
                    if (oferta > puncte && oferta <= 4)
                    {
                        puncte = oferta;
                        index_jucator = i;
                        Console.WriteLine($"Oferta acceptata! Noul maxim: {puncte}");
                    }
                    else
                    {
                        Console.WriteLine("Oferta invalida. Se considera 'pas'.");
                    }
                }
                else
                {
                    Console.WriteLine("Input invalid. Se considera 'pas'.");
                }
            }
            Console.WriteLine("Apasa ENTER pentru a continua.");
            Console.ReadLine();
        }
        Console.Clear();
        puncte_max = puncte;
        Console.WriteLine($"\nLicitatia s-a incheiat! Castigator: {jucatori[index_jucator].nume} cu {puncte_max} puncte mari.");
        alegeTromf(jucatori[index_jucator]);
        Console.WriteLine("\nApasa ENTER pentru a trece la alegerea tromfului.");
        Console.ReadLine();
    }
    public void alegeTromf(Jucator castigator)
    {
        Console.Clear();
        Console.WriteLine($"SCOR GLOBAL | {NumeEchipa1}: {ScorEchipa1}  vs  {NumeEchipa2}: {ScorEchipa2}");
        Console.WriteLine($"\n{castigator.nume}, ai castigat licitatia! Alege TROMFUL.");
        Console.WriteLine("Cartile tale sunt:\n");
        foreach (var c in castigator.mana)
        {
            Console.WriteLine($"{c}");
        }
        Console.WriteLine("\nOptiuni: 0=Rosu, 1=Duba, 2=Verde, 3=Ghinda");
        bool alegere = false;
        while (!alegere)
        {
            Console.Write("Introdu codul culorii: ");
            string input = Console.ReadLine();
        
            if (int.TryParse(input, out int cod) && cod >= 0 && cod <= 3)
            {
                tromf = (Culoare)cod;
                alegere = true;
                Console.WriteLine($"Tromful a fost stabilit: {tromf}!");
            }
            else
            {
                Console.WriteLine("Culoare invalida! Incearca 0, 1, 2 sau 3.");
            }
        }
    }

    public bool Validare(Carte carteAleasa, Jucator jucator, Carte primaCarteJos)
    {
        if(primaCarteJos==null)
            return true;
        Culoare culoareCeruta=primaCarteJos.culoare;
        bool areCuloare = false;
        bool areTromf = false;
        foreach (var c in jucator.mana)
        {
            if(c.culoare==culoareCeruta)
                areCuloare = true;
            if(c.culoare==tromf)
                areTromf = true;
        }

        if (areCuloare)
        {
            if (carteAleasa.culoare == culoareCeruta)
                return true;
            else
                return false;
        }

        if (areTromf)
        {
            if (carteAleasa.culoare == tromf)
                return true;
            else
                return false;
        }

        return true;
    }

    public int CastigatorTura(List<Tuple<Jucator, Carte>> cartiJos)
    {
        Carte carteCastigatoare = cartiJos[0].Item2;
        int indexJucator=jucatori.IndexOf(cartiJos[0].Item1);
        for (int i = 1; i < cartiJos.Count; i++)
        {
            Carte carteCurenta = cartiJos[i].Item2;
            Jucator jucatorCurent = cartiJos[i].Item1;
            bool esteTromf=carteCurenta.culoare == tromf;
            bool castigatorTromf=carteCastigatoare.culoare == tromf;
            bool esteCuloareBaza = carteCurenta.culoare == cartiJos[0].Item2.culoare;
            
            if (esteTromf && !castigatorTromf)
            {
                carteCastigatoare = carteCurenta;
                indexJucator = jucatori.IndexOf(jucatorCurent);
            }
            else if (esteTromf && castigatorTromf)
            {
                if (carteCurenta.valoare > carteCastigatoare.valoare)
                {
                    carteCastigatoare = carteCurenta;
                    indexJucator = jucatori.IndexOf(jucatorCurent);
                }
            }
            else if (!castigatorTromf && esteCuloareBaza && carteCurenta.valoare > carteCastigatoare.valoare)
            {
                carteCastigatoare = carteCurenta;
                indexJucator = jucatori.IndexOf(jucatorCurent);
            }
        }
        Console.WriteLine($"\nMana castigata de {jucatori[indexJucator].nume} cu {carteCastigatoare}");
        return indexJucator;
    }

    public void Joc6Ture()
    {
        int indexJucatorCurent = index_jucator;
        CartiCastigateEchipa1.Clear();
        CartiCastigateEchipa2.Clear();
        AnunturiEchipa1 = 0;
        AnunturiEchipa2 = 0;
        for (int tura = 1; tura <= 6; tura++)
        {
            List<Tuple<Jucator,Carte>>cartiJos=new List<Tuple<Jucator, Carte>>();
            Carte primaCarte = null;
            for (int i = 0; i < 4; i++)
            {
                int indexReal=(indexJucatorCurent+i)%4;
                Jucator jucator = jucatori[indexReal];
                Console.Clear(); 
                Console.WriteLine($"--- TURA {tura} / 6 ---");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine($"SCOR GLOBAL | {NumeEchipa1}: {ScorEchipa1}  vs  {NumeEchipa2}: {ScorEchipa2}");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine($"TROMF: {tromf} | Puncte licitate: {puncte_max} puncte");
                Console.WriteLine("------------------------------------------");
                Console.Write("Masa: ");
                if (cartiJos.Count == 0) Console.Write("[GOL]");
                foreach (var par in cartiJos)
                {
                    Console.Write($" [{par.Item2}] ");
                }
                Console.WriteLine("\n------------------------------------------");
                
                Console.WriteLine($"Este randul lui: {jucator.nume}");
                Console.WriteLine("Mana ta:");
                for (int k = 0; k < jucator.mana.Count; k++)
                {
                    Console.WriteLine($"   {k}: {jucator.mana[k]}");
                }
                Console.WriteLine("------------------------------------------");
                

                bool mutareCorecta = false;
                Carte carteAleasa = null;
                int index = -1;
                while (!mutareCorecta)
                {
                    Console.Write("Index carte: ");
                    string input=Console.ReadLine();
                    if (int.TryParse(input, out index) && index >= 0 && index < jucator.mana.Count)
                    {
                        carteAleasa=jucator.mana[index];
                        bool areTromfInMana = false;
                        foreach(var c in jucator.mana)
                        {
                            if (c.culoare == tromf) 
                                areTromfInMana = true;
                        }

                        if (tura == 1 && i == 0 && areTromfInMana && carteAleasa.culoare != tromf)
                        {
                            Console.WriteLine($"REGULA: Fiind prima tura, esti OBLIGAT sa iesi cu TROMF ({tromf})!");
                        }
                        else if (Validare(carteAleasa, jucator, primaCarte))
                        {
                            mutareCorecta = true;
                        }
                        else
                        {
                            Console.WriteLine("Mutare invalida! Trebuie sa dai la culoare sau sa tai cu tromf.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Index gresit. Alege un numar din lista ta.");
                    }
                }

                if (i==0 && (carteAleasa.valoare == Valoare.Trei || carteAleasa.valoare == Valoare.Patru))
                {
                    Valoare valoarePereche = (carteAleasa.valoare == Valoare.Trei) ? Valoare.Patru : Valoare.Trei;
                    bool arePerechea = false;

                    foreach (var c in jucator.mana)
                    {
                        if(c.culoare==carteAleasa.culoare && c.valoare == valoarePereche)
                            arePerechea = true;
                    }

                    if (arePerechea)
                    {
                        int puncteAnunt=(carteAleasa.culoare == tromf) ? 40 : 20;
                        Console.WriteLine($"! STRIGARE: {puncteAnunt} PUNCTE !");
                        if(indexReal==0 || indexReal==2)
                            AnunturiEchipa1+=puncteAnunt;
                        else
                        {
                            AnunturiEchipa2+=puncteAnunt;
                        }
                    }
                }
                Console.WriteLine($"{jucator.nume} a jucat: {carteAleasa}");
                cartiJos.Add(new Tuple<Jucator, Carte>(jucator,carteAleasa));
                jucator.mana.RemoveAt(index);
                if(i==0)
                    primaCarte = carteAleasa;
                Console.WriteLine("Carte jucata! Apasa Enter pentru urmatorul jucator...");
                Console.ReadLine();
            }
            int indexCastigator=CastigatorTura(cartiJos);
            if (indexCastigator == 0 || indexCastigator == 2)
            {
                foreach(var par in cartiJos) 
                    CartiCastigateEchipa1.Add(par.Item2);
                Console.WriteLine($"Mana merge la Echipa {NumeEchipa1}");
            }
            else
            {
                foreach(var par in cartiJos) 
                    CartiCastigateEchipa2.Add(par.Item2);
                Console.WriteLine($"Mana merge la Echipa {NumeEchipa2}.");
            }
            indexJucatorCurent = indexCastigator;
        
            Console.WriteLine("Apasa Enter pentru a continua...");
            Console.ReadLine();
        }
    }

    public void CalculeazaScor()
    {
        Console.Clear();
        Console.WriteLine("-Scor Mana-");
        
        int pctMiciE1 = 0;
        foreach (var c in CartiCastigateEchipa1) 
            pctMiciE1 += c.getPuncte();

        int pctMiciE2 = 0;
        foreach (var c in CartiCastigateEchipa2) 
            pctMiciE2 += c.getPuncte();
        
        int totalE1 = pctMiciE1 + AnunturiEchipa1;
        int totalE2 = pctMiciE2 + AnunturiEchipa2;
        
        int pctMariE1 = totalE1 / 33;
        int pctMariE2 = totalE2 / 33;

        Console.WriteLine($"Puncte Mici -> {NumeEchipa1}: {totalE1} | {NumeEchipa2}: {totalE2}");
        Console.WriteLine($"Puncte Mari Realizate -> {NumeEchipa1}: {pctMariE1} | {NumeEchipa2}: {pctMariE2}");
        
        bool echipa1ALicitat = (index_jucator == 0 || index_jucator == 2);
        
        if (echipa1ALicitat)
        {
            Console.WriteLine($"Echipa {NumeEchipa1} a licitat {puncte_max}.");
            if (pctMariE1 >= puncte_max)
            {
                Console.WriteLine("Au facut punctele! Se adauga punctele realizate.");
                ScorEchipa1 += pctMariE1;
            }
            else
            {
                Console.WriteLine("NU au facut punctele! Se scad punctele licitate.");
                ScorEchipa1 -= puncte_max;
            }
            ScorEchipa2 += pctMariE2;
        }
        else
        {
            Console.WriteLine($"Echipa {NumeEchipa2} a licitat {puncte_max}.");
            if (pctMariE2 >= puncte_max)
            {
                Console.WriteLine("Au facut punctele! Se adauga punctele realizate.");
                ScorEchipa2 += pctMariE2;
            }
            else
            {
                Console.WriteLine("NU au facut punctele! Se scad punctele licitate.");
                ScorEchipa2 -= puncte_max;
            }
            ScorEchipa1 += pctMariE1;
        }

        Console.WriteLine("\n--- SCOR GLOBAL ---");
        Console.WriteLine($"Echipa {NumeEchipa1}: {ScorEchipa1}");
        Console.WriteLine($"Echipa {NumeEchipa2}: {ScorEchipa2}");
        Console.WriteLine("Apasa Enter pentru a continua...");
        Console.ReadLine();
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("      JOC DE CRUCE (C#)       ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Start Joc Scurt (pana la 11)");
            Console.WriteLine("2. Start Joc Lung (pana la 21)");
            Console.WriteLine("3. Iesire");
            Console.Write("Alege o optiune: ");

            string optiune = Console.ReadLine();

            if (optiune == "1")
            {
                Console.Clear();
                JoacaPartida(11);
            }
            else if (optiune == "2")
            {
                Console.Clear();
                JoacaPartida(21);
            }
            else if (optiune == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Optiune invalida!");
            }
        }
    }

    public static List<string> CitesteNumeJucatori()
    {
        Console.Clear();
        Console.WriteLine("==============================");
        Console.WriteLine("     CONFIGURARE JUCATORI     ");
        Console.WriteLine("==============================");

        List<string> nume = new List<string>();
        string[] echipe = { "Nord (Echipa 1)", "Est (Echipa 2)", "Sud (Echipa 1)", "Vest (Echipa 2)" };

        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Introdu nume pentru {echipe[i]}: ");
            string input = Console.ReadLine();

            // Daca utilizatorul da doar Enter, punem un nume default
            if (string.IsNullOrWhiteSpace(input))
            {
                input = $"Jucator {i + 1}";
            }

            nume.Add(input);
        }

        return nume;
    }

    public static void JoacaPartida(int puncteTarget)
    {
        List<string> numeJucatori = CitesteNumeJucatori();
        Joc joc = new Joc(numeJucatori);
        bool avemCastigator = false;
        while (!avemCastigator)
        {
            joc.Start();
            joc.Licitatie();
            joc.Joc6Ture();
            joc.CalculeazaScor();
            if (joc.ScorEchipa1 >= puncteTarget)
            {
                Console.Clear();
                Console.WriteLine($"   FELICITARI ECHIPA {joc.NumeEchipa1}!   ");
                Console.WriteLine("          ATI CASTIGAT PARTIDA!           ");
                avemCastigator = true;
                Console.ReadLine();
            }
            else if (joc.ScorEchipa2 >= puncteTarget)
            {
                Console.Clear();
                Console.WriteLine($"   FELICITARI ECHIPA {joc.NumeEchipa2}!   ");
                Console.WriteLine("          ATI CASTIGAT PARTIDA!           ");
                avemCastigator = true;
                Console.ReadLine();
            }
        }
    }
}