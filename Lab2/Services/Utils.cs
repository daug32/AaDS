using Lab2.Models;

namespace Lab2.Services;

public static class Utils
{
    public static Keywords ParseKeyword( string word )
    {
        string lowerWord = StringUtils.ToLower( word );

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
