using UnityEngine;

public static class GuitarFretboard
{
    public static readonly string[,] Notes =
    {
        // String 6 (Low E)
        {
            "E2","F2","F#2","G2","G#2","A2","A#2","B2","C3","C#3","D3","D#3","E3"
        },

        // String 5 (A)
        {
            "A2","A#2","B2","C3","C#3","D3","D#3","E3","F3","F#3","G3","G#3","A3"
        },

        // String 4 (D)
        {
            "D3","D#3","E3","F3","F#3","G3","G#3","A3","A#3","B3","C4","C#4","D4"
        },

        // String 3 (G)
        {
            "G3","G#3","A3","A#3","B3","C4","C#4","D4","D#4","E4","F4","F#4","G4"
        },

        // String 2 (B)
        {
            "B3","C4","C#4","D4","D#4","E4","F4","F#4","G4","G#4","A4","A#4","B4"
        },

        // String 1 (High E)
        {
            "E4","F4","F#4","G4","G#4","A4","A#4","B4","C5","C#5","D5","D#5","E5"
        }
    };
}