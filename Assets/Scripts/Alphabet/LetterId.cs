public struct LetterId
{
    int value;
    public LetterId(int value)
    {
        this.value = value;
    }


    public int toInt()
    {
        return value;
    }
    public char toChar()
    {
        return LetterUtility.letterToChar(value);
    }
}