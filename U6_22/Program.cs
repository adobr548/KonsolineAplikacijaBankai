// Audris Dobrikas IFZ-5/3 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
// U6-22
namespace U6_22
{
    // Klase banku duomenims saugoti
    class Bankas
    {
        private string pav; // banko pavadinimas
        private string val; // valstybes pavadinimas
        private int skyriai; // banko skyriu skaicius
        private int terminas; // 6 men terminas rikiavimui
  
        // pradiniai banko duomenys
        public Bankas()
        {
            pav = "";
            val = "";
            skyriai = 0;
            terminas = 0;
        
        }
        // Banko duomenu irasymas
        // pav - banko pavadinimas
        // val - valstybes pavadinimas
        // skyriai - banko skyriu skaicius
        public void Deti(string pav, string val, int skyriai)
        {
            this.pav = pav;
            this.val = val;
            this.skyriai = skyriai;

        }
       
        public void DetiTerm(int ter) { terminas = ter; } // Deda 6 men termina
        public string ImtiPav() { return pav; }  //grazina banko pavadinima
        public string ImtiVal() { return val; } // grazina valstybes pavadinima
        public int ImtiSkyr() { return skyriai; } // grazina skyriu skaiciu
        public int ImtiTerm() { return terminas; } // grazina 6 men termina
  
        // spausdinimo metodas
        public override string ToString()
        {
            string eilutė;         
            eilutė = string.Format("{0} {1,21} {2,5} ",
                                                pav, val, skyriai);
            return eilutė;
        }

        //Operatorius grazina true, jeigu 6 men palukanos yra didesnes
        // uz kito banko palukanas, arba palukanos yra
        // lygios, o banko pavadinimas yra mazesnis uz kito banko
        // pavadinima; false - kitais atvejais. 
        // terminas - 6 men. palukanos
        public static bool operator <= (Bankas pirmas, Bankas antras)
        {
            int kint1;
            if (pirmas.terminas > antras.terminas)
                kint1 = 1;
            else
                if (pirmas.terminas < antras.terminas)
                    kint1 = -1;
                else
                    kint1 = 0;

            int kint2 = String.Compare(pirmas.pav, antras.pav, StringComparison.CurrentCulture);
            return (kint1 > 0 || (kint1 == 0 && kint2 < 0));
        }
        // Operatorius grazina true, jeigu 6 men palukanos yra mazesnes
       // uz kito banko palukanas, arba palukanos yra
        // lygios, o banko pavadinimas yra didesnis uz kito banko
       // pavadinima; false - kitais atvejais. 
        // terminas - 6 men. palukanos
        public static bool operator >=(Bankas pirmas, Bankas antras)
        {
            int kint1;
            if (pirmas.terminas > antras.terminas)
                kint1 = 1;
            else
                if (pirmas.terminas < antras.terminas)
                    kint1 = -1;
                else
                    kint1 = 0;

            int kint2 = String.Compare(pirmas.pav, antras.pav, StringComparison.CurrentCulture);
            return (kint1 < 0 || (kint1 == 0 && kint2 > 0));
        }
        
    }

    // Klase palukanu duomenims saugoti 
    class Palukanos
    {
        const int CMaxBank = 100; // didziausias galimas banku skaicius
        const int CMaxTer = 7; // didziausias galimas terminu skaicius
        private Bankas[] Bankai; // banku duomenys
        public int n { get; set; } // eiluciu skaicius(bankai)
        private int[,] A; // duomenu matrica
        public int m { get; set; } // stulpeliu skaicus (terminai)

        //Pradinis matricos duomenų nustatymas
        public Palukanos()
        {
            n = 0;
            Bankai = new Bankas[CMaxBank];

            m = 0;
            A = new int[CMaxBank, CMaxTer];

        }

        // Grąžina nurodyto indekso banko objektą
        // nr - banko indeksas
        public Bankas Imti(int nr) { return Bankai[nr]; }
        // padeda i banku masyva nauja banka ir masyvo dydi padidina vienetu
        // ob - banko objektas
        public void Deti(Bankas ob) { Bankai[n++] = ob; }
        //Priskiria klasės matricos kintamąjam reikšmę.
        // i - eilutes indeksas
        // j - stulpelio indeksas
        // p - palukanu skaicius
        public void DetiA(int i, int j, int p) { A[i, j] = p; }
        // grazina palukanu kieki
        public int ImtiA(int i, int j) { return A[i, j]; }
        // pakeicia banko obj. masyvo banka,kurio numeris nr
        // nr - keiciamo banko nr
        // B - banko obj.masyvas
        public void PakeistiBank(int nr, Bankas B) { Bankai[nr] = B; }
        //Sukeičia dvi eilutes vietomis dvimačiame masyve A(n, m)
        //nr1 - pirmos eilutės numeris
        //nr2 - antros eilutės numeris
        public void SukeistiEilutes(int nr1, int nr2)
        {
            for (int j = 0; j < m; j++)
            {
                int d = A[nr1, j];
                A[nr1, j] = A[nr2, j];
                A[nr2, j] = d;
            }
        }
        // papildo duomenis 6 men palukanomis,kad galetu pagal jas surikiuoti
        public void PapildytiDuomenis()
        {
            int sesmen = 0;
            Bankas B = new Bankas();
            for (int i = 0; i < n; i++)
            {
                int j = 3; // Ima ketvirto stulpelio t.y. 6 men. palukanas
                B = Imti(i);
                sesmen = ImtiA(i, j);
                B.DetiTerm(sesmen);
                PakeistiBank(i,B);
            }
        }
        // rikiuoja pagal 6 men palukanas ir banku pavadinimus        
        public void Rikiuoja()
        {
            Bankas B;
            for (int i = 0; i < n - 1; i++)
            {
                int nr = i;
                for (int j = i + 1; j < n; j++)
                    if (Imti(j) <= Imti(nr))
                        nr = j;
                B = Imti(i);
                //pakeitimai masyvuose Bankas ir A(n,m)
                PakeistiBank(i, Imti(nr));
                PakeistiBank(nr, B);
                SukeistiEilutes(i, nr);
            }
        }

    }

    
    class Program
    {
        const string CFd = "Duomenys.txt"; // duomenu failas
        const string CFr = "Rezultatai.txt"; // rezultatu failas

        static void Main(string[] args)
        {
            if (File.Exists(CFr))  // jei failas egzistuoja,tai faila isvalo
                File.Delete(CFr);

            Palukanos P = new Palukanos(); // palukanu duomenys
            Skaityti(CFd, P); // skaito duomenis
            Spausdina(CFr, P, "Informacija apie bankus:"); //spausdina pirminius duomenis
            SpausdinaMatricą(CFr, P, "Palukanos"); // spausdina palukanas(matrica)

            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine(" Rezultatai:");
                fr.WriteLine();
            }
           
            Maxpaluk(CFr,P); // suranda kuriuose bankuose geriausia laikyti indelius kiekvienu laikotarpiu
            KeliBankai(CFr,P); // atspausdina banku valstybes pavadinimus ir kieki

            P.PapildytiDuomenis(); // papildo duomenis 6 men palukanomis,kad galetu atlikti rikiavima
            P.Rikiuoja(); // rikiuoja pagal 6 men palukanas ir banku pavadinimus
            Spausdina(CFr, P, " Rikiuotas sarasas pagal palukanas  6 men ir banku pavadinimus"); // atspausdina surikiuota sarasa

            Console.WriteLine(" Programa baige darba !");
        }

        //Failo duomenis surašo į konteinerį ir į dvimatį skaičių masyvą A(n, m)
        // fd - duomenu failas
        // Pa - palukanu duomenu konteineris
        static void Skaityti(string fd, Palukanos Pa)
        {
            int nn;
            int mm = 7;
            string pav;
            string val;
            int sk;
            int pal;
            string line;
            string[] parts;
            using (StreamReader reader = new StreamReader(fd, Encoding.GetEncoding(1257)))
            {
                line = reader.ReadLine();
                parts = line.Split(' ');
                nn = int.Parse(parts[0]);
                Pa.m = mm;
                for (int i = 0; i < nn; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(';');
                    pav = parts[0];
                    val = parts[1];
                    sk = int.Parse(parts[2]);
                    Bankas B;
                    B = new Bankas();
                    B.Deti(pav, val, sk);
                    Pa.Deti(B);
                }
                for (int i = 0; i < Pa.n; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(';');
                    for (int j = 0; j < Pa.m; j++)
                    {
                        pal = int.Parse(parts[j]);
                        Pa.DetiA(i, j, pal);
                    }
                }
            }
        }

        //Spausdina konteinerio duomenis faile.
        static void Spausdina(string fv, Palukanos Pa, string eilute)
        {
            using (var fr = File.AppendText(fv))
            {
                string line = new string('-', 50);
                fr.WriteLine(eilute);
                fr.WriteLine(line);
                fr.WriteLine(" Nr. Banko pavadinimas   Valstybe  Skyriu sk.   ");                
                fr.WriteLine(line);
                for (int i = 0; i < Pa.n; i++)
                    fr.WriteLine("  {0}. {1}  ", i + 1, Pa.Imti(i).ToString());
                fr.WriteLine(line);
                fr.WriteLine();
            }
        }
        // Spausdina palukanu duomenis faile
        static void SpausdinaMatricą(string CFr, Palukanos Pa, string eilute)
        {
            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine("{0}   1 men, 2 men, 3 men, 6 men, 1 met, 2 met, 3 met:", eilute);
                fr.WriteLine();
                for (int i = 0; i < Pa.n; i++)
                {
                    fr.Write("Bankas nr{0}.", i+1);
                    for (int j = 0; j < Pa.m; j++)
                        fr.Write("{0, 6:d} ", Pa.ImtiA(i, j));
                    fr.WriteLine();
                }
                fr.WriteLine();
            }

        }
       
        // Pagal didziausias palukanas is kiekvieno stulpelio atrenka banku pavadinimus ir iraso i faila
        static void Maxpaluk(string fv, Palukanos Pa)
        {
            int max = 0;
            string pavad = "";
           
            
            Bankas B = new Bankas();
            for (int j = 0; j < Pa.m; j++)
            {
                for (int i = 0; i < Pa.n; i++)
                {
                    B = Pa.Imti(i);
                    if (Pa.ImtiA(i, j) > max)
                    {
                        max = Pa.ImtiA(i, j);
                        pavad = B.ImtiPav();
                        
                    }
                    
                }
                


                using (var fr = File.AppendText(fv))
                {
                    if( j  == 0)
                        fr.WriteLine(" 1 men.  geriausiai laikyti savo indelius {0} ", pavad);
                     if( j  == 1)
                         fr.WriteLine(" 2 men.  geriausiai laikyti savo indelius {0} ", pavad);
                    if(j == 2)
                        fr.WriteLine(" 3 men.  geriausiai laikyti savo indelius {0} ", pavad);
                    if(j == 3)
                        fr.WriteLine(" 6 men.  geriausiai laikyti savo indelius {0} ", pavad);
                        if(j == 4)
                            fr.WriteLine(" 1 m.  geriausiai laikyti savo indelius {0} ", pavad);
                    if(j == 5)
                        fr.WriteLine(" 2 m.  geriausiai laikyti savo indelius {0} ", pavad);
                     if(j == 6)
                         fr.WriteLine(" 3 m.  geriausiai laikyti savo indelius {0} ", pavad);
                }                              
            }
                                                              
        }

        // atrenka valstybiu pavadinimus ir ju kieki
        static void KeliBankai(string fv, Palukanos Pa)
        {
          
            string valst = "";
            int kiek = 0;
            using (var fr = File.AppendText(fv))
            {
                fr.WriteLine();
                fr.WriteLine(" Vasltybes :");
                for (int i = 0; i < Pa.n; i++)
                {
                    
                    valst = Pa.Imti(i).ImtiVal();                    
                    kiek++;                                 
                        fr.WriteLine(valst);

                       
                } 
                fr.WriteLine("Valstybiu skaicius : {0}",kiek++);
                                                              
               
            }
        }

    }
}

