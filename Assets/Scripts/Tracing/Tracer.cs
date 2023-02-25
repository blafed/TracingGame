using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    public PatternCode patternCode { get; private set; }
    public Letter letter { get; private set; }


    public void setup(PatternCode patternCoed, Letter letter)
    {
        this.patternCode = patternCoed;
        this.letter = letter;
    }




}