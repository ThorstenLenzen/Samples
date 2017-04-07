namespace Toto.Simple.EPPLUS.Core
{
    public enum YesNoDecision
    {
        Unknown = 0,

        [StringValue("Ja")]
        Yes = 1,

        [StringValue("Nein")]
        No = 2
    }
}
