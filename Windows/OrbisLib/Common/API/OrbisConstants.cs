namespace OrbisLib2.Common.API
{
    public enum BuzzerType
    {
        RingOnce = 1,
        RingThree,
        LongRing,
        ThreeLongRing,
        ThreeLongDoubleBeeps,
    }

    public enum ConsoleTypes
    {
        UNK,
        DIAG, //0x80
        DEVKIT, //0x81
        TESTKIT, //0x82
        RETAIL, //0x83 -> 0x8F
        KRATOS, //0xA0 IMPOSSIBLE??
    };

    public enum ConsoleLEDColours
    {
        white,
        white_Blinking,
        Blue_Blinking,
    };
}
