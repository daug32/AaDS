using Lab2.Services;

namespace Lab2.Tests;

internal class ValidationServiceTest
{
    private static ValidatorService _syntaxService;

    public static void RunTests()
    {
        TestSyntax();
        TestNestedCycles();

        Console.WriteLine( $"{nameof( ValidatorService )} was tested successfuly" );
    }

    private static void TestNestedCycles()
    {
        string text;

        text = @"FOR I1 = 123
                    FOR I3 = 321
                    NEXT I3
                NEXT I1";
        TestSyntax( text, true );

        text = @"FOR I1 = 123 
                    FOR I3 
                NEXT I1";
        TestSyntax( text, false );

        text = @"FOR I1 = 123 
                    NEXT I4 
                NEXT I1";
        TestSyntax( text, false );
    }

    private static void TestSyntax()
    {
        TestSyntax( "", true );

        TestSyntax( "FOR i = something NEXT i", true );
        TestSyntax( "FOR i =something NEXT i", true );
        TestSyntax( "FOR m2 = something NEXT m2", true );
        TestSyntax( "FOR i1 = something      NEXT i1", true );
        TestSyntax( "something      useful FOR m2 = something is there NEXT m2", true );

        // Incorrect id
        TestSyntax( "FOR i = something NEXT i1", false );

        TestSyntax( "FOR 1 = something NEXT i", false );
        TestSyntax( "FOR ii = something NEXT i", false );
        TestSyntax( "FOR ii9 = something NEXT i", false );
        TestSyntax( "FOR iii = something NEXT i", false );
        TestSyntax( "FOR i = something NEXT someone", false );
        TestSyntax( "FOR = something NEXT someone", false );

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
