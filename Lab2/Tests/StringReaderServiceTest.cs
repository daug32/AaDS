using Lab2.Services;

namespace Lab2.Tests;

public static class StringReaderServiceTest
{
    private static StringReaderService _stringReader;

    public static void RunTests()
    {
        TestWhitespaces();
    }

    private static void TestWhitespaces()
    {
        TestWordsReading( "   word   ", "word" );
        TestWordsReading( " \n  word   ", "word" );
        TestWordsReading( "word word2   ", "word" );
        TestWordsReading( "", "" );
        TestWordsReading( "      ", "" );
    }

    private static void TestWordsReading( string text, string expected )
    {
        _stringReader = new StringReaderService( text );

        string result = _stringReader.NextWord();
        if ( result != expected )
        {
            throw new Exception( $"Expected {expected}, but got {result}" );
        }
    }
}
