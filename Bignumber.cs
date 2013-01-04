using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuillaumeLeclerc
{
    class Bignumber
    {
        private List<Int16> contenu ;
        public int Count
        {
            get
            {
                return contenu.Count;
            }
        }

        public Bignumber(string source)
        {
            int length = source.Length;
            contenu = new List<short>(length);
            short temp;
            char lettre;
            for(int i =length-1 ; i>=0;i--)
            {
                lettre = source[i];
                if (Int16.TryParse(lettre.ToString(), out temp))
                {
                    contenu.Add(temp);
                }
                else
                {
                    throw new ArgumentException("Veuillez entrer un nombre");
                }
            }
        }

        public Bignumber(List<short> source)
        {
            this.contenu = new List<short>(source.Count);
            foreach (short nb in source)
            {
                contenu.Add(nb);
            }
        }

        public Bignumber Copy()
        {
            return new Bignumber(this.contenu);
        }

        public override string ToString()
        {
            String chaine="";
            foreach (short nb in this.contenu)
            {
                chaine = nb.ToString()+ chaine;
            }
            return chaine;
        }

        public List<short> GetContenu()
        {
            return this.contenu;
        }

        public Bignumber Somme(Bignumber autre)
        {
            List<short> resultat = new List<short>();
            int maxnb = this.contenu.Count;
            short retenue,a,b;
            int calcul;
            retenue=0;
            if (autre.contenu.Count > maxnb)
            {
                maxnb = autre.contenu.Count;
            }
            for (int i = 0; i < maxnb; i++)
            {
                if (i >= contenu.Count)
                    a = 0;
                else
                    a = contenu[i];
                if (i >= autre.contenu.Count)
                    b = 0;
                else
                    b = autre.contenu[i];
                calcul = a + b + retenue;
                if (calcul > 9)
                {
                    retenue = (short)(Math.Floor((decimal)calcul / 10));
                    calcul -= 10;
                }
                else
                {
                    retenue = 0;
                }
                resultat.Add(((short)calcul));
            }
            if (retenue != 0)
            {
                resultat.Add(retenue);
            }
            return new Bignumber(resultat);
        }

        public bool égalité(Bignumber autre)
        {
            if (this.contenu.Count != autre.contenu.Count)
            {
                return false;
            }
            for (int i = 0; i < contenu.Count; i++)
            {
                if (contenu[i] != autre.contenu[i])
                    return false;
            }
            return true;
        }

        public Bignumber multiplication(Bignumber autre)
        {
            if (autre == 0 || this == 0)
            {
                return new Bignumber("0");
            }
            Bignumber resultat = new Bignumber("0");
            Bignumber temp = new Bignumber("0");
            int i = 0;
            foreach (short mult in autre.contenu)
            {
                temp = new Bignumber("0");
                for(int u=0;u<mult;u++)
                {
                    temp += this;
                }

                string zeros="";

                for (int j = 0; j < i; j++)
                {
                    zeros += "0";
                }
                resultat += new Bignumber(temp.ToString() + zeros);
                i++;
            }
            return resultat;
            
        }

        public Bignumber puissance(Bignumber b)
        {
            Bignumber resultat;
            if (b.Count==1 && b.GetContenu()[0]==0)
                return new Bignumber("1");
            else if (b.Count == 1 && b.GetContenu()[0] == 1)
                return this;
            else
            {
                Bignumber temp = this.puissance(b / 2);
                if (b.Is_pair())
                {
                    resultat = temp * temp;
                }
                else
                {
                    resultat = temp * temp * this;
                }
            }
            
            return resultat.Copy();
        }

        public Bignumber soustraction(Bignumber b)
        {
            if (this < b)
            {
                throw new ArgumentException("On travaille sur des nombres positifs. cette opération est impossible");
            }

            int retenue = 0;
            Bignumber resultat = new Bignumber("0");
            string zeros;
            int x, y,z;
            for (int i = 0; i < this.Count; i++)
            {
                x = this.contenu[i];
                if (i >= b.Count)
                    y = 0;
                else
                    y = b.contenu[i];
                y += retenue;
                if (y > x)
                {
                    retenue = 1;
                    x += 10;
                }
                else
                {
                    retenue = 0;
                }
                z=x-y;
                zeros = "";
                for (int j = 0; j < i; j++)
                {
                    zeros += "0";
                }
                resultat += new Bignumber(z.ToString() + zeros);
            }

            for (int i = resultat.Count - 1; i > 0; i--)
            {
                if (resultat.contenu[i] != 0)
                    break;
                resultat.contenu.RemoveAt(i);
            }

                return resultat;
        }

        public Bignumber[] quotient(Bignumber b)
        {
            if (this < b)
            {
                return new Bignumber[2] {new Bignumber("0"),this.Copy()};
            }
            string resultat = "";
            Bignumber nb = new Bignumber(""); ;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (nb.Count==0)
                {
                    nb = new Bignumber(this.contenu[i].ToString());
                }
                else
                {
                    string add = nb.ToString();
                    if (add == "0")
                    {
                        add = "";
                    }
                    nb = new Bignumber(add + this.contenu[i].ToString());
                }

                if (nb == new Bignumber("0"))
                {
                    resultat += "0";
                    nb = new Bignumber("");
                }
                else 
                {
                    if (nb >= b)
                    {
                        int quotient = 0;
                        Bignumber autre = b.Copy();
                        while (autre <= nb)
                        {
                            quotient++;
                            autre += b;
                        }
                        nb = nb - (autre - b);
                        resultat += quotient.ToString();
                    }
                    else if (resultat!="")
                    {
                        resultat += "0";
                    }
                }
            }

            return new Bignumber[2] { new Bignumber(resultat), nb };            
        }
        

        private int ordre(Bignumber b)
        {
            if (this.Count > b.Count)
                return 1;
            else if(this.Count<b.Count)
                return -1;
            else
            {
                for (int i = this.Count - 1; i >= 0; i--)
                {
                    if (this.contenu[i] > b.contenu[i])
                        return 1;
                    else if (this.contenu[i] < b.contenu[i])
                        return -1;
                }
                return 0;
            }
        }

        public bool  Is_pair()
        {
            short nb=this.contenu[0];
            if (nb % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator >(Bignumber a, Bignumber b)
        {
            if (a.ordre(b) == 1)
                return true;
            else
                return false;
        }

        public static bool operator <(Bignumber a, Bignumber b)
        {
            if (a.ordre(b) == -1)
                return true;
            else
                return false;
        }

        public static bool operator >=(Bignumber a, Bignumber b)
        {
            return !(a < b);
        }
        public static bool operator <=(Bignumber a, Bignumber b)
        {
            return !(a > b);
        }

        public static Bignumber operator +(Bignumber a, Bignumber b)
        {
            return a.Somme(b);
        }

        public static bool operator ==(Bignumber a, Bignumber b)
        {
            return a.égalité(b);
        }

        public static bool operator ==(Bignumber a, long b)
        {
            return a.égalité(new Bignumber(b.ToString()));
        }

        public static bool operator !=(Bignumber a, long b)
        {
            return !a.égalité(new Bignumber(b.ToString()));
        }

        public static bool operator ==(long b,Bignumber a)
        {
            return a.égalité(new Bignumber(b.ToString()));
        }

        public static bool operator !=(long b, Bignumber a)
        {
            return !a.égalité(new Bignumber(b.ToString()));
        }

        public static bool operator !=(Bignumber a, Bignumber b)
        {
            return !a.égalité(b);
        }

        public static Bignumber operator ++(Bignumber a)
        {
            return a+(new Bignumber("1"));
        }

        public static Bignumber operator *(Bignumber a, Bignumber b)
        {
            return a.multiplication(b);
        }

        public static Bignumber operator *(Bignumber a, long b)
        {
            return a.multiplication(new Bignumber (b.ToString()));
        }
        public static Bignumber operator *(long b,Bignumber a)
        {
            return (new Bignumber(b.ToString())).multiplication(a);
        }

        public static Bignumber operator ^(Bignumber a , Bignumber b)
        {
            return a.puissance(b);
        }

        public static Bignumber operator ^(Bignumber a, long b)
        {
            return a.puissance(new Bignumber(b.ToString()));
        }

        public static Bignumber operator ^(long a, Bignumber b)
        {
            return (new Bignumber(a.ToString())).puissance(b);
        }

        public static Bignumber operator -(Bignumber a, Bignumber b)
        {
            return a.soustraction(b);
        }

        public static Bignumber operator -(Bignumber a, long b)
        {
            return a.soustraction(new Bignumber(b.ToString()));
        }

        public static Bignumber operator -(long b,Bignumber a)
        {
            return a.soustraction(new Bignumber(b.ToString()));
        }

        public static Bignumber operator /(Bignumber a, Bignumber b)
        {
            return a.quotient(b)[0];
        }

        public static Bignumber operator /(Bignumber a, long b)
        {
            return a.quotient(new Bignumber(b.ToString()))[0];
        }
        public static Bignumber operator /(long a, Bignumber b)
        {
            return (new Bignumber(a.ToString())).quotient(b)[0];
        }

        public static Bignumber operator %(Bignumber a, Bignumber b)
        {
            return a.quotient(b)[1];
        }

        public static Bignumber operator %(Bignumber a, long b)
        {
            return a.quotient(new Bignumber(b.ToString()))[1];
        }
        public static Bignumber operator %(long a, Bignumber b)
        {
            return (new Bignumber(a.ToString())).quotient(b)[1];
        }

        public static bool TryParse(string nb, out Bignumber resultat)
        {
            try
            {
                resultat = new Bignumber(nb);
                return true;
            }
            catch
            {
                resultat = new Bignumber("0");
                return false;
            }
        }
    }
}
