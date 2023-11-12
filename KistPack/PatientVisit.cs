using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KistPack
{
    internal class PatientVisit
    {
        private int pat;
        private int per;
        private string surname;
        private string givenname;

        public PatientVisit(int _pat, int _per, String _surname, String _givenname)
        {
            this.Pat = _pat;
            this.Per = _per;    
            this.Surname = _surname;
            this.Givenname = _givenname;


        }

        public int Pat { get => pat; set => pat = value; }
        public int Per { get => per; set => per = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Givenname { get => givenname; set => givenname = value; }
    }
}
