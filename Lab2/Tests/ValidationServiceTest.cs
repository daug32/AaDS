using Lab2.Services;

namespace Lab2.Tests;

internal class ValidationServiceTest
{
    private static ValidatorService _syntaxService;

    public static void RunTests()
    {
        TestSyntax();
        TestNestedCycles();
    }

    private static void TestNestedCycles()
    {
        string text;

        text = @"FOR I1 = 123
                    FOR I3 = 321
                    NEXT I4
                NEXT I2";
        TestSyntax( text, true );

        text = @"FOR I1 = 123 
                    FOR I3 
                NEXT I2";
        TestSyntax( text, false );

        text = @"FOR I1 = 123 
                    NEXT I4 
                NEXT I2";
        TestSyntax( text, false );
    }

    private static void TestSyntax()
    {
        TestSyntax( "", true );

        TestSyntax( "FOR i = something NEXT i", true );
        TestSyntax( "FOR i1 = something    NEXT i1", true );
        TestSyntax( "FOR m2 = something NEXT b9", true );
        TestSyntax( "something      useful FOR m2 = something is there NEXT b9", true );

        TestSyntax( "FOR i =something NEXT i", true );

        // Incorrect id
        TestSyntax( "FOR 1 = something NEXT i", false );
        TestSyntax( "FOR ii = something NEXT i", false );
        TestSyntax( "FOR ii9 = something NEXT i", false );
        TestSyntax( "FOR iii = something NEXT i", false );
        TestSyntax( "FOR i = something NEXT someone", false );

        // Incorrect syntax
        //TestSyntax( "FOR i i = something NEXT i", false );
        //TestSyntax( "FOR i = something NEXT ", false );
        //TestSyntax( "i = something NEXT i", false );
        //TestSyntax( "FOR i = NEXT i", false );
        //TestSyntax( "FOR i something NEXT i", false );
        //TestSyntax( "FOR i = NEXT i", false );
    }

    private static void TestSyntax( string text, bool expected )
    {
        _syntaxService = new ValidatorService( text );

        bool result = _syntaxService.IsSyntaxCorrect();
        if ( result != expected )
        {
            throw new Exception( $"Expected {expected}, but got {result}" );
        }
    }
}
