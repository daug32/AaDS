﻿using Lab2.Models;

namespace Lab2.Services;

public interface IValidatorService
{
    bool IsSyntaxCorrect();

    void Copy( Action<string> copy );
}

public class ValidatorService : IValidatorService
{
    private readonly StringReaderService _stringReader;
    private readonly CustomStack<Keywords> _activeNestings;

    public ValidatorService( string text )
    {
        _stringReader = new StringReaderService( text );
        _activeNestings = new CustomStack<Keywords>();
    }

    public void Copy( Action<string> copy )
    {
        _stringReader.Reset();

        int tabsCount = 0;
        int tabsMultipier = 2;

        bool hadEndOfLine = false;

        for ( string word = _stringReader.NextWord(); word.Length > 0; word = _stringReader.NextWord() )
        {
            Keywords keyword = Utils.ParseKeyword( word );
            if ( keyword == Keywords.Next )
            {
                tabsCount--;
            }

            string tabs = !hadEndOfLine ? "" : new String( ' ', tabsCount * tabsMultipier );
            hadEndOfLine = _stringReader.IsEndOfLine;

            copy( $"{tabs}{word}{( _stringReader.IsEndOfLine ? '\n' : ' ' )}" );

            if ( keyword == Keywords.For )
            {
                tabsCount++;
            }
        }
    }

    public bool IsSyntaxCorrect()
    {
        _stringReader.Reset();

        int wordsAfterLastKeywordWithRequiredId = 0;
        Keywords prevKeyword = Keywords.NotKeyword;
        bool requireId = false;

        for ( string word = _stringReader.NextWord(); word.Length > 0; word = _stringReader.NextWord() )
        {
            Keywords keyword = Utils.ParseKeyword( word );

            if ( keyword == Keywords.NotKeyword )
            {
                requireId = false;
                wordsAfterLastKeywordWithRequiredId++;
                if ( !ProcessNotKeyword( word, prevKeyword, wordsAfterLastKeywordWithRequiredId ) )
                {
                    return false;
                }
            }

            else if ( keyword == Keywords.For )
            {
                requireId = true;
                prevKeyword = Keywords.For;
                wordsAfterLastKeywordWithRequiredId = 0;
                _activeNestings.Push( Keywords.For );
            }

            else if ( keyword == Keywords.Next )
            {
                requireId = true;
                prevKeyword = Keywords.Next;
                wordsAfterLastKeywordWithRequiredId = 0;
                if ( !ProcessNextKeyword( word ) )
                {
                    return false;
                }
            }
        }

        return !requireId && _activeNestings.IsEmpty;
    }

    public bool CheckId( string word )
    {
        if ( word.Length == 0 || word.Length > 2 )
        {
            return false;
        }

        if ( !StringUtils.IsLetter( word[ 0 ] ) )
        {
            return false;
        }

        if ( word.Length > 1 && !StringUtils.IsNumber( word[ 1 ] ) )
        {
            return false;
        }

        return true;
    }

    private bool ProcessNotKeyword( string word, Keywords prevKeyword, int wordsAfterLastKeywordWithId )
    {
        if ( prevKeyword == Keywords.NotKeyword )
        {
            return true;
        }

        if ( wordsAfterLastKeywordWithId == 1 )
        {
            if ( !CheckId( word ) )
            {
                return false;
            }
        }

        else if ( wordsAfterLastKeywordWithId == 2 )
        {
            if ( !StringUtils.IsOperator( word ) && !StringUtils.StartsWithOperator( word ) )
            {
                return false;
            }
        }

        return true;
    }

    private bool ProcessNextKeyword( string word )
    {
        if ( _activeNestings.IsEmpty )
        {
            return false;
        }

        Keywords lastKeyword = _activeNestings.Pop();
        if ( lastKeyword != Keywords.For )
        {
            return false;
        }

        return true;
    }
}
