using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KistPack
{
    internal class PatientVisit
    {
        private string charge;
        private string kiste;
        private string fallnummer;
        private string person;
        private string gebdat;
        private string nachname;
        private string vorname;
        private string scandatum;
        private string scanuser;
        private string scanclient;
        private string scanhostname;
        private string fallstorno;



        public PatientVisit(String _fallnummer, String _person, String _gebdat, String _vorname , String _nachname, String _fallstorno)
        {
            this.Fallnummer = _fallnummer;
            this.Person = _person;
            this.Gebdat = _gebdat;
            this.Vorname = _vorname;
            this.Nachname = _nachname;
            this.Fallstorno = _fallstorno;

        }

        public PatientVisit(String _charge, String _kiste, String _fallnummer, String _person, String _gebdat, String _vorname, String _nachname, String _scandatum, String _scanuser, String _scanclient, String _scanhostname)
        {
            this.charge = _charge;
            this.kiste = _kiste;
            this.Fallnummer = _fallnummer;
            this.Person = _person;
            this.Gebdat = _gebdat;
            this.Vorname = _vorname;
            this.Nachname = _nachname;            
            this.Scandatum = _scandatum;
            this.Scanuser = _scanuser; 
            this.Scanhostname = _scanhostname;
            this.Scanclient = _scanclient;
            


        }
        public string Fallnummer { get => fallnummer; set => fallnummer = value; }
        public string Person { get => person; set => person = value; }
        public string Nachname { get => nachname; set => nachname = value; }
        public string Vorname { get => vorname; set => vorname = value; }
        public string Charge { get => charge; set => charge = value; }
        public string Kiste { get => kiste; set => kiste = value; }
        public string Scandatum { get => scandatum; set => scandatum = value; }
        public string Scanuser { get => scanuser; set => scanuser = value; }
        public string Scanclient { get => scanclient; set => scanclient = value; }
        public string Scanhostname { get => scanhostname; set => scanhostname = value; }
        public string Fallstorno { get => fallstorno; set => fallstorno = value; }
        public string Gebdat { get => gebdat; set => gebdat = value; }
    }
}
