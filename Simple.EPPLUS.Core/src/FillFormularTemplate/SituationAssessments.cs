namespace Toto.Simple.EPPLUS.Core
{
    public enum SituationAssessments
    {
        Unknown = 0,

        [StringValue("ROT - Ausfall der kritischen Versorgungsdienstleistung auf lokaler, regionaler, nationaler Ebene erwartet bzw. eingetreten")]
        Red = 1,

        [StringValue("ORANGE - Beeinträchtigung der kritischen Versorgungsdienstleistung bis hin zum Notbetrieb erwartet bzw. eingetreten")]
        Orange = 2,

        [StringValue("GELB - Verstärkte Auffälligkeiten in der Kritischen Informationsinfrastruktur, aber keine Beeinträchtigung der Versorgungsdienstleistung eingetreten, oder es werden nur geringe Beeinträchtigungen erwartet")]
        Yellow = 3,

        [StringValue("GRAU - Keine Auffälligkeiten in der Kritischen Informationsinfrastruktur")]
        Grey = 4
    }
}
