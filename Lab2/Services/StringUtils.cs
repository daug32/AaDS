namespace Lab2.Services;

public static class StringUtils
{
    private static readonly string _whitespaces = " \n\t\r";

    public static bool IsWhitespace( char symbol )
    {
        return _whitespaces.Any( whitespaceSymbol => whitespaceSymbol == symbol );
    }

    public static bool IsNumber( char symbol ) => symbol >= '0' && symbol <= '9';
    public static bool IsLowerLetter( char symbol ) => symbol >= 'a' && symbol <= 'z';
    public static bool IsUpperLetter( char symbol ) => symbol >= 'A' && symbol <= 'Z';
    public static bool IsLetter( char symbol ) => IsLowerLetter( symbol ) || IsUpperLetter( symbol );

    internal static string ToLower( string word )
    {
        int diff = 'A' - 'a';

        char[] result = word
            .Select( symbol =>
            {
                if ( IsUpperLetter( symbol ) )
                {
                    return ( char )( symbol - diff );
                }

                return symbol;
            } )
            .ToArray();

        return new String( result );
    }

    private static readonly string[] operators = new string[] { "=" };

    public static bool IsOperator( string word )
    {
        return operators.Any( operatorWord => operatorWord == word );
    }

    public static bool StartsWithOperator( string word )
    {
        foreach ( string operatorWord in operators )
        {
            if ( word.StartsWith( operatorWord ) )
            {
                return true;
            }
        }

        return false;
    }
}
