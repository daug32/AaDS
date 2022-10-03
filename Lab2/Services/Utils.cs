using Lab2.Models;

namespace Lab2.Services;

public static class Utils
{
    private static string[] operators { get; } = new string[]
    {
        "="
    };

    public static bool IsOperator( string word )
    {
        return operators.Any( operatrorWord => operatrorWord == word );
    }

    public static bool StartsWithOperator( string word )
    {
        return operators.Any( word.StartsWith );
    }

    public static Keywords ParseKeyword( string word )
    {
        string lowerWord = word.ToLower();

        switch ( lowerWord )
        {
            case "for":
                return Keywords.For;
            case "next":
                return Keywords.Next;
        }

        return Keywords.NotKeyword;
    }
}
